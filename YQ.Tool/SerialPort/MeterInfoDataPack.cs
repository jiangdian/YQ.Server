using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Tool
{
    public class MeterInfoDataPack
    {
        //起始位为0x68
        private const byte StartByte = 0x68;
        //停止位为0x16
        private const byte EndByte = 0x16;
        public const long TypeID_698 = 698L;
        private static MeterInfoDataPack? m_Instance;
        public static MeterInfoDataPack Instance
        {
            get
            {
                if (m_Instance is null)
                    m_Instance = new MeterInfoDataPack();
                return m_Instance;
            }
        }
        public DataPackMetaData TryPackData(IEnumerable<byte> Bytes)
        {
            int DataLength = Bytes.Count();
            for (int i = 0; i < DataLength; i++)
            {
                int PackLength = ParseAvailable(Bytes, i);
                if (PackLength <= 0)
                    continue;
                if (Bytes.ElementAtOrDefault(i - 4) == 0xFE)
                {
                    return new DataPackMetaData((uint)(i - 4), (uint)(PackLength + 4), TypeID_698);
                }
                else
                {
                    return new DataPackMetaData((uint)i, (uint)PackLength, TypeID_698);
                }
                //return new DataPackMetaData((uint)i, (uint)PackLength, TypeID_698);
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
                if (Data.ElementAtOrDefault(Offset + Len + 1) == EndByte)
                {
                    return Len + 2;
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
    public struct DataPackMetaData
    {
        private uint m_StartIndex;

        private uint m_Length;

        private long m_TypeID;

        public uint StartIndex
        {
            get
            {
                return m_StartIndex;
            }
            set
            {
                m_StartIndex = value;
            }
        }

        public uint Length
        {
            get
            {
                return m_Length;
            }
            set
            {
                m_Length = value;
            }
        }

        public long TypeID
        {
            get
            {
                return m_TypeID;
            }
            set
            {
                m_TypeID = value;
            }
        }

        public static DataPackMetaData Null => new DataPackMetaData(0u, 0u, 0L);

        public DataPackMetaData(uint StartIndex, uint Length, long TypeID)
        {
            m_StartIndex = StartIndex;
            m_Length = Length;
            m_TypeID = TypeID;
        }

        public static bool operator ==(DataPackMetaData Left, DataPackMetaData Right)
        {
            return ((long)Left.Length == 0L && (long)Right.Length == 0L) || (Left.StartIndex == Right.StartIndex && Left.Length == Right.Length && Left.TypeID == Right.TypeID);
        }

        public static bool operator !=(DataPackMetaData Left, DataPackMetaData Right)
        {
            return !(Left == Right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DataPackMetaData))
            {
                return false;
            }

            return (DataPackMetaData)obj == this;
        }
    }
}
