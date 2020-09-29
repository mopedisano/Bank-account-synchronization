using System;

namespace BankAccount.Models
{
    public class AccountOwner
    {
        public AccountOwner(string idNumber, string firstname, string lastname, string username,string password)
        {
            //validate input are not null 
            if (string.IsNullOrEmpty(idNumber))
            {
                throw new ArgumentNullException(idNumber,$"ID number is required and cannot be null");
            }
            else
            {
                IdNumber = idNumber;
            }
            if (string.IsNullOrEmpty(firstname))
            {
                throw new ArgumentNullException(firstname, $"firstname is required and cannot be null");
            }
            else
            {
                FirstName = firstname;
            }
            if (string.IsNullOrEmpty(lastname))
            {
                throw new ArgumentNullException(lastname, $"lastname is required and cannot be null");
            }
            else
            {
                LastName = lastname;
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(username, $"username is required and cannot be null");
            }
            else
            {
                Username = username;
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(password, $"password is required and cannot be null");
            }
            else
            {
                Password = password;
            }
        }
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public override string ToString()
        {
            return $"ID Number: {IdNumber}, First Name: {FirstName}, Last Name: {LastName}";
        }
    }
}