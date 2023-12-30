using SupplyManagementAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyManagementAPI.Models
{
    [Table("tb_m_persons")]
    public class Person : BaseEntity
    {
        [Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column("email", TypeName = "nvarchar(50)")]
        public string Email { get; set; }


    }
}
