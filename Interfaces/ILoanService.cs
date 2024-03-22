using Finals.Dtos;
using Finals.Models;

namespace Finals.Interfaces;

public interface ILoanService
{
    Task<Loan> CreateLoan(LoanRequestDto loanDto);
    Task<LoanDto> GetLoan(int id);
    Task<LoanDtos> GetAllLoans();
    Task<bool> DeleteLoan(int id);
    Task<Loan> ModifyLoan(int id, LoanRequestDto loanDto);
    Task<bool> DeclineLoan(int id);
    Task<bool> AcceptLoan(int id);
}