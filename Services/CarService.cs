using Finals.Contexts;
using Finals.Interfaces;
using Finals.Models;

namespace Finals.Services;

public class CarService : ICarService
{
    private readonly ApplicationDbContext _dbContext;

    public CarService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Car> CreateCar(string model, decimal price)
    {
        var car = new Car
        {
            Model = model,
            Price = price
        };

        _dbContext.Cars.Add(car);
        await _dbContext.SaveChangesAsync();

        return car;
    }
}