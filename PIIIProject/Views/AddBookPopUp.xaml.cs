using PIIIProject.Models;
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
using PIIIProject.Repos;
using System.Net;

namespace PIIIProject.Views
{
    /// <summary>
    /// Interaction logic for AddBookPopUp.xaml
    /// </summary>
    public partial class AddBookPopUp : Window
    {
        //Data fields:
        private Library library = new Library();

        //Methods:
        /// <summary>
        /// Go reads the informations in the txt file.
        /// </summary>
        public void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();
        }

        /// <summary>
        /// Initializes a new instance of the AddBookPopUp class.
        /// </summary>
        public AddBookPopUp()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();
        }

        /// <summary>
        /// Validates the click on the add book, by verifying if the fields are null or empty.
        /// Then it will create a new object book and save it to file by calling another method.
        /// </summary>
        /// <param name="sender">object that triggered the event</param>
        /// <param name="e">the event</param>
        private void Btn_AddBook(object sender, RoutedEventArgs e)
        {
            //Validation
            if (string.IsNullOrEmpty(txbGenre.Text))
            {
                MessageBox.Show("Please enter a genre", "Genre Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txbTitle.Text))
            {
                MessageBox.Show("Please enter a title", "Title Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txbAuthor.Text))
            {
                MessageBox.Show("Please enter an author", "Auhtor Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txbPageNum.Text))
            {
                MessageBox.Show("Please enter page number", "Page Number Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Book newBook = new Book(txbGenre.Text, txbTitle.Text, txbAuthor.Text, int.Parse(txbPageNum.Text),DateTime.Parse("null"),"Available", "UsernameOfOwner");

            library.Books.Add(newBook);
            SaveBookToFile(newBook);
            MessageBox.Show($"Book added successfuly. Title is {newBook.Title} by {newBook.Author}");
            Close();
        }

        /// <summary>
        /// Saves the new book to the existing txt book file.
        /// </summary>
        /// <param name="newBook"></param>
        private void SaveBookToFile(Book newBook)
        {
            string filePath = "../../../TestFiles/Books.txt";
            string bookLine = $"{newBook.Genre},{newBook.Title},{newBook.Author},{newBook.PageNumber},{newBook.Borrower},{newBook.Status},{newBook.DueDate}";

            try
            {
                // Append the new client line to the existing file or create a new file if it doesn't exist -> UPDATING the file
                File.AppendAllText(filePath, bookLine + Environment.NewLine);

                MessageBox.Show("Book information saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);       //Informs the user
            }
            catch (Exception ex)    //For error handling
            {
                MessageBox.Show($"An error occurred while saving book information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }

        /// <summary>
        /// Will close the current opened window
        /// </summary>
        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
