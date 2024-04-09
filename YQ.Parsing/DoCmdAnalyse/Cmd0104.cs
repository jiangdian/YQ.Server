
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary>
    ///  台体生产日期
    /// 厂家设置，YYYY.MM.DD 10个字符表示
    /// </summary>
    public class Cmd0104 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
            return rlt;           
        }
    }
}
