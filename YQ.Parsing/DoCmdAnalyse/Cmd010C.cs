
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary>
    ///  工业相机数
    /// 厂家设置，2个字符 10进制
    /// </summary>
    public class Cmd010C : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
            return rlt;
        }
    }
}
