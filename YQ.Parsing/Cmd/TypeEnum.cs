using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Parsing
{
    public enum TypeEnum
    {
        RS232,
        RS485,
        RJ45,
        红外,
        蓝牙
    }
    public enum SerialPoartEnum
    {
        [Description("485-1")]
        RS4851=1,
        [Description("485-2")]
        RS4852,
        [Description("485-3,逆变器")]
        RS4853,
        [Description("485-4")]
        RS4854,
        [Description("485-5,逆变器")]
        RS4855,
        [Description("485-6")]
        RS4856,
        [Description("蓝牙")]
        蓝牙,
        [Description("STA")]
        STA,
        [Description("CCO")]
        CCO
    }
}
