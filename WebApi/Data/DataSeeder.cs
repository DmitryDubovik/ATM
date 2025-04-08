using System;

namespace WebApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(ATMDbContext context)
        {
            if (context.Accounts.Any())
            {
                return; // Database has been seeded
            }

            context.Accounts.AddRange(
                new Models.Account { AccountType = "Checking", Balance = 10000.00m, AccountNumber = "001" },
                new Models.Account { AccountType = "Savings", Balance = 10000.00m, AccountNumber = "002" }
            );

            context.SaveChanges();
        }
    }
}
