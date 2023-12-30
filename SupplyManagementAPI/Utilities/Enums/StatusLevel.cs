using System.ComponentModel.DataAnnotations;

namespace SupplyManagementAPI.Utilities.Enums
{
    public enum StatusLevel
    {
        Requested,
        Approved,
        Rejected,
        Canceled,
        [Display(Name = "Non Aktif")] NonAktif
    }
}
