using System;
using System.Collections.Generic;
using System.Linq;
using YQ.Tool;

namespace ModbusTest
{
    public class ModbusDataPack
    {
        private static ModbusDataPack? m_Instance;
        public static ModbusDataPack Instance
        {
            get
            {
                if (m_Instance is null)
                    m_Instance = new ModbusDataPack();
                return m_Instance;
            }
        }
        public DataPackMetaData TryPackData(IEnumerable<byte> Bytes)
        {
            int DataLength = Bytes.Count();
            for (int i = 0; i < DataLength-5; i++)
            {
                int PackLength = CalculateCrc((byte[])Bytes,i,DataLength-i);
                if (PackLength==0)
                {
                    return new DataPackMetaData((uint)i, (uint)(DataLength - i), 0);
                }
            }
            return DataPackMetaData.Null;
        }
        /// <summary>
        /// ModBus校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
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
    }
    
    
}
