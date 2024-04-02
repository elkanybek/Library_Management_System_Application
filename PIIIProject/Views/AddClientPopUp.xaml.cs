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
    /// Interaction logic for AddClientPopUp.xaml
    /// </summary>
    public partial class AddClientPopUp : Window
    {
        //Data fields:
        private Library library = new Library();

        //Methods:

        /// <summary>
        /// Gets the data from the files 
        /// </summary>
        public void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();
        }

        /// <summary>
        /// Initializes a new instance of the AddClientPopUp class.
        /// </summary>
        public AddClientPopUp()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();
        }

        /// <summary>
        /// Validates the click on the add client, by verifying if the fields are null or empty.
        /// </summary>
        /// <param name="sender">the object that triggered the event</param>
        /// <param name="e">the event</param>
        private void Btn_AddClient(object sender, RoutedEventArgs e)
        {
            //Checks if the user input left no empty fields to log in

            if (string.IsNullOrEmpty(txbFirstName.Text))
            {
                MessageBox.Show("Please enter a first name", "First Name Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txbLastName.Text))
            {
                MessageBox.Show("Please enter a last name", "Last Name Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txbPassword.Text))
            {
                MessageBox.Show("Please enter password", "Password Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Client newClient = new Client(txbFirstName.Text, txbLastName.Text, txbPassword.Text);
            string generatedUsername = newClient.Username;


            // Validate if the generated username already exists in the library
            bool usernameExists = ExistUsername(generatedUsername);


            // If a client with the generated username exists, show an error message
            if (usernameExists)
            {
                MessageBox.Show("A client with this username already exists", "Username Exists", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                library.Client.Add(newClient);
                SaveClientToFile(newClient);
                MessageBox.Show($"Client added successfuly. Username is {newClient.Username} and password {newClient.Password}");
                Close();
            }
        }

        /// <summary>
        /// Will update the user txt file by adding the new client to it.
        /// </summary>
        /// <param name="newClient">the new client object to add to the list</param>
        private void SaveClientToFile(Client newClient)
        {
            string filePath = "../../../TestFiles/Users.txt";

            // Create a new line with the client's information
            string clientLine = $"1,{newClient.FirstName},{newClient.LastName},{newClient.Username},{newClient.Password}";

            try
            {
                // Append the new client line to the existing file or create a new file if it doesn't exist
                File.AppendAllText(filePath, clientLine + Environment.NewLine);

                MessageBox.Show("Client information saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving client information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if a generated username already exists among the clients in the library.
        /// </summary>
        /// <param name="generatedUsername">the passed created username</param>
        /// <returns>a bool, true if the username alr exists in the library.</returns>
        private bool ExistUsername(string generatedUsername)
        {
            bool usernameExists = false;

            foreach (Client existingClient in library.Client)
            {
                if (existingClient.Username == generatedUsername)
                {
                    usernameExists = true; // Username exists
                }
            }

            return usernameExists;
        }

        /// <summary>
        /// Closes the current window
        /// </summary>
        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
