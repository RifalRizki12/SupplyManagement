using System.ComponentModel.DataAnnotations;

namespace SupplyManagementAPI.Utilities.Enums
{
    public enum StatusVendor
    {
        [Display(Name = "Belum Terdaftar")]
        none = 0,

        [Display(Name = "menunggu persetujuan")]
        waiting = 1,

        [Display(Name = "disejutui admin")]
        approvedByAdmin = 2,

        [Display(Name = "disetujui manager")]
        approvedByManager = 3,

        [Display(Name = "Ditolak")]
        reject = 4,
    }
}
