using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using YQ.FreeSQL;
using YQ.FreeSQL.SQLite;

namespace YQ.Parsing
{
    public abstract class AbstractCmdAnalyse : ICmdAnalyse
    {
        //public static PowerParam LastPower;

        /// <summary>
        /// 上电电流
        /// </summary>
        public static double PowerI = 0;


        // <summary>
        /// 上电电压
        /// </summary>
        public static double PowerPhase = 0;

        public IFreeSql freeSql;
        /// <summary>
        /// 误差的测试次数
        /// </summary>

        //存储误差结果
        //public Queue<String> Err_data = new Queue<string>();

        /// <summary>
        /// 正在走字
        /// </summary>
        public static bool IsWalk = false;

        /// <summary>
        /// 读取标准表的示值缓存 -- 降低标准表读取接口的调用频率
        /// </summary>
        public static List<string> StdBuffer = new List<string>();


        public abstract AbstractCmd GetResponseCmd(AbstractCmd requestCmd);

        protected string GetCmdString(string cmd, string data = "null", int ret = -1)
        {
            string rlt = "";
            if (ret == -1)
            {
                rlt = string.Format("cmd={0},data={1}", cmd, data ?? "null");
            }
            else
            {
                rlt = string.Format("cmd={0},ret={1},data={2}", cmd, ret, data ?? "null");
            }
            return rlt;
        }
        protected string GetCmdString(string cmd, int ret = -1, string data = "null")
        {
            return GetCmdString(cmd, data, ret);
        }


        /// <summary>
        /// 自动短接
        /// </summary>
        /// <param name="sn">表位序号，代表00所有表</param>
        /// <param name="isNormal">false=短接/断开；true=正常/接入</param>
        public bool AutoReset(int sn, bool isNormal)
        {
            bool rlt = true;
            return rlt;
        }


        /// <summary>
        /// 执行电机操作
        /// </summary>
        /// <param name="requestCmd"></param>
        /// <param name="OpenClose">True 上行/false 下行 </param>
        /// <returns></returns>
        public AbstractCmd AutoBonde(AbstractCmd requestCmd, bool OpenClose)
        {
            //ComParamter com = new ComParamter(PowerHelper.config.YajieCom);
            //YQARMHelper armHelper = new YQARMHelper(com.PortName, com.BaudRate);

            //bool isSend = armHelper.Open();
            //int sn = requestCmd.sn;

            //byte Mater = 0x00;//TODO：自动压接与表位数量关联
            //switch (sn)
            //{
            //    case 0:
            //        Mater = 0xAA;
            //        break;
            //    case 1:
            //        Mater = 0x00;
            //        break;
            //    case 2:
            //        Mater = 0x01;
            //        break;
            //    case 3:
            //        Mater = 0x02;
            //        break;
            //    default:
            //        break;
            //}
            //byte closeOpen;
            //if (OpenClose)
            //{
            //    closeOpen = 0x00;
            //}
            //else
            //{
            //    closeOpen = 0x01;
            //}
            //try
            //{
            //    armHelper.AutoBonde(Mater, closeOpen);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //armHelper.Close();

            AbstractCmd rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 0));
            return rlt;

        }
        public AbstractCmdAnalyse()
        {
            freeSql = new FreeSqliteHelper(ConfigurationManager.AppSettings["SqlStr"].ToString()).GetDB();
        }

    }
}
