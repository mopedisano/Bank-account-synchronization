using System;
using System.Collections.Concurrent;
using BankAccount.Models;

namespace BankAccount.Services
{
    /// <summary>
    /// Bank account service that will be used for demonstration. I will dont have DB so I will use cache.
    /// I will use this service as singleton so when ever i need it will be available
    /// </summary>
    public class BankAccountService : IBankAccountService
    {
        #region Singleton initiation

        private static readonly Lazy<IBankAccountService>
            lazy =
                new Lazy<IBankAccountService>
                    (() => new BankAccountService());

        public static IBankAccountService Instance => lazy.Value;

        private BankAccountService()
        {
        }

        #endregion
        private readonly ConcurrentDictionary<string, Models.BankAccount> _bankAccountCache = new ConcurrentDictionary<string, Models.BankAccount>();


        public string CreateAccount()
        {
            var account = new Models.BankAccount();
            if (!_bankAccountCache.TryAdd(account.AccountId, account))
            {
                //notify that from some reason account not created
                Console.WriteLine("Unknown error: Unable to create new account");
                throw new Exception("Unknown error: Unable to create new account");
            }
            else
            {
                Console.WriteLine($"New account was created with account ID: {account.AccountId}");
                return account.AccountId;
            }
        }

        public void AddOwnerToAccount(string accountId, AccountOwner ownerDetails)
        {
            var account = GetAccountFromCache(accountId);
            account.AddOwner(ownerDetails);
        }

        public void Deposit(string accountId, string ownerUsername, string ownerPassword, decimal value)
        {
            var account = GetAccountFromCache(accountId);
            //need to validate owner first
            account.ValidateOwner(ownerUsername, ownerPassword);
            account.ExecuteTransaction(TransactionTypeEnum.Deposit, value);
            account.PrintAccountStatus();
        }
        public void Withdrawal(string accountId, string ownerUsername, string ownerPassword, decimal value)
        {
            var account = GetAccountFromCache(accountId);
            //need to validate owner first
            account.ValidateOwner(ownerUsername, ownerPassword);
            account.ExecuteTransaction(TransactionTypeEnum.WithDrawal, value);
            account.PrintAccountStatus();
        }
        private Models.BankAccount GetAccountFromCache(string accountId)
        {
            if (_bankAccountCache.TryGetValue(accountId, out Models.BankAccount account))
            {

                //use the bank account and assign owner
                return account;
            }
            else
            {
                //account does not exist -  we dont want to tell endpoint user that account does not exist since it can cause hackers to try other account id until the y succeed

                Console.WriteLine($"Problem with setting owner to account {accountId}.");
                throw new Exception($"Problem with setting owner to account {accountId}.");
            }
        }
    }
}