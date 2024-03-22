using Finals.Enums;
using Finals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finals.Contexts;

public class ApplicationDbContext : IdentityUserContext<ApplicationUser>
{
    private readonly ILogger<ApplicationDbContext> _logger;
    private readonly IConfiguration _configuration;
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Log> Logs => Set<Log>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger,
        IConfiguration configuration) : base(options)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        _logger.LogInformation("Using connection string: {ConnectionString}", connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        try
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var accountantEmail =
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SiteSettings")[
                    "AccountantEmail"];
            var accountantPassword =
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SiteSettings")[
                    "AccountantPassword"];

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.ApplicationUser)
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.ApplicationUserId)
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "80c8b6b1-e2b6-45e8-b044-8f2178a90111",
                    UserName = "Accountant",
                    FirstName = "Giorgi",
                    LastName = "Ubiria",
                    Age = 20,
                    Salary = 1500,
                    NormalizedUserName = accountantEmail.ToUpper(),
                    PasswordHash = hasher.HashPassword(null, accountantPassword),
                    Email = accountantEmail,
                    NormalizedEmail = accountantEmail.ToUpper(),
                    Role = Role.Accountant
                }
            );
            _logger.LogInformation("Created an accountant user.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while configuring the model.");
            throw;
        }
    }
}