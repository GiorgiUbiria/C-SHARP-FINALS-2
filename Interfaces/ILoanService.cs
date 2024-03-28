using Finals.Dtos;
using Finals.Models;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Interfaces;

public interface ILoanService
{
    Task<LoanDto> GetLoan(int id);
    Task<LoansDto> GetAllLoans();
    Task<LoansDto> GetPendingLoans();
    Task<LoansDto> GetAcceptedLoans();
    Task<LoansDto> GetDeclinedLoans();
    Task<LoansDto> GetCompletedLoans();
    Task<bool> DeleteLoan(int id);
    Task<bool> DeclineLoan(int id);
    Task<bool> AcceptLoan(int id);
    Task<MonthlyPaymentDto> PayOneMonthDue(int loanId);
}