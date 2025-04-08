using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
    public class AccountService:IAccountService
    {
        private readonly ATMDbContext _context;

        public AccountService(ATMDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();

            return accounts;
        }

        public async Task<Account> GetAccountById(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id == accountId);
            return account;
        }


        public async Task<Account> GetAnotherAccount(int accountId)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id != accountId);
            return account;
        }

        public async Task<List<Transaction>> GetAccountTransactions(int accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .ToListAsync();
            return transactions;
        }


        public async Task Deposit(int accountId, decimal amount)
        {
           
            var account = await GetAccountById(accountId);
            account.Balance += amount;

            var transaction = new Transaction
            {
                Type = "Deposit",
                Amount = amount,
                Date = DateTime.Now,
                AccountBalance = account.Balance,
                AccountId = account.Id
            };

            account.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

        }

        public async Task Deposit(Account account, decimal amount)
        {
            account.Balance += amount;

            var transaction = new Transaction
            {
                Type = "Deposit",
                Amount = amount,
                Date = DateTime.Now,
                AccountBalance = account.Balance,
                AccountId = account.Id
            };

            account.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

        }

        public async Task Withdraw(Account account, decimal amount)
        {
            account.Balance -= amount;

            var transaction = new Transaction
            {
                Type = "Withdraw",
                Amount = amount,
                Date = DateTime.Now,
                AccountBalance = account.Balance,
                AccountId = account.Id
            };

            account.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task Transfer(Account sourceAccount, Account destinationAccount, decimal amount)
        {
            sourceAccount.Balance -= amount;
            destinationAccount.Balance += amount;

            var sourceTransaction = new Transaction
            {
                Type = "Withdraw",
                Amount = amount,
                Date = DateTime.Now,
                AccountBalance = sourceAccount.Balance,
                AccountId = sourceAccount.Id
            };

            var destinsalionTransaction = new Transaction
            {
                Type = "Deposit",
                Amount = amount,
                Date = DateTime.Now,
                AccountBalance = destinationAccount.Balance,
                AccountId = destinationAccount.Id
            };

            sourceAccount.Transactions.Add(sourceTransaction);
            destinationAccount.Transactions.Add(destinsalionTransaction);

            await _context.SaveChangesAsync();
        }

        public async Task ClearTransactions(int accountId)
        {
            var transactions = await GetAccountTransactions(accountId);

            if (transactions != null)
            {
                _context.Transactions.RemoveRange(transactions);
                await _context.SaveChangesAsync();
            }
        }
    }
}
