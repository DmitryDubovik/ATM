using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly AccountsController _controller;

        public AccountsControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountsController(_mockAccountService.Object);
        }

        [Fact]
        public async Task GetAccounts_ReturnsOkResult_WithAccounts()
        {
            // Arrange
            var accounts = new List<Account> { new Account { Id = 1, AccountNumber = "12345", Balance = 1000 } };
            _mockAccountService.Setup(s => s.GetAccounts()).ReturnsAsync(accounts);

            // Act
            var result = await _controller.GetAccounts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(accounts, okResult.Value);
        }

        [Fact]
        public async Task GetAccounts_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockAccountService.Setup(s => s.GetAccounts()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetAccounts();

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async Task GetAccount_ReturnsOkResult_WithAccount()
        {
            // Arrange
            var account = new Account { Id = 1, AccountNumber = "12345", Balance = 1000 };
            _mockAccountService.Setup(s => s.GetAccountById(1)).ReturnsAsync(account);

            // Act
            var result = await _controller.GetAccount(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(account, okResult.Value);
        }

        [Fact]
        public async Task GetAccount_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            // Arrange
            _mockAccountService.Setup(s => s.GetAccountById(1)).ReturnsAsync((Account)null);

            // Act
            var result = await _controller.GetAccount(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAccountTransactions_ReturnsOkResult_WithTransactions()
        {
            // Arrange
            var transactions = new List<Transaction> { new Transaction { Id = 1, Amount = 100, Type = "Deposit" } };
            _mockAccountService.Setup(s => s.GetAccountById(1)).ReturnsAsync(new Account());
            _mockAccountService.Setup(s => s.GetAccountTransactions(1)).ReturnsAsync(transactions);

            // Act
            var result = await _controller.GetAccountTransactions(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(transactions, okResult.Value);
        }

        [Fact]
        public async Task Deposit_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            var account = new Account { Id = 1, Balance = 1000 };
            _mockAccountService.Setup(s => s.GetAccountById(1)).ReturnsAsync(account);

            // Act
            var result = await _controller.Deposit(1, 500);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Deposit_ReturnsBadRequest_WhenAmountIsInvalid()
        {
            // Act
            var result = await _controller.Deposit(1, -500);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Withdraw_ReturnsOkResult_OnSuccess()
        {
            // Arrange
            var account = new Account { Id = 1, Balance = 1000 };
            _mockAccountService.Setup(s => s.GetAccountById(1)).ReturnsAsync(account);

            // Act
            var result = await _controller.Withdraw(1, 500);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Withdraw_ReturnsBadRequest_WhenAmountExceedsBalance()
        {
            // Arrange
            var account = new Account { Id = 1, Balance = 1000 };
            _mockAccountService.Setup(s => s.GetAccountById(1)).ReturnsAsync(account);

            // Act
            var result = await _controller.Withdraw(1, 1500);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
