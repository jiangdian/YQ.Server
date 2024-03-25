using Prism.Commands;
using Prism.Mvvm;
using System.Configuration;
using YQ.UI.Views;

namespace YQ.UI.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        private string _title = string.Empty;
        public string STitle
        {
            get
            {
                return _title;
            }
            set
            {
                SetProperty(ref _title, value);
            }
        }
        private DelegateCommand _About;
        public DelegateCommand About =>
            _About ?? (_About = new DelegateCommand(AboutCommand));

        void AboutCommand()
        {
            AboutWindow aboutWindow = new AboutWindow();//this.Top,this.Left 作用是将当前父窗体的位置传给子窗体            
            aboutWindow.ShowDialog();
        }
        public MainWindowViewModel()
        {
            STitle = ConfigurationManager.AppSettings["AppName"].ToString();
        }
    }
}
