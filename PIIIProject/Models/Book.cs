using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace PIIIProject.Models
{
    public class Book 
    {
        //Data field: 
        private string _genre;
        private string _title;
        private string _author;
        private int _pageNumber;
        private DateTime _dueDate;
        private BookStatus _status;
        public string _borrowerUsername;

       
        //Constructors:
        public Book(string genre_, string title_, string author_, int pageNumber_, DateTime date, string status, string borrower)
        {
            Genre= genre_;
            Title = title_;
            Author = author_;
            PageNumber = pageNumber_;
            DueDate = date;
            if (status.ToLower() == "available")
            {
                _status = BookStatus.Available;
            }
            else if(status.ToLower() == "overdue")
            {
                _status = BookStatus.Overdue;
            }
            else if(status.ToLower() == "borrowed")
            {
                _status = BookStatus.Borrowed;
            }

            Borrower = borrower;
        }

        //Properties:   set directly with the value
        public BookStatus Status
        { 
            get { return _status; } 
            set { _status = value; }
        }
            
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                if( value <= 0 )
                {
                    throw new ArgumentException("Page number for a book cannot be negative or 0");
                }
                _pageNumber = value;
            }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        public string Borrower
        {
            get { return _borrowerUsername; }
            set { _borrowerUsername = value; }
        }

        

        //Override ToString
        public override string ToString()
        {
            string availabilityInfo = "";

            // Include information about the book's availability
            switch (Status)
            {
                case BookStatus.Available:
                    availabilityInfo = "Available";
                    break;

                case BookStatus.Borrowed:
                    availabilityInfo = $"Borrowed, Available on: {DueDate}";
                    break;

                case BookStatus.Overdue:
                    availabilityInfo = $"Borrowed, Available on: Unknown";
                    break;
            }

            return $"Title: {Title}\nAuthor: {Author}\nPageNumber: {PageNumber}\nStatus: {availabilityInfo}";

        }

        //Enums for the status of the books
        public enum BookStatus
        {
            Available,
            Borrowed,
            Overdue
        }

        public bool IsAvailable()
        {
            return Status == BookStatus.Available;
        }

    }
}
