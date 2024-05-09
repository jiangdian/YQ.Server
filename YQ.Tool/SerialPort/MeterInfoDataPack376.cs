using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Tool
{
    public class MeterInfoDataPack376
    {
        //起始位为0x68
        private const byte StartByte = 0x68;
        //停止位为0x16
        private const byte EndByte = 0x16;
        public const long TypeID_698 = 698L;
        private static MeterInfoDataPack376? m_Instance;
        public static MeterInfoDataPack376 Instance
        {
            get
            {
                if (m_Instance is null)
                    m_Instance = new MeterInfoDataPack376();
                return m_Instance;
            }
        }
        public DataPackMetaData TryPackData(IEnumerable<byte> Bytes)
        {
            int DataLength = Bytes.Count();
            for (int i = 0; i < DataLength; i++)
            {
                int PackLength = ParseAvailable(Bytes, i);
                if (PackLength ==-2)
                    return DataPackMetaData.Null;
                if (PackLength ==-1)
                    continue;
                return new DataPackMetaData((uint)i, (uint)PackLength, TypeID_698);
            }
            return DataPackMetaData.Null;
        }

        public static int ParseAvailable(IEnumerable<byte> Data, int Offset)
        {
            if (Data.ElementAtOrDefault(Offset) != StartByte)
            {
                return -1;
            }
            if (Offset + 2 < Data.Count())
            {
                int Len = GetLength(Data, Offset);
                if (Data.Count()< Offset + Len )
                {
                    return -2;
                }
                if (Data.ElementAtOrDefault(Offset + Len - 1) == EndByte)
                {
                    return Len ;
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
            Array.Copy(Data.ToArray(), offect + 1, bytes, 0, 2);
            int Len = BitConverter.ToUInt16(bytes, 0);
            return Len;
        }
    }
    
    
}
