using Finals.Dtos;

namespace Finals.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetProduct(int id);
    Task<ProductsDto> GetAllProducts();
}