using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Tool
{
    public class DLT698FCSHelper
    {
        #region 全局变量和常量定义
        const ushort PPPINITFCS16 = 0xffff;
        const ushort PPPGOODFCS16 = 0xf0b8;
        const ushort PPP = 0x8408;
        const int FCS_TABLE_SIZE = 256;

        static ushort[] fcstab = null;
        #endregion

        #region 方法定义
        /*
         * 函数名：CreateFcsTable
         *   参数：第一个参数是fcstab数组的引用，第二个参数是要初始化数组为多少长度
         * 返回值：void
         *   功能：初始化一个fcstab数组并生产fcs表
         */
        private static void CreateFCSTable(ref ushort[] fcstab, int size)
        {
            uint v;
            fcstab = new ushort[size];
            //Console.Write("static ushort fcstab[{0}] = {{", size);
            for (uint count = 0; count < size; ++count)
            {
                //if (count % 8 == 0)
                //Console.WriteLine();
                v = count;

                for (int i = 8; i > 0; i--)
                    v = (v & 0x01) > 0 ? (v >> 1) ^ PPP : v >> 1;
                fcstab[count] = (ushort)(v & 0xffff);
                //Console.Write("\t0x{0:X4},", (v & 0xffff));
            }
        }

        /*
         * 函数名：PPPFcs16
         *   参数：第一个参数是PPPINITFCS16码，第二个参数是待计算校验的数组，第三个参数是需要校验的长度
         * 返回值：返回一个校验码
         *   功能：该函数计算一个指定长度的数组的校验码，然后返回它的校验码
         */
        static private ushort PPPFcs16(ushort fcs, byte[] data, int length)
        {
            uint index = 0;

            if (fcstab == null)                                 /* 如果fcstab为空，则去初始化它 */
                CreateFCSTable(ref fcstab, FCS_TABLE_SIZE);     /* 初始化一个数组，fcstab */

            while ((length--) > 0)
                fcs = (ushort)((fcs >> 8) ^ fcstab[(fcs ^ data[index++]) & 0x00ff]);
            return fcs;
        }

        /*
         * 函数名：GenerateFcs16
         *   参数：data是需要计算校验码的数组，第二个参数是需要校验的长度
         * 返回值：返回最终的一个校验码
         *   功能：该函数计算一个指定数组长度的校验码并返回它的校验码
         */
        public static ushort CreateFCS16(byte[] data, int index, int len)
        {
            ushort trialfcs;
            trialfcs = PPPFcs16(PPPINITFCS16, data.Skip(index).ToArray(), len);
            trialfcs ^= 0xffff;

            return trialfcs;
        }
        #endregion
    }
}
