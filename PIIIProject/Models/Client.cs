using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PIIIProject.Models
{
    //Inherits from the User class:
    public class Client : User       
    {
        private const int MAX_DAYS_BORROWED = 14;

        //Constructors will uses the base class, so no need for an implementaion here
        public Client(string firstName, string lastName, string username, string password) 
            : base(firstName, lastName, username, password) { }

        public Client(string firstName, string lastName, string password) 
            : base(firstName, lastName, password) { }

       
        //Override the virtal Function:
        public override bool PasswordAuthentication(string enteredPassword)
        {
            return base.PasswordAuthentication(enteredPassword);
        }

        public void BorrowBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentException("Cannot borrow a null book.");
            }

            if (book.Status == Book.BookStatus.Available)
            {
                book.Status = Book.BookStatus.Borrowed;
                book.DueDate = DateTime.Now.AddDays(MAX_DAYS_BORROWED);

                // Set the borrower for the book
                book.Borrower = Username;
            }
        }
    }
}
