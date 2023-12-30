namespace SupplyManagementAPI.DTOs.Tokens
{
    public class ClaimsDto
    {
        public Guid CompanyGuid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string StatusAccount { get; set; }
        public string StatusVendor { get; set; }
        public string? Foto { get; set; }
        public List<string> Role { get; set; }
    }
}
