using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YQ.FunctionModule.Bll
{
    internal class ListenInBll : BaseBll
    {
        public List<ComSet> GetComs()
        {
            var values = freeSql.Select<ComSet>()
                .Where(x =>x.ComType!=null&& x.ComType >0&&x.ComType<7||x.ComType==9)
                .ToList();
            return values;
        }
        public ListenInBll(IEventAggregator eventAggregator, IContainerExtension container) : base(eventAggregator, container)
        {
        }
    }
}
