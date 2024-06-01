using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YQ.FunctionModule.Views
{
    /// <summary>
    /// ListenIn.xaml 的交互逻辑
    /// </summary>
    public partial class ListenIn : UserControl
    {
        private static readonly object oRcvLock = new object();
        private static readonly object oSendLock = new object();
        private ILogService log;
        public ListenIn(IEventAggregator eventAggregator, IContainerExtension container)
        {
            InitializeComponent();
            eventAggregator.GetEvent<RcvEvent>().Subscribe(ShowRcvMsg);
            eventAggregator.GetEvent<SendEvent>().Subscribe(ShowSendMsg);
            eventAggregator.GetEvent<ClearEvent>().Subscribe(Clear);
            log = container.Resolve<ILogService>();
        }
        private void ShowSendMsg(string msg)
        {
            lock (oSendLock)
            {
                rtxtSrvSend.BeginInvoke(() =>
                {
                    if (rtxtSrvSend.Document.Blocks.Count > 100)
                    {
                        rtxtSrvSend.Document.Blocks.Clear();
                        rtxtSrvSend.AppendText(Environment.NewLine);
                    }
                    rtxtSrvSend.AppendText(DateTime.Now.ToString("HH:mm:ss.fff ") + msg + Environment.NewLine);
                    rtxtSrvSend.ScrollToEnd();
                });
            }
            log.Info(msg);
        }
        private void ShowRcvMsg(string msg)
        {
            //lock (oRcvLock)
            //{
                rtxtSrvRcv.BeginInvoke(new Action(() =>
                {
                    if (rtxtSrvRcv.Document.Blocks.Count > 100)
                    {
                        rtxtSrvRcv.Document.Blocks.Clear();
                        rtxtSrvRcv.AppendText(Environment.NewLine);
                    }
                    rtxtSrvRcv.AppendText(DateTime.Now.ToString("HH:mm:ss.fff ") + msg + Environment.NewLine);
                    rtxtSrvRcv.ScrollToEnd();
                }));
            //}
            log.Info(msg);
        }
        private void Clear()
        {
            rtxtSrvRcv.Document.Blocks.Clear();
            rtxtSrvRcv.AppendText(Environment.NewLine);
            rtxtSrvSend.Document.Blocks.Clear();
            rtxtSrvSend.AppendText(Environment.NewLine);

        }
    }
}
