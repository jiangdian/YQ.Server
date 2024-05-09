using System;
using System.Diagnostics.Metrics;
using YQ.Tool;
using YQ.Tool.JY;
using static YQ.Parsing.DoCmdAnalyse.Cmd0506;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 电压回路测试
    /// 测试电能表电压回路是否短路或故障，测试电能表是否能上电
    /// 电压回路状态：1表示正常，0表示故障
    /// </summary>
    public class Cmd0506 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            ComParamter com = new ComParamter(ConfigHelper.GetValue("GHCom"));
            SerialManager.Instance.CreateAndOpenPort(com);
            byte[] b = GongHao.ReadGH(requestCmd.data[0]);
            com.SendData.AddRange(b);
            SerialPortService.SendData1(com);
            var gh = GongHao.GetGH(com.RecData.ToArray());
            var data = requestCmd.data[0]+";"
                +GongHao.ToFloat(gh.PA) + ";" + GongHao.ToFloat(gh.SA) + ";" + 0 + ";"
                 + GongHao.ToFloat(gh.PB) + ";" + GongHao.ToFloat(gh.SB) + ";" + 0 + ";"
                 + GongHao.ToFloat(gh.PC) + ";" + GongHao.ToFloat(gh.SC) + ";" + 0;
            rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 0, data));
            return rlt;
        }
        public static class GongHao
        {
            public static byte[] ReadGH(string MeterID)
            {
                byte[] src = new byte[7];
                src[0] = 0xAA;
                src[1] = Convert.ToByte(MeterID); ;
                src[2] = 0x04;
                src[3] = 0xFB;
                src[4] = 0x05;
                src[5] = 0x01;
                src[6] = 0x01;
                return src;
            }
            public static byte[] ChangeGH(string MeterID)
            {
                byte[] src = new byte[7];
                src[0] = 0xAA;
                src[1] = Convert.ToByte(MeterID); ;
                src[2] = 0x04;
                src[3] = 0xD1;
                src[4] = 0x26;
                src[5] = 0x01;
                src[6] = 0xF8;
                return src;
            }
            public static BaseProtocol GetGH(byte[] src)
            {
                if (src.Length>37)
                {
                    
                    if (src[0]==0xAA)
                    {
                        BaseProtocol baseProtocol = new BaseProtocol()
                        {
                            MeterID = src[1],
                            DataLength = src[2],
                            ControlCode=new byte[] { src[3], src[4] },
                            Address = src[5],
                            type = src[6],
                            SA=new byte[] { src[7], src[8], src[9], src[10], src[11] },
                            SB=new byte[] { src[12], src[13], src[14], src[15], src[16] },
                            SC =new byte[] { src[17], src[18], src[19], src[20], src[21] },
                            PA=new byte[] { src[22], src[23], src[24], src[25], src[26] },
                            PB=new byte[] { src[27], src[28], src[29], src[30], src[31] },
                            PC=new byte[] { src[32], src[33], src[34], src[35], src[36] },
                            calc = src[37]
                        };
                        return baseProtocol;
                    }

                }
                return null;
            }
            public static float ToFloat(byte[] bytes)
            {
                int jiefu = bytes[0] >> 4;
                int jiema = bytes[0] & 0xf;
                int shufu = bytes[1] >> 4;
                string s = (bytes[1] & 0xf).ToString();
                for (int i = 2; i < 5; i++)
                {
                    s += bytes[i].ToString("x2");
                }
                Int64 zhi = Convert.ToInt64(s);

                float convertedValue = (float)((shufu == 1 ? -1 : 1) * zhi * Math.Pow(0.1, jiema+6));
                return convertedValue;
            }
        }
        public  class BaseProtocol 
        {
            public const byte FrameHead = 0xAA;
            public byte MeterID { get; set; }
            public byte DataLength { get; set; }
            public byte[] ControlCode { get; set; }= new byte[2] {0xFB,0x05};
            public byte Address { get; set; }
            public byte type { get; set; }
            public byte[] SA { get; set; }=new byte[5];
            public byte[] SB { get; set; }=new byte[5];
            public byte[] SC { get; set; }=new byte[5];
            public byte[] PA { get; set; }=new byte[5];
            public byte[] PB { get; set; }=new byte[5];
            public byte[] PC { get; set; }=new byte[5];           
            public byte calc { get; set; }
        }
        public class FloatConverter
        {
            static void Main()
            {
                // 假设我们有一个单精度浮点数的阶码、阶符和数符
                int exponent = 134; // 阶码
                int sign = 0; // 数符，0表示正数，1表示负数
                uint mantissa = 0b01000000000000000000000; // 尾数的二进制表示，注意这里只有23位

                // 将阶码和数符转换为32位浮点数的二进制表示
                uint floatBits = (uint)(sign << 31); // 将数符左移31位
                floatBits |= (uint)((exponent & 0xFF) << 23); // 将阶码左移23位并与数符合并
                floatBits |= mantissa; // 将尾数与阶码和数符合并

                // 使用BitConverter将二进制表示转换为浮点数
                float floatValue = BitConverter.ToSingle(BitConverter.GetBytes(floatBits), 0);
            }
        }
    }
}
