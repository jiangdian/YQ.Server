using MahApps.Metro.Controls;
using System.Configuration;
using System.Windows.Media.Animation;

namespace YQ.UI.Views
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
            tbVer.Text = ConfigurationManager.AppSettings["Version"].ToString();
        }
    }
}
