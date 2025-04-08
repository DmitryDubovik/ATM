using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccounts();

        Task<Account> GetAccountById(int accountId);

        Task<Account> GetAnotherAccount(int accountId);

        Task<List<Transaction>> GetAccountTransactions(int accountId);

        Task Deposit(Account account, decimal amount);

        Task Withdraw(Account account, decimal amount);

        Task Transfer(Account sourceAccount, Account destinationAccount, decimal amount);

        Task ClearTransactions(int accountId);
    }
}
