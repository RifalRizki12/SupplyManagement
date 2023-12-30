using SupplyManagementClient.Contract;
using SupplyManagementAPI.DTOs.Accounts;
using SupplyManagementAPI.DTOs.Tokens;
using SupplyManagementAPI.Utilities.Handler;

namespace SupplyManagementClient.Contract
{
    public interface IAccountRepository : IRepository<AccountDto, Guid>
    {
        Task<ResponseOKHandler<ClaimsDto>> GetClaimsAsync(string token);
        Task<object> Login(LoginDto login);
        Task<object> RegisterClient(RegisterCompanyDto registrationCDto);

    }
}
