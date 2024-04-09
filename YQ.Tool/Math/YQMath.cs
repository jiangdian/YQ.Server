using System;
using System.Collections.Generic;

namespace YQ.Tool
{
    public class YQMath
    {
        /// <summary>
        /// 获取double值
        /// </summary>
        /// <param name="strval">double字符串</param>
        /// <param name="default">默认值，默认0</param>
        /// <param name="decimal">小数位数，默认3</param>
        /// <returns></returns>
        public static double GetDouble(string strval, double @default = 0, int @decimal = 3)
        {
            double val = @default;
            if (double.TryParse(strval, out val))
            {
                return Math.Round(val, @decimal);
            }
            return Math.Round(@default, @decimal);
        }

        /// <summary>
        /// 除法
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static double Div(double d1, double d2, int decimals)
        {
            if (d2 != 0)
            {
                return Math.Round(d1 / d2, decimals);
            }
            return 0;
        }

        /// <summary>
        /// 获取Int32值
        /// </summary>
        /// <param name="strval">Int32字符串</param>
        /// <param name="default">默认值</param>
        /// <returns></returns>
        public static int GetInt32(string strval, int @default = 0)
        {
            int val = @default;
            if (int.TryParse(strval, out val))
            {
                return val;
            }
            return @default;
        }

        /// <summary>
        /// CRC16校验 低字节在前
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CRC16(List<byte> data)
        {
            return CRC16(data.ToArray());
        }

        /// <summary>
        /// CRC16校验 低字节在前
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CRC16(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置

                return new byte[] { lo, hi };
            }
            return new byte[] { 0, 0 };
        }
    }
}
