using SECS.Parsing;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YQ.FreeSQL.Entity;
using YQ.FunctionModule.Bll;
using YQ.Parsing;
using YQ.Tool;
using YQ.Tool.JY;
using YQ.Tool.UDP;

namespace YQ.FunctionModule.ViewModels
{
    internal class ListenInViewModel : BindableBase
    {
        #region 属性
        private readonly IContainerExtension container;
        private readonly IEventAggregator eventAggregator;
        private static readonly object oRcvLock = new object();
        private static readonly object oSendLock = new object();
        private static readonly object oListionLock = new object();
        private bool IsC = false;
        private ILogService log;
        private Socket client;
        private IPEndPoint clientip;
        private IPEndPoint serverip;
        private ListenInBll listenInBll;
        public IPEndPoint Remote;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public static Queue<ReceiveData> queue = new Queue<ReceiveData>();
        private int defaultschemeid;//判断重复列队 -1运行重复
        private UDPServer UDPSrv { get; set; }
        private List<ComSet> comList= new List<ComSet>();   
        /// <summary>
        /// combox下拉框内容
        /// </summary>
        private List<CmdData> cmdList;  
        public List<CmdData> CmdList
        {
            get { return cmdList; }
            set { SetProperty(ref cmdList, value); }
        }
        /// <summary>
        /// textbox显示内容
        /// </summary>
        private string _SCmd;
        public string SCmd
        {
            get { return _SCmd; }
            set { SetProperty(ref _SCmd, value); }
        }
        /// <summary>
        /// 接受消息框内容
        /// </summary>
        private string _TextRcv = string.Empty;
        public string TextRcv
        {
            get { return _TextRcv; }
            set { SetProperty(ref _TextRcv, value); }
        }
        /// <summary>
        /// 发送消息框内容
        /// </summary>
        private string _TextSend = string.Empty;
        public string TextSend
        {
            get { return _TextSend; }
            set { SetProperty(ref _TextSend, value); }
        }
        #endregion
        #region Command
        /// <summary>
        /// Combox_SelectionChanged
        /// </summary>
        private DelegateCommand<object> _SelectionChanged;
        public DelegateCommand<object> SelectionChangedCommand =>
            _SelectionChanged ?? (_SelectionChanged = new DelegateCommand<object>(SelectionChanged));
        private void SelectionChanged(object sender)
        {
            if (sender!=null)
            {
                SCmd = (sender as CmdData).Msg;
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        private DelegateCommand _SendCommand;
        public DelegateCommand SendCommand =>
            _SendCommand ?? (_SendCommand = new DelegateCommand(Send));
        private void Send()
        {
            if (string.IsNullOrWhiteSpace(SCmd))
            {
                return;
            }
            AbstractCmd cmd = new RequestCmd(SCmd);
            string msg = cmd.ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            client.SendTo(buffer, serverip);
        }
        /// <summary>
        /// 开始
        /// </summary>
        private DelegateCommand _StartCommand;
        public DelegateCommand StartCommand =>
            _StartCommand ?? (_StartCommand = new DelegateCommand(Start));
        private void Start()
        {
            if (UDPSrv != null)
            {
                return;
            }
            UDPSrv = new UDPServer();
            UDPSrv.DataReceived += UDPSrv_DataReceived;
            UDPSrv.DataSended += UDPSrv_DataSended;
            UDPSrv.StartUDPServer();
            ShowSendMsg("启动成功!");
            Task.Run(() =>
            {
                PowerHelper.HangPos?.Clear();
                var com = new ComParamter(ConfigHelper.GetValue("ComParamter"));
                var PowerCom = com.PortName.Substring(3);
                PowerHelper.SetDev_Port(Convert.ToByte(PowerCom));
                PowerHelper.InitMeterParams();
                //for (int i = 0; i < 12; i++)
                //{
                //    PowerHelper.NoMeter(i, false);
                //}
                ListionCom();
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    DoUdpData();                 
                }
            });
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    cDoUdpData();
                }
            });
        }
        /// <summary>
        /// 停止
        /// </summary>
        private DelegateCommand _StopCommand;
        public DelegateCommand StopCommand =>
            _StopCommand ?? (_StopCommand = new DelegateCommand(Stop));
        private void Stop()
        {
            try
            {
                if (UDPSrv != null)
                {
                    UDPSrv.StopUDPServer();
                    ShowSendMsg("已停止!");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            UDPSrv = null;
            cancellationTokenSource.Cancel();
        }
        /// <summary>
        /// 清除
        /// </summary>
        private DelegateCommand _ClearCommand;
        public DelegateCommand ClearCommand =>
            _ClearCommand ?? (_ClearCommand = new DelegateCommand(Clear));
        private void Clear()
        {
            TextRcv=string.Empty;
            TextSend=string.Empty;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 接受消息展示
        /// </summary>
        /// <param name="msg">消息内容</param>
        private void ShowRcvMsg(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            lock (oRcvLock)
            {
                if (TextRcv.Length > 12000)
                {
                    TextRcv = string.Empty;
                }
                TextRcv += DateTime.Now.ToString("HH:mm:ss.fff ") + msg + Environment.NewLine;
            }
            log.Info(msg);
        }
        /// <summary>
        /// 发送消息展示
        /// </summary>
        /// <param name="msg">消息内容</param>
        private void ShowSendMsg(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            lock (oSendLock)
            {
                if (TextSend.Length > 12000)
                {
                    //TextSend = TextSend.Substring(TextSend.IndexOf(Environment.NewLine, 2000) + 2);
                    TextSend = string.Empty;
                }
                TextSend += DateTime.Now.ToString("HH:mm:ss.fff ") + msg + Environment.NewLine;
            }
            log.Info(msg);
        }
        /// <summary>
        /// 应答数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool UDPSrv_DataSended(byte[] data)
        {
            try
            {
                string msg = Encoding.UTF8.GetString(data);
                ShowSendMsg("应答:" + msg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return true;
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="remote"></param>
        private void UDPSrv_DataReceived(byte[] data, IPEndPoint remote)
        {
            string rcvmsg = Encoding.UTF8.GetString(data);
            AbstractCmd cmd_rcv = new RequestCmd(rcvmsg);
            ShowRcvMsg("接收:" + rcvmsg.ToString());
            ReceiveData receiveData = new ReceiveData(cmd_rcv.cmd, cmd_rcv, remote.Address, remote.Port);
            Remote=remote;
            IsC = true;
            if (defaultschemeid == -1)
            {
                //var ldata = queue.Where(t => t.id == cmd_rcv.cmd&&t.abstractCmd.data==cmd_rcv.data);//判断重复命令的队列
                //if (ldata.Count() == 0)
                //{
                    queue.Enqueue(receiveData);
                //}
            }
            else
            {
                if (queue.Count == 0)//只处理一条命令的队列
                {
                    queue.Enqueue(receiveData);
                }
                else
                {
                    Thread.Sleep(1551);
                    SendUDPMsgBack(receiveData.RemoteIP, receiveData.RemotePort, new ResponseCmd(cmd_rcv.cmd, 1, "busy"));
                }
            }
        }
        private void DoUdpData()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    ReceiveData receiveDat = queue.Peek();
                    Task.Run(() => {
                        DealWidthRequest(receiveDat.abstractCmd, receiveDat.RemoteIP, receiveDat.RemotePort);
                    });                    
                    queue.Dequeue();
                    Thread.Sleep(50);
                }
            }
        }
        private void cDoUdpData()
        {
            while (true)
            {
                if (cqueue.Count > 0&&IsC)
                {
                    var bbb = cqueue.TryPeek(out byte[] receiveDat);
                    if (bbb)
                    {
                        UDPSrv?.SendData(receiveDat, Remote);
                        cqueue.TryDequeue(out byte[] receiveDat1);
                        Thread.Sleep(50);
                    }
                }
            }
        }
        private void ListionCom()
        {
            var list = comList;
            foreach (var item in list)
            {
                var com = new ComParamter(item.ComName +"-"+ item.ComPara);
                SerialManager.Instance.CreateAndOpenPort(com);
                if (SerialManager.Instance.PortList.ContainsKey(item.ComName.ToUpper()))
                {
                    SerialManager.Instance.PortList[item.ComName.ToUpper()].DataReceived += ListenInViewModel_DataReceived;

                }
            }
        }
        List<byte> ReceiveData = new List<byte>();
        Dictionary<string, List<byte>> RData = new Dictionary<string, List<byte>>()
        {
            {"1",new List<byte>() },
            {"2",new List<byte>() },
            {"3",new List<byte>() },
            {"4",new List<byte>() },
            {"5",new List<byte>() },
            {"6",new List<byte>() },
            {"7",new List<byte>() },
            {"8",new List<byte>() },
            {"9",new List<byte>() },
            {"10",new List<byte>() },
            {"11",new List<byte>() },
            {"12",new List<byte>() }
        };
        public static ConcurrentQueue<byte[]> cqueue = new ConcurrentQueue<byte[]>();
        private void ListenInViewModel_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                var port = sender as SerialPort;               
                lock (port)
                {
                    string data = string.Empty;
                    string res = string.Empty;
                    var dataArray = port.PortName.Substring(3).ToCharArray();
                    byte[] buffer1;
                    if (port.PortName.Substring(3) == "3")
                    {                        
                        if (port.BytesToRead > 0)
                        {
                            buffer1 = new byte[port.BytesToRead];
                            port.Read(buffer1, 0, buffer1.Length);
                            
                            
                            ReceiveData.AddRange(buffer1);
                            var Result = MeterInfoDataPack376.Instance.TryPackData(ReceiveData.ToArray());
                            if (Result != DataPackMetaData.Null)
                            {
                                byte[] resbytes= ReceiveData.Skip((int)Result.StartIndex).Take((int)(Result.Length)).ToArray();
                                data = "0" + ";" + "9" + ";" + BitConverter.ToString(resbytes).Replace("-", "");
                                res = $"cmd=1007,ret=0,data={data}";
                                var str = string.Empty;
                                foreach (byte b in resbytes)
                                {
                                    str += b.ToString("X2");
                                }
                                LogService.Instance.Info(port.PortName + " 上报:" + str);
                                ReceiveData.RemoveRange(0, (int)Result.StartIndex+ (int)Result.Length);
                            }
                        }
                    }
                    else if (port.PortName.EndsWith("1"))
                    {
                        if (port.BytesToRead > 0)
                        {
                            buffer1 = new byte[port.BytesToRead];
                            port.Read(buffer1, 0, buffer1.Length);
                            var str = string.Empty;
                            foreach (byte b in buffer1)
                            {
                                str += b.ToString("X2");
                            }
                            LogService.Instance.Info(port.PortName + " 上报:" + str);
                            var key=string.Empty;
                            if (port.PortName.Length==5)
                            {
                                key = port.PortName.Substring(3, 1);
                            }
                            else if(port.PortName.Length == 6)
                            {
                                key = port.PortName.Substring(3, 2);
                            }
                            RData[key].AddRange(buffer1);
                            var Result = MeterInfoDataPack.Instance.TryPackData(RData[key].ToArray());
                            if (Result != DataPackMetaData.Null)
                            {
                                byte[] resbytes = RData[key].Skip((int)Result.StartIndex).Take((int)(Result.Length)).ToArray();
                                data = key + ";" + "1" + ";" + BitConverter.ToString(resbytes).Replace("-", "");
                                res = $"cmd=1007,ret=0,data={data}";
                                RData[key].RemoveRange(0, (int)Result.StartIndex + (int)Result.Length);
                            }
                        }
                    }
                    else
                    {
                        if (port.BytesToRead > 0)
                        {
                            buffer1 = new byte[port.BytesToRead];
                            port.Read(buffer1, 0, buffer1.Length);
                            var str = string.Empty;
                            foreach (byte b in buffer1)
                            {
                                str += b.ToString("X2");
                            }
                            LogService.Instance.Info(port.PortName + " 上报:" + str);
                        }
                        else
                        {
                            return;
                        }
                        if (buffer1.Length < 5)
                        {
                            return;
                        }
                        data = dataArray[0] + ";" + dataArray[1] + ";" + BitConverter.ToString(buffer1).Replace("-", "");
                        res = $"cmd=1007,data={data}";
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(res);
                    if (buffer.Length<5)
                    {
                        return;
                    }
                    cqueue.Enqueue(buffer);
                    port.DiscardInBuffer();
                }
            }
        }

        private void SendUDPMsgBack(IPAddress remoteIP, int remotePort, AbstractCmd res)
        {
            try
            {
                string sendstr = res.ToString();
                byte[] data = Encoding.UTF8.GetBytes(sendstr);
                //发送信息
                UDPSrv.SendData(data, new IPEndPoint(remoteIP, remotePort));
            }
            catch (Exception ex)
            {
                ShowSendMsg("发送信息失败！" + ex.Message);
            }
        }
        public void DealWidthRequest(AbstractCmd cmd, IPAddress remoteIP, int remotePort)
        {
            string strcmd = cmd.ToString();
            try
            {
                AbstractCmd res;
                ICmdAnalyse analyse = AnalyseFactory.Instance.GetCmdAnalyse(cmd.cmd);
                try
                {
                    
                    res = analyse.GetResponseCmd(cmd);
                    if (cmd.cmd == "1007")
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("处理命令失败！");
                    res = new ResponseCmd(cmd.cmd,  1, ex.Message);
                }
                SendUDPMsgBack(remoteIP, remotePort, res);
            }
            catch (Exception ex)
            {
                log.Error("处理命令失败！");
                try
                {
                    AbstractCmd abstractCmd = new ResponseCmd(cmd.cmd, 1, "server error:" + ex.Message);
                    byte[] data = Encoding.UTF8.GetBytes(abstractCmd.ToString());
                    UDPSrv.SendData(data, new IPEndPoint(remoteIP, remotePort));
                }
                catch
                {
                    log.Error("处理命令失败2！");
                }
            }
        }
        #endregion
        public ListenInViewModel(IEventAggregator eventAggregator, IContainerExtension container)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
            CmdList = this.container.Resolve<DataSourceThreePhase>().GetData();
            this.eventAggregator.GetEvent<RcvEvent>().Subscribe(ShowRcvMsg);
            this.eventAggregator.GetEvent<SendEvent>().Subscribe(ShowSendMsg);
            SCmd = "cmd=0101,data=null";
            client = new Socket(SocketType.Dgram, ProtocolType.Udp);
            serverip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);
            clientip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21000);//TODO:界面输入
            client.Bind(clientip);
            defaultschemeid = Convert.ToInt32(ConfigHelper.GetValue("defaultschemeid"));
            PowerHelper.Std_consant= Convert.ToDouble(ConfigHelper.GetValue("edtFreq"));
            log = this.container.Resolve<ILogService>();
            listenInBll = this.container.Resolve<ListenInBll>();
            comList = listenInBll.GetComs();
            Start();
            
        }
    }
}
