using Finals.Controllers;
using Finals.Dtos;
using Finals.Enums;
using Finals.Interfaces;
using Finals.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Finals.Test.Controllers;

using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

[TestFixture]
public class FastLoansControllerTests
{
    private Mock<IFastLoanService> _mockFastLoanService;
    private Mock<ILogger<FastLoansController>> _mockLogger;
    private Mock<IValidator<FastLoanRequestDto>> _mockFastLoanValidator;
    private FastLoansController _controller;

    [SetUp]
    public void Setup()
    {
        _mockFastLoanService = new Mock<IFastLoanService>();
        _mockLogger = new Mock<ILogger<FastLoansController>>();
        _mockFastLoanValidator = new Mock<IValidator<FastLoanRequestDto>>();
        _controller = new FastLoansController(_mockFastLoanService.Object, _mockLogger.Object,
            _mockFastLoanValidator.Object);
    }

    [Test]
    public async Task CreateFastLoan_ReturnsBadRequest_WhenFormDataIsInvalid()
    {
        var fastLoanDto = new FastLoanRequestDto
        {
            RequestedAmount = 1000,
            LoanPeriod = (LoanPeriod)0,
            LoanType = LoanType.FAST,
            LoanCurrency = Currency.GEL
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>());

        _mockFastLoanValidator.Setup(validator => validator.ValidateAsync(fastLoanDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _controller.CreateFastLoan(fastLoanDto);

        Assert.IsInstanceOf<ActionResult<Loan>>(result);
        var actionResult = result as ActionResult<Loan>;

        Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;

        Assert.AreEqual(validationResult.Errors, badRequestResult.Value);
    }

    [Test]
    public async Task CreateFastLoan_ReturnsOk_WhenLoanIsCreatedSuccessfully()
    {
        var fastLoanDto = new FastLoanRequestDto
        {
            RequestedAmount = 1000,
            LoanPeriod = (LoanPeriod)12,
            LoanType = LoanType.FAST,
            LoanCurrency = Currency.GEL
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>());

        var loan = new Loan();

        _mockFastLoanValidator.Setup(validator => validator.ValidateAsync(fastLoanDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _mockFastLoanService.Setup(service => service.CreateFastLoan(fastLoanDto)).ReturnsAsync(loan);

        var result = await _controller.CreateFastLoan(fastLoanDto);

        Assert.IsInstanceOf<ActionResult<Loan>>(result);
        var actionResult = result as ActionResult<Loan>;

        Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
        var okResult = actionResult.Result as OkObjectResult;

        Assert.AreEqual(loan, okResult.Value);
    }

    [Test]
    public async Task ModifyLoan_ReturnsBadRequest_WhenFormDataIsInvalid()
    {
        int id = 1;
        var fastLoanDto = new FastLoanRequestDto
        {
            RequestedAmount = 1000,
            LoanPeriod = (LoanPeriod)0,
            LoanType = LoanType.FAST,
            LoanCurrency = Currency.GEL
        };
        var validationResult = new ValidationResult(new List<ValidationFailure>
            { new ValidationFailure("PropertyName", "Invalid data") });
        _mockFastLoanValidator.Setup(validator => validator.ValidateAsync(fastLoanDto, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var result = await _controller.ModifyLoan(id, fastLoanDto);

        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.AreEqual(validationResult.Errors, badRequestResult.Value);
    }

    [Test]
    public async Task ModifyLoan_ReturnsOk_WhenLoanIsModifiedSuccessfully()
    {
        int id = 1;
        var fastLoanDto = new FastLoanRequestDto
        {
            RequestedAmount = 1500,
            LoanPeriod = LoanPeriod.OneYear,
            LoanType = LoanType.FAST,
            LoanCurrency = Currency.USD
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>());

        var modifiedLoan = new Loan
        {
            Id = id,
            RequstedAmount = 1700,
            FinalAmount = Finals.Helpers.LoanCalculator.CalculateFinalAmount(1700, LoanPeriod.TenYears),
            AmountLeft = Finals.Helpers.LoanCalculator.CalculateFinalAmount(1700, LoanPeriod.TenYears),
            LoanPeriod = LoanPeriod.TwoYears,
            LoanType = LoanType.FAST,
            LoanCurrency = Currency.USD,
            LoanStatus = LoanStatus.PENDING
        };

        _mockFastLoanValidator.Setup(validator => validator.ValidateAsync(fastLoanDto, CancellationToken.None))
            .ReturnsAsync(validationResult);

        _mockFastLoanService.Setup(service => service.ModifyFastLoan(id, fastLoanDto))
            .ReturnsAsync(modifiedLoan);

        var result = await _controller.ModifyLoan(id, fastLoanDto);

        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.AreEqual(modifiedLoan, okResult.Value);
    }
}