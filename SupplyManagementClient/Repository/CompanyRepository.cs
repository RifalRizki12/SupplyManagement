using Newtonsoft.Json;
using SupplyManagementAPI.DTOs.Accounts;
using SupplyManagementAPI.DTOs.Companies;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Handler;
using SupplyManagementClient.Contract;

namespace SupplyManagementClient.Repository
{
    public class CompanyRepository : GeneralRepository<Company, Guid>, ICompanyRepository
    {
        public CompanyRepository(string request = "Company/") : base(request)
        {

        }

        public async Task<ResponseOKHandler<IEnumerable<CompanyDetailDto>>> GetDetailClient()
        {
            // Ganti request ke endpoint yang sesuai
            var requestUrl = "allCompany-detailsNone";

            using (var response = await httpClient.GetAsync(request + requestUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<IEnumerable<CompanyDetailDto>>>(apiResponse);
                return entityVM;
            }
        }
    }
}
