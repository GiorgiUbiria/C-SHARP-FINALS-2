using Finals.Dtos;
using Finals.Models;

namespace Finals.Interfaces;

public interface ILoanService
{
    Task<LoanDto> GetLoan(int id);
    Task<LoansDto> GetAllLoans();
    Task<LoansDto> GetPendingLoans();
    Task<LoansDto> GetAcceptedLoans();
    Task<LoansDto> GetDeclinedLoans();
    Task<bool> DeleteLoan(int id);
    Task<Loan> ModifyLoan(int id, LoanRequestDto loanDto);
    Task<bool> DeclineLoan(int id);
    Task<bool> AcceptLoan(int id);
}