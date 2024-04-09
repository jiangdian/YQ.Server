namespace YQ.FreeSQL.SQLite
{
    public class FreeSqliteHelper : IFreeSqlHelper
    {
        private IFreeSql m_DB;
        public FreeSqliteHelper(string connectionString)
        {
            m_DB = new FreeSql.FreeSqlBuilder()
           .UseConnectionString(FreeSql.DataType.Sqlite, connectionString)
           //.UseMonitorCommand(cmd => cmd.CommandTimeout = 3, null)
           .Build(); //请务必定义成 Singleton 单例模式
        }
        public IFreeSql GetDB()
        {
            return m_DB;
        }
    }
}
