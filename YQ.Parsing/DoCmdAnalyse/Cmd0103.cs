
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary>
    ///关闭台体多路服务器串口
    /// </summary>
    public class Cmd0103 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            SerialManager.Instance.ClosePort("com"+requestCmd.data[0]);
            AbstractCmd rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
            return rlt;           
        }
    }
}
