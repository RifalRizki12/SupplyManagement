using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Enums;

namespace SupplyManagementAPI.DTOs.Vendors
{
    public class VendorDto
    {
        public Guid Guid { get; set; }
        public string BidangUsaha { get; set; }
        public string JenisPerusahaan { get; set; }
        public string? Status { get; set; }

        // Operator eksplisit untuk mengonversi Company ke CompanyDto.
        public static explicit operator VendorDto(Vendor vendor)
        {
            return new VendorDto
            {
                Guid = vendor.Guid, // Mengisi properti Guid dengan nilai dari entitas Company.
                BidangUsaha = vendor.BidangUsaha,
                JenisPerusahaan = vendor.JenisPerusahaan,
                Status = vendor.StatusVendor.ToString(),
            };
        }

        // Operator implisit untuk mengonversi CompanyDto ke Company.
        public static implicit operator Vendor(VendorDto vendorDto)
        {
            return new Vendor
            {
                Guid = vendorDto.Guid,       // Mengisi properti Guid dengan nilai dari CompanyDto.
                BidangUsaha = vendorDto.BidangUsaha,       // Mengisi properti Name dengan nilai dari CompanyDto.
                JenisPerusahaan = vendorDto.JenisPerusahaan,
                StatusVendor = StatusVendor.waiting,
                ModifiedDate = DateTime.Now // Mengisi properti ModifiedDate dengan tanggal dan waktu saat ini.
            };
        }
    }
}
