using BankAccount.Models;

namespace BankAccount.Services
{
    public interface IBankAccountService
    {
        void AddOwnerToAccount(string accountId, AccountOwner ownerDetails);
        string CreateAccount();
        void Deposit(string accountId, string ownerUsername, string ownerPassword, decimal value);
        void Withdrawal(string accountId, string ownerUsername, string ownerPassword, decimal value);
    }
}