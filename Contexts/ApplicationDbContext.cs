using Finals.Enums;
using Finals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finals.Contexts;

public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
{
    public DbSet<Loan> Loans => Set<Loan>();
    
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var hasher = new PasswordHasher<ApplicationUser>();

        var accountantEmail = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SiteSettings")["AccountantEmail"];
        var accountantPassword = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SiteSettings")["AccountantPassword"];
        
        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                UserName = "Accountant",
                NormalizedUserName = accountantEmail.ToUpper(),
                PasswordHash = hasher.HashPassword(null, accountantPassword),
                Email = accountantEmail,
                NormalizedEmail = accountantEmail.ToUpper(),
                Role = Role.Accountant
            }
        );
    }
}