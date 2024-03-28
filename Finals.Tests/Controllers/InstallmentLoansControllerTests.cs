using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Finals.Controllers;
using Finals.Dtos;
using Finals.Enums;
using Finals.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Finals.Models;

namespace Finals.Tests.Controllers
{
    [TestFixture]
    public class InstallmentLoansControllerTests
    {
        private Mock<IInstallmentLoanService> _mockInstallmentLoanService;
        private Mock<ILogger<LoansController>> _mockLogger;
        private Mock<IValidator<InstallmentLoanRequestDto>> _mockInstallmentLoanValidator;
        private InstallmentLoansController _controller;

        [SetUp]
        public void Setup()
        {
            _mockInstallmentLoanService = new Mock<IInstallmentLoanService>();
            _mockLogger = new Mock<ILogger<LoansController>>();
            _mockInstallmentLoanValidator = new Mock<IValidator<InstallmentLoanRequestDto>>();
            _controller = new InstallmentLoansController(_mockInstallmentLoanService.Object, _mockLogger.Object,
                _mockInstallmentLoanValidator.Object);
        }

        [Test]
        public async Task CreateInstallmentLoan_ReturnsBadRequest_WhenFormDataIsInvalid()
        {
            var loanDto = new InstallmentLoanRequestDto
            {
                LoanPeriod = (LoanPeriod)0,
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>());

            _mockInstallmentLoanValidator
                .Setup(validator => validator.ValidateAsync(loanDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var result = await _controller.CreateInstallmentLoan(loanDto);

            Assert.IsInstanceOf<ActionResult<Loan>>(result);
            var actionResult = result as ActionResult<Loan>;
            Assert.IsNotNull(actionResult.Result);
            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);

            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult.Value);
            Assert.AreEqual(validationResult.Errors, badRequestResult.Value);
        }

        [Test]
        public async Task CreateInstallmentLoan_ReturnsOk_WhenLoanIsCreatedSuccessfully()
        {
            var loanDto = new InstallmentLoanRequestDto
            {
                LoanPeriod = (LoanPeriod)12,
                ProductId = 9,
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            var loan = new Loan();

            _mockInstallmentLoanValidator
                .Setup(validator => validator.ValidateAsync(loanDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);
            _mockInstallmentLoanService.Setup(service => service.CreateInstallmentLoan(loanDto)).ReturnsAsync(loan);

            var result = await _controller.CreateInstallmentLoan(loanDto);

            Assert.IsInstanceOf<ActionResult<Loan>>(result);
            var actionResult = result as ActionResult<Loan>;
            Assert.IsNotNull(actionResult.Result);
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.AreEqual(loan, okResult.Value);
        }

        [Test]
        public async Task ModifyLoan_ReturnsBadRequest_WhenFormDataIsInvalid()
        {
            int id = 1;
            var loanDto = new InstallmentLoanRequestDto
            {
                LoanPeriod = (LoanPeriod)0,
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("PropertyName", "Invalid data")
            });

            _mockInstallmentLoanValidator.Setup(validator => validator.ValidateAsync(loanDto, CancellationToken.None))
                .ReturnsAsync(validationResult);

            var result = await _controller.ModifyLoan(id, loanDto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.AreEqual(validationResult.Errors, badRequestResult.Value);
        }

        [Test]
        public async Task ModifyLoan_ReturnsOk_WhenLoanIsModifiedSuccessfully()
        {
            int id = 1;
            var loanDto = new InstallmentLoanRequestDto
            {
                LoanPeriod = LoanPeriod.OneYear,
                ProductId = 10,
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            var modifiedLoan = new Loan();

            _mockInstallmentLoanValidator.Setup(validator => validator.ValidateAsync(loanDto, CancellationToken.None))
                .ReturnsAsync(validationResult);
            _mockInstallmentLoanService.Setup(service => service.ModifyInstallmentLoan(id, loanDto))
                .ReturnsAsync(modifiedLoan);

            var result = await _controller.ModifyLoan(id, loanDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.AreEqual(modifiedLoan, okResult.Value);
        }
    }
}