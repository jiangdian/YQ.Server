using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace YQ.FreeSQL.Entity
{
    public class ComSet
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public int? ComID { get; set; }
        public string? ComName { get; set; }
        public string? ComPara { get; set; }
        public string? ComPro { get; set; }  //协议类型
        public int? ComType { get; set; }  //串口类型 SerialPoartEnum
        public int? MeterID { get; set; }  //表位号

    }
}
