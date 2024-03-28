using Finals.Controllers;
using Finals.Dtos;
using Finals.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Test.Controllers;

using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

[TestFixture]
public class ProductsControllerTests
{
    private Mock<IProductService> _mockProductService;
    private Mock<ILogger<ProductsController>> _mockLogger;
    private ProductsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockProductService = new Mock<IProductService>();
        _mockLogger = new Mock<ILogger<ProductsController>>();
        _controller = new ProductsController(_mockProductService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetAllProducts_ReturnsNoContent_WhenNoProducts()
    {
        _mockProductService.Setup(service => service.GetAllProducts()).ReturnsAsync((ProductsDto)null);

        var result = await _controller.GetAllProducts();

        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task GetAllProducts_ReturnsOk_WhenProductsExist()
    {
        var productsDto = new ProductsDto { Products = new List<ProductDto> { new ProductDto() } };
        _mockProductService.Setup(service => service.GetAllProducts()).ReturnsAsync(productsDto);

        var result = await _controller.GetAllProducts();

        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.AreEqual(productsDto, okResult.Value);
    }

    [Test]
    public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
    {
        int id = 1;
        _mockProductService.Setup(service => service.GetProduct(id)).ReturnsAsync((ProductDto)null);

        var result = await _controller.GetProduct(id);

        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task GetProduct_ReturnsProduct_WhenProductExists()
    {
        int id = 1;
        var productDto = new ProductDto { Id = id, Title = "Test Product", Price = 20 };
        _mockProductService.Setup(service => service.GetProduct(id)).ReturnsAsync(productDto);

        var result = await _controller.GetProduct(id);

        Assert.IsInstanceOf<ActionResult<ProductDto>>(result);
        var actionResult = result as ActionResult<ProductDto>;
        Assert.AreEqual(productDto, actionResult.Value);
    }
}