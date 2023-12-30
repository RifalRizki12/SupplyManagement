using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace SupplyManagementAPI.Models
{
    [Table("tb_m_vendor")]
    public class Vendor : BaseEntity
    {
        [Column("bidang_usaha", TypeName = "nvarchar(100)")]
        public string BidangUsaha { get; set; }
        
        [Column("jenis_perusahaan", TypeName = "nvarchar(50)")]
        public string JenisPerusahaan { get; set; }

        [Column("status_vendor")]
        public StatusVendor? StatusVendor { get; set; }

        public Company? Company { get; set; }

    }
}
