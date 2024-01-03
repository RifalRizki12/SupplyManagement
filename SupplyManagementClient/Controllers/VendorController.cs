using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplyManagementAPI.DTOs.Accounts;
using SupplyManagementAPI.DTOs.Vendors;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Handler;
using SupplyManagementClient.Contract;

namespace SupplyManagementClient.Controllers
{
    public class VendorController : Controller
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorController(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        [AllowAnonymous]
        [HttpPut("/Vendor/updateVendor")]
        public async Task<IActionResult> UpdateVendor(VendorDto updateDto)
        {
            try
            {
                var result = await _vendorRepository.UpdateVendor(updateDto);

                if (result is ResponseOKHandler<Vendor> successResult)
                {
                    // Pembaruan berhasil
                    return Ok(new { status = "OK", message = successResult });
                }
                else if (result is ResponseErrorHandler errorResult)
                {
                    // Pembaruan gagal atau ada kesalahan
                    return BadRequest(new { status = "Error", message = errorResult });
                }
                return BadRequest(new { success = false, message = "Data tidak valid." });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    status = "Error",
                    message = "Terjadi kesalahan server. Silakan coba lagi nanti.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("Vendor/PutVendorByAdmin/{guid}")]
        public async Task<JsonResult> PutVendorByAdmin(Guid guid, [FromBody] UpdateVendorDto vendorDto)
        {
            var response = await _vendorRepository.UpdateStatusVendor(guid, vendorDto);

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
                return Json(new { error = "An error occurred while updating the Vendor." });
            }
        }

        [AllowAnonymous]
        [HttpGet("Vendor/{guid}")]
        public async Task<JsonResult> GetGuidVendor(Guid guid)
        {
            var result = await _vendorRepository.GetGuidVendor(guid);
            var vendor = new VendorDto();

            if (result.Data?.Guid != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(vendor);
            }
        }

    }
}
