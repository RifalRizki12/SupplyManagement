using SupplyManagementAPI.Data;
using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Repositories;

namespace API.Repositories
{
    public class CompanyRepository : GeneralRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(SupplyManagementDbContext context) : base(context) { }

        public Company GetAdminEmployee()
        {
            return _context.Companies.FirstOrDefault(e => e.Account.Role.Name == "admin");
        }

        public Company GetByCompanyEmail(string companyEmail)
        {
            // Implementasi metode GetByEmployeeEmail di sini
            // Menggunakan LINQ untuk mencari karyawan berdasarkan email
            return _context.Companies.FirstOrDefault(company => company.Email == companyEmail);
        }
    }
}
