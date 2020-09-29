using System;
using System.Collections.Concurrent; 
using System.Threading;

namespace BankAccount.Models
{
    public class BankAccount : IBankAccount
    {
        private decimal _balance;
        private readonly SemaphoreSlim _transactionLock = new SemaphoreSlim(1, 1);
        private readonly ConcurrentDictionary<string, AccountOwner> _accountOwners;

        public BankAccount()
        {
            AccountId = Guid.NewGuid().ToString();
            _accountOwners = new ConcurrentDictionary<string, AccountOwner>();
        }
        public string AccountId { get; }

        public void AddOwner(AccountOwner owner)
        {
            if (_accountOwners.Count < 2)
            {
                //if owner already existed with same username 
                if (_accountOwners.ContainsKey(owner.Username))
                {
                    throw new Exception($"Account ID: {AccountId}. Problem adding owner {owner}. Error: Account owner with this username already assigned!");

                }
                //assuming username is unique per owner
                if (!_accountOwners.TryAdd(owner.Username, owner))
                {
                    throw new Exception($"Account ID: {AccountId}. Problem adding owner {owner}");
                }
            }
            else
            {
                //notify we are in full capacity
                throw new Exception($"Account ID: {AccountId} has already maximum number of two owners");
            }
        }

        public void ExecuteTransaction(TransactionTypeEnum transactionType, decimal value)
        {
            try
            {
                _transactionLock.Wait();

                switch (transactionType)
                {
                    case TransactionTypeEnum.WithDrawal:
                        Withdrawal(value);
                        break;
                    case TransactionTypeEnum.Deposit:
                        Deposit(value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(transactionType), transactionType, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                _transactionLock.Release();
            }
        }

        public void PrintAccountStatus()
        {
            Console.WriteLine($"Account {AccountId} status:{Environment.NewLine} Current balance is {_balance}");
        }
        private void Withdrawal(decimal value)
        {
            //work only with non negative values
            if (value <= 0)
            {
                throw new Exception($"Account ID: {AccountId}. Withdrawal value must be value greater then zero!");
            }
            else
            {
                //protect with this condition the situation where withdrawal is not allowed when balance reaches to zero or may become negative
                if (_balance < value || _balance == 0)
                {
                    //withdrawal is not permitted since it balance before withdraw is zero or balance after withdraw will be negative
                    throw new Exception("withdrawal is not permitted since it balance before withdrawal is zero or balance after withdraw will be negative");
                }

                Console.WriteLine($"Account ID: {AccountId}. Performing withdrawal of total {value}");
                _balance -= value;
            }
        }
        private void Deposit(decimal value)
        {
            //work only with non negative values
            if (value <= 0)
            {
                throw new Exception($"Account ID: {AccountId}. Deposit value must be value greater then zero!");
            }
            else
            {
                Console.WriteLine($"Account ID: {AccountId}. Performing deposit of total {value}");
                _balance += value;
            }
        }

        public void ValidateOwner(string ownerUsername, string ownerPassword)
        {
            Console.WriteLine($"Validating user {ownerUsername}");
            if (!_accountOwners.TryGetValue(ownerUsername, out AccountOwner owner))
            {
                //should notify to internal log that user does not exist but endpoint user should get exception that general error occur
                throw new Exception($"Error validating user: {ownerUsername}");
            }
            //simple dummy check if passwords are equal since i dont hve time to use password hashing mechanism or identity 
            else if (owner.Password.Equals(ownerPassword))
            {
                Console.WriteLine($"Validating user {ownerUsername} complete. User is validated."); 
            }
            else
            {
                throw new Exception($"Owner with user name {ownerUsername} is not validated or not exist");
            }
        }
    }
}