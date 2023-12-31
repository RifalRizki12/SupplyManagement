using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplyManagementAPI.DTOs.Accounts;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Handler;
using SupplyManagementClient.Contract;
using SupplyManagementClient.Models;

namespace SupplyManagementClient.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Logins()
        {
            return View();
        }

        public IActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost("/Account/RegisterCompany")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterCompany([FromForm] RegisterCompanyDto registrationCDto)
        {
            var result = await _accountRepository.RegisterClient(registrationCDto);

            if (result is ResponseOKHandler<Company> successResult)
            {
                // Pendaftaran berhasil
                return Json(new { status = "OK", message = successResult });
            }
            else if (result is ResponseErrorHandler errorResult)
            {
                // Pendaftaran gagal atau ada kesalahan
                return Json(new { status = "Error", message = errorResult });
            }

            return Json(new { success = false, message = "Data tidak valid." });

        }

        [HttpPost("/Account/logins")]
        public async Task<IActionResult> Logins([FromBody] LoginDto login)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.Login(login);

                if (result is ResponseOKHandler<TokenDto> successResult)
                {
                    // Respons sukses
                    HttpContext.Session.SetString("JWToken", successResult.Data.Token);

                    // Mengambil klaim pengguna dari JWT
                    var claims = await _accountRepository.GetClaimsAsync(successResult.Data.Token);

                    if (claims != null)
                    {
                        // Simpan klaim dalam session
                        HttpContext.Session.SetString("Name", claims.Data.Name);
                        HttpContext.Session.SetString("CompanyGuid", claims.Data.CompanyGuid.ToString());
                        HttpContext.Session.SetString("StatusAccount", claims.Data.StatusAccount.ToString());
                        HttpContext.Session.SetString("StatusVendor", claims.Data.StatusVendor.ToString());
                        HttpContext.Session.SetString("Email", claims.Data.Email);
                        HttpContext.Session.SetString("Foto", claims.Data.Foto ?? "");
                        HttpContext.Session.SetString("Role", claims.Data.Role.FirstOrDefault() ?? "");

                        string role = HttpContext.Session.GetString("Role"); // Ambil peran dari session
                        string statusAccount = HttpContext.Session.GetString("StatusAccount");
                        string statusVendor = HttpContext.Session.GetString("StatusVendor");

                        // Lakukan pengalihan berdasarkan peran
                        if (role == "admin")
                        {
                            // Pengguna memiliki peran "admin", lakukan tindakan admin
                            return Json(new { redirectTo = Url.Action("GetCompany", "Company") });
                        }
                        if (role == "manager")
                        {
                            return Json(new { redirectTo = Url.Action("GetCompanyManager", "Company") });
                        }
                        else if (statusAccount == "Requested")
                        {
                            return Json(new { status = "Error", message = "Status Akun Masih Requested, silahkan menuggu !!!" });
                        }
                        else if (statusAccount != "Requested" && statusAccount != "Approved")
                        {
                            return Json(new { status = "Error", message = "Status Akun Non-Aktif/Rejected, silahkan Menghubungi Admin !!!" });
                        }
                        else if (statusAccount == "Approved" && statusVendor == "none" || statusVendor == "reject")
                        {
                            return Json(new { redirectTo = Url.Action("HomeCompany", "Company") });
                        }
                        else if (statusAccount == "Approved" && statusVendor == "waiting" || statusVendor == "approvedByAdmin")
                        {
                            return Json(new { redirectTo = Url.Action("HomeCompanyWaiting", "Company") });
                        }
                        else if (statusAccount == "Approved" && statusVendor == "approvedByManager")
                        {
                            return Json(new { redirectTo = Url.Action("HomeCompanyVendor", "Company") });
                        }

                    }
                    else
                    {
                        // Jika klaim pengguna tidak tersedia
                        return Json(new { status = "BadRequest", message = "User claims not available." });
                    }
                }
                else if (result is ResponseErrorHandler errorResult)
                {
                    // Respons error
                    return Json(new { status = "Error", message = errorResult });
                }
            }

            // Jika login gagal atau data yang dikirimkan tidak valid
            return Json(new { redirectTo = Url.Action("Logins", "Account") });
        }

        [HttpGet("Logout/")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Logins", "Account");
        }

        [HttpGet("Account/GuidAccount/{guid}")]
        public async Task<JsonResult> GuidAccount(Guid guid)
        {
            var result = await _accountRepository.Get(guid);
            var employee = new AccountDto();

            if (result.Data?.Guid != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(employee);
            }
        }

        [HttpPut("Account/UpdateAccount/{guid}")]
        public async Task<JsonResult> UpdateAccount(Guid guid, [FromBody] AccountDto accountDto)
        {
            var response = await _accountRepository.Put(guid, accountDto);

            if (response != null)
            {
                if (response.Code == 200)
                {
                    return Json(new { data = response.Data });
                }
                else
                {
                    return Json(new { error = response.Message });
                }
            }
            else
            {
                return Json(new { error = "An error occurred while updating the employee." });
            }
        }

    }
}
