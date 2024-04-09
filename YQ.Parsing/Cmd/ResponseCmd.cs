using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace YQ.Parsing
{
    public class ResponseCmd : AbstractCmd
    {
        //应答
        public ResponseCmd(string p_cmd, int p_ret, string s_data)
        {
            this.cmd = p_cmd;
            this.ret = p_ret;
            if (!string.IsNullOrEmpty(s_data))
            {
                this.data = s_data.Split(';').ToList();
            }
        }

        public ResponseCmd(string strcmd)
        {
            ParsingCmd(strcmd);
        }
        private int _ret;
        /// <summary>
        /// 应答结果
        /// </summary>
        public int ret
        {
            get
            {
                return _ret;
            }

            set
            {
                _ret = value;
            }
        }

        public override string ToString()
        {
            string rltstr = string.Format("cmd={0},ret={1},data={2}", this.cmd, this.ret, GetDataString());
            return rltstr;
        }
        
        /// <summary>
        /// cmd=命令字,sn=序号,ret=应答状态,data=数据1;数据2;…数据n
        /// </summary>
        /// <param name="strcmd"></param>
        protected override void ParsingCmd(string strcmd)
        {
            if (string.IsNullOrEmpty(strcmd))
            {
                throw new ArgumentNullException("命令字符串为空！");
            }

            Regex reg_ret = new Regex("(?<=ret=)[0-9]");//(ret=)之后的数据
            try
            {
                Match match;

                match = reg_ret.Match(strcmd);
                this.ret = Convert.ToInt32(match.Value, 16);
            }
            catch (Exception ex)
            {
                throw new Exception("命令格式错误，解析失败!", ex);
            }
            base.ParsingCmd(strcmd);
        }
    }
}
