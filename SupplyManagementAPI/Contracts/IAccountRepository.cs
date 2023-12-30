using SupplyManagementAPI.Models;

namespace SupplyManagementAPI.Contracts
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
        Account GetByCompanyEmail(string companyEmail);
    }
}
