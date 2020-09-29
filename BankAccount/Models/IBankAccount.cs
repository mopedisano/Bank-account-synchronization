
namespace BankAccount.Models
{
    public interface IBankAccount
    {
        string AccountId { get; } 

        void AddOwner(AccountOwner owner);
        void ExecuteTransaction(TransactionTypeEnum transactionType, decimal value);
        void ValidateOwner(string ownerUsername, string ownerPassword);
        void PrintAccountStatus();
    }
}