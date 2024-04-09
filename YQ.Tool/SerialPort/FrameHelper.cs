using System;
using System.Text;

namespace YQ.Tool
{
    /*
     缓冲区工具类
     */
    public class FrameHelper
    {
        public static byte[] parse(byte[] receive_fram, int data_save)//接收解析数据帧 无数据标识
        {
            int l, k;
            byte[] hill = new byte[data_save];
            int i = 0;
            try
            {
                for (; i < receive_fram.Length; i++)
                {
                    if ((receive_fram[i] == 0x68))//判断数据帧的帧头是否合法，校验是否正确& (receive_fram[i + 7] == 0x68) & (checksum(receive_fram.Length - i + 1,receive_fram) == receive_fram[receive_fram.Length - 1])
                    {
                        l = receive_fram[i + 9];
                        for (k = 0; k < l; k++)
                        {
                            hill[k] = receive_fram[i + 10 + k];
                        }
                        break;
                    }
                }
            }
            catch
            {
            }
            return hill;
        }

        /// <summary>
        /// 计算校验和 TODO：测试
        /// </summary>
        /// <param name="dataframe"></param>
        /// <returns></returns>
        public static byte checksum(byte[] dataframe)
        {
            int m = 0;
            byte n = 0;
            for (int i = 0; i < (dataframe.Length); i++)
            {
                m = m + dataframe[i];
            }
            return n = (byte)(m & 0xff);
        }

        /// <summary>
        /// 数据反转减33H  TODO：测试
        /// </summary>
        /// <param name="byteArr"></param>
        /// <returns></returns>
        public static byte[] Reverse_sub33(byte[] byteArr)
        {
            byte[] additer = new byte[byteArr.Length];
            byte[] temp = new byte[byteArr.Length];
            Array.Copy(byteArr, temp, byteArr.Length);
            Array.Reverse(temp);
            for (int i = 0; i < temp.Length; i++)
            {
                int ll = temp[i];
                if (ll < 0x33)
                {
                    ll += 0x100;
                }
                additer[i] = Convert.ToByte((ll - 0x33));
                //additer[temp.Length - 1 - i] = Convert.ToByte((ll - 0x33));
            }
            return additer;
        }

        /// <summary>
        /// 数据反转  TODO：测试
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static byte[] Reverse(byte[] byteArr)
        {
            if (byteArr == null)
                return null;
            byte[] temp = new byte[byteArr.Length];
            Array.Copy(byteArr, temp, byteArr.Length);
            Array.Reverse(temp);
            return temp;
        }

        /// <summary>
        /// 数据反转加33H  TODO：测试
        /// </summary>
        /// <param name="byteArr"></param>
        /// <returns></returns>

        public static byte[] Reverse_add33(byte[] byteArr)
        {
            byte[] additer = new byte[byteArr.Length];
            byte[] temp = new byte[byteArr.Length];
            Array.Copy(byteArr, temp, byteArr.Length);
            Array.Reverse(temp);
            for (int i = 0; i < temp.Length; i++)
            {
                int ll = temp[i];
                if (ll + 0x33 > 0xFF)
                {
                    additer[i] = Convert.ToByte(ll + 0x33 - 0x100);
                }
                else
                {
                    additer[i] = Convert.ToByte(ll + 0x33);
                }
            }
            return additer;
        }

        public static byte[] Add33(byte[] temp) //数据反转+33H
        {
            byte[] additer = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                int ll = temp[i];
                if (ll + 0x33 > 0xFF)
                {
                    additer[i] = Convert.ToByte(ll + 0x33 - 0x100);
                }
                else
                {
                    additer[i] = Convert.ToByte(ll + 0x33);
                }
            }
            return additer;
        }
        public static byte[] Sub33(byte[] temp) //数据反转+33H
        {
            byte[] additer = new byte[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                int ll = temp[i];
                if (ll - 0x33 > 0xFF)
                {
                    additer[i] = Convert.ToByte(ll - 0x33 - 0x100);
                }
                else
                {
                    additer[i] = Convert.ToByte(ll - 0x33);
                }
            }
            return additer;
        }

        /// <summary>
        /// byte[]转成string字符串，空格分隔
        /// </summary>
        /// <param name="inbyte"></param>
        /// <returns></returns>
        public static string bytetostr(byte[] inbyte)  //byte转为string
        {
            string temp = "";

            if (inbyte != null)
            {
                foreach (byte b in inbyte)
                    temp += b.ToString("X2") + " ";
            }

            return temp;
        }

        /// <summary>
        /// byte[]转成string字符串，空格分隔
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes) // 整型16进制转换成 16进制的string
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2") + " ");
                }
                hexString = strB.ToString();
            }
            return hexString.Trim();
        }

        /// <summary>
        /// byte[]转成string字符串，无分隔
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString_1(byte[] bytes) // 整型16进制转换成 16进制的string
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2")); //different
                }
                hexString = strB.ToString();
            }
            return hexString;
        }


        public static byte[] StrToHexByte(string hexString)    //16进制字符串转byte
        {
            if (hexString == null)
                return null;
            hexString = hexString.Replace(" ", "");   //去掉空格


            double dlen = Math.Ceiling((double)hexString.Length / 2);          //防止奇数位不能整除
            int len = (int)dlen;
            byte[] returnBytes = new byte[len];
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            catch
            {

            }
            return returnBytes;
        }

        public static int HexStringToint(string HexString) // 16进制字符串转为int数值
        {
            int result = 0;

            string[] temp = HexString.Trim().Split(' ');

            result = Int32.Parse(temp[0], System.Globalization.NumberStyles.HexNumber) * 256 + Int32.Parse(temp[1], System.Globalization.NumberStyles.HexNumber);
            //result = Convert.ToInt16(temp[0]) *256+ Convert.ToInt16(temp[1]);
            return result;
        }
        public static int HexString2Int(string HexString)  //16进制字符串转int："8E2"-->2274
        {
            if (string.IsNullOrEmpty(HexString))
                return -1;
            int num = Int32.Parse(HexString, System.Globalization.NumberStyles.HexNumber);
            return num;
        }
        public static string DecToHex(string x)  //十进制字符转换为16进制字符
        {
            string Result = "";
            if (x == "")
            {
                return Result;
            }
            int DataValue = int.Parse(x);
            Result = DataValue.ToString("X8");
            return Result;
        }

        /// <summary>
        /// 获取2进制
        /// </summary>
        /// <param name="x">字符串</param>
        /// <param name="len">字节长度</param>
        /// <returns></returns>
        public static string HexToBin(string x, int len)
        {
            string Result = "";
            x = x.Replace(" ", "");
            if (x == "")
            {
                return Result;
            }
            int Datain = Convert.ToInt32(x, 16);
            Result = System.Convert.ToString(Datain, 2);
            while (Result.Length < len * 8)
            {
                Result = "0" + Result;
            }

            return Result;
        }
        /// <summary>
        /// 二进制字符串转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int BinToInt(string str)
        {

            int strToint = Convert.ToInt32(str, 2);
            return strToint;

        }
      
    }
}
