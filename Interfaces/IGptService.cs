namespace Finals.Interfaces;

public interface IGptService
{
    Task<decimal> GetCarPriceAsync(string model);
}