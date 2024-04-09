using System;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 通信模块断开 IO控制器1-8路与载波1-8对应
    /// 断开所有通信模块和表位通讯链路。
    /// </summary>
    public class Cmd0406 : AbstractCmdAnalyse
    { 
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
        
            return rlt;
        }
     
    }
}
