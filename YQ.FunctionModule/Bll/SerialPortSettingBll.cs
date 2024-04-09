using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YQ.FunctionModule.Bll
{
    internal class SerialPortSettingBll : BaseBll
    {
        public List<ComSet> GetComSets()
        {
            var values= freeSql.Select<ComSet>()
                .Where(x=>x.ComType!=null)
                .ToList();
            return values;
        }
        public void UpdateCom(List<ComSet> comSets)
        {
           freeSql.Update<ComSet>()
                .SetSource(comSets)
                .ExecuteAffrows();
        }
        public SerialPortSettingBll(IEventAggregator eventAggregator, IContainerExtension container) : base(eventAggregator, container)
        {
        }
    }
}
