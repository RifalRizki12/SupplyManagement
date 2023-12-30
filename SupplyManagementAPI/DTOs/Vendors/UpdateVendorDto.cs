using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Enums;

namespace SupplyManagementAPI.DTOs.Vendors
{
    public class UpdateVendorDto
    {
        public Guid Guid { get; set; }
        public StatusVendor? StatusVendor { get; set; }

        // Operator eksplisit untuk mengonversi Company ke CompanyDto.
        public static implicit operator Vendor(UpdateVendorDto createDto)
        {
            return new Vendor
            {
                Guid = createDto.Guid,
                StatusVendor = createDto.StatusVendor,
                CreatedDate = DateTime.Now, // Mengisi properti CreatedDate dengan tanggal dan waktu saat ini.
                ModifiedDate = DateTime.Now // Mengisi properti ModifiedDate dengan tanggal dan waktu saat ini.
            };
        }
    }
}
