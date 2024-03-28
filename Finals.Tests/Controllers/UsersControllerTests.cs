using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Finals.Controllers;
using Finals.Dtos;
using Finals.Enums;
using Finals.Models;
using Finals.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;
using NUnit.Framework;

namespace Finals.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<ILogger<UsersController>> _mockLogger;
        private Mock<IValidator<RegisterRequestDto>> _mockRegisterValidator;
        private Mock<IValidator<AuthRequestDto>> _mockAuthValidator;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _mockRegisterValidator = new Mock<IValidator<RegisterRequestDto>>();
            _mockAuthValidator = new Mock<IValidator<AuthRequestDto>>();
            _controller = new UsersController(
                _mockUserService.Object,
                _mockLogger.Object,
                _mockAuthValidator.Object,
                _mockRegisterValidator.Object
            );
        }

        [Test]
        public async Task Register_ReturnsBadRequest_WhenValidationFails()
        {
            var request = new RegisterRequestDto
            {
                Email = null,
                Username = "faziskado",
                Password = "Fazis@123",
                FirstName = "Giorgi",
                LastName = "Tester",
                Age = 20,
                Salary = 1600
            };
            var validationResult = new ValidationResult(new List<ValidationFailure>
                { new ValidationFailure("PropertyName", "Invalid data") });
            _mockRegisterValidator.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(validationResult);

            var result = await _controller.Register(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual(validationResult.Errors, badRequestResult.Value);
        }

        [Test]
        public async Task Register_ReturnsCreatedAtAction_WhenRegistrationSucceeds()
        {
            var request = new RegisterRequestDto
            {
                Email = "test@test.com",
                Username = "faziskado",
                Password = "Fazis@123",
                FirstName = "Giorgi",
                LastName = "Tester",
                Age = 20,
                Salary = 1600
            };
            var validationResult = new ValidationResult(new List<ValidationFailure>());
            _mockRegisterValidator.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(validationResult);
            var user = new ApplicationUser { Email = "test@example.com" };
            _mockUserService.Setup(service => service.RegisterUser(It.IsAny<RegisterRequestDto>()))
                .ReturnsAsync(new RegisterResponseDto
                {
                    User = new IdentityResult(),
                    Message = "New user created successfully"
                });

            var result = await _controller.Register(request);

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(UsersController.Register), createdAtActionResult.ActionName);
            Assert.AreEqual(user.Email, (createdAtActionResult.Value as RegisterRequestDto).Email);
        }

        [Test]
        public async Task Authenticate_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var request = new AuthRequestDto
            {
                Email = null,
                Password = null
            };
            var validationResult = new ValidationResult(new List<ValidationFailure>
                { new ValidationFailure("PropertyName", "Invalid data") });
            _mockAuthValidator.Setup(validator => validator.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);

            var result = await _controller.Authenticate(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestResult = result.Result as BadRequestObjectResult;

            Assert.AreEqual(validationResult.Errors, badRequestResult.Value);
        }

        [Test]
        public async Task Authenticate_ReturnsBadRequest_WhenAuthenticationFails()
        {
            var request = new AuthRequestDto
            {
                Email = "test@test.comru",
                Password = "Password@123"
            };
            var validationResult = new ValidationResult(new List<ValidationFailure>());
            _mockAuthValidator.Setup(validator => validator.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);
            _mockUserService.Setup(service => service.AuthenticateUser(request)).ReturnsAsync((AuthResponseDto)null);

            var result = await _controller.Authenticate(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestResult = result.Result as BadRequestObjectResult;

            Assert.AreEqual("Bad credentials", badRequestResult.Value);
        }

        [Test]
        public async Task Authenticate_ReturnsOk_WhenAuthenticationSucceeds()
        {
            var request = new AuthRequestDto
            {
                Email = "accountant@test.com",
                Password = "Accountant@123"
            };
            var validationResult = new ValidationResult(new List<ValidationFailure>());
            _mockAuthValidator.Setup(validator => validator.ValidateAsync(request, default))
                .ReturnsAsync(validationResult);
            var authResponse = new AuthResponseDto { Email = "test@example.com", Token = "dummyToken" };
            _mockUserService.Setup(service => service.AuthenticateUser(request)).ReturnsAsync(authResponse);

            var result = await _controller.Authenticate(request);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;

            Assert.AreEqual(authResponse, okResult.Value);
        }

        [Test]
        public async Task GetCurrentUser_ReturnsNotFound_WhenUserIsNotAuthenticated()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal()
                }
            };

            var result = await _controller.GetCurrentUser();

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetCurrentUser_ReturnsOk_WhenUserIsAuthenticatedAndFound()
        {
            var userDto = new UserDto();

            var jwtSub = "user123";
            var user = new ApplicationUser
            {
                Id = "1",
                UserName = "john_doe",
                Email = "john.doe@example.com",
                Role = Role.Customer
            };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"))
                }
            };

            _mockUserService.Setup(service => service.GetCurrentUser(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(userDto);

            var result = await _controller.GetCurrentUser();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(userDto, okResult.Value);
        }
    }
}