using Finals.Models;

namespace Finals.Interfaces;

public interface ICarService
{
    Task<Car> CreateCar(string model, decimal price);
}