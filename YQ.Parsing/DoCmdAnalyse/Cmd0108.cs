
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary>
    ///  接线方式
    /// 厂家设置，国网统一编码，01-单相 02-三相 03…以后扩展
    /// </summary>
    public class Cmd0108 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
            return rlt;
        }
    }
}
