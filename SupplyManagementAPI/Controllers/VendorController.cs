using Microsoft.AspNetCore.Mvc;
using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.DTOs.Accounts;
using SupplyManagementAPI.DTOs.Vendors;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Handler;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorController(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        // GET api/vendor
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _vendorRepository untuk mendapatkan semua data vendor.
            var result = _vendorRepository.GetAll();

            if (!result.Any())
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Vendor Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (VendorDto)x);

            // Mengembalikan data yang ditemukan dalam respons OK.
            return Ok(new ResponseOKHandler<IEnumerable<VendorDto>>(data));
        }

        // GET api/vendor/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _vendorRepository dengan parameter GUID.
            var result = _vendorRepository.GetByGuid(guid);

            if (result is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Vendor Data with Specific GUID Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object).
            return Ok(new ResponseOKHandler<VendorDto>((VendorDto)result));
        }

        // PUT api/vendor
        [HttpPut]
        public IActionResult Update(VendorDto vendorDto)
        {
            try
            {
                // Memeriksa apakah entitas vendor yang akan diperbarui ada dalam database.
                var entity = _vendorRepository.GetByGuid(vendorDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Vendor with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Vendor toUpdate = vendorDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _vendorRepository untuk memperbarui data vendor.
                _vendorRepository.Update(toUpdate);


                // Mengembalikan pesan sukses dalam respons OK.
                return Ok(new ResponseOKHandler<string>("Data Has Been Update !"));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to update data",
                    Error = ex.Message
                });
            }
        }

        // PUT api/vendor
        [HttpPut("PutVendorByAdmin")]
        public IActionResult UpdateVendor(UpdateVendorDto vendorDto)
        {
            try
            {
                // Memeriksa apakah entitas vendor yang akan diperbarui ada dalam database.
                var entity = _vendorRepository.GetByGuid(vendorDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Vendor with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Vendor toUpdate = vendorDto;
                toUpdate.BidangUsaha = entity.BidangUsaha;
                toUpdate.JenisPerusahaan = entity.JenisPerusahaan;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _vendorRepository untuk memperbarui data vendor.
                _vendorRepository.Update(toUpdate);

                // Mengembalikan pesan sukses dalam respons OK.
                return Ok(new ResponseOKHandler<string>("Data Has Been Update !"));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to update data",
                    Error = ex.Message
                });
            }
        }
    }
}
