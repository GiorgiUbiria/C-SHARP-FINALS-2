using Finals.Contexts;
using Finals.Dtos;
using Finals.Helpers;
using Finals.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Finals.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ProductService> _logger;
    private readonly GetUserFromContext _getUserFromContext;

    public ProductService(ApplicationDbContext dbContext, ILogger<ProductService> logger,
        GetUserFromContext getUserFromContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _getUserFromContext = getUserFromContext;
    }

    public async Task<ProductDto> GetProduct(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve product with ID: {ProductId}", id);

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                _logger.LogInformation("Product not found.");
                return null;
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price
            };

            _logger.LogInformation("Product with ID {ProductId} retrieved successfully.", id);
            return productDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving product with ID: {ProductId}. Error: {ErrorMessage}",
                id, ex.Message);
            throw;
        }
    }

    public async Task<ProductsDto> GetAllProducts()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve all products.");

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            var products = _dbContext.Products.AsQueryable();
            if (products == null || !products.Any())
            {
                _logger.LogInformation("No products found.");
                return null;
            }

            var productsDto = new ProductsDto();
            
            foreach (var product in products)
            {
                var productDto = new ProductDto() 
                {
                    Id = product.Id,
                    Price = product.Price,
                    Title = product.Title
                };

                productsDto.Products.Add(productDto);
            }
            _logger.LogInformation("All products retrieved successfully.");
            return productsDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all products. Error: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}