using Finals.Dtos;
using Finals.Models;

namespace Finals.Interfaces;

public interface IAutoLoanService
{
    Task<(Car car, Loan loan)> GetCarAndLoanAsync(AutoLoanRequestDto autoLoanRequestDto);
}