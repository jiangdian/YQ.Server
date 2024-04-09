using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Tool.PowerSupply
{
    public class ClockHelper
    {
        public int Mtetr_No { get; set; }
        public int TestTime { get; set; }
        public int TestCS { get; set; }
        public List<double> TestValueList { get; set; }
        public List<double> TestDayList { get; set; }

    }
}
