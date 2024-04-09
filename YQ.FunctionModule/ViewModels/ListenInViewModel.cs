using SECS.Parsing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YQ.Parsing;
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
        private ILogService log;
        private Socket client;
        private IPEndPoint clientip;
        private IPEndPoint serverip;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public static Queue<ReceiveData> queue = new Queue<ReceiveData>();
        private int defaultschemeid;//判断重复列队 -1运行重复
        private UDPServer UDPSrv { get; set; }
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
            var com = new ComParamter(ConfigHelper.GetValue("ComParamter"));
            SerialManager.Instance.CreateAndOpenPort(com);
            var PowerCom=com.PortName.Substring(3);
            PowerHelper.SetDev_Port(Convert.ToByte(PowerCom));
            PowerHelper.InitFNParams();
            UDPSrv = new UDPServer();
            UDPSrv.DataReceived += UDPSrv_DataReceived;
            UDPSrv.DataSended += UDPSrv_DataSended;
            UDPSrv.StartUDPServer();
            ShowSendMsg("启动成功!");

            // thread = new Thread(() =>
            //{
            //    DoUdpData();
            //});
            //thread.IsBackground = true;
            //thread.Start();
            Task.Run(() =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    DoUdpData();
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
                if (TextRcv.Length > 2000)
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
                if (TextSend.Length > 2000)
                {
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

            if (defaultschemeid == -1)
            {
                var ldata = queue.Where(t => t.id == cmd_rcv.cmd);//判断重复命令的队列
                if (ldata.Count() == 0)
                {
                    queue.Enqueue(receiveData);
                }
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
                    DealWidthRequest(receiveDat.abstractCmd, receiveDat.RemoteIP, receiveDat.RemotePort);
                    queue.Dequeue();
                    Thread.Sleep(50);
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
            serverip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000);
            clientip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 21000);//TODO:界面输入
            client.Bind(clientip);
            defaultschemeid = Convert.ToInt32(ConfigHelper.GetValue("defaultschemeid"));
            PowerHelper.Std_consant= Convert.ToDouble(ConfigHelper.GetValue("edtFreq"));
            log = this.container.Resolve<ILogService>();
            Start();
        }
    }
}
