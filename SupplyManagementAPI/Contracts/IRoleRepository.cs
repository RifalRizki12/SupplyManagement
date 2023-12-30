using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.Models;

namespace SupplyManagementAPI.Contracts
{
    public interface IRoleRepository : IGeneralRepository<Role>
    {
        Guid? GetDefaultGuid();
    }
}
