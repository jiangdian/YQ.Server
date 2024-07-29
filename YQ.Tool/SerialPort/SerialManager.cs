using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace YQ.Tool
{
    /// <summary>
    /// 项目持久化串口管理类
    /// </summary>
    public class SerialManager
    {
        #region private属性
        private static readonly object lockobj = new object();//线程锁
        private static SerialManager instance;
        private Dictionary<string, SerialPort> portList;
        private Dictionary<string, List<byte>> portReceiveData;
        public event Action<byte[]> portReceive;
        #endregion

        #region public属性

        /// <summary>
        /// 串口列表
        /// </summary>
        public Dictionary<string, SerialPort> PortList => portList;
        public Dictionary<string, List<byte>> PortReceiveData => portReceiveData;

        /// <summary>
        /// 串口管理类实例
        /// </summary>
        public static SerialManager Instance => GetInstance();

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        private SerialManager()
        {
            portList = new Dictionary<string, SerialPort>();
            portReceiveData = new Dictionary<string, List<byte>>();
            // ShengDiHelper.SetDev_Port((byte)LocalSeting.powercom);//config read
        }
        #endregion

        public static SerialManager GetInstance()
        {
            if (instance == null)
            {
                lock (lockobj)
                {
                    if (instance == null)
                    {
                        instance = new SerialManager();
                    }
                }
            }
            return instance;
        }

        public void ClosePorts()
        {
            lock (lockobj)
            {
                foreach (SerialPort port in this.PortList.Values)
                {
                    try
                    {
                        port.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
                this.PortList.Clear();
                this.PortReceiveData.Clear();
                instance = null;
            }
        }

        public void ClosePort(string portName)
        {
            lock (lockobj)
            {
                if (string.IsNullOrEmpty(portName))
                {
                    return;
                }
                portName = portName.ToUpper();
                try
                {
                    if (this.PortList.Keys.Contains(portName))
                    {
                        SerialPort port = this.PortList[portName];
                        port.Close();
                    }
                }
                catch (Exception)
                {
                }
                finally
                { 
                    try 
                    { 
                        this.PortList.Remove(portName); 
                        this.PortReceiveData.Remove(portName); 
                    } 
                    catch { } 
                }
            }
        }


        /// <summary>
        /// 创建并打开串口，或返回已创建的串口
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public SerialPort CreateAndOpenPort(ComParamter com)
        {
            return CreateAndOpenPort(com.PortName, com.BaudRate, com.Parity, com.DataBits, com.StopBits);
        }

        /// <summary>
        /// 创建并打开串口，或返回已创建的串口
        /// </summary>
        /// <param name="portNo">串口号</param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <returns></returns>
        /// <returns></returns>
        public SerialPort CreateAndOpenPort(int portNo, int baudRate = 2400, Parity parity = Parity.Even, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            return CreateAndOpenPort("COM" + portNo, baudRate, parity, dataBits, stopBits);
        }

        /// <summary>
        /// 创建并打开串口，或返回已创建的串口
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <returns></returns>
        public SerialPort CreateAndOpenPort(string portName, int baudRate = 2400, Parity parity = Parity.Even, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            if (string.IsNullOrEmpty(portName))
            {
                return null;
            }
            
            lock (lockobj)
            {
                SerialPort port;
                portName = portName.ToUpper();
                if (this.PortList.Keys.Contains(portName))
                {
                    port = this.PortList[portName];
                    if (port.BaudRate != baudRate || port.Parity != parity || port.DataBits != dataBits)
                    {
                        ClosePort(portName);
                        port = OpenOnePort(portName, baudRate, parity, dataBits, stopBits);
                    }
                }
                else
                {                   
                    //port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
                    //port.RtsEnable = false;
                    //try
                    //{
                    //    port.Open();
                    //}
                    //catch (Exception e)
                    //{
                    //    LogService.Instance.Error("创建串口失败！");
                    //    throw new Exception();
                    //    return null;
                    //}
                    //this.PortList.Add(portName, port);
                    port = OpenOnePort(portName, baudRate, parity, dataBits, stopBits);
                }
                return port;
            }
            
        }
        SerialPort OpenOnePort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            SerialPort port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            List<byte> ReceiveData = new List<byte>();
            port.ReadTimeout = 500;
            port.WriteTimeout = 500;
            try
            {
                port.Open();
                port.DataReceived += Port_DataReceived;
                portList.Add(portName, port);
                portReceiveData.Add(portName, ReceiveData);
                return port;
            }
            catch (Exception e)
            {
                LogService.Instance.Error($"打开{portName}失败", e);
                return null;
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                var port = sender as SerialPort;
                var receviceData = PortReceiveData[port.PortName.ToUpper()];
                lock (port)
                {
                    string data = string.Empty;
                    string res = string.Empty;
                    
                    byte[] buffer1;
                    if (port.BytesToRead > 0)
                    {
                        buffer1 = new byte[port.BytesToRead];
                        port.Read(buffer1, 0, buffer1.Length);
                        receviceData.AddRange(buffer1);
                        if (port.PortName.Substring(3) == "3")//cco  376.2
                        {
                            var Result = MeterInfoDataPack376.Instance.TryPackData(receviceData.ToArray());
                            while (Result != DataPackMetaData.Null)
                            {
                                byte[] resbytes = receviceData.Skip((int)Result.StartIndex).Take((int)(Result.Length)).ToArray();
                                data = "0" + ";" + "9" + ";" + BitConverter.ToString(resbytes).Replace("-", "");
                                res = $"cmd=1007,ret=0,data={data}";
                                receviceData.RemoveRange(0, (int)Result.StartIndex + (int)Result.Length);
                                Result = MeterInfoDataPack376.Instance.TryPackData(receviceData.ToArray());//循环校验
                            }
                        }
                        else if (port.PortName.EndsWith("1"))//485-1  698
                        {
                            var Result = MeterInfoDataPack.Instance.TryPackData(receviceData.ToArray());
                            if (Result != DataPackMetaData.Null)
                            {
                                var key = Regex.Replace(port.PortName, @"^.{3}|.$", "");//前三个字符、最后一个字符，替换为空
                                byte[] resbytes = receviceData.Skip((int)Result.StartIndex).Take((int)(Result.Length)).ToArray();
                                data = key + ";" + "1" + ";" + BitConverter.ToString(resbytes).Replace("-", "");
                                res = $"cmd=1007,ret=0,data={data}";
                                receviceData.RemoveRange(0, (int)Result.StartIndex + (int)Result.Length);
                            }
                        }
                        else//modbus，后新增645（485-2）
                        {
                            if (receviceData.ToArray().Length < 5)
                            {
                                return;
                            }
                            else if (receviceData.ToArray().Length > 256)
                            {
                                receviceData.Clear();
                                return;
                            }                                 
                            string[] parts = Regex.Split(port.PortName.Substring(3), @"^(.*)(.)$").Where(p => !string.IsNullOrEmpty(p)).ToArray();
                            if (receviceData.ElementAtOrDefault(0) == 0xFE|| receviceData.ElementAtOrDefault(0) == 0x68)
                            {
                                var Result = MeterInfoDataPack645.Instance.TryPackData(receviceData.ToArray());
                                if (Result != DataPackMetaData.Null)
                                {
                                    byte[] resbytes = receviceData.Skip((int)Result.StartIndex).Take((int)(Result.Length)).ToArray();
                                    data = parts[0] + ";" + parts[1] + ";" + BitConverter.ToString(resbytes).Replace("-", "");
                                    res = $"cmd=1007,ret=0,data={data}";
                                    receviceData.RemoveRange(0, (int)Result.StartIndex + (int)Result.Length);
                                }
                            }
                            else
                            {
                                receviceData.Clear();
                                data = parts[0] + ";" + parts[1] + ";" + BitConverter.ToString(receviceData.ToArray()).Replace("-", "");
                                res = $"cmd=1007,data={data}";
                            }
                        }
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(res);
                    if (buffer.Length < 5)
                    {
                        return;
                    }
                    portReceive.Invoke(buffer);
                }
            }
        }

        /// <summary>
        /// 获取串口
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        private SerialPort GetPort(string portName)
        {
            if (string.IsNullOrEmpty(portName))
            {
                return null;
            }
            lock (lockobj)
            {
                portName = portName.ToUpper();
                SerialPort port;
                if (!string.IsNullOrEmpty(portName) && this.PortList.Keys.Contains(portName))
                {
                    port = this.PortList[portName];
                }
                else
                {
                    port = null;
                    LogService.Instance.Error($"串口{portName}未实例化!");
                }
                return port;
            }
        }

        /// <summary>
        /// 查询串口状态，如果关闭，尝试打开串口
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="trytimes">尝试次数，默认1次，等待时间：100ms/次 </param>
        /// <returns></returns>
        public bool IsOpen(string portName, int trytimes = 1)
        {
            if (string.IsNullOrEmpty(portName))
            {
                return false;
            }
            lock (lockobj)
            {
                bool rlt = false;
                do
                {
                    SerialPort port = GetPort(portName);
                    if (port == null)
                    {
                        return false;
                    }
                    rlt = port.IsOpen;
                    if (rlt)
                    {
                        break;
                    }
                    else
                    {
                        try
                        {
                            port.Open();
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                    trytimes--;
                } while (trytimes > 0);
                return rlt;
            }
        }
        /// <summary>
        /// 发送命令，并获取应答
        /// </summary>
        /// <param name="portNo">串口号，例如：1</param>
        /// <param name="sendData">发送数据</param>
        /// <param name="sendTimes">尝试发送次数</param>
        /// <param name="rcvTimes">每次发送后等待接收次数</param>
        /// <param name="rcvSleep">每次接收前等待时间</param>
        /// <param name="hopeRcvLen">期望得到的应答数据长度</param>
        /// <returns></returns>
        public byte[] SendCommand(int portNo, byte[] sendData, int sendTimes, int rcvTimes, int rcvSleep, int hopeRcvLen)
        {
            return SendCommand("COM" + portNo, sendData, sendTimes, rcvTimes, rcvSleep, hopeRcvLen);
        }

        /// <summary>
        /// 发送命令，并获取应答
        /// </summary>
        /// <param name="portName">串口名，例如：COM1</param>
        /// <param name="sendData">发送数据</param>
        /// <param name="sendTimes">尝试发送次数</param>
        /// <param name="rcvTimes">每次发送后等待接收次数</param>
        /// <param name="rcvSleep">每次接收前等待时间</param>
        /// <param name="hopeRcvLen">期望得到的应答数据长度</param>
        /// <returns></returns>
        public byte[] SendCommand(string portName, byte[] sendData, int sendTimes, int rcvTimes, int rcvSleep, int hopeRcvLen)
        {
            List<byte> rcvData = null;
            SerialPort port = this.GetPort(portName);
            if (rcvTimes <= 0)
            {
                rcvTimes = 1;
            }
            if (port.SendCommand(sendData, sendTimes, ref rcvData, rcvTimes, rcvSleep, hopeRcvLen) > 0)
            {
                return rcvData.ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 发送命令，并获取应答
        /// </summary>
        /// <param name="portName">串口名，例如：COM1</param>
        /// <param name="sendData">发送数据</param>
        /// <param name="rcvSleep">每次接收前等待时间</param>
        /// <returns></returns>
        public List<byte> SendData(string portName, List<byte> sendData, int rcvSleep)
        {
            List<byte> ReceiveData = new List<byte>();
            SerialPort port = this.GetPort(portName);
            Thread.Sleep(1);
            if (port == null)
            {
                return null;
            }
            lock (port)
            {
                port.DiscardInBuffer();
                port.DiscardOutBuffer();
                port.Write(sendData.ToArray(), 0, sendData.Count);
                {
                    string str = "";
                    foreach (byte b in sendData.ToArray())
                    {
                        str += b.ToString("X2");
                    }
                    LogService.Instance.Info(str);
                }
                
                //Thread.Sleep(rcvSleep);
                //if (port.BytesToRead > 0)
                //{
                //    byte[] buffer = new byte[port.BytesToRead];
                //    port.Read(buffer, 0, buffer.Length);
                //    ReceiveData.AddRange(buffer);
                //    return ReceiveData;
                //}
                DateTime dt = DateTime.Now;
                Thread.Sleep(100);
                while (DateTime.Now < dt.AddMilliseconds(rcvSleep))
                {
                    if (port.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[port.BytesToRead];
                        port.Read(buffer, 0, buffer.Length);
                        string str = "";
                        foreach(byte b in buffer){
                            str += b.ToString("X2");
                        }
                        //LogService.Instance.Info(str);
                        ReceiveData.AddRange(buffer);
                        var Result = MeterInfoDataPack.Instance.TryPackData(ReceiveData.ToArray());
                        if (Result != DataPackMetaData.Null)
                        {
                            return ReceiveData;
                        }
                        Thread.Sleep(100);
                    }
                }
                return null;
            }
        }
        public List<byte> SendData2(string portName, List<byte> sendData, int rcvSleep)
        {
            List<byte> ReceiveData = new List<byte>();
            SerialPort port = this.GetPort(portName);
            Thread.Sleep(1);
            if (port == null)
            {
                return null;
            }
            lock (port)
            {
                port.DiscardInBuffer();
                port.DiscardOutBuffer();
                port.Write(sendData.ToArray(), 0, sendData.Count);
                {
                    string str = "";
                    foreach (byte b in sendData.ToArray())
                    {
                        str += b.ToString("X2");
                    }
                    LogService.Instance.Info(str);
                }

                DateTime dt = DateTime.Now;
                Thread.Sleep(100);
                while (DateTime.Now < dt.AddMilliseconds(rcvSleep))
                {
                    if (port.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[port.BytesToRead];
                        port.Read(buffer, 0, buffer.Length);

                        //LogService.Instance.Info(str);
                        ReceiveData.AddRange(buffer);
                        return ReceiveData;                     
                    }
                }
                return null;
            }
        }
        public List<byte> SendData3(string portName, List<byte> sendData, int rcvSleep)
        {
            List<byte> ReceiveData = new List<byte>();
            SerialPort port = this.GetPort(portName);
            Thread.Sleep(1);
            if (port == null)
            {
                return null;
            }
            lock (port)
            {
                port.DiscardInBuffer();
                port.DiscardOutBuffer();
                port.Write(sendData.ToArray(), 0, sendData.Count);
                {
                    string str = "";
                    foreach (byte b in sendData.ToArray())
                    {
                        str += b.ToString("X2");
                    }
                    LogService.Instance.Info(str);
                }

                DateTime dt = DateTime.Now;
                Thread.Sleep(100);
                while (DateTime.Now < dt.AddMilliseconds(rcvSleep+ rcvSleep))
                {
                    if (port.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[port.BytesToRead];
                        port.Read(buffer, 0, buffer.Length);

                        ReceiveData.AddRange(buffer);
                        var Result = MeterInfoDataPack.Instance.TryPackData(ReceiveData.ToArray());
                        if (Result != DataPackMetaData.Null)
                        {
                            return ReceiveData;
                        }
                    }
                    Thread.Sleep(100);
                }
                return null;
            }
        }
        public List<byte> SendData1(string portName, List<byte> sendData, int rcvSleep)
        {
            List<byte> ReceiveData = new List<byte>();
            SerialPort port = this.GetPort(portName);
            if (port == null)
            {
                return null;
            }
            //lock (port)
            //{
                //port.DiscardInBuffer();
                //port.DiscardOutBuffer();
                port.Write(sendData.ToArray(), 0, sendData.Count);
                //{
                //    string str = "";
                //    foreach (byte b in sendData.ToArray())
                //    {
                //        str += b.ToString("X2");
                //    }
                //    LogService.Instance.Info(port.PortName+" Send:"+str);
                //}
                byte[] buffer = new byte[] { 1, 2 };
                ReceiveData.AddRange(buffer);
                return ReceiveData;
            //}
        }
        /// <summary>
        /// 发送命令，并获取接收信息，
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收信息</param> 
        /// <param name="receiveCount">接收信息尝试次数：每50毫秒接收一次信息</param> 
        /// <returns>接收数据的长度，串口未打开返回-1</returns>
        public  List<byte> SendByte(string portName, List<byte> SendData, int receiveCount)
        {
            SerialPort serialPort = this.GetPort(portName);
            List<byte> ReceiveData = new List<byte>();
            if (!serialPort.IsOpen)
            {
                return ReceiveData;
            }
            try
            {
                serialPort.DiscardInBuffer();//清空接收缓冲区
                serialPort.DiscardOutBuffer();
                serialPort.Write(SendData.ToArray(), 0, SendData.Count);
                while (receiveCount > 0)
                {
                    Thread.Sleep(50);
                    if (serialPort.BytesToRead > 0)
                    {
                        byte[] buffer = new byte[serialPort.BytesToRead];
                        serialPort.Read(buffer, 0, buffer.Length);
                        //serialPort.DiscardInBuffer();//清空接收缓冲区
                        ReceiveData.AddRange(buffer);
                    }
                    else
                    {
                        continue;
                    }
                    receiveCount--;
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error($"发送数据失败!{ex.Message}");
            }
            return ReceiveData;
        }

        ~SerialManager()
        {
            ClosePorts();
        }
    }
}
