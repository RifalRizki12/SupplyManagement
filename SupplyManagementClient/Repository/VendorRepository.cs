using Newtonsoft.Json;
using SupplyManagementAPI.DTOs.Vendors;
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Handler;
using SupplyManagementClient.Contract;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace SupplyManagementClient.Repository
{
    public class VendorRepository : GeneralRepository<Vendor, Guid>, IVendorRepository
    {
        public VendorRepository(string request = "Vendor/") : base(request)
        {

        }

        public async Task<object> UpdateVendor(VendorDto clientDto)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(clientDto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"{request}updateVendor", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<Vendor>>(apiResponse);
                        return entityVM;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine(apiResponse);
                        dynamic dynamicResponse = JsonConvert.DeserializeObject(apiResponse);

                        // Handle pesan kesalahan validasi di sini
                        if (dynamicResponse != null)
                        {
                            var errors = dynamicResponse.error.ToObject<List<string>>() ?? "";
                            var errorString = string.Join("\n", errors);

                            // Mengembalikan objek ResponseErrorHandler dengan kesalahan validasi
                            try
                            {
                                var errorResponse = new ResponseErrorHandler
                                {
                                    Code = dynamicResponse.code,
                                    Status = dynamicResponse.status,
                                    Message = dynamicResponse.message,
                                    Error = errorString
                                };
                                return errorResponse;
                            }
                            catch (Exception ex)
                            {
                                // Tampilkan pesan pengecualian ke konsol atau log
                                Console.WriteLine("Exception: " + ex.Message);
                            }
                        }
                    }
                    // Handle respons lainnya seperti sebelumnya
                    return new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = "Terjadi kesalahan server. Silakan coba lagi nanti.",
                        Error = null
                    };
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                throw; // Consider whether re-throwing the exception is the best course of action
            }
        }

        public async Task<ResponseOKHandler<UpdateVendorDto>> UpdateStatusVendor(Guid id, UpdateVendorDto entity)
        {
            string requestUrl = "PutVendorByAdmin";
            ResponseOKHandler<UpdateVendorDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request + requestUrl, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<UpdateVendorDto>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseOKHandler<VendorDto>> GetGuidVendor(Guid guid)
        {

            ResponseOKHandler<VendorDto> entity = null;

            using (var response = await httpClient.GetAsync(request + guid))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ResponseOKHandler<VendorDto>>(apiResponse);
            }
            return entity;
        }
    }
}
