using Finals.Dtos;
using Finals.Models;

namespace Finals.Interfaces;

public interface IInstallmentLoanService
{
   Task<Loan> CreateInstallmentLoan(InstallmentLoanRequestDto loanDto);
}