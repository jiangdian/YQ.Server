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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                JYHelper.Instanse().OpenDO(254, i);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string[] BarCodes = {"123","12345" };
            Array.Sort(BarCodes, (x, y) => y.Length.CompareTo(x.Length));
            //JYHelper.Instanse().CloseAllDO();
        }
    }
}
