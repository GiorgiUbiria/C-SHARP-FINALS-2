using Finals.Dtos;
using Finals.Models;

namespace Finals.Interfaces;

public interface ILoanService
{
    Task<Loan> CreateLoan(LoanDto loanDto);
    Task<LoanDto> GetLoan(int id);
    Task<LoanDtos> GetAllLoans();
    Task<ApplicationUser> GetUser();
    Task<bool> DeleteLoan(int id);
    Task<LoanDto> ModifyLoan(int id, LoanDto loanDto);
}