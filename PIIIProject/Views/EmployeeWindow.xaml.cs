using PIIIProject.Models;
using PIIIProject.Repos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        //Data-fields:
        private Library library = new Library();

        //Methods:
        /// <summary>
        /// Will fetch the infos from the txt files in the bin
        /// </summary>
        public void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();
        }

        /// <summary>
        /// Initializes a new instance of the EmployeeWindow class.
        /// </summary>
        public EmployeeWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();
        }

        /// <summary>
        /// Opens a BookSearch window.
        /// </summary>
        /// <param name="sender"> object that triggered the event.</param>
        /// <param name="e">the event</param>
        private void Btn_BookSearch(object sender, RoutedEventArgs e)
        {
            BookSearch bookSearch = new BookSearch();
            bookSearch.Show();
        }

        /// <summary>
        /// Opens a ClientSearch window.
        /// </summary>
        /// <param name="sender">object that triggered the event.</param>
        /// <param name="e">The event</param>
        private void Client_Search(object sender, RoutedEventArgs e)
        {
            ClientSearch clientSearch = new ClientSearch();
            clientSearch.Show();
        }

        /// <summary>
        /// Opens an AddEmployeePopUp window.
        /// </summary>
        /// <param name="sender">object that triggered the event.</param>
        /// <param name="e">the event</param>
        private void Btn_AddEmployee(object sender, RoutedEventArgs e)
        {
            AddEmployeePopUp addEmployee = new AddEmployeePopUp();
            addEmployee.Show();
        }

        /// <summary>
        /// Closes the current window, logging out the user.
        /// </summary>
        /// <param name="sender">object that triggered the event.</param>
        /// <param name="e">the event</param>
        private void Btn_Logout(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
