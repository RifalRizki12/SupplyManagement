using SupplyManagementAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace SupplyManagementAPI.Models
{
    [Table("tb_m_company")]
    public class Company : BaseEntity
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column("address", TypeName = "nvarchar(100)")]
        public string Address { get; set; }

        [Column("email", TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column("phone_number", TypeName = "nvarchar(15)")]
        public string PhoneNumber { get; set; }

        [Column("foto")]
        public string? Foto { get; set; }

        public Account? Account { get; set; }
        public Vendor? Vendor { get; set; }
    }
}
