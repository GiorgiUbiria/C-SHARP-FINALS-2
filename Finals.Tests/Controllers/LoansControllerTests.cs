using Finals.Controllers;
using Finals.Interfaces;
using Finals.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Finals.Dtos;
using Finals.Enums;

namespace Finals.Tests.Controllers
{
    [TestFixture]
    public class LoansControllerTests
    {
        private Mock<ILoanService> _mockLoanService;
        private Mock<ILogger<LoansController>> _mockLogger;
        private LoansController _controller;

        [SetUp]
        public void Setup()
        {
            _mockLoanService = new Mock<ILoanService>();
            _mockLogger = new Mock<ILogger<LoansController>>();
            _controller = new LoansController(_mockLoanService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetAllLoans_ReturnsNoContent_WhenNoLoansExist()
        {
            _mockLoanService.Setup(service => service.GetAllLoans())
                .ReturnsAsync(new LoansDto { Loans = new List<LoanDto>() });

            var result = await _controller.GetAllLoans();

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetAllLoans_ReturnsOkWithLoans_WhenLoansExist()
        {
            var loan1 = new LoanDto
            {
                Id = 1,
                RequestedAmount = 1000,
                FinalAmount = 1000,
                AmountLeft = 1000,
                LoanPeriod = LoanPeriod.OneYear,
                LoanType = LoanType.FAST,
                LoanCurrency = Currency.GEL,
                LoanStatus = LoanStatus.PENDING,
                ProductId = 1,
                Product = new Product(),
                CarId = 1,
                Car = new Car(),
                UserEmail = "test@example.com"
            };

            var loan2 = new LoanDto
            {
                Id = 2,
                RequestedAmount = 2000,
                FinalAmount = 2000,
                AmountLeft = 2000,
                LoanPeriod = LoanPeriod.TwoYears,
                LoanType = LoanType.AUTO,
                LoanCurrency = Currency.GEL,
                LoanStatus = LoanStatus.PENDING,
                ProductId = 2,
                Product = new Product(),
                CarId = 2,
                Car = new Car(),
                UserEmail = "test2@example.com"
            };

            var loans = new LoansDto();
            loans.Loans.Add(loan1);
            loans.Loans.Add(loan2);

            _mockLoanService.Setup(service => service.GetAllLoans()).ReturnsAsync(loans);

            var result = await _controller.GetAllLoans();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<LoansDto>(okResult.Value);
            var loansDto = okResult.Value as LoansDto;
            Assert.AreEqual(loans.Loans.Count, loansDto.Loans.Count);
        }

        [Test]
        public async Task GetLoan_ReturnsNotFound_WhenLoanDoesNotExist()
        {
            int loanId = 1;
            _mockLoanService.Setup(service => service.GetLoan(loanId)).ReturnsAsync((LoanDto)null);

            var result = await _controller.GetLoan(loanId);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetLoan_ReturnsLoanDto_WhenLoanExists()
        {
            int loanId = 1;
            var loanDto = new LoanDto
            {
                Id = 1,
                RequestedAmount = 1000,
                FinalAmount = 1000,
                AmountLeft = 1000,
                LoanPeriod = LoanPeriod.OneYear,
                LoanType = LoanType.FAST,
                LoanCurrency = Currency.GEL,
                LoanStatus = LoanStatus.PENDING,
                ProductId = 1,
                Product = new Product(),
                CarId = 1,
                Car = new Car(),
                UserEmail = "test@example.com"
            };
            _mockLoanService.Setup(service => service.GetLoan(loanId)).ReturnsAsync(loanDto);

            var actionResult = await _controller.GetLoan(loanId);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnedLoanDto = okResult.Value as LoanDto;
            Assert.IsNotNull(returnedLoanDto);
            Assert.AreEqual(loanDto.Id, returnedLoanDto.Id);
        }

        [Test]
        public async Task DeleteLoan_ReturnsOk_WhenLoanIsDeletedSuccessfully()
        {
            int loanId = 1;
            _mockLoanService.Setup(service => service.DeleteLoan(loanId)).ReturnsAsync(true);

            var result = await _controller.DeleteLoan(loanId);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DeleteLoan_ReturnsNotFound_WhenLoanNotFound()
        {
            int loanId = 1;
            _mockLoanService.Setup(service => service.DeleteLoan(loanId)).ReturnsAsync(false);

            var result = await _controller.DeleteLoan(loanId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteLoan_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int loanId = 1;
            var errorMessage = "Error message";
            _mockLoanService.Setup(service => service.DeleteLoan(loanId))
                .ThrowsAsync(new InvalidOperationException(errorMessage));

            var result = await _controller.DeleteLoan(loanId);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(errorMessage, badRequestResult.Value);
        }

        [Test]
        public async Task AcceptLoan_ReturnsOk_WhenLoanIsAcceptedSuccessfully()
        {
            int loanId = 1;
            _mockLoanService.Setup(service => service.AcceptLoan(loanId)).ReturnsAsync(true);

            var result = await _controller.AcceptLoan(loanId);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task AcceptLoan_ReturnsNotFound_WhenLoanNotFound()
        {
            int loanId = 1;
            _mockLoanService.Setup(service => service.AcceptLoan(loanId)).ReturnsAsync(false);

            var result = await _controller.AcceptLoan(loanId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AcceptLoan_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int loanId = 1;
            var errorMessage = "Error message";
            _mockLoanService.Setup(service => service.AcceptLoan(loanId))
                .ThrowsAsync(new InvalidOperationException(errorMessage));

            var result = await _controller.AcceptLoan(loanId);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(errorMessage, badRequestResult.Value);
        }

        [Test]
        public async Task DeclineLoan_ReturnsOk_WhenLoanIsDeclinedSuccessfully()
        {
            int loanId = 1;
            _mockLoanService.Setup(service => service.DeclineLoan(loanId)).ReturnsAsync(true);

            var result = await _controller.DeclineLoan(loanId);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DeclineLoan_ReturnsNotFound_WhenLoanNotFound()
        {
            int loanId = 1;
            _mockLoanService.Setup(service => service.DeclineLoan(loanId)).ReturnsAsync(false);

            var result = await _controller.DeclineLoan(loanId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeclineLoan_ReturnsBadRequest_WhenExceptionOccurs()
        {
            int loanId = 1;
            var errorMessage = "Error message";
            _mockLoanService.Setup(service => service.DeclineLoan(loanId))
                .ThrowsAsync(new InvalidOperationException(errorMessage));

            var result = await _controller.DeclineLoan(loanId);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(errorMessage, badRequestResult.Value);
        }

        [Test]
        public async Task GetPendingLoans_ReturnsOk_WhenPendingLoansExist()
        {
            var loanDtos = new LoansDto
            {
                Loans = new List<LoanDto>
                {
                    new LoanDto
                    {
                        Id = 1,
                        LoanStatus = LoanStatus.PENDING,
                        RequestedAmount = 0,
                        FinalAmount = 0,
                        AmountLeft = 0,
                        LoanPeriod = (LoanPeriod)0,
                        LoanType = LoanType.FAST,
                        LoanCurrency = Currency.GEL,
                        ProductId = 0,
                        Product = null,
                        CarId = 0,
                        Car = null,
                        UserEmail = null
                    },
                    new LoanDto
                    {
                        Id = 2,
                        LoanStatus = LoanStatus.PENDING,
                        RequestedAmount = 0,
                        FinalAmount = 0,
                        AmountLeft = 0,
                        LoanPeriod = (LoanPeriod)0,
                        LoanType = LoanType.FAST,
                        LoanCurrency = Currency.GEL,
                        ProductId = 0,
                        Product = null,
                        CarId = 0,
                        Car = null,
                        UserEmail = null
                    }
                }
            };
            _mockLoanService.Setup(service => service.GetPendingLoans()).ReturnsAsync(loanDtos);

            var result = await _controller.GetPendingLoans();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetPendingLoans_ReturnsNoContent_WhenNoPendingLoansExist()
        {
            var loanDtos = new LoansDto { Loans = new List<LoanDto>() };
            _mockLoanService.Setup(service => service.GetPendingLoans()).ReturnsAsync(loanDtos);

            var result = await _controller.GetPendingLoans();

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetPendingLoans_ReturnsBadRequest_WhenLoanServiceThrowsException()
        {
            var errorMessage = "Error message";
            _mockLoanService.Setup(service => service.GetPendingLoans())
                .ThrowsAsync(new InvalidOperationException(errorMessage));

            var result = await _controller.GetPendingLoans();

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(errorMessage, badRequestResult.Value);
        }

        [Test]
        public async Task GetDeclinedLoans_ReturnsOk_WhenDeclinedLoansExist()
        {
            var loansDto = new LoansDto
            {
                Loans = new List<LoanDto>
                {
                    new LoanDto
                    {
                        Id = 1,
                        LoanStatus = LoanStatus.DECLINED,
                        RequestedAmount = 0,
                        FinalAmount = 0,
                        AmountLeft = 0,
                        LoanPeriod = (LoanPeriod)0,
                        LoanType = LoanType.FAST,
                        LoanCurrency = Currency.GEL,
                        ProductId = 0,
                        Product = null,
                        CarId = 0,
                        Car = null,
                        UserEmail = null
                    },
                    new LoanDto
                    {
                        Id = 2,
                        LoanStatus = LoanStatus.DECLINED,
                        RequestedAmount = 0,
                        FinalAmount = 0,
                        AmountLeft = 0,
                        LoanPeriod = (LoanPeriod)0,
                        LoanType = LoanType.FAST,
                        LoanCurrency = Currency.GEL,
                        ProductId = 0,
                        Product = null,
                        CarId = 0,
                        Car = null,
                        UserEmail = null
                    }
                }
            };
            _mockLoanService.Setup(service => service.GetDeclinedLoans()).ReturnsAsync(loansDto);

            var result = await _controller.GetDeclinedLoans();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetDeclinedLoans_ReturnsNoContent_WhenNoDeclinedLoansExist()
        {
            var loansDto = new LoansDto { Loans = new List<LoanDto>() };
            _mockLoanService.Setup(service => service.GetDeclinedLoans()).ReturnsAsync(loansDto);

            var result = await _controller.GetDeclinedLoans();

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetCompletedLoans_ReturnsOk_WhenCompletedLoansExist()
        {
            var loansDto = new LoansDto
            {
                Loans = new List<LoanDto>
                {
                    new LoanDto
                    {
                        Id = 1,
                        LoanStatus = LoanStatus.COMPLETED,
                        RequestedAmount = 0,
                        FinalAmount = 0,
                        AmountLeft = 0,
                        LoanPeriod = (LoanPeriod)0,
                        LoanType = LoanType.FAST,
                        LoanCurrency = Currency.GEL,
                        ProductId = 0,
                        Product = null,
                        CarId = 0,
                        Car = null,
                        UserEmail = null
                    },
                    new LoanDto
                    {
                        Id = 2,
                        LoanStatus = LoanStatus.COMPLETED,
                        RequestedAmount = 0,
                        FinalAmount = 0,
                        AmountLeft = 0,
                        LoanPeriod = (LoanPeriod)0,
                        LoanType = LoanType.FAST,
                        LoanCurrency = Currency.GEL,
                        ProductId = 0,
                        Product = null,
                        CarId = 0,
                        Car = null,
                        UserEmail = null
                    }
                }
            };
            _mockLoanService.Setup(service => service.GetCompletedLoans()).ReturnsAsync(loansDto);

            var result = await _controller.GetCompletedLoans();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetCompletedLoans_ReturnsNoContent_WhenNoCompletedLoansExist()
        {
            var loansDto = new LoansDto { Loans = new List<LoanDto>() };
            _mockLoanService.Setup(service => service.GetCompletedLoans()).ReturnsAsync(loansDto);

            var result = await _controller.GetCompletedLoans();

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetAcceptedLoans_ReturnsOk_WhenAcceptedLoansExist()
        {
            var loansDto = new LoansDto
            {
                Loans = new List<LoanDto>
                {
                    new LoanDto
                    {
                        Id = 1,
                        LoanStatus = LoanStatus.ACCEPTED,
                        RequestedAmount = 0,
                        FinalAmount = 0,
                        AmountLeft = 0,
                        LoanPeriod = (LoanPeriod)0,
                        LoanType = LoanType.FAST,
                        LoanCurrency = Currency.GEL,
                        ProductId = 0,
                        Product = null,
                        CarId = 0,
                        Car = null,
                        UserEmail = null
                    },
                    new LoanDto
                    {
                        Id = 2,
                        LoanStatus = LoanStatus.ACCEPTED,
                        RequestedAmount = 0,
                        FinalAmount = 0,
                        AmountLeft = 0,
                        LoanPeriod = (LoanPeriod)0,
                        LoanType = LoanType.FAST,
                        LoanCurrency = Currency.GEL,
                        ProductId = 0,
                        Product = null,
                        CarId = 0,
                        Car = null,
                        UserEmail = null
                    }
                }
            };
            _mockLoanService.Setup(service => service.GetAcceptedLoans()).ReturnsAsync(loansDto);

            var result = await _controller.GetAccpetedLoans();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAcceptedLoans_ReturnsNoContent_WhenNoAcceptedLoansExist()
        {
            var loansDto = new LoansDto { Loans = new List<LoanDto>() };
            _mockLoanService.Setup(service => service.GetAcceptedLoans()).ReturnsAsync(loansDto);

            var result = await _controller.GetAccpetedLoans();

            Assert.IsInstanceOf<NoContentResult>(result);
        }
        
        [Test]
        public async Task PayOneMonthDue_ReturnsOk_WhenPaymentIsSuccessful()
        {
            int loanId = 1;
            MonthlyPaymentDto monthlyPayment = new MonthlyPaymentDto
            {
                MonthlyDue = 500,
                Id = 1,
                InitialAmount = 1500,
                AmountLeft = 1000
            };

            _mockLoanService.Setup(service => service.PayOneMonthDue(loanId)).ReturnsAsync(monthlyPayment);
            var result = await _controller.PayOneMonthDue(loanId);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task PayOneMonthDue_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
        {
            int loanId = 1;
            string errorMessage = "Invalid operation";
            _mockLoanService.Setup(service => service.PayOneMonthDue(loanId)).ThrowsAsync(new InvalidOperationException(errorMessage));

            var result = await _controller.PayOneMonthDue(loanId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task PayOneMonthDue_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            int loanId = 1;
            _mockLoanService.Setup(service => service.PayOneMonthDue(loanId)).ThrowsAsync(new Exception());

            var result = await _controller.PayOneMonthDue(loanId);

            Assert.IsInstanceOf<StatusCodeResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
    }
}