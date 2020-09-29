using System;
using BankAccount.Models;
using BankAccount.Services;

namespace BankAccount
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowMainMenu();
        }

        private static void ShowMainMenu()
        {
            bool runMenu = true;
            do
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Please choose the type of activity you want to perform:");
                    Console.WriteLine("1. Creation of the bank account.");
                    Console.WriteLine("2. Assign bank account owner");
                    Console.WriteLine("3. Deposit Funds");
                    Console.WriteLine("4. Withdrawal Funds");
                    Console.WriteLine("5. Exit");
                    Console.WriteLine(); 
                    int userChoice = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    switch (userChoice)
                    {
                        case 1: //Creation of the bank account
                            Console.WriteLine("You choose to create account!");
                            var accountId = BankAccountService.Instance.CreateAccount();
                            Console.WriteLine($"Congrats.. new bank account created with account ID {accountId}");
                            break;

                        case 2: //Assign bank account owner
                            Console.WriteLine("You choose to assign bank account owner!");
                            ShowAssignAccountOwnerMenu();
                            break;

                        case 3: //Deposit Funds
                            Console.WriteLine("You choose to deposit funds!");
                            ShowDepositMenu();
                            break;

                        case 4: //Withdrawal Funds
                            Console.WriteLine("You choose to withdrawal funds!");
                            ShowWithdrawalMenu();
                            break;
                        
                        case 5: //Exits
                            runMenu = false;
                            break;

                        default:
                            Console.WriteLine("Input not parsed correctly.");
                            Console.WriteLine();
                            break;
                    }

                }
                catch (Exception e)
                {
                    //print error and loop to the beginning
                    Console.WriteLine(e.Message);
                }

            } while (runMenu);
        }

        private static void ShowWithdrawalMenu()
        {
            bool runMenu = true;
            do
            {

                var accountId = GetParameter("Please enter account ID for transaction.");
                //read owner details - username and password
                var username = GetParameter("Please enter username of account owner.");
                var password = GetParameter("Please enter password of account owner.");
                //read amount of money to withdrawal
                var withdrawalValue = GetDecimalParameter("Please enter non negative value to withdrawal.");
                //performing withdrawal
                BankAccountService.Instance.Withdrawal(accountId, username, password, withdrawalValue);
                Console.WriteLine($"Withdrawal of {withdrawalValue} from Account ID {accountId} completed successfully.");
                runMenu = false;
            } while (runMenu);
        }

        private static void ShowDepositMenu()
        {
            bool runMenu = true;
            do
            {

                var accountId = GetParameter("Please enter account ID for transaction.");
                //read owner details - username and password
                var username = GetParameter("Please enter username of account owner.");
                var password = GetParameter("Please enter password of account owner.");
                //read amount of money to deposit
                var depositValue = GetDecimalParameter("Please enter non negative value to deposit.");
                //performing deposit
                BankAccountService.Instance.Deposit(accountId, username, password, depositValue);
                Console.WriteLine($"Deposit of {depositValue} to Account ID {accountId} completed successfully.");
                runMenu = false;
            } while (runMenu);
        }

        private static void ShowAssignAccountOwnerMenu()
        {
            bool runMenu = true;
            do
            {
                var accountId = GetParameter("Please enter account ID to assign owner to.");
                //read owner details

                var idNumber = GetParameter("Please enter ID number of account owner.");
                var firstName = GetParameter("Please enter first name of account owner.");
                var lastName = GetParameter("Please enter last name of account owner.");
                //getting username and password for account owner and not demanding special username or special password with special chars since it for test purposes
                var username = GetParameter("Please enter username of account owner.");
                var password = GetParameter("Please enter password of account owner.");
                //creating owner
                var owner = new AccountOwner(idNumber, firstName, lastName, username, password);
                //assign to account
                BankAccountService.Instance.AddOwnerToAccount(accountId, owner);
                runMenu = false;


            } while (runMenu);
        }

        private static string GetParameter(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                Console.WriteLine("or enter 0 to return to main menu.");
                Console.WriteLine();
                var arg = Console.ReadLine();
                if (string.IsNullOrEmpty(arg)) Console.WriteLine("input argument cannot be empty");
                if (int.TryParse(arg, out int userChoice) && userChoice == 0) throw new Exception("Going back to main menu");

                return arg;
            }

        }
        private static decimal GetDecimalParameter(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                Console.WriteLine("or enter 0 to return to main menu.");
                Console.WriteLine();
                var arg = Console.ReadLine();
                if (string.IsNullOrEmpty(arg)) Console.WriteLine("input argument cannot be empty");
                if (int.TryParse(arg, out int userChoice) && userChoice == 0) throw new Exception("Going back to main menu");
                if (decimal.TryParse(arg, out decimal result)) return result;
            }

        }
    }
}
