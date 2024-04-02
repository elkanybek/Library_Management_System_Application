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
    /// Interaction logic for BookSearch.xaml
    /// </summary>
    public partial class BookSearch : Window
    {
        private Library library = new Library();

        public void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();

        }
        public BookSearch()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();

        }

        /// <summary>
        ///  When client enters 4 or more characters in the search box and presses the search button, the books starting with those characters are displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SearchBookEmployee(object sender, RoutedEventArgs e)
        {

            string searchText = txtSearch.Text.ToLower();

            if (searchText.Length < 4)// makes sure the user entered at least 4 characters
            {
                MessageBox.Show("Search text must be at least 4 characters long.");
                return;
            }

            List<Book> filteredBooks = new List<Book>();

            foreach (Book book in library.Books)//finds the books containing the characters entered
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
        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxBooks.SelectedItem is Book selectedBook)
            {
                // Display details of the selected book in the TextBlock using ToString
                txtSearchDetails.Text = selectedBook.ToString();
            }
        }

        /// <summary>
        /// Deletes the selected book
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_DeleteSelectedBook(object sender, RoutedEventArgs e)
        {

            if (lbxBooks.SelectedItem is Book selectedBook)
            {
                // Confirm with the user before deleting the book
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the book:\n{selectedBook.Title}?",
                                                          "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question); 
                if (result == MessageBoxResult.Yes)
                {
                    // Delete the selected book from the library.Books collection
                    library.Books.Remove(selectedBook);

                    // save changes to a file
                    SaveBooksToFile();

                    // Update the ListBox
                    lbxBooks.ItemsSource = null;
                    lbxBooks.ItemsSource = library.Books;

                    // message to indicate successful deletion
                    MessageBox.Show("Book deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Saves the information to the file 
        /// </summary>
        private void SaveBooksToFile()
        {
            string filePath = "../../../TestFiles/Books.txt";
            // Read existing content from the file
            List<string> lines = File.ReadAllLines(filePath).ToList();
            // Get the selected book for deletion
            Book selectedBook = lbxBooks.SelectedItem as Book;
            List<string> newLines = new List<string>();

            // Iterate through each line and exclude the line corresponding to the selected book
            foreach (string line in lines)
            {
                
                if (!line.Contains(selectedBook.Title))
                {
                    newLines.Add(line);
                }
            }

            // Write the modified content back to the file
            File.WriteAllLines(filePath, newLines);

            //message to indicate successful save
            MessageBox.Show("Books deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Returns to previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_BookSearchDone(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Brings to a page to add new book to library
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_AddBook(object sender, RoutedEventArgs e)
        {
            AddBookPopUp addBook = new AddBookPopUp();
            addBook.Show();
        }
    }
}
