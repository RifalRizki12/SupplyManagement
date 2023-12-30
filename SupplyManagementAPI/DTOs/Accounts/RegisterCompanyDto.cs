using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Enums;
using System.Numerics;

namespace SupplyManagementAPI.DTOs.Accounts
{
    public class RegisterCompanyDto
    {
        public string NameCompany { get; set; }
        public IFormFile? FotoCompany { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressCompany { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public static implicit operator Account(RegisterCompanyDto accountDto)
        {
            return new Account
            {
                Guid = Guid.NewGuid(),
                Password = accountDto.ConfirmPassword,
                Otp = 0,
                IsUsed = true,
                Status = StatusLevel.Requested,
                RoleGuid = Guid.NewGuid(),
                ExpiredTime = DateTime.Now,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        public static implicit operator Company(RegisterCompanyDto registrationDto)
        {
            return new Company
            {
                Guid = Guid.NewGuid(),
                Name = registrationDto.NameCompany,
                Email = registrationDto.Email,
                PhoneNumber = registrationDto.PhoneNumber,
                Foto = null,
                Address = registrationDto.AddressCompany,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
        
        public static implicit operator Vendor(RegisterCompanyDto registrationDto)
        {
            return new Vendor
            {
                BidangUsaha = "",
                JenisPerusahaan = "",
                StatusVendor = StatusVendor.none,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
