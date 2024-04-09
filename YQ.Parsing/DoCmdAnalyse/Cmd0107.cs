
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary>
    /// 资产编码
    /// 厂家设置，国网统一编码
    /// </summary>
    public class Cmd0107 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
            return rlt;
        }
    }
}
