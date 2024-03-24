using Finals.Dtos;
using Finals.Models;

namespace Finals.Interfaces;

public interface IFastLoanService
{
    Task<Loan> CreateFastLoan(LoanRequestDto loanDto);
    Task<LoanDto> GetFastLoan(int id);
}