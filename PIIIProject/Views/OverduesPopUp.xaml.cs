using PIIIProject.Models;
using PIIIProject.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
namespace PIIIProject.Views
{
    /// <summary>
    /// Interaction logic for OverduesPopUp.xaml
    /// </summary>
    public partial class OverduesPopUp : Window
    {
        private Library library = new Library();
        private Client user;

        public OverduesPopUp(Client client)
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();
            user = client;

            // Call the method to populate the ListBox with overdue books
            PopulateOverdueBooks();
        }

        /// <summary>
        /// Displays all the books that are overdue
        /// </summary>
        private void PopulateOverdueBooks()
        {
            List<Book> overdueBooks = new List<Book>();
            foreach (Book book in library.Books) // searches for all books that have an overdue status or that the due date is passed
            {
                if(book.Borrower == user.Username && (book.Status == Book.BookStatus.Overdue || DateTime.Now > book.DueDate))
                {
                    overdueBooks.Add(book); // adds it to the list
                }
            }
            // displays them
            lxbOverdues.ItemsSource = overdueBooks;
        }

     

        private void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();

        }

        /// <summary>
        /// Allows user to return an overdue book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ReturnBook(object sender, RoutedEventArgs e)
        {
            if (lxbOverdues.SelectedItem is Book selectedBook)
            {
                
                selectedBook.Status = Book.BookStatus.Available; // changes the status to available
                
                selectedBook.DueDate = DateTime.Parse("1/1/0001 12:00:00 AM"); // sets the time to default
               
                selectedBook.Borrower = "UsernameOfOwner"; // sets a default username

               
                MessageBox.Show($"Book '{selectedBook.Title}' returned successfully."); // displays a succes message
                PopulateOverdueBooks();
                UpdateBookInFile(selectedBook);
              
            }
            else
            {
                // Show a message if no book is selected
                MessageBox.Show("Please select a book to return.");
            }
        }

        /// <summary>
        /// Updates the book status in file
        /// </summary>
        /// <param name="updatedBook"></param>
        private void UpdateBookInFile(Book updatedBook)
        {
            string filePath = "../../../TestFiles/Books.txt";
            List<string> lines = File.ReadAllLines(filePath).ToList();

            // Find the line that corresponds to the updated book
            for (int i = 0; i < lines.Count; i++)
            {
                string[] bookData = lines[i].Split(',');

               
                string genre = bookData[0];
                string title = bookData[1];
                string author = bookData[2];

                if (genre == updatedBook.Genre && title == updatedBook.Title && author == updatedBook.Author)
                {
                    // Modify the necessary information
                    lines[i] = $"{updatedBook.Genre},{updatedBook.Title},{updatedBook.Author},{updatedBook.PageNumber},{updatedBook.Borrower},{updatedBook.Status},{updatedBook.DueDate}";

                    // Stop searching once the book is found and updated
                    break;
                }
            }

            // Write the modified content back to the file
            File.WriteAllLines(filePath, lines);
        }


        /// <summary>
        /// returns to the previous window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Done(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
