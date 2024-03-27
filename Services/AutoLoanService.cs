using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Helpers;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.EntityFrameworkCore;

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

        if (autoLoanRequestDto.LoanPeriod != LoanPeriod.TenYears)
        {
            throw new InvalidOperationException("Auto loan can only be of length - 10 years.");
        }

        if (price > user.Salary * 10)
        {
            throw new InvalidOperationException(
                "User's salary is insufficient to afford the loan for the selected car.");
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
            FinalAmount = Helpers.LoanCalculator.CalculateFinalAmount((int)price, autoLoanRequestDto.LoanPeriod),
            AmountLeft = Helpers.LoanCalculator.CalculateFinalAmount((int)price, autoLoanRequestDto.LoanPeriod),
            LoanPeriod = autoLoanRequestDto.LoanPeriod,
            LoanCurrency = Currency.GEL,
            LoanType = LoanType.AUTO,
            LoanStatus = LoanStatus.PENDING,
            ApplicationUser = user,
            ApplicationUserId = user.Id,
            Product = null,
            ProductId = null,
            CarId = car.Id,
            Car = car,
            ApplicationUserEmail = user.Email,
        };

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync();

        return (car, loan);
    }

    public async Task<Loan> ModifyAutoLoan(int id, AutoLoanRequestDto autoLoanRequestDto)
    {
        try
        {
            _logger.LogInformation("Attempting to modify loan with ID: {LoanId}", id);

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                throw new InvalidOperationException("User not found.");
            }

            var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null)
            {
                _logger.LogInformation("Loan not found.");
                throw new InvalidOperationException("Loan not found.");
            }

            if (loan.LoanType != LoanType.AUTO)
            {
                _logger.LogInformation("Cannot modify loans that are not of type AUTO.");
                throw new InvalidOperationException("Cannot modify loans that are not of type AUTO.");
            }

            if (user.Role == Role.Customer &&
                (loan.ApplicationUserId != user.Id || loan.LoanStatus != LoanStatus.PENDING))
            {
                _logger.LogInformation("Operation not allowed.");
                throw new InvalidOperationException("Operation not allowed.");
            }

            var carModel = await _dbContext.Cars.FindAsync(loan.CarId);
            if (carModel == null)
            {
                throw new InvalidOperationException("Car not found.");
            }

            if (user.Role == Role.Accountant || loan.LoanStatus == LoanStatus.PENDING)
            {
                decimal price = await _gptService.GetCarPriceAsync(autoLoanRequestDto.Model);
                
                if (price > user.Salary * 10)
                {
                    throw new InvalidOperationException(
                        "User's salary is insufficient to afford the loan for the selected car.");
                }

                var car = new Car
                {
                    Model = autoLoanRequestDto.Model,
                    Price = price
                };

                _dbContext.Cars.Add(car);
                
                await _dbContext.SaveChangesAsync();

                loan.LoanPeriod = autoLoanRequestDto.LoanPeriod;
                loan.RequstedAmount = (int)price;
                loan.CarId = car.Id;
                loan.FinalAmount =
                    Helpers.LoanCalculator.CalculateFinalAmount((int)price, autoLoanRequestDto.LoanPeriod);
                loan.AmountLeft =
                    Helpers.LoanCalculator.CalculateFinalAmount((int)price, autoLoanRequestDto.LoanPeriod);
                loan.Car = car;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Loan modified successfully.");
                
                return loan;
            }

            _logger.LogInformation("Operation not allowed.");
            throw new InvalidOperationException("Operation not allowed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while modifying loan with ID: {LoanId}. Error: {ErrorMessage}", id,
                ex.Message);
            throw;
        }
    }
}