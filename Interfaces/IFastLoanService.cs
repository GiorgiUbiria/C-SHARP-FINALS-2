using Finals.Dtos;
using Finals.Models;

namespace Finals.Interfaces;

public interface IFastLoanService
{
    Task<Loan> CreateFastLoan(FastLoanRequestDto fastLoanDto);
    Task<FastLoanDto> GetFastLoan(int id);
    Task<Loan> ModifyFastLoan(int id, FastLoanRequestDto fastLoanDto);
}