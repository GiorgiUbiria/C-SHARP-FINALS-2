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

    public ProductService(ApplicationDbContext dbContext, ILogger<ProductService> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<ProductDto> GetProduct(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve product with ID: {ProductId}", id);

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

            var products = _dbContext.Products.AsQueryable();
            if (products == null || !products.Any())
            {
                _logger.LogInformation("No products found.");
                return null;
            }

            var productsDto = new ProductsDto
            {
                Products = products.Select(product => new ProductDto
                {
                    Id = product.Id,
                    Price = product.Price,
                    Title = product.Title
                }).ToList()
            };

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