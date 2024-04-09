using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace YQ.Tool
{
    public class SerialPortService
    {
        /// <summary>
        /// 创建并打开串口，或返回已创建的串口
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <returns></returns>
        public static SerialPort CreateAndOpenPort(ComParamter comParmater)
        {
            if (string.IsNullOrEmpty(comParmater.PortName))
            {
                return null;
            }
            SerialPort port = new SerialPort(comParmater.PortName, comParmater.BaudRate, comParmater.Parity, comParmater.DataBits, comParmater.StopBits);
            port.RtsEnable = false;
            try
            {
                port.Open();
            }
            catch (Exception e)
            {
                LogService.Instance.Error("创建串口失败！");
                return null;
            }
            return port;
        }
        public static void ClosePort(SerialPort port)
        {
            try
            {
                port.Close();
            }
            catch (Exception ex)
            {
                LogService.Instance.Error($"关闭串口失败!{port}");
            }
        }
        /// <summary>
        /// 向串口发送数据
        /// </summary> 
        /// <returns></returns>
        public static void SendData(ComParamter comParmater)
        {
            SerialPort port = CreateAndOpenPort(comParmater);
            if (port == null)
            {
                comParmater.RecData = null;
            }
            else
            {
                comParmater.RecData = SendByte(port, comParmater.SendData, comParmater.RcvCount);
            }
            ClosePort(port);
        }
        /// <summary>
        /// 发送命令，并获取接收信息，
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收信息</param> 
        /// <param name="receiveCount">接收信息尝试次数：每50毫秒接收一次信息</param> 
        /// <returns>接收数据的长度，串口未打开返回-1</returns>
        public static List<byte> SendByte(SerialPort serialPort, List<byte> SendData, int receiveCount)
        {
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
                    System.Threading.Thread.Sleep(50);
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
    }


}
