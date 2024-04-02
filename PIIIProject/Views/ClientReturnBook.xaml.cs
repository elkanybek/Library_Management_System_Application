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
    /// Interaction logic for ClientRetrunBook.xaml
    /// </summary>
    public partial class ClientRetrunBook : Window
    {
        private Library library = new Library();
        private Client user;
        public ClientRetrunBook(Client client)
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();
            user = client;
            PopulateBorrowedBooks();
            
        }

        /// <summary>
        /// Displays all borrowed books by client
        /// </summary>
        private void PopulateBorrowedBooks()
        {
            List<Book> books = new List<Book>();
            foreach (Book book in library.Books) 
            {
                if(book.Borrower == user.Username) // finds books where the borrower name is like the username
                {
                    books.Add(book); // adds book to list
                }
            }
            lxbBorrowedBooks.ItemsSource= books; // displays books
        }

        private void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();

        }

        /// <summary>
        /// Allows client to return the selected book 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ReturnBooks(object sender, RoutedEventArgs e)
        {
            if(lxbBorrowedBooks.SelectedItem is Book selectedBook)
            {
                selectedBook.Status = Book.BookStatus.Available; // sets the books status to available

                selectedBook.DueDate = DateTime.Parse("1/1/0001 12:00:00 AM"); // sets the due date to default time

                selectedBook.Borrower = "UsernameOfOwner"; // sets the user to default user


                MessageBox.Show($"Book '{selectedBook.Title}' returned successfully.");
                PopulateBorrowedBooks(); // displays updated list of books
                UpdateBookInFile(selectedBook);
            }
            else
            {
                // Show a message if no book is selected
                MessageBox.Show("Please select a book to return.");
            }
        }

        /// <summary>
        /// Updates the data file
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
        /// return to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Done(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
