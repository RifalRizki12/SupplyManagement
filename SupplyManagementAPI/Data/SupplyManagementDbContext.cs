using SupplyManagementAPI.Models;
using SupplyManagementAPI.Utilities.Enums;
using SupplyManagementAPI.Utilities.Handler;
using Microsoft.EntityFrameworkCore;
using SupplyManagementAPI.Models;

namespace SupplyManagementAPI.Data
{
    public class SupplyManagementDbContext : DbContext
    {
        public SupplyManagementDbContext(DbContextOptions<SupplyManagementDbContext> options) : base(options) { }

        //add model to migrate
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Person> Persons { get; set; }

        //pembutan method overrid untuk atribut uniq
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Company>().HasIndex(e => e.PhoneNumber).IsUnique();
            modelBuilder.Entity<Person>().HasIndex(e => e.Email).IsUnique();

            modelBuilder.Entity<Company>()
                .HasOne(v => v.Vendor)
                .WithOne(c => c.Company)
                .HasForeignKey<Vendor>(v => v.Guid)
                .OnDelete(DeleteBehavior.Restrict);
                        
            modelBuilder.Entity<Company>()
                .HasOne(a => a.Account)
                .WithOne(c => c.Company)
                .HasForeignKey<Account>(a => a.Guid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
            .HasMany(ar => ar.Accounts)
            .WithOne(e => e.Role)
            .HasForeignKey(ar => ar.RoleGuid)
            .OnDelete(DeleteBehavior.Restrict);

            SeedEmployeeAndAccount(modelBuilder);
            SeedRole(modelBuilder);
        }

        private void SeedRole(ModelBuilder modelBuilder)
        {
            var roleId = Guid.NewGuid();

            var role = new Role
            {
                Guid = roleId,
                Name = "vendor",
            };

            modelBuilder.Entity<Role>().HasData(role);
        }

        private void SeedEmployeeAndAccount(ModelBuilder modelBuilder)
        {
            var roleId = Guid.NewGuid();
            var roleId2 = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var accountId2 = Guid.NewGuid();

            var role = new Role
            {
                Guid = roleId,
                Name = "admin",
                // Set properti untuk role
            };

            var role2 = new Role
            {
                Guid = roleId2,
                Name = "manager",
                // Set properti untuk role
            };

            var account = new Account
            {
                Guid = accountId,
                Password = HashHandler.HashPassword("Admin12345"),
                RoleGuid = roleId,
                Status = StatusLevel.Approved,
                // Set properti untuk account
            };

            var account2 = new Account
            {
                Guid = accountId2,
                Password = HashHandler.HashPassword("Manager12345"),
                RoleGuid = roleId2,
                Status = StatusLevel.Approved,
                // Set properti untuk account
            };

            var company = new Company
            {
                Guid = accountId,
                Name = "Admin",
                Email = "admin@mail.com",
                Address= "null",
                PhoneNumber = "00000",
                Foto = "null",
                CreatedDate = DateTime.Now,
            };

            var company2 = new Company
            {
                Guid = accountId2,
                Name = "Manager Logistic",
                Email = "manager@mail.com",
                Address = "null",
                PhoneNumber = "11111",
                Foto = "nul",
                CreatedDate = DateTime.Now,
            };

            modelBuilder.Entity<Role>().HasData(role);
            modelBuilder.Entity<Account>().HasData(account);
            modelBuilder.Entity<Company>().HasData(company);

            modelBuilder.Entity<Role>().HasData(role2);
            modelBuilder.Entity<Account>().HasData(account2);
            modelBuilder.Entity<Company>().HasData(company2);
        }

    }
}
