using SupplyManagementAPI.DTOs.Vendors;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Handler;

namespace SupplyManagementClient.Contract
{
    public interface IVendorRepository : IRepository<Vendor, Guid>
    {
        public Task<object> UpdateVendor(VendorDto clientDto);
        public Task<ResponseOKHandler<UpdateVendorDto>> UpdateStatusVendor(Guid id, UpdateVendorDto entity);
        public Task<ResponseOKHandler<VendorDto>> GetGuidVendor(Guid guid);
    }
}
