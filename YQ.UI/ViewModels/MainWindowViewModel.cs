using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using YQ.Tool;
using YQ.Tool.JY;
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
        private string _DianYa = string.Empty;
        public string DianYa
        {
            get
            {
                return _DianYa;
            }
            set
            {
                SetProperty(ref _DianYa, value);
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
        private DelegateCommand _ClosedCommand;
        public DelegateCommand ClosedCommand =>
            _ClosedCommand ?? (_ClosedCommand = new DelegateCommand(Closed));

        void Closed()
        {
            Task.Run(() => {
                PowerHelper.IsPowering = true;
                PowerHelper.Power_Off(1);//一般情况下的升降源
                CloseLight();
                PowerHelper.IsPowering = false;
            });
        }
        private void CloseLight()
        {
            //JYHelper jYHelper = new JYHelper();
            JYHelper jYHelper2 = new JYHelper();
            if (jYHelper2.JYConnet(ConfigHelper.GetValue("JY2IP"), Convert.ToInt32(ConfigHelper.GetValue("JY2Port"))))
            {
                //jYHelper.CloseAllDO(Convert.ToInt32(ConfigHelper.GetValue("JY1Addr")), Convert.ToInt32(ConfigHelper.GetValue("JY1Num")));
                jYHelper2.CloseAllDO(Convert.ToInt32(ConfigHelper.GetValue("JY2Addr")), Convert.ToInt32(ConfigHelper.GetValue("JY2Num")));
            }
            PowerHelper.HangPos?.Clear();
        }
        private void ReadDY()
        {
            Task.Run(async () => {
                await Task.Delay(5000);
                while (true)
                {
                    await Task.Delay(2000);
                    double U1;
                    CommCtr.FDevComm.GetU(out U1, out double U2, out double U3, false);
                    DianYa = U1.ToString("F5");
                }
            });
        }
        public MainWindowViewModel()
        {
            STitle = ConfigurationManager.AppSettings["AppName"].ToString();
            ReadDY();
        }
    }
}
