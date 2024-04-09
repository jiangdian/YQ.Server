using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Parsing
{
    public interface ICmdAnalyse
    {
        AbstractCmd GetResponseCmd(AbstractCmd requestCmd);
    }
}
