using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Helpers;
using Finals.Interfaces;
using Finals.Models;

namespace Finals.Services;

public class AutoLoanService : IAutoLoanService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AutoLoanService> _logger;
    private readonly IGptService _gptService;
    private readonly GetUserFromContext _getUserFromContext;

    public AutoLoanService(ApplicationDbContext dbContext, ILogger<AutoLoanService> logger, IGptService gptService,
        GetUserFromContext getUserFromContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _gptService = gptService;
        _getUserFromContext = getUserFromContext;
    }

    public async Task<(Car car, Loan loan)> GetCarAndLoanAsync(AutoLoanRequestDto autoLoanRequestDto)
    {
        var user = await _getUserFromContext.GetUser();
        if (user == null || user.IsBlocked)
        {
            throw new InvalidOperationException("User not found or is blocked.");
        }

        decimal price = await _gptService.GetCarPriceAsync(autoLoanRequestDto.Model);

        if (price > user.Salary * 10)
        {
            throw new InvalidOperationException(
                "User's salary is insufficient to afford the loan for the selected car.");
        }

        if (autoLoanRequestDto.LoanPeriod == LoanPeriod.HalfYear)
        {
            throw new InvalidOperationException("Can't get an Auto Loan for less than a 1 year period.");
        }

        var car = new Car
        {
            Model = autoLoanRequestDto.Model,
            Price = price
        };

        _dbContext.Cars.Add(car);
        await _dbContext.SaveChangesAsync();

        var loan = new Loan
        {
            RequstedAmount = (int)price,
            FinalAmount = (int)price,
            LoanPeriod = autoLoanRequestDto.LoanPeriod,
            LoanCurrency = Currency.GEL,
            LoanType = LoanType.AUTO,
            LoanStatus = LoanStatus.PENDING,
            ApplicationUser = user,
            Product = null,
            ProductId = null,
            CarId = car.Id,
            Car = car
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return (car, loan);
    }
}