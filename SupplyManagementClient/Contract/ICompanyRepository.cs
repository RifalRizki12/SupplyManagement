using SupplyManagementAPI.DTOs.Companies;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Handler;

namespace SupplyManagementClient.Contract
{
    public interface ICompanyRepository : IRepository<Company, Guid>
    {
        Task<ResponseOKHandler<IEnumerable<CompanyDetailDto>>> GetDetailClient();
        Task<ResponseOKHandler<IEnumerable<CompanyDetailDto>>> GetCompanyApproveAdmin();
        Task<ResponseOKHandler<IEnumerable<CompanyDetailDto>>> GetCompanyApproveManager();
    }
}
