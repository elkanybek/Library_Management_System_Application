using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    public abstract class User    //Abstract for later use
    {
        //Data-members
        private string _firstName;
        private string _lastName;
        private string _username;
        private string _password;

        //Avoid magic numbers:
        private const int MIN_PASSWORD_LENGHT = 6;
        private const int FIRST_THREE_LETTERS_FN = 3;
        private const int FIRST_FOUR_LETTERS_LN = 4;

        

        // constructor for new users
        // username gets created for them
        public User(string firstName_, string lastName_, string password_) 
        { 
            FirstName = firstName_;
            LastName = lastName_;
            Password = password_;
            Username = CreateUsername(firstName_, lastName_);
        }
        
        
        // constructor for incoming file
        public User(string firstName, string lastName, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;     
        }

        //Properties:
        public string FirstName 
        { 
            get { return _firstName;}
            set {
                if (string.IsNullOrEmpty(value))    //checks if null or empty
                {
                    throw new ArgumentException("First name cannot be null or empty.");
                }
                _firstName = value;
            }      
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Last name cannot be null or empty.");
                }
                _lastName = value;
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if(value.Length <= MIN_PASSWORD_LENGHT)
                {
                    throw new Exception($"Password must be {MIN_PASSWORD_LENGHT} characters minimum");
                }
                _password = value;
            }
        }
        
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// The function creates usernames based on the first 3 letters of the first name and the first 4 letters of the family name.
        /// </summary>
        /// <param name="firstName">firstname of the user</param>
        /// <param name="lastName">last name of  the user</param>
        /// <returns>The string username created</returns>
        static public string CreateUsername(string firstName, string lastName)
        {
            if(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))       //Validation
            {
                throw new ArgumentException("First name or last name cannot be null or empty.");
            }

            string firstPart = string.Empty;
            string lastPart = string.Empty;
            
            for (int i = 0; i < FIRST_THREE_LETTERS_FN; i++)
            {
                firstPart += firstName[i];
            }

            for (int i = 0; i < FIRST_FOUR_LETTERS_LN; i++)
            {
                lastPart += lastName[i];
            }
            string username = firstPart + lastPart;
            return username;
        }

        /// <summary>
        /// Authenticates the passed password using the properrtie
        /// </summary>
        /// <param name="enteredPassword">Password</param>
        public virtual bool PasswordAuthentication(string enteredPassword)
        {
            return Password == enteredPassword;
        }

        //Override ToString:
        public override string ToString()
        {
            return $"Name: {FirstName} {LastName}\nUsername: {Username}\nPassword: {Password}";
        }
    }
}
