namespace YQ.FunctionModule.Common
{
    [Serializable]
    public class ReceiveData
    {
        public string id { set; get; }
        public AbstractCmd abstractCmd { set; get; }

        public IPAddress RemoteIP { set; get; }
        public int RemotePort { set; get; }
        public ReceiveData(string _id, AbstractCmd _abstractCmd, IPAddress ip, int port)
        {
            this.id = _id;
            this.abstractCmd = _abstractCmd;
            this.RemoteIP = ip;
            this.RemotePort = port;
        }

    }
}
