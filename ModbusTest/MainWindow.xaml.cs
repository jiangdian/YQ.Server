using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using YQ.Tool;
using YQ.Tool.JY;

namespace ModbusTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            InitCom();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var sdata= code.Text;
            if (sdata.Length % 2 != 0)
            {
                sdata.PadLeft(sdata.Length + 1, '0');
            }
            List<byte> data = new List<byte>();
            for (int i = 0; i < sdata.Length; i += 2)
            {
                byte b = Convert.ToByte(sdata.Substring(i, 2), 16);
                data.Add(b);
            }
            //var data = Encoding.UTF8.GetBytes(code.Text);
            //var crc = CMBRTU.CalculateCrc(data.ToArray(), data.Count());
            var crc = CalculateCrc(data.ToArray(), 0,data.Count());
            var buffer1=BitConverter.GetBytes(crc);
            var str = string.Empty;
            foreach (byte b in buffer1)
            {
                str += b.ToString("X2");
            }
            Crc.Text = str;
        }
        private static ushort CalculateCrc(byte[] data, int start, int length)
        {
            ushort crc = 0xFFFF;

            for (int i = start; i < start + length; i++)
            {
                crc ^= data[i];

                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            return crc;
        }
        private void InitCom()
        {
            var com = new ComParamter("COM1-9000-n-8-1");
            SerialManager.Instance.CreateAndOpenPort(com);
            if (SerialManager.Instance.PortList.ContainsKey("COM1"))
            {
                SerialManager.Instance.PortList["COM1"].DataReceived += ListenInViewModel_DataReceived;

            }
        }
        List<byte> ReceiveData = new List<byte>();
        private void ListenInViewModel_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                SerialPort port = sender as SerialPort;
                byte[] buffer = new byte[port.BytesToRead];
                port.Read(buffer, 0, buffer.Length);
                ReceiveData.AddRange(buffer);
                if (ReceiveData.Count<5)
                {
                    return;
                }
                else
                {
                    var Result = ModbusDataPack.Instance.TryPackData(ReceiveData.ToArray());
                    if (Result != DataPackMetaData.Null)
                    {
                        byte[] resbytes = ReceiveData.Skip((int)Result.StartIndex).Take((int)(Result.Length)).ToArray();
                        ReceiveData.RemoveRange(0, (int)Result.StartIndex + (int)Result.Length);
                        var str = string.Empty;
                        foreach (byte b in resbytes)
                        {
                            str += b.ToString("X2");
                        }
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            code.Text = str;
                        }));
                        
                    }                    
                }              
            }
        }
    }
}
