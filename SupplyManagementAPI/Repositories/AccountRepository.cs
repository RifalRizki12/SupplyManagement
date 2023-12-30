
using SupplyManagementAPI.Models;
using SupplyManagementAPI.Contracts;
using SupplyManagementAPI.Data;

namespace SupplyManagementAPI.Repositories
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        private readonly SupplyManagementDbContext _context;

        // Konstruktor AccountRepository yang menerima BookingManagementDbContext
        public AccountRepository(SupplyManagementDbContext context) : base(context)
        {
            _context = context;
        }

        // Metode untuk mendapatkan akun berdasarkan alamat email karyawan
        public Account GetByCompanyEmail(string companyEmail)
        {
            // Melakukan join antara tabel Akun (Account) dan Employee berdasarkan email Employee
            var account = _context.Accounts
                .Join(
                    _context.Companies,
                    account => account.Guid,
                    company => company.Guid,
                    (account, company) => new
                    {
                        Account = account,
                        Company = company
                    }
                )
                // Memfilter hasil join berdasarkan alamat email karyawan
                .Where(joinResult => joinResult.Company.Email == companyEmail)
                // Memilih objek akun sebagai hasil akhir
                .Select(joinResult => joinResult.Account)
                .FirstOrDefault();

            return account;
        }
    }
}
