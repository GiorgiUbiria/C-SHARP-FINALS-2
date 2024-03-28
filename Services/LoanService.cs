using Finals.Contexts;
using Finals.Dtos;
using Finals.Enums;
using Finals.Helpers;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.EntityFrameworkCore;

namespace Finals.Services;

public class LoanService : ILoanService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<LoanService> _logger;
    private readonly GetUserFromContext _getUserFromContext;

    public LoanService(ApplicationDbContext dbContext,
        ILogger<LoanService> logger, GetUserFromContext getUserFromContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _getUserFromContext = getUserFromContext;
    }

    public async Task<LoanDto> GetLoan(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve loan with ID: {LoanId}", id);

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            if (user.Role == Role.Accountant)
            {
                var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
                if (loan == null)
                {
                    _logger.LogInformation("Loan not found.");
                    return null;
                }

                var loanDto = new LoanDto
                {
                    Id = loan.Id,
                    RequestedAmount = loan.RequstedAmount,
                    FinalAmount = loan.FinalAmount,
                    AmountLeft = loan.AmountLeft,
                    LoanPeriod = loan.LoanPeriod,
                    LoanType = loan.LoanType,
                    LoanCurrency = loan.LoanCurrency,
                    LoanStatus = loan.LoanStatus,
                    ProductId = loan.ProductId.HasValue ? loan.ProductId.Value : default,
                    CarId = loan.CarId.HasValue ? loan.CarId.Value : default,
                    Product = loan.Product,
                    Car = loan.Car,
                    UserEmail = loan.ApplicationUserEmail
                };

                _logger.LogInformation("Loan with ID {LoanId} retrieved successfully.", id);
                return loanDto;
            }
            else if (user.Role == Role.Customer)
            {
                var loan = await _dbContext.Loans.FirstOrDefaultAsync(l =>
                    l.Id == id && l.ApplicationUserId == user.Id);

                if (loan == null)
                {
                    _logger.LogInformation("Loan not found for the current user.");
                    return null;
                }

                var loanDto = new LoanDto
                {
                    Id = loan.Id,
                    RequestedAmount = loan.RequstedAmount,
                    FinalAmount = loan.FinalAmount,
                    AmountLeft = loan.AmountLeft,
                    LoanPeriod = loan.LoanPeriod,
                    LoanType = loan.LoanType,
                    LoanCurrency = loan.LoanCurrency,
                    LoanStatus = loan.LoanStatus,
                    ProductId = loan.ProductId.HasValue ? loan.ProductId.Value : default,
                    CarId = loan.CarId.HasValue ? loan.CarId.Value : default,
                    Product = loan.Product,
                    Car = loan.Car,
                    UserEmail = loan.ApplicationUserEmail
                };

                _logger.LogInformation("Loan with ID {LoanId} retrieved successfully for the current user.", id);
                return loanDto;
            }
            else
            {
                _logger.LogInformation("Unauthorized access.");
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving loan with ID: {LoanId}. Error: {ErrorMessage}", id,
                ex.Message);
            throw;
        }
    }

    public async Task<LoansDto> GetAllLoans()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve all loans.");

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            IQueryable<Loan> loansFromDb = Enumerable.Empty<Loan>().AsQueryable();

            if (user.Role == Role.Accountant)
            {
                loansFromDb = _dbContext.Loans.AsQueryable();
            }
            else if (user.Role == Role.Customer)
            {
                loansFromDb = _dbContext.Loans.Where(l => l.ApplicationUserId == user.Id).AsQueryable();
            }

            var loansDto = new LoansDto();

            if (loansFromDb == null || !loansFromDb.Any())
            {
                _logger.LogInformation("No loans found for the user.");
                return loansDto;
            }

            loansDto.Loans = loansFromDb.Select(loan => new LoanDto
            {
                Id = loan.Id,
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
                AmountLeft = loan.AmountLeft,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus,
                ProductId = loan.ProductId.HasValue ? loan.ProductId.Value : default,
                CarId = loan.CarId.HasValue ? loan.CarId.Value : default,
                Product = loan.Product,
                Car = loan.Car,
                UserEmail = loan.ApplicationUserEmail
            }).ToList();

            _logger.LogInformation("All loans retrieved successfully.");
            return loansDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all loans: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<LoansDto> GetPendingLoans()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve pending loans.");

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            IQueryable<Loan> loansFromDb = Enumerable.Empty<Loan>().AsQueryable();

            if (user.Role == Role.Accountant)
            {
                loansFromDb = _dbContext.Loans.Where(l => l.LoanStatus == LoanStatus.PENDING).AsQueryable();
            }
            else if (user.Role == Role.Customer)
            {
                loansFromDb = _dbContext.Loans
                    .Where(l => l.ApplicationUserId == user.Id && l.LoanStatus == LoanStatus.PENDING).AsQueryable();
            }

            var loansDto = new LoansDto();

            if (loansFromDb == null || !loansFromDb.Any())
            {
                _logger.LogInformation("No pending loans found for the user.");
                return loansDto;
            }

            loansDto.Loans = loansFromDb.Select(loan => new LoanDto
            {
                Id = loan.Id,
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
                AmountLeft = loan.AmountLeft,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus,
                ProductId = loan.ProductId.HasValue ? loan.ProductId.Value : default,
                CarId = loan.CarId.HasValue ? loan.CarId.Value : default,
                Product = loan.Product,
                Car = loan.Car,
                UserEmail = loan.ApplicationUserEmail
            }).ToList();

            _logger.LogInformation("Pending loans retrieved successfully.");
            return loansDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving pending loans: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<LoansDto> GetAcceptedLoans()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve accepted loans.");

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            IQueryable<Loan> loansFromDb = Enumerable.Empty<Loan>().AsQueryable();

            if (user.Role == Role.Accountant)
            {
                loansFromDb = _dbContext.Loans.Where(l => l.LoanStatus == LoanStatus.ACCEPTED).AsQueryable();
            }
            else if (user.Role == Role.Customer)
            {
                loansFromDb = _dbContext.Loans
                    .Where(l => l.ApplicationUserId == user.Id && l.LoanStatus == LoanStatus.ACCEPTED).AsQueryable();
            }

            var loansDto = new LoansDto();

            if (loansFromDb == null || !loansFromDb.Any())
            {
                _logger.LogInformation("No accepted loans found for the user.");
                return loansDto;
            }

            loansDto.Loans = loansFromDb.Select(loan => new LoanDto
            {
                Id = loan.Id,
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
                AmountLeft = loan.AmountLeft,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus,
                ProductId = loan.ProductId.HasValue ? loan.ProductId.Value : default,
                CarId = loan.CarId.HasValue ? loan.CarId.Value : default,
                Product = loan.Product,
                Car = loan.Car,
                UserEmail = loan.ApplicationUserEmail
            }).ToList();

            _logger.LogInformation("Accepted loans retrieved successfully.");
            return loansDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving accepted loans: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<LoansDto> GetDeclinedLoans()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve declined loans.");

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            IQueryable<Loan> loansFromDb = Enumerable.Empty<Loan>().AsQueryable();

            if (user.Role == Role.Accountant)
            {
                loansFromDb = _dbContext.Loans.Where(l => l.LoanStatus == LoanStatus.DECLINED).AsQueryable();
            }
            else if (user.Role == Role.Customer)
            {
                loansFromDb = _dbContext.Loans
                    .Where(l => l.ApplicationUserId == user.Id && l.LoanStatus == LoanStatus.DECLINED).AsQueryable();
            }

            var loansDto = new LoansDto();

            if (loansFromDb == null || !loansFromDb.Any())
            {
                _logger.LogInformation("No declined loans found for the user.");
                return loansDto;
            }

            loansDto.Loans = loansFromDb.Select(loan => new LoanDto
            {
                Id = loan.Id,
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
                AmountLeft = loan.AmountLeft,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus,
                ProductId = loan.ProductId.HasValue ? loan.ProductId.Value : default,
                CarId = loan.CarId.HasValue ? loan.CarId.Value : default,
                Product = loan.Product,
                Car = loan.Car,
                UserEmail = loan.ApplicationUserEmail
            }).ToList();

            _logger.LogInformation("Declined loans retrieved successfully.");
            return loansDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving declined loans: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<LoansDto> GetCompletedLoans()
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve completed loans.");

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                return null;
            }

            IQueryable<Loan> loansFromDb = Enumerable.Empty<Loan>().AsQueryable();

            if (user.Role == Role.Accountant)
            {
                loansFromDb = _dbContext.Loans.Where(l => l.LoanStatus == LoanStatus.COMPLETED).AsQueryable();
            }
            else if (user.Role == Role.Customer)
            {
                loansFromDb = _dbContext.Loans
                    .Where(l => l.ApplicationUserId == user.Id && l.LoanStatus == LoanStatus.COMPLETED).AsQueryable();
            }

            var loansDto = new LoansDto();

            if (loansFromDb == null || !loansFromDb.Any())
            {
                _logger.LogInformation("No completed loans found for the user.");
                return loansDto;
            }

            loansDto.Loans = loansFromDb.Select(loan => new LoanDto
            {
                Id = loan.Id,
                RequestedAmount = loan.RequstedAmount,
                FinalAmount = loan.FinalAmount,
                AmountLeft = loan.AmountLeft,
                LoanPeriod = loan.LoanPeriod,
                LoanType = loan.LoanType,
                LoanCurrency = loan.LoanCurrency,
                LoanStatus = loan.LoanStatus,
                ProductId = loan.ProductId.HasValue ? loan.ProductId.Value : default,
                CarId = loan.CarId.HasValue ? loan.CarId.Value : default,
                Product = loan.Product,
                Car = loan.Car,
                UserEmail = loan.ApplicationUserEmail
            }).ToList();

            _logger.LogInformation("Completed loans retrieved successfully.");
            return loansDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving declined loans: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<bool> DeleteLoan(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to delete loan with ID: {LoanId}", id);

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

            if (user.Role == Role.Customer &&
                (loan.ApplicationUserId != user.Id || loan.LoanStatus != LoanStatus.PENDING))
            {
                _logger.LogInformation("User does not have permission to delete the loan.");
                return false;
            }

            if (user.Role == Role.Accountant || loan.LoanStatus == LoanStatus.PENDING)
            {
                _dbContext.Loans.Remove(loan);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Loan deleted successfully.");
                return true;
            }

            _logger.LogInformation("User does not have permission to delete the loan.");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting loan with ID: {LoanId}. Error: {ErrorMessage}", id,
                ex.Message);
            throw;
        }
    }

    public async Task<bool> AcceptLoan(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to accept loan with ID: {LoanId}", id);

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                throw new InvalidOperationException("User not found.");
            }

            if (user.Role != Role.Accountant)
            {
                _logger.LogInformation("Operation not allowed.");
                throw new UnauthorizedAccessException("Only users with the Accountant role can accept loans.");
            }

            var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null)
            {
                _logger.LogInformation("Loan not found.");
                throw new InvalidOperationException("Loan not found.");
            }

            if (loan.LoanStatus != LoanStatus.PENDING)
            {
                _logger.LogInformation("Loan is not in a PENDING status.");
                throw new InvalidOperationException("Loan is not in a PENDING status.");
            }

            loan.LoanStatus = LoanStatus.ACCEPTED;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Loan accepted successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while accepting loan with ID: {LoanId}. Error: {ErrorMessage}", id,
                ex.Message);
            throw;
        }
    }

    public async Task<bool> DeclineLoan(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to decline loan with ID: {LoanId}", id);

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                throw new InvalidOperationException("User not found.");
            }

            if (user.Role != Role.Accountant)
            {
                _logger.LogInformation("Operation not allowed.");
                throw new UnauthorizedAccessException("Only users with the Accountant role can decline loans.");
            }

            var loan = await _dbContext.Loans.FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null)
            {
                _logger.LogInformation("Loan not found.");
                throw new InvalidOperationException("Loan not found.");
            }

            if (loan.LoanStatus != LoanStatus.PENDING)
            {
                _logger.LogInformation("Loan is not in a PENDING status.");
                throw new InvalidOperationException("Loan is not in a PENDING status.");
            }

            loan.LoanStatus = LoanStatus.DECLINED;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Loan declined successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while declining loan with ID: {LoanId}. Error: {ErrorMessage}", id,
                ex.Message);
            throw;
        }
    }

    public async Task<MonthlyPaymentDto> PayOneMonthDue(int loanId)
    {
        try
        {
            _logger.LogInformation("Attempting to pay one month's due for loan with ID: {LoanId}", loanId);

            var user = await _getUserFromContext.GetUser();
            if (user == null)
            {
                _logger.LogInformation("User not found.");
                throw new InvalidOperationException("User not found.");
            }

            var loan = await _dbContext.Loans.FirstOrDefaultAsync(l =>
                l.Id == loanId && l.ApplicationUserId == user.Id && l.LoanStatus == LoanStatus.ACCEPTED);

            if (loan == null)
            {
                _logger.LogInformation(
                    "Loan not found, or it does not belong to the current user, or its status is not accepted, or it is completed.");
                throw new InvalidOperationException(
                    "Loan not found, or it does not belong to the current user, or its status is not accepted, or it is completed.");
            }

            decimal monthly = 0;
            switch (loan.LoanPeriod)
            {
                case LoanPeriod.HalfYear:
                    monthly = loan.FinalAmount / 6;
                    break;
                case LoanPeriod.OneYear:
                    monthly = loan.FinalAmount / 12;
                    break;
                case LoanPeriod.TwoYears:
                    monthly = loan.FinalAmount / 24;
                    break;
                case LoanPeriod.FiveYears:
                    monthly = loan.FinalAmount / 60;
                    break;
                case LoanPeriod.TenYears:
                    monthly = loan.FinalAmount / 120;
                    break;
                default:
                    _logger.LogInformation("Invalid loan period or loan amount.");
                    throw new InvalidOperationException("Invalid loan period or loan amount.");
            }

            if (monthly == 0)
            {
                _logger.LogInformation("Invalid loan period or loan amount.");
                throw new InvalidOperationException("Invalid loan period or loan amount.");
            }

            loan.AmountLeft -= monthly;

            if (loan.AmountLeft == 0)
            {
                loan.LoanStatus = LoanStatus.COMPLETED;
            }

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("One month's due paid successfully for loan with ID: {LoanId}", loanId);

            return new MonthlyPaymentDto
            {
                Id = loan.Id,
                InitialAmount = loan.FinalAmount,
                AmountLeft = loan.AmountLeft,
                MonthlyDue = monthly
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error occurred while paying one month's due for loan with ID: {LoanId}. Error: {ErrorMessage}", loanId,
                ex.Message);
            throw;
        }
    }
}