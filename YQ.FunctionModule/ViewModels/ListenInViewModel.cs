using SECS.Parsing;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Metrics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Xml.Linq;
using YQ.FreeSQL.Entity;
using YQ.FunctionModule.Bll;
using YQ.FunctionModule.Common;
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
        private List<ComSet> comList = new List<ComSet>();
        /// <summary>
        /// combox下拉框内容
        /// </summary>
        private List<CmdData> cmdList;
        public List<CmdData> CmdList
        {
            get { return cmdList; }
            set { SetProperty(ref cmdList, value); }
        }

        private ObservableCollection<Msgs> rcvmsg=new ObservableCollection<Msgs>();
        public ObservableCollection<Msgs> Rcvmsg
        {
            get { return rcvmsg; }
            set { SetProperty(ref rcvmsg, value); }
        }

        private ObservableCollection<Msgs> sendmsg = new ObservableCollection<Msgs>();
        public ObservableCollection<Msgs> Sendmsg
        {
            get { return sendmsg; }
            set { SetProperty(ref sendmsg, value); }
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
            if (sender != null)
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
            //this.eventAggregator.GetEvent<SendEvent>().Publish("启动成功!");
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
                    //this.eventAggregator.GetEvent<SendEvent>().Publish("已停止!");
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
            Rcvmsg.Clear();
            Sendmsg.Clear();
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

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (Rcvmsg.Count > 100)
                    {
                        Rcvmsg.Clear();
                        GC.Collect();
                    }
                    Rcvmsg.Insert(0, new Msgs { Message = DateTime.Now.ToString("HH:mm:ss.fff ") + msg });
                });
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (Sendmsg.Count > 100)
                    {
                        //TextSend = TextSend.Substring(TextSend.IndexOf(Environment.NewLine, 2000) + 2);
                        Sendmsg.Clear();
                        GC.Collect();
                    }

                    Sendmsg.Insert(0, new Msgs { Message = DateTime.Now.ToString("HH:mm:ss.fff ") + msg });
                });
               
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

                //ShowSendMsg("应答:" + msg);
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
            //this.eventAggregator.GetEvent<RcvEvent>().Publish("接收:" + rcvmsg.ToString());
            //ShowRcvMsg("接收:" + cmd_rcv.ToString());
            ReceiveData receiveData = new ReceiveData(cmd_rcv.cmd, cmd_rcv, remote.Address, remote.Port);
            Remote = remote;
            IsC = true;
            //if (defaultschemeid == -1)
            //{
                //var ldata = queue.Where(t => t.id == cmd_rcv.cmd&&t.abstractCmd.data==cmd_rcv.data);//判断重复命令的队列
                //if (ldata.Count() == 0)
                //{
                queue.Enqueue(receiveData);
                //}
            //}
            //else
            //{
            //    if (queue.Count == 0)//只处理一条命令的队列
            //    {
            //        queue.Enqueue(receiveData);
            //    }
            //    else
            //    {
            //        Thread.Sleep(1551);
            //        SendUDPMsgBack(receiveData.RemoteIP, receiveData.RemotePort, new ResponseCmd(cmd_rcv.cmd, 1, "busy"));
            //    }
            //}
        }

        /// <summary>
        /// 从检测软件收到数据
        /// </summary>
        private void DoUdpData()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    ReceiveData receiveDat = queue.Peek();
                    Task.Run(() => {
                        ShowRcvMsg("接收:" + receiveDat.abstractCmd.ToString());
                        DealWidthRequest(receiveDat.abstractCmd, receiveDat.RemoteIP, receiveDat.RemotePort);
                    });
                    queue.Dequeue();
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// UDP发送
        /// </summary>
        private void cDoUdpData()
        {
            while (true)
            {
                if (cqueue.Count > 0 && IsC)
                {
                    var bbb = cqueue.TryPeek(out byte[] receiveDat);
                    if (bbb)
                    {
                        UDPSrv?.SendData(receiveDat, Remote);
                        cqueue.TryDequeue(out byte[] receiveDat1);
                        Thread.Sleep(1);
                    }
                }
            }
        }

        /// <summary>
        /// 计算byte数组中的bit数
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static long CountBits(byte[] bytes)
        {
            long totalBits = bytes.Length * 8; // 计算总的bit数（byte数组长度乘以8）

            if (bytes.Length > 0)
            {
                int remainingBits = 8 - (bytes.Length - 1) % 8; // 计算最后一个byte的剩余bit数
                totalBits -= remainingBits < 8 ? remainingBits : 0; // 如果剩余的bit数小于8，则从总bit数中减去
            }

            return totalBits;
        }


        private void ListionCom()
        {
            var list = comList;
            foreach (var item in list)
            {
                var com = new ComParamter(item.ComName + "-" + item.ComPara);
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
                            var str1 = string.Empty;

                            ReceiveData.AddRange(buffer1);
                            foreach (byte b in ReceiveData.ToArray())
                            {
                                str1 += b.ToString("X2");
                            }
                            LogService.Instance.Info(port.PortName + " 上报#:" + str1);
                            var Result = MeterInfoDataPack376.Instance.TryPackData(ReceiveData.ToArray());
                            while (Result != DataPackMetaData.Null)
                            {
                                byte[] resbytes = ReceiveData.Skip((int)Result.StartIndex).Take((int)(Result.Length)).ToArray();
                                data = "0" + ";" + "9" + ";" + BitConverter.ToString(resbytes).Replace("-", "");
                                res = $"cmd=1007,ret=0,data={data}";
                                var str = string.Empty;
                                foreach (byte b in resbytes)
                                {
                                    str += b.ToString("X2");
                                }
                                LogService.Instance.Info(port.PortName + " 上报:" + str);
                                ReceiveData.RemoveRange(0, (int)Result.StartIndex + (int)Result.Length);
                                Result = MeterInfoDataPack376.Instance.TryPackData(ReceiveData.ToArray());
                            }
                            //if (Result != DataPackMetaData.Null)
                            //{
                            //    byte[] resbytes = ReceiveData.Skip((int)Result.StartIndex).Take((int)(Result.Length)).ToArray();
                            //    data = "0" + ";" + "9" + ";" + BitConverter.ToString(resbytes).Replace("-", "");
                            //    res = $"cmd=1007,ret=0,data={data}";
                            //    var str = string.Empty;
                            //    foreach (byte b in resbytes)
                            //    {
                            //        str += b.ToString("X2");
                            //    }
                            //    LogService.Instance.Info(port.PortName + " 上报:" + str);
                            //    ReceiveData.RemoveRange(0, (int)Result.StartIndex + (int)Result.Length);
                            //}
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
                            var key = string.Empty;
                            if (port.PortName.Length == 5)
                            {
                                key = port.PortName.Substring(3, 1);
                            }
                            else if (port.PortName.Length == 6)
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
                        if (buffer1.Length < 5 || buffer1.Length > 256)
                        {
                            return;
                        }
                        if(dataArray.Length==3)
                        {
                            data = dataArray[0].ToString()+ dataArray[1].ToString() + ";" + dataArray[2] + ";" + BitConverter.ToString(buffer1).Replace("-", "");
                        }
                        else
                        {
                            data = dataArray[0] + ";" + dataArray[1] + ";" + BitConverter.ToString(buffer1).Replace("-", "");
                        }

                        
                        res = $"cmd=1007,data={data}";
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(res);
                    if (buffer.Length < 5)
                    {
                        return;
                    }
                    cqueue.Enqueue(buffer);
                    //port.DiscardInBuffer();
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
                //this.eventAggregator.GetEvent<SendEvent>().Publish("发送信息失败！" + ex.Message);

                ShowSendMsg("发送信息失败！" + ex.Message);
            }
        }
        public void DealWidthRequest(AbstractCmd cmd, IPAddress remoteIP, int remotePort)
        {
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
                    res = new ResponseCmd(cmd.cmd, 1, ex.Message);
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
            //this.eventAggregator.GetEvent<RcvEvent>().Subscribe(ShowRcvMsg);
            //this.eventAggregator.GetEvent<SendEvent>().Subscribe(ShowSendMsg);
            SCmd = "cmd=0101,data=null";
            client = new Socket(SocketType.Dgram, ProtocolType.Udp);
            serverip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);
            clientip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21000);//TODO:界面输入
            client.Bind(clientip);
            defaultschemeid = Convert.ToInt32(ConfigHelper.GetValue("defaultschemeid"));
            PowerHelper.Std_consant = Convert.ToDouble(ConfigHelper.GetValue("edtFreq"));
            log = this.container.Resolve<ILogService>();
            listenInBll = this.container.Resolve<ListenInBll>();
            comList = listenInBll.GetComs();
            Start();
            //List<byte> bytes = new List<byte>();
            //var ss = "685A008314000100004C090301052420000301052420F10100033C00683A00C32602090301052420A07847900120CCDC080E8C2FFB1605A5033DEDD3303CA44F16D301B7F543E75A7A9578EFDE0F01000429B16C5B1D04169816685E008314000100004D080301052420000301052420F10100034000FEFEFEFE683A00C32602080301052420A0ADD8900120D3D10BBCD7F1D6FEB2F83E2648DACC0895B26B227025F0F213078EED393171730100048FD9ADB00CB6164716";
            //for (int i = 0; i < ss.Length; i += 2)
            //{
            //    byte b = Convert.ToByte(ss.Substring(i, 2), 16);
            //    bytes.Add(b);
            //}
            //var Result = MeterInfoDataPack376.Instance.TryPackData(bytes.ToArray());
            //while (Result != DataPackMetaData.Null)
            //{
            //    MessageBox.Show(Result.ToString());
            //    bytes.RemoveRange(0, (int)Result.StartIndex + (int)Result.Length);
            //    Result = MeterInfoDataPack376.Instance.TryPackData(bytes.ToArray());
            //}

            //Task.Run(async () =>
            //{
            //    await Task.Delay(5000);
            //    int i = 1001;
            //    AbstractCmd cmd_rcv = new RequestCmd("cmd=1007,ret=0,data=8;1;68470043260201000000000010294707010148227f000101020412000116021a07e804120301020203110911000101020216041001f40203110911000101020216041001f400ecc716");
            //    //this.eventAggregator.GetEvent<RcvEvent>().Publish("接收:" + rcvmsg.ToString());
            //    //ShowRcvMsg("接收:" + cmd_rcv.ToString());
            //    ReceiveData receiveData = new ReceiveData(cmd_rcv.cmd, cmd_rcv, clientip.Address, 10000);
            //    while (i > 0)
            //    {
            //        queue.Enqueue(receiveData);
            //        //ShowRcvMsg("接收:" + "cmd = 1007, ret = 0, data = 8; 1; 68470043260201000000000010294707010148227f000101020412000116021a07e804120301020203110911000101020216041001f40203110911000101020216041001f400ecc716");
            //        i--;
            //        //await Task.Delay(1);
            //    }
            //});
            //Task.Run(async () =>
            //{
            //    await Task.Delay(5000);
            //    int i = 100000;
            //byte[] data = Encoding.UTF8.GetBytes("cmd = 1007, ret = 0, data = 8; 1; 68470043260201000000000010294707010148227f000101020412000116021a07e804120301020203110911000101020216041001f40203110911000101020216041001f400ecc716");

            //    while (i > 0)
            //    {
            //        //ShowSendMsg("发送:" + "cmd = 1007, ret = 0, data = 8; 1; 68470043260201000000000010294707010148227f000101020412000116021a07e804120301020203110911000101020216041001f40203110911000101020216041001f400ecc716");
            //       cqueue.Enqueue(data);
            //        i--;
            //        await Task.Delay(1);

            //    }
            //});
        }
    }
}
