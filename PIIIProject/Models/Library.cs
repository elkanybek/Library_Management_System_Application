using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIIIProject.Models
{
    internal class Library
    {
        //Data-members:
        private List<Client> _client;
        private List<Employee> _employees;
        private List<Book> _books;

        //Constructors:
        public Library()
        {
            this._client = new List<Client>();
            this._employees = new List<Employee>();
            this._books = new List<Book>();
        }

        public Library(List<Client> clients, List<Employee> employees, List<Book> books)
        {
            Client= clients;
            Employees= employees;
            Books= books;
        }
        
        //Properties:
        public List<Client> Client
        {
            get { return _client; }
            set {
                if (value == null)
                {
                    throw new ArgumentException("The client list cannot be empty.");
                }
                _client = value; 
            }
        }

        public List<Employee> Employees
        {
            get { return _employees; }
            set {
                if (value == null)
                {
                    throw new ArgumentException("The employee list cannot be empty.");
                }
                _employees = value; 
            }
        }

        public List<Book> Books
        {
            get { return _books; }
            set{
                if (value == null)
                {
                    throw new ArgumentException("The book list cannot be empty.");
                }
                _books = value; 
            }        
        }

        //Methods:
        /// <summary>
        /// The method lets us add a new client to the list.
        /// </summary>
        /// <param name="newClient">new client object</param>
        /// <exception cref="ArgumentNullException">in case error occurs</exception>
        public void AddClient(Client newClient)
        {
            if (newClient == null)      //Verification if its null
            {
                throw new ArgumentNullException("New Client is null");
            }
            else
            {
                this._client.Add(newClient);
            }

        }

        /// <summary>
        /// The method lets us add a new employee to the list.
        /// </summary>
        /// <param name="newEmployee">new employee object</param>
        /// <exception cref="ArgumentNullException">in case any error occurs</exception>
        public void AddEmployee(Employee newEmployee)
        {
            if (newEmployee == null)        //Verification if its null
            {
                throw new ArgumentNullException("New Employee is null");
            }
            else
            {
                this._employees.Add(newEmployee);
            }

        }

        /// <summary>
        /// The method lets us add a new book to the list.
        /// </summary>
        /// <param name="newBook">new book object</param>
        /// <exception cref="ArgumentNullException">in case any error occurs</exception>
        public void AddBook(Book newBook)
        {
            if (newBook == null)         //Verification if its null
            {
                throw new ArgumentNullException("New Book is null");
            }
            else
            {
                this._books.Add(newBook);
            }
        }
    }
}
