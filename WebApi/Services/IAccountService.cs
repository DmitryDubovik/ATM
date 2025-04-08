using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IAccountService
    {
        Task<List<Account>> GetAccounts();

        Task<Account> GetAccountById(int accountId);

        Task<List<Transaction>> GetAccountTransactions(int accountId);

        //Task Deposit(int accountId, decimal amount);

        Task Deposit(Account account, decimal amount);

        Task Withdraw(Account account, decimal amount);
    }
}
