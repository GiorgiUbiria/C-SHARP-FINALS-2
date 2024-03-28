using Finals.Controllers;
using Finals.Dtos;
using Finals.Enums;
using Finals.Interfaces;
using Finals.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Finals.Tests.Controllers;

[TestFixture]
public class AutoLoansControllerTests
{
    private Mock<IAutoLoanService> _mockAutoLoanService;
    private Mock<ILogger<AutoLoansController>> _mockLogger;
    private Mock<IValidator<AutoLoanRequestDto>> _mockAutoLoanValidator;
    private AutoLoansController _controller;

    [SetUp]
    public void Setup()
    {
        _mockAutoLoanService = new Mock<IAutoLoanService>();
        _mockLogger = new Mock<ILogger<AutoLoansController>>();
        _mockAutoLoanValidator = new Mock<IValidator<AutoLoanRequestDto>>();
        _controller = new AutoLoansController(_mockAutoLoanService.Object, _mockLogger.Object,
            _mockAutoLoanValidator.Object);
    }

    [Test]
    public async Task CreateAutoLoan_ReturnsBadRequest_WhenFormDataIsInvalid()
    {
        var autoLoanDto = new AutoLoanRequestDto
        {
            LoanPeriod = LoanPeriod.TenYears,
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>());

        _mockAutoLoanValidator.Setup(validator => validator.ValidateAsync(autoLoanDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _controller.CreateAutoLoan(autoLoanDto);

        Assert.IsInstanceOf<ActionResult<Loan>>(result);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.AreEqual(validationResult.Errors, badRequestResult.Value);
    }

    [Test]
    public async Task CreateAutoLoan_ReturnsOk_WhenLoanIsCreatedSuccessfully()
    {
        var autoLoanDto = new AutoLoanRequestDto
        {
            Model = "Sudan VTS 2015",
            LoanPeriod = LoanPeriod.TenYears
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>());
        var loan = new Loan();

        _mockAutoLoanValidator.Setup(validator => validator.ValidateAsync(autoLoanDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        _mockAutoLoanService.Setup(service => service.GetCarAndLoanAsync(autoLoanDto))
            .ReturnsAsync((car: new Car(), loan: loan));

        var result = await _controller.CreateAutoLoan(autoLoanDto);

        Assert.IsInstanceOf<ActionResult<Loan>>(result);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.AreEqual(loan, okResult.Value);    }

    [Test]
    public async Task ModifyLoan_ReturnsBadRequest_WhenFormDataIsInvalid()
    {
        int id = 1;
        var autoLoanDto = new AutoLoanRequestDto
        {
            Model = "Mercedes Benz"
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>
            { new ValidationFailure("PropertyName", "Invalid data") });
        _mockAutoLoanValidator.Setup(validator => validator.ValidateAsync(autoLoanDto, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var result = await _controller.ModifyLoan(id, autoLoanDto);

        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.AreEqual(validationResult.Errors, badRequestResult.Value);
    }

    [Test]
    public async Task ModifyLoan_ReturnsOk_WhenLoanIsModifiedSuccessfully()
    {
        int id = 1;
        var autoLoanDto = new AutoLoanRequestDto
        {
            Model = "BMW M5",
            LoanPeriod = LoanPeriod.TenYears
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>());
        var modifiedLoanDto = new Loan();

        _mockAutoLoanValidator.Setup(validator => validator.ValidateAsync(autoLoanDto, CancellationToken.None))
            .ReturnsAsync(validationResult);
        _mockAutoLoanService.Setup(service => service.ModifyAutoLoan(id, autoLoanDto))
            .ReturnsAsync(modifiedLoanDto);
        var result = await _controller.ModifyLoan(id, autoLoanDto);

        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.AreEqual(modifiedLoanDto, okResult.Value);
    }
}