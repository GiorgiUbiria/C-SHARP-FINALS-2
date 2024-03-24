using Finals.Dtos;
using Finals.Models;

namespace Finals.Interfaces;

public interface IInstallmentLoanService
{
    Task<Loan> CreateInstallmentLoan(InstallmentLoanRequestDto loanDto);
    Task<InstallmentLoanDto> GetLoan(int id);
    Task<Loan> ModifyInstallmentLoan(int id, InstallmentLoanRequestDto installmentLoanDto);
}