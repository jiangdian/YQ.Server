using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace YQ.Parsing
{
    public abstract class AbstractCmd
    {
        private string _cmd;
        private List<string> _data = new List<string>();

        /// <summary>
        /// 命令字
        /// </summary>
        public string cmd
        {
            get
            {
                return _cmd;
            }

            set
            {
                _cmd = value;
            }
        }


        public List<string> data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
            }
        }

        /// <summary>
        /// 分析命令字符串
        /// </summary>
        /// <param name="strcmd"></param>
        protected virtual void ParsingCmd(string strcmd)
        {

            if (string.IsNullOrEmpty(strcmd))
            {
                throw new ArgumentNullException("命令字符串为空！");
            }
            Regex reg_cmd = new Regex("(?<=cmd=)[0-9A-Fa-f]+");//(cmd=)之后的数据
            Regex reg_data = new Regex("(?<=data=)[^,]+");//(data=)之后的数据
            strcmd = strcmd.Trim();
            try
            {
                Match match;
                //cmd
                match = reg_cmd.Match(strcmd);
                this.cmd = match.Value;
                //data
                match = reg_data.Match(strcmd);
                string data_str = match.Value;
                if (data_str != "null")
                {
                    string[] arr_data = data_str.Split(';');
                    this.data = arr_data.ToList();
                }
                else
                {
                    this.data = new List<string>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("命令格式错误，解析失败!", ex);
            }
        }

        public override abstract string ToString();

        public string GetDataString()
        {
            List<string> lstdata = this.data;
            string dstr = GetDataString(lstdata);
            return dstr;
        }

        public static string GetDataString(List<string> lstdata)
        {
            if (lstdata == null || lstdata.Count == 0)
            {
                return "null";
            }
            string dstr = "";
            foreach (var str in lstdata)
            {
                dstr += string.Format("{0};", str);
            }
            dstr = dstr.TrimEnd(';');
            if (dstr == "")
            {
                return "null";
            }
            return dstr;
        }
    }
}
