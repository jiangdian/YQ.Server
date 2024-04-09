namespace YQ.FunctionModule.Data
{
    public abstract class DataSource : IDataSource
    {
        public virtual List<CmdData> GetData()
        {
            List<CmdData> lstDatas = new List<CmdData>
            {
                new CmdData("读标准表", "cmd=0301,data=null"),
                new CmdData("开始日计时-表位1", "cmd=0411,data=1;60;10"),
                new CmdData("停止日计时-表位1", "cmd=0412,data=1"),//TODO:峰恩一停全停
                new CmdData("日计时查询-表位1", "cmd=0413,data=1"),
                new CmdData("功耗查询-表位1", "cmd=0417,data=1"),//TODO:峰恩接口未给
                new CmdData("初始化串口", "cmd=0106,data=1;0;1;9600-n-8-1"),
            };
            return lstDatas;
        }
    }
}
