using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace JYTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private static readonly object _lock = new object();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                
                Task.Run(() =>//异步测试
                {
                    //lock (_lock)
                    //{
                        JYHelper.Instanse().OpenDO(254, i);
                       
                    //}
                    
                });
                Thread.Sleep(10);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //string[] BarCodes = {"123","12345" };
            //Array.Sort(BarCodes, (x, y) => y.Length.CompareTo(x.Length));  //数组排序测试
            JYHelper.Instanse().CloseAllDO(254,8);
        }
    }
}
