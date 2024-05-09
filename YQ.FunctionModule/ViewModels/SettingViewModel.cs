using System.Windows.Media.TextFormatting;

namespace YQ.FunctionModule.ViewModels
{
    public class SettingViewModel: BindableBase
    {
        private string _MeterNum;
        public string MeterNum
        {
            get { return _MeterNum; }
            set { SetProperty(ref _MeterNum, value); }
        }
        private string _ComParamter;
        public string ComParamter
        {
            get { return _ComParamter; }
            set { SetProperty(ref _ComParamter, value); }
        }
        private string _GHCom;
        public string GHCom
        {
            get { return _GHCom; }
            set { SetProperty(ref _GHCom, value); }
        }
        private string _PowerStyle;
        public string PowerStyle
        {
            get { return _PowerStyle; }
            set { SetProperty(ref _PowerStyle, value); }
        }
        private string _defaultschemeid;
        public string defaultschemeid
        {
            get { return _defaultschemeid; }
            set { SetProperty(ref _defaultschemeid, value); }
        }
        private string _edtFreq;
        public string edtFreq
        {
            get { return _edtFreq; }
            set { SetProperty(ref _edtFreq, value); }
        }
        private string _MeterUb;
        public string MeterUb
        {
            get { return _MeterUb; }
            set { SetProperty(ref _MeterUb, value); }
        }
        private string _MeterFreq;
        public string MeterFreq
        {
            get { return _MeterFreq; }
            set { SetProperty(ref _MeterFreq, value); }
        }

        private string _JY1IP;
        public string JY1IP
        {
            get { return _JY1IP; }
            set { SetProperty(ref _JY1IP, value); }
        }
        private string _JY1Port;
        public string JY1Port
        {
            get { return _JY1Port; }
            set { SetProperty(ref _JY1Port, value); }
        }
        private string _JY1Addr;
        public string JY1Addr
        {
            get { return _JY1Addr; }
            set { SetProperty(ref _JY1Addr, value); }
        }
        private string _JY1Num;
        public string JY1Num
        {
            get { return _JY1Num; }
            set { SetProperty(ref _JY1Num, value); }
        }

        private string _JY2IP;
        public string JY2IP
        {
            get { return _JY2IP; }
            set { SetProperty(ref _JY2IP, value); }
        }
        private string _JY2Port;
        public string JY2Port
        {
            get { return _JY2Port; }
            set { SetProperty(ref _JY2Port, value); }
        }
        private string _JY2Addr;
        public string JY2Addr
        {
            get { return _JY2Addr; }
            set { SetProperty(ref _JY2Addr, value); }
        }
        private string _JY2Num;
        public string JY2Num
        {
            get { return _JY2Num; }
            set { SetProperty(ref _JY2Num, value); }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand =>
            _SaveCommand ?? (_SaveCommand = new DelegateCommand(Save));
        private void Save()
        {
            ConfigHelper.SetValue("MeterNum", MeterNum);
            ConfigHelper.SetValue("ComParamter", ComParamter);
            ConfigHelper.SetValue("GHCom", GHCom);
            ConfigHelper.SetValue("PowerStyle", PowerStyle);
            ConfigHelper.SetValue("defaultschemeid", defaultschemeid);
            ConfigHelper.SetValue("edtFreq", edtFreq);
            ConfigHelper.SetValue("MeterUb", MeterUb);
            ConfigHelper.SetValue("MeterFreq", MeterFreq);
            ConfigHelper.SetValue("JY1IP", JY1IP);
            ConfigHelper.SetValue("JY1Port", JY1Port);
            ConfigHelper.SetValue("JY1Addr", JY1Addr);
            ConfigHelper.SetValue("JY1Num", JY1Num);
            ConfigHelper.SetValue("JY2IP", JY2IP);
            ConfigHelper.SetValue("JY2Port", JY2Port);
            ConfigHelper.SetValue("JY2Addr", JY2Addr);
            ConfigHelper.SetValue("JY2Num", JY2Num);
        }

        public SettingViewModel()
        {
            MeterNum=ConfigHelper.GetValue("MeterNum");
            ComParamter = ConfigHelper.GetValue("ComParamter");
            GHCom = ConfigHelper.GetValue("GHCom");
            PowerStyle = ConfigHelper.GetValue("PowerStyle");
            defaultschemeid = ConfigHelper.GetValue("defaultschemeid");
            edtFreq = ConfigHelper.GetValue("edtFreq");
            MeterUb = ConfigHelper.GetValue("MeterUb");
            MeterFreq = ConfigHelper.GetValue("MeterFreq");
            JY1IP = ConfigHelper.GetValue("JY1IP");
            JY1Port = ConfigHelper.GetValue("JY1Port");
            JY1Addr = ConfigHelper.GetValue("JY1Addr");
            JY1Num = ConfigHelper.GetValue("JY1Num");
            JY2IP = ConfigHelper.GetValue("JY2IP");
            JY2Port = ConfigHelper.GetValue("JY2Port");
            JY2Addr = ConfigHelper.GetValue("JY2Addr");
            JY2Num = ConfigHelper.GetValue("JY2Num");
        }    
    }
}
