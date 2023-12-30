using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.Data;
using SupplyManagementAPI.Models;

namespace SupplyManagementAPI.Repositories
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        public RoleRepository(SupplyManagementDbContext context) : base(context) { }

        public Guid? GetDefaultGuid()
        {
            // Mengambil role dengan nama "user" dari database
            return _context.Set<Role>().FirstOrDefault(r => r.Name == "vendor")?.Guid;
        }

    }
}
