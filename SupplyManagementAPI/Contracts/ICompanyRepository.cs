using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.Models;

namespace SupplyManagementAPI.Contracts
{
    public interface ICompanyRepository : IGeneralRepository<Company>
    {
        Company GetByCompanyEmail(string companyEmail);
        Company GetAdminEmployee();
    }
}
