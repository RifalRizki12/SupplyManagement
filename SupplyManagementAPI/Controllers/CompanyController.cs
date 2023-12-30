using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.DTOs.Companies;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Repositories;
using SupplyManagementAPI.Utilities.Enums;
using SupplyManagementAPI.Utilities.Handler;
using System.Net;
using System.Numerics;

namespace SupplyManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {

        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IVendorRepository _vendorRepository;



        public CompanyController(IAccountRepository accountRepository,
            IVendorRepository vendorRepository, ICompanyRepository companyRepository)
        {

            _accountRepository = accountRepository;
            _companyRepository = companyRepository;
            _vendorRepository = vendorRepository;



        }

        [HttpGet("allCompany-detailsNone")]
        public IActionResult GetAllCompanyDetailsNone()
        {
            try
            {

                // pengambilan data dari tabel Company
                var companies = _companyRepository.GetAll();

                // pengambilan data dari tabel Account
                var accounts = _accountRepository.GetAll();
                var vendors = _vendorRepository.GetAll();

                // Gabungkan data dari tabel sesuai dengan hubungannya
                var clientDetails = (from com in companies
                                     join ven in vendors on com.Guid equals ven.Guid
                                     join acc in accounts on com.Guid equals acc.Guid
                                     where acc.Status == StatusLevel.Requested
                                     select new CompanyDetailDto
                                     {
                                         CompanyGuid = com.Guid,
                                         NameCompany = com.Name,
                                         Address = com.Address,
                                         Email = com.Email,
                                         Foto = com.Foto,
                                         PhoneNumber = com.PhoneNumber,
                                         StatusAccount = acc.Status.ToString(),
                                         StatusVendor = ven.StatusVendor.ToString(),
                                         VendorGuid = ven.Guid,
                                         BidangUsaha = ven.BidangUsaha,
                                         JenisPerusahaan = ven.JenisPerusahaan,
                                         
                                     }).ToList();

                

                return Ok(new ResponseOKHandler<IEnumerable<CompanyDetailDto>>(clientDetails));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to retrieve client details. " + ex.Message
                });
            }
        }

        [HttpGet("company-DetailsWaiting")]
        public IActionResult GetAllCompanyWaiting()
        {
            try
            {

                // pengambilan data dari tabel Company
                var companies = _companyRepository.GetAll();

                // pengambilan data dari tabel Account
                var accounts = _accountRepository.GetAll();
                var vendors = _vendorRepository.GetAll();

                // Gabungkan data dari tabel sesuai dengan hubungannya
                var clientDetails = (from com in companies
                                     join ven in vendors on com.Guid equals ven.Guid
                                     join acc in accounts on com.Guid equals acc.Guid
                                     where acc.Status == StatusLevel.Approved && ven.StatusVendor == StatusVendor.waiting
                                     select new CompanyDetailDto
                                     {
                                         CompanyGuid = com.Guid,
                                         NameCompany = com.Name,
                                         Address = com.Address,
                                         Email = com.Email,
                                         Foto = com.Foto,
                                         PhoneNumber = com.PhoneNumber,
                                         StatusAccount = acc.Status.ToString(),
                                         StatusVendor = ven.StatusVendor.ToString(),
                                         VendorGuid = ven.Guid,
                                         BidangUsaha = ven.BidangUsaha,
                                         JenisPerusahaan = ven.JenisPerusahaan,

                                     }).ToList();



                return Ok(new ResponseOKHandler<IEnumerable<CompanyDetailDto>>(clientDetails));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to retrieve client details. " + ex.Message
                });
            }
        }

        [HttpGet("company-ApproveByAdmin")]
        public IActionResult GetCompanyApproveByAdmin()
        {
            try
            {

                // pengambilan data dari tabel Company
                var companies = _companyRepository.GetAll();

                // pengambilan data dari tabel Account
                var accounts = _accountRepository.GetAll();
                var vendors = _vendorRepository.GetAll();

                // Gabungkan data dari tabel sesuai dengan hubungannya
                var clientDetails = (from com in companies
                                     join ven in vendors on com.Guid equals ven.Guid
                                     join acc in accounts on com.Guid equals acc.Guid
                                     where acc.Status == StatusLevel.Approved && ven.StatusVendor == StatusVendor.approvedByAdmin
                                     select new CompanyDetailDto
                                     {
                                         CompanyGuid = com.Guid,
                                         NameCompany = com.Name,
                                         Address = com.Address,
                                         Email = com.Email,
                                         Foto = com.Foto,
                                         PhoneNumber = com.PhoneNumber,
                                         StatusAccount = acc.Status.ToString(),
                                         StatusVendor = ven.StatusVendor.ToString(),
                                         VendorGuid = ven.Guid,
                                         BidangUsaha = ven.BidangUsaha,
                                         JenisPerusahaan = ven.JenisPerusahaan,

                                     }).ToList();

                return Ok(new ResponseOKHandler<IEnumerable<CompanyDetailDto>>(clientDetails));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to retrieve client details. " + ex.Message
                });
            }
        }

        [HttpGet("company-ApproveByManager")]
        public IActionResult GetCompanyApproveByManager()
        {
            try
            {

                // pengambilan data dari tabel Company
                var companies = _companyRepository.GetAll();

                // pengambilan data dari tabel Account
                var accounts = _accountRepository.GetAll();
                var vendors = _vendorRepository.GetAll();

                // Gabungkan data dari tabel sesuai dengan hubungannya
                var clientDetails = (from com in companies
                                     join ven in vendors on com.Guid equals ven.Guid
                                     join acc in accounts on com.Guid equals acc.Guid
                                     where acc.Status == StatusLevel.Approved && ven.StatusVendor == StatusVendor.approvedByManager
                                     select new CompanyDetailDto
                                     {
                                         CompanyGuid = com.Guid,
                                         NameCompany = com.Name,
                                         Address = com.Address,
                                         Email = com.Email,
                                         Foto = com.Foto,
                                         PhoneNumber = com.PhoneNumber,
                                         StatusAccount = acc.Status.ToString(),
                                         StatusVendor = ven.StatusVendor.ToString(),
                                         VendorGuid = ven.Guid,
                                         BidangUsaha = ven.BidangUsaha,
                                         JenisPerusahaan = ven.JenisPerusahaan,

                                     }).ToList();

                return Ok(new ResponseOKHandler<IEnumerable<CompanyDetailDto>>(clientDetails));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to retrieve client details. " + ex.Message
                });
            }
        }
    }

}