using Microsoft.AspNetCore.Mvc;
using SupplyManagementClient.Contract;

namespace SupplyManagementClient.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<IActionResult> GetCompany()
        {
            return View();
        }

        public async Task<IActionResult> HomeCompany()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCompanyData()
        {
            var result = await _companyRepository.GetDetailClient();
            return Json(new { data = result.Data });
        }
    }
}
