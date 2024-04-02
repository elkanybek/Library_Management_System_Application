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
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private Library library = new Library();

        private string _username;

        /// <summary>
        /// Retrieves the list of books, employees, client.
        /// </summary>
        public void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();
        }
        public ClientWindow(string username)
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize; 
            GetData();
            _username = username; // keeps username of the client that logged in
            PopulateSearch(); // fills the search box
        }

        /// <summary>
        /// Populates the search result box with all books
        /// </summary>
        private void PopulateSearch()
        {
            List<Book> allBooks = new List<Book>(); 
            foreach (Book book in library.Books) // gets all the books and adds them to a list
            {
               allBooks.Add(book);
            }
            lbxBooks.ItemsSource = allBooks; // displays them

        }

        /// <summary>
        /// When client enters 4 or more characters in the search box and presses the search button, the books starting with those characters are displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_BookSearchClient(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            if (searchText.Length < 4) // makes sure the user entered at least 4 characters
            {
                MessageBox.Show("Search text must be at least 4 characters long.");
                return;
            }

            List<Book> filteredBooks = new List<Book>();

            foreach (Book book in library.Books) //finds the books containing the characters entered
            {
                if (book.Title.ToLower().Contains(searchText))
                {
                    filteredBooks.Add(book);
                }
            }

            //Updates the search results
            lbxBooks.ItemsSource = filteredBooks;
        }

        /// <summary>
        /// // Display details of the selected book in the TextBlock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbxBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxBooks.SelectedItem is Book selectedBook)
            {
                // Display details of the selected book in the TextBlock using ToString
                txtSearchDetails.Text = selectedBook.ToString();
            }
            PopulateSearch(); // once the client has selected a book the search results show all of the books in the library
        }

        /// <summary>
        /// Allows Client to borrow a book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_BorrowBook(object sender, RoutedEventArgs e)
        {
            if (lbxBooks.SelectedItem is Book selectedBook) // if the client selected a book
            {
                // Get the logged-in client
                Client loggedInClient = GetLoggedInClient(); 

                if (selectedBook.Status == Book.BookStatus.Borrowed)
                {
                    // Show an error message if the book is already borrowed
                    MessageBox.Show($"Error: '{selectedBook.Title}' is already borrowed by {selectedBook.Borrower}.",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    try
                    {
                        // Borrow the selected book
                        loggedInClient.BorrowBook(selectedBook);

                        // Show a success message
                        MessageBox.Show($"Book '{selectedBook.Title}' borrowed successfully. Due date: {selectedBook.DueDate}");

                        // Update the book list
                        lbxBooks.ItemsSource = null;
                        lbxBooks.ItemsSource = library.Books;
                        lbxBooks.SelectedItem = null;

                        UpdateBookInFile(selectedBook); // Update the file
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, such as a null client or other errors
                        MessageBox.Show($"Error borrowing book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                // Show a message if no book is selected
                MessageBox.Show("Please select a book to borrow.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// finds the client that logged in using the entered username
        /// </summary>
        /// <returns></returns>
        private Client GetLoggedInClient() 
        {
            foreach (Client client in library.Client)
            {
                if (client.Username == _username)
                {
                    return client;
                }
            }

            return null;
        }


        /// <summary>
        /// Updates the file 
        /// </summary>
        /// <param name="updatedBook"></param>
        public void UpdateBookInFile(Book updatedBook)
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
        /// Button that displays a window with the books that are overdue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ViewOverdues(object sender, RoutedEventArgs e)
        {
            Client loggedInClient = GetLoggedInClient();
            OverduesPopUp overdues= new OverduesPopUp(loggedInClient);
            overdues.Show();
        }

        /// <summary>
        /// Button that displays a window with the books that are rented and can be returned
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ReturnBook(object sender, RoutedEventArgs e)
        {
            Client loggedInClient = GetLoggedInClient();
            ClientRetrunBook borrowedBooks = new ClientRetrunBook(loggedInClient);
            borrowedBooks.Show();
        }

        /// <summary>
        /// brings back to the main page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Logout(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
