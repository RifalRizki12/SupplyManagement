using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.Data;
using SupplyManagementAPI.Models;

namespace SupplyManagementAPI.Repositories
{
    public class VendorRepository : GeneralRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(SupplyManagementDbContext context) : base(context) { }

    }
}
