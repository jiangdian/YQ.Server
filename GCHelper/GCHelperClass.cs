using Lierda.WPFHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCHelper
{
    public class GCHelperClass
    {
        LierdaCracker cracker = new LierdaCracker();

        public void Start()
        {
            cracker.Cracker(600);
        }
    }
}
