using System;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 电压回路接入 IO控制器的11~16路是电压控制
    /// 表位电压接入
    /// </summary>
    public class Cmd0405 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            
            return rlt;
        }

    }
}
