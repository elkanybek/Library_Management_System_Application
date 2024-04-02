using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIIIProject.Models;
using System.IO;

namespace PIIIProject.Repos
{
    internal class Data
    {
        //Avoid magic numbers:
        private const int BOOKTXT_FIELDS_LENGTH = 7;
        //private const int CLIENT_FIELDS_LENGTH = 5;
        private const int EMPLOYEE_FIELDS_LENGTH = 6;
        private const string EMPLOYEE_NUMBER_CHECK = "2";

        /// <summary>
        /// Reads the content from the file path and splits the content in 4 and assigns each elements placemnt to their designed backing field to create a book.
        /// </summary>
        /// <returns>the list of books</returns>
        public static List<Book> GetBooksData()
        {
            List<Book> books = new List<Book>();

            try
            {
                // Read all lines from the file
                string filePath = "../../../TestFiles/Books.txt";
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    // Split the line into fields
                    string[] fields = line.Split(',');
                  
                    if (fields.Length == BOOKTXT_FIELDS_LENGTH)
                    {
                        Book book = new Book(fields[0], fields[1], fields[2], int.Parse(fields[3]), DateTime.Parse(fields[6]), fields[5], fields[4]);
                        books.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return books;
        }

        /// <summary>
        ///  Reads the content from the file path and splits the content and assigns each elements placement to their designed backing field to create an employee.
        /// </summary>
        /// <returns></returns>
        public static List<Employee> GetEmployeeData()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                // Read all lines from the file
                string filePath = "../../../TestFiles/Users.txt";
                string[] lines = File.ReadAllLines(filePath);
                
                foreach (string line in lines)
                {
                    // Split the line into fields
                    string[] fields = line.Split(',');

                    // Check if the number is equal to 2
                    if (fields.Length == EMPLOYEE_FIELDS_LENGTH && fields[0] == EMPLOYEE_NUMBER_CHECK)
                    {
                        // Create an employee object and add it to the list
                        Employee employee = new Employee(fields[1], fields[2], fields[3], fields[4], int.Parse(fields[5]));
                        employees.Add(employee);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return employees;
        }

        /// <summary>
        ///  Reads the content from the file path and splits the content and assigns each elements placement to their designed backing field to create a client.
        /// </summary>
        /// <returns></returns>
        public static List<Client> GetClientData()
        {
            List<Client> clients = new List<Client>();

            try
            {
                // Read all lines from the file
                string filePath = "../../../TestFiles/Users.txt";
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    // Split the line into fields
                    string[] fields = line.Split(',');

                    if (fields.Length <= EMPLOYEE_FIELDS_LENGTH)
                    {
                        Client client = new Client(fields[1], fields[2], fields[3], fields[4]);
                        clients.Add(client);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return clients;
        }
    }
}
