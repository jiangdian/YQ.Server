using YQ.FunctionModule.Bll;

namespace YQ.FunctionModule.ViewModels
{
    internal class SerialPortSettingViewModel: BindableBase
    {
        private IContainerExtension container;
        private SerialPortSettingBll serialPortSettingBll;
        /// <summary>
        /// SerialPort列表
        /// </summary>
        private List<ComSet> dataList;
        public List<ComSet> DataList
        {
            get { return dataList; }
            set { SetProperty(ref dataList, value); }
        }
        /// <summary>
        /// 保存
        /// </summary>
        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand =>
            _SaveCommand ?? (_SaveCommand = new DelegateCommand(Save));
        private void Save()
        {
            dataList = serialPortSettingBll.GetComSets();
        }
        public SerialPortSettingViewModel(IContainerExtension container)
        {
            this.container = container;
            serialPortSettingBll = this.container.Resolve<SerialPortSettingBll>();
            dataList = serialPortSettingBll.GetComSets();
        }
    }
}
