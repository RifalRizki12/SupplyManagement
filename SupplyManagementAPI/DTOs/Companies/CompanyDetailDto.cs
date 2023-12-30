
using SupplyManagementAPI.Models;

namespace SupplyManagementAPI.DTOs.Companies;

public class CompanyDetailDto
{
    //inisiasi properti yang akan digunakan 
    public Guid CompanyGuid { get; set; }
    public string NameCompany { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string? Foto { get; set; }
    public string StatusAccount { get; set; }
    public string PhoneNumber { get; set; }
    public string StatusEmployee { get; set; }
    public string BidangUsaha { get; set; }
    public string JenisPerusahaan { get; set; }
    public string StatusVendor { get; set; }
    public Guid VendorGuid { get; set; }



    public static explicit operator CompanyDetailDto(Company company)
    {
        return new CompanyDetailDto
        {
            CompanyGuid = company.Guid,
            NameCompany = company.Name,
            Address = company.Address,
            Email = company.Email,
            Foto=company.Foto,
            PhoneNumber = company.PhoneNumber,
        };
    }

    public static explicit operator CompanyDetailDto(Account account)
    {
        return new CompanyDetailDto
        {
            StatusAccount = account.Status.ToString(),
        };
    }

    public static explicit operator CompanyDetailDto(Vendor vendor)
    {
        return new CompanyDetailDto
        {

            VendorGuid = vendor.Guid,
            BidangUsaha = vendor.BidangUsaha,
            JenisPerusahaan = vendor.JenisPerusahaan,
            StatusVendor = vendor.StatusVendor.ToString(),
        };
    }

}
