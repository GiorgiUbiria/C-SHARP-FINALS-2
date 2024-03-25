using System.Text.Json;
using Finals.Dtos;
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
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Car> Cars => Set<Car>();

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
            
            modelBuilder.Entity<Loan>()
                .ToTable("Loan")
                .HasOne(l => l.Product)
                .WithOne()
                .HasForeignKey<Loan>(l => l.ProductId)
                .IsRequired(false);

            modelBuilder.Entity<Loan>()
                .ToTable("Loan")
                .HasOne(l => l.Car)
                .WithOne()
                .HasForeignKey<Loan>(l => l.CarId)
                .IsRequired(false);

            modelBuilder.Entity<Loan>()
                .ToTable("Loan", tbl => tbl.HasCheckConstraint("CK_Loan_Product_For_Installment", "((LoanType = 2 AND ProductId IS NOT NULL) OR LoanType != 2)"));

            modelBuilder.Entity<Loan>()
                .ToTable("Loan", tbl => tbl.HasCheckConstraint("CK_Loan_Car_For_CarLoan", "((LoanType = 1 AND CarId IS NOT NULL) OR LoanType != 1)"));
            
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

            var productsTask = FetchProductsFromExternalApi();
            var products = productsTask.Result;
            
            foreach(var product in products)
            {
                modelBuilder.Entity<Product>().HasData(
                    new Product
                    {
                        Id = product.Id,
                        Price = product.Price,
                        Title = product.Title
                    }
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while configuring the model.");
            throw;
        }
    }

    private async Task<IEnumerable<Product>> FetchProductsFromExternalApi()
    {
        var products = new List<Product>();

        using (var httpClient = new HttpClient())
        {
            try
            {
                var response =
                    await httpClient.GetAsync("https://fakestoreapi.com/products/category/electronics?limit=50");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonProducts = JsonSerializer.Deserialize<IEnumerable<ExternalProductDto>>(jsonString);

                foreach (var item in jsonProducts)
                {
                    var product = new Product
                    {
                        Id = item.id,
                        Title = item.title,
                        Price = item.price,
                    };
                    products.Add(product);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching products from the external API: {ErrorMessage}", ex.Message);
            }
        }

        return products;
    }
}