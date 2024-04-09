using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.Parsing
{
    public class RequestCmd : AbstractCmd
    {
        //请求
        public RequestCmd(string p_cmd, string s_data = null)
        {
            this.cmd = p_cmd;
            if (!string.IsNullOrEmpty(s_data))
            {
                this.data = s_data.Split(';').ToList();
            }
        }

        public RequestCmd(string strcmd)
        {
            ParsingCmd(strcmd);// 分析命令字
        }

        public override string ToString()
        {
            string rltstr = string.Format("cmd={0},data={1}",  this.cmd, GetDataString());
            return rltstr;
        }

        protected override void ParsingCmd(string strcmd)
        {
            if (string.IsNullOrEmpty(strcmd))
            {
                throw new ArgumentNullException("命令字符串为空！");
            }
            base.ParsingCmd(strcmd);
        }
    }
}
