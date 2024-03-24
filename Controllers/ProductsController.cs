using Asp.Versioning;
using Finals.Dtos;
using Finals.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Controllers;

[ApiVersion(1.0)]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        _logger.LogInformation("Attempting to retrieve all products.");

        var productsDto = await _productService.GetAllProducts();

        if (productsDto == null || productsDto.Products.Count == 0)
        {
            _logger.LogInformation("No products found.");
            return NoContent();
        }

        _logger.LogInformation("All products retrieved successfully.");
        return Ok(productsDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        _logger.LogInformation("Attempting to retrieve product with ID: {id}", id);

        var productDto = await _productService.GetProduct(id);
        if (productDto == null)
        {
            _logger.LogInformation("Product with ID {id} not found.", id);
            return NotFound();
        }

        _logger.LogInformation("Product with ID {id} retrieved successfully.", id);
        return productDto;
    }
}