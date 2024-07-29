using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Tool
{
    public class MeterInfoDataPackModbus
    {
        //起始位为0x68
        private const byte StartByte = 0x68;
        //停止位为0x16
        private const byte EndByte = 0x16;
        public const long TypeID_698 = 888L;
        byte[] Controls = { 0x03, 0x04,0x06,0x08,0x10,0x13 };
        private static MeterInfoDataPackModbus? m_Instance;
        public static MeterInfoDataPackModbus Instance
        {
            get
            {
                if (m_Instance is null)
                    m_Instance = new MeterInfoDataPackModbus();
                return m_Instance;
            }
        }
        public DataPackMetaData TryPackData(IEnumerable<byte> Bytes)
        {
            int DataLength = Bytes.Count();
            for (int i = DataLength-1; i >0; i--)
            {
                if (Controls.Contains(Bytes.ElementAtOrDefault(i)))
                {
                    byte[] bytes = GetCRc16(Bytes.Skip(i - 1).ToArray());
                    if (bytes[0] == 0x00 && bytes[1] == 0x00)
                    { return new DataPackMetaData((uint)i - 1, (uint)(DataLength - i + 1), 888); }
                }
            }
            return DataPackMetaData.Null;
        }

        private byte[] GetCRc16(byte[] bytes)
        {
            ushort crc = 0xFFFF;

            for (int i = 0; i < bytes.Length; i++)
            {
                crc ^= bytes[i];

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
            return BitConverter.GetBytes(crc);
        }

        public static int ParseAvailable(IEnumerable<byte> Data, int Offset)
        {
            if (Data.ElementAtOrDefault(Offset) != StartByte)
            {
                return -1;
            }
            if (Offset + 9 < Data.Count())
            {
                int Len = GetLength(Data, Offset);
                if (Data.ElementAtOrDefault(Offset + Len + 11) == EndByte)
                {
                    return Len + 12;
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取报文长度
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static int GetLength(IEnumerable<byte> Data, int offect)
        {
            byte[] bytes = new byte[2];
            Array.Copy(Data.ToArray(), offect + 9, bytes, 0, 1);
            int Len = BitConverter.ToUInt16(bytes);
            return Len;
        }
    }
    
}
