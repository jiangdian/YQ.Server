using System;
using System.Text;
using System.Threading;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 停止计时误差测试，并清空测试结果数据
    /// </summary>
    public class Cmd0412 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            try
            {
                PowerHelper.Clock_Error_End();
                
                rlt = new ResponseCmd(requestCmd.cmd, 0, null);
            }
            catch (Exception)
            {
                rlt = new ResponseCmd(requestCmd.cmd, 1, null);
            }
            return rlt;
        }
    }
}
