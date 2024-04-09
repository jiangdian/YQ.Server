namespace YQ.FunctionModule.BLL
{
    public class BaseBll
    {
        public IFreeSql freeSql;
        public IFreeSqlHelper freeSqlHelper;
        public readonly IContainerExtension container;
        public readonly IEventAggregator eventAggregator;
        public BaseBll(IEventAggregator eventAggregator, IContainerExtension container)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
            freeSqlHelper = this.container.Resolve<IFreeSqlHelper>();
            freeSql = this.container.Resolve<IFreeSqlHelper>().GetDB();
        }
    }
}
