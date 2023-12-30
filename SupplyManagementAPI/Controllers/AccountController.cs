using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.DTOs.Accounts;
using SupplyManagementAPI.DTOs.Tokens;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Handler;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Transactions;

namespace SupplyManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly ITokenHandlers _tokenHandler;

        public AccountController(ICompanyRepository companyRepository, IAccountRepository accountRepository, IRoleRepository roleRepository, IVendorRepository vendorRepository, ITokenHandlers tokenHandler)
        {
            _companyRepository = companyRepository;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _vendorRepository = vendorRepository;
            _tokenHandler = tokenHandler;
        }

        [Authorize]
        [HttpGet("GetClaims/{token}")]
        public IActionResult GetClaims(string token)
        {
            var claims = _tokenHandler.ExtractClaimsFromJwt(token);
            return Ok(new ResponseOKHandler<ClaimsDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Claims has been retrieved",
                Data = claims
            });
        }

        [HttpPost("registerCompany")]
        public async Task<IActionResult> RegisterClient([FromForm] RegisterCompanyDto registrationDto)
        {
            if (ModelState.IsValid)
            {
                using (var transactionScope = new TransactionScope())
                {
                    try
                    {
                        Account account = registrationDto;
                        Company company = registrationDto;
                        Vendor vendor = registrationDto;

                        // Handle pengunggahan foto
                        byte[] photoBytes = null;

                        if (registrationDto.FotoCompany != null && registrationDto.FotoCompany.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                registrationDto.FotoCompany.CopyTo(memoryStream);
                                photoBytes = memoryStream.ToArray();
                            }

                            // Simpan nama berkas unik ke atribut Foto pada objek company
                            company.Foto = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid()}_{Path.GetFileName(registrationDto.FotoCompany.FileName)}";
                        }

                        // Handle konfirmasi password
                        if (registrationDto.ConfirmPassword != registrationDto.Password)
                        {
                            return BadRequest(new ResponseErrorHandler
                            {
                                Code = StatusCodes.Status400BadRequest,
                                Status = HttpStatusCode.BadRequest.ToString(),
                                Message = "Password and Confirm Password do not match."
                            });
                        }

                        // Simpan Employee dalam repository
                        _companyRepository.Create(company);

                        // Simpan foto ke sistem file jika ada
                        if (photoBytes != null)
                        {
                            string uploadPath = "Utilities/File/FotoCompany/"; // Ganti dengan direktori yang sesuai
                            string filePath = Path.Combine(uploadPath, company.Foto);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                fileStream.Write(photoBytes, 0, photoBytes.Length);
                            }
                        }

                        //hubungkan vendor dengan company
                        vendor.Guid = company.Guid;
                        _vendorRepository.Create(vendor);

                        // Hubungkan Account dengan Employee pemilik
                        account.Guid = company.Guid;
                        account.RoleGuid = _roleRepository.GetDefaultGuid() ?? throw new Exception("Default role not found");
                        account.Password = HashHandler.HashPassword(registrationDto.Password);

                        // Simpan Account dalam repository
                        _accountRepository.Create(account);

                        // Commit transaksi jika semua operasi berhasil
                        transactionScope.Complete();

                        return Ok(new ResponseOKHandler<string>("Registration successful, Waiting for Admin Approval"));
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaksi jika terjadi kesalahan
                        return BadRequest(new ResponseErrorHandler
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Registration failed. " + ex.Message
                        });
                    }
                }
            }

            return BadRequest(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid request data."
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto request)
        {
            try
            {
                // Validasi input data menggunakan ModelState
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Invalid input!"
                    });
                }

                // Cari pengguna (akun) berdasarkan alamat email
                var user = _accountRepository.GetByCompanyEmail(request.Email);
                var company = _companyRepository.GetByCompanyEmail(request.Email);

                if (user == null || company == null || !HashHandler.VerifyPassword(request.Password, user.Password))
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Account or Password is invalid!",
                    });
                }

                var account = _accountRepository.GetByGuid(company.Guid);
                var vendor = _vendorRepository.GetByGuid(company.Guid);

                var claims = new List<Claim>
                {
                    new Claim("Email", company.Email),
                    new Claim("Name", string.Concat(company.Name)),
                    new Claim("Foto", company.Foto ?? ""),
                    new Claim("StatusAccount", account.Status.ToString() ?? "")
                };

                // Memeriksa apakah entitas Company memiliki Vendor
                if (vendor != null && vendor.StatusVendor != null)
                {
                    claims.Add(new Claim("StatusVendor", vendor.StatusVendor.ToString() ?? ""));
                }
                else
                {
                    // Jika tidak ada Vendor, Anda bisa memberikan nilai default atau tidak menyertakan klaim ini
                    claims.Add(new Claim("StatusVendor", "DefaultValueForNoVendor"));
                }

                // Menggunakan RoleRepository untuk mendapatkan peran yang sesuai dengan akun
                var role = _roleRepository.GetByGuid(user.RoleGuid);

                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                // Menambahkan klaim GUID karyawan ke dalam token
                var companyGuidClaim = new Claim("CompanyGuid", company.Guid.ToString());
                claims.Add(companyGuidClaim);

                var generateToken = _tokenHandler.Generate(claims);

                // Jika validasi berhasil, kirim respons OK dengan pesan login berhasil
                return Ok(new ResponseOKHandler<object>("Login Success", new { Token = generateToken }));
            }
            catch (Exception ex)
            {
                // Tangani pengecualian dan kembalikan respons 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error during login",
                    Error = ex.Message
                });
            }
        }

        // GET api/account/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            try
            {
                // Memanggil metode GetByGuid dari _accountRepository dengan parameter GUID.
                var result = _accountRepository.GetByGuid(guid);

                if (result is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                // Mengembalikan data yang ditemukan dalam respons OK.
                return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to retrieve data",
                    Error = ex.Message
                });
            }
        }

        // PUT api/account
        [HttpPut]
        public IActionResult Update(AccountDto accountDto)
        {
            try
            {
                //get data by guid dan menggunakan format DTO 
                var entity = _accountRepository.GetByGuid(accountDto.Guid);
                if (entity is null) //cek apakah data berdasarkan guid tersedia 
                {

                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data Not Found"
                    });
                }
                //convert data DTO dari inputan user menjadi objek Account
                Account toUpdate = accountDto;
                //menyimpan createdate yg lama
                toUpdate.CreatedDate = entity.CreatedDate;
                toUpdate.Password = entity.Password;
                toUpdate.RoleGuid = entity.RoleGuid;

                //update Account dalam repository
                _accountRepository.Update(toUpdate);

                // return HTTP OK dengan kode status 200 dan return "data updated" untuk sukses update.
                return Ok(new ResponseOKHandler<string>("Data Updated"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to Update data",
                    Error = ex.Message
                });
            }
        }

    }
}
