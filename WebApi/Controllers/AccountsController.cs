using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var accounts = await _accountService.GetAccounts();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{accountId:int}")]
        public async Task<IActionResult> GetAccount(int accountId)
        {
            try
            {
                var account = await _accountService.GetAccountById(accountId);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }
                else
                {
                    return Ok(account);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("{accountId:int}/transactions")]
        public async Task<IActionResult> GetAccountTransactions(int accountId)
        {
            try
            {
                var account = await _accountService.GetAccountById(accountId);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }
                else
                {
                    var transactions = await _accountService.GetAccountTransactions(accountId);
                    return Ok(transactions);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("{accountId:int}/deposit")]
        public async Task<IActionResult> Deposit(int accountId, [FromBody] decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    return BadRequest("Deposit amount must be greater than zero.");
                }

                if (amount > 10000)
                {
                    return BadRequest("Deposit amount exceeds the limit of 10,000.");
                }
                var account = await _accountService.GetAccountById(accountId);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }
               
               

                await _accountService.Deposit(account, amount);

                return Ok("Deposit successful.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{accountId:int}/withdraw")]
        public async Task<IActionResult> Withdraw(int accountId, [FromBody] decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    return BadRequest("Withdraw amount must be greater than zero.");
                }
                var account = await _accountService.GetAccountById(accountId);
                if (account == null)
                {
                    return NotFound("Account not found.");
                }
               
                if (amount > account.Balance)
                {
                    return BadRequest("Withdraw amount exceeds the account balance.");
                }

                await _accountService.Withdraw(account, amount);

                return Ok("Withdraw successful.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
