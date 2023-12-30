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

        public async Task<IActionResult> HomeCompanyVendor()
        {
            return View();
        }

        public async Task<IActionResult> HomeCompanyWaiting()
        {
            return View();
        }

        public async Task<IActionResult> GetCompanyManager()
        {
            return View();
        }

        public async Task<IActionResult> GetCompanyAdmin()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCompanyData()
        {
            var result = await _companyRepository.GetDetailClient();
            return Json(new { data = result.Data });
        }

        [HttpGet("/Company/GetCompanyApproveAdmin")]
        public async Task<JsonResult> GetCompanyApproveAdmin()
        {
            var result = await _companyRepository.GetCompanyApproveAdmin();
            return Json(new { data = result.Data });
        }

        [HttpGet("/Company/GetCompanyApproveManager")]
        public async Task<JsonResult> GetCompanyApproveManager()
        {
            var result = await _companyRepository.GetCompanyApproveManager();
            return Json(new { data = result.Data });
        }
    }
}
