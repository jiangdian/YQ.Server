using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using YQ.Tool.UDP;
using System.Threading;
using YQ.Tool;
using System.Collections.Concurrent;

namespace UdpTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            client = new Socket(SocketType.Dgram, ProtocolType.Udp);
            clientip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);//TODO:界面输入
            client.Bind(clientip);
            serverip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10004);
            Start();
        }

        private Socket client;
        private IPEndPoint clientip;
        private IPEndPoint serverip;
        private UDPServer UDPSrv { get; set; }
        public IPEndPoint Remote=new IPEndPoint (IPAddress.Parse("127.0.0.1") ,10001);
        private static readonly object oListionLock = new object();
        public static Queue<byte[]> queue = new Queue<byte[]>();
        public static ConcurrentQueue<byte[]> cqueue = new ConcurrentQueue<byte[]>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //int i = Convert.ToInt32(cishu.Text);
            //while (i>0)
            //{
                Task.Run(() =>
                {
                    Send();
                });
            //    i--;
            //}
        }
        private void Start()
        {
            if (UDPSrv != null)
            {
                return;
            }

            UDPSrv = new UDPServer();
            UDPSrv.DataReceived += UDPSrv_DataReceived;
            UDPSrv.DataSended += UDPSrv_DataSended;
            UDPSrv.StartUDPServer();
            Task.Run(() =>
            {
                DoUdpData();
            });
            Task.Run(() =>
            {
                cDoUdpData();
            });
        }
        private void DoUdpData()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    byte[] receiveDat = queue.Peek();
                    Task.Run(() =>
                    {
                        //Thread.Sleep(1000);
                        UDPSrv?.SendData(receiveDat, serverip);
                    });

                    queue.Dequeue();
                    Thread.Sleep(1);
                }
            }
        }
        private void cDoUdpData()
        {
            while (true)
            {
                if (cqueue.Count > 0)
                {
                    var bbb  = cqueue.TryPeek(out byte[] receiveDat);
                    if (bbb)
                    {
                        Task.Run(() =>
                        {

                            UDPSrv?.SendData(receiveDat, serverip);
                        });

                        cqueue.TryDequeue(out byte[] receiveDat1);
                        Thread.Sleep(1);
                    }
                    
                }
            }
        }
        private bool UDPSrv_DataSended(byte[] data)
        {
            return true;
        }

        private void UDPSrv_DataReceived(byte[] data, IPEndPoint remote)
        {
            
        }
        private void Send()
        {
            int i = 10000;


            lock (oListionLock)
            {
                while (i > 0)
                {
                    var res = "681900c305000000000000a0725186010020001000f0000074516681900c305000000000000a0725186010020001000f0000";
                    byte[] buffer = Encoding.UTF8.GetBytes(res);
                    queue.Enqueue(buffer);
                    Thread.Sleep(5);
                    i--;
                }
            }
            
        }
        private Dictionary<int,object> dics=new Dictionary<int,object>();
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int ii = Convert.ToInt32(cishu.Text);
            //for (int i = 0; i < ii; i++)
            //{
            //    dics[i] = new  object();
            //}
            while (ii > 0)
            {
                Task.Run(() =>
                {
                    Send2(ii);
                });
                ii--;
                //Thread.Sleep(10);
            }
        }

        private void Send2(int i)
        {
            int ix = 1000;


            //lock (dics[i])
            //{
                while (ix > 0)
                {
                    var res = "681900c305000000000000a0725186010020001000f0000074516681900c305000000000000a0725186010020001000f0000";
                    byte[] buffer = Encoding.UTF8.GetBytes(res);
                //lock (oListionLock)
                //{
                    //UDPSrv?.SendData(buffer, serverip);
                    cqueue.Enqueue(buffer);
                ix--;
            //}
            
            }
            //}

        }
    }
}
