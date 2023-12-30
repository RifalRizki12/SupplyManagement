using SupplyManagementAPI.DTOs.Tokens;
using System.Security.Claims;

namespace SupplyManagementAPI.Contracts
{
    public interface ITokenHandlers
    {
        string Generate(IEnumerable<Claim> claims);

        ClaimsDto ExtractClaimsFromJwt(string token);
    }
}
