using FreeSql.DataAnnotations;

namespace YQ.FreeSQL.Entity
{
    public class MeterSet
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public int MeterID { get; set; }
        public string? RS4851 { get; set; }
        public string? RS4852 { get; set; }
        public string? RJ4514851 { get; set; }
        public string? RJ4514852 { get; set; }
        public string? RJ4524852 { get; set; }
        public string? RJ4524851 { get; set; }
        public string? Bluetooth { get; set; }
        public string? Bak { get; set; }
        public string? EventReport { get; set; }
        public string? CCO { get; set; }
    }
}
