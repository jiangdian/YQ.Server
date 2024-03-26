namespace YQ.FunctionModule.Data
{
    public abstract class DataSource : IDataSource
    {
        public virtual List<CmdData> GetData()
        {
            List<CmdData> lstDatas = new List<CmdData>
            {
                new CmdData("表位初始化", "cmd=0401,data=null"),
                new CmdData("全部压接", "cmd=0408;sn=00;data=null"),
                new CmdData("撤销压接", "cmd=0409;sn=00;data=null"),
                new CmdData("读标准表", "cmd=0301,data=null"),
                new CmdData("开始日计时-表位1", "cmd=0531,data=30;1"),
                new CmdData("日计时查询-表位1", "cmd=0533,data=null"),
                new CmdData("开始误差-表位1", "cmd=0501,data=0;1200;12;1"),
                new CmdData("误差查询-表位1", "cmd=0503,data=null"),
                new CmdData("开始潜动", "cmd=0511,data=0;1;180"),
                new CmdData("潜动查询", "cmd=0513,data=null"),
                new CmdData("停止误差潜动等-总清", "cmd=0512,data=null"),
                new CmdData("载波接入(1表位5路载波)", "cmd=0407,data=5"),
                new CmdData("载波断开", "cmd=0406,data=null"),
                new CmdData("电流旁路", "cmd=0402,data=null"),
                new CmdData("电流接入", "cmd=0403,data=null"),
                new CmdData("电压断开-表位1", "cmd=0404,data=null"),
                new CmdData("电压接入-表位1", "cmd=0405,data=null"),
                new CmdData("开始走字", "cmd=0521,data=0"),
                new CmdData("走字结果-表位1", "cmd=0523,data=null"),
                new CmdData("停止走字", "cmd=0522,data=null"),
                new CmdData("电压回路测试", "cmd=0410,data=null")
            };
            return lstDatas;
        }
    }
}
