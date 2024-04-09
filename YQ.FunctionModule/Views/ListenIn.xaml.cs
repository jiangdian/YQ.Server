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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YQ.FunctionModule.Views
{
    /// <summary>
    /// ListenIn.xaml 的交互逻辑
    /// </summary>
    public partial class ListenIn : UserControl
    {
        public ListenIn()
        {
            InitializeComponent();
        }

        private void rtxtSrvRcv_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtxtSrvRcv.ScrollToEnd();
        }

        private void rtxtSrvSend_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtxtSrvSend.ScrollToEnd();
        }
    }
}
