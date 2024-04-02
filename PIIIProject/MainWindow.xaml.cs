using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PIIIProject.Models;
using PIIIProject.Repos;
using PIIIProject.Views;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //data-members:
        private Library library = new Library();

        /// <summary>
        /// Retrieves the list of books, employees, client.
        /// </summary>
        public void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();
        }
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();
        }

        /// <summary>
        /// This function is responsible for the checking of the login as an employee or a client and display appropriate info messages and redirects to the new page window.
        /// </summary>
        private void BtnClick_LogIn(object sender, RoutedEventArgs e)
        {
            //Checks if the user input left no empty fields to log in

            if(string.IsNullOrEmpty(txbUsernameLogIn.Text))
            {
                MessageBox.Show("Please enter username", "Username Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(pwbPasswordLogIn.Password))
            {
                MessageBox.Show("Please enter password", "Password Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (EmployeeRadioButton.IsChecked == true)
            {
                //Checks if the user exist in the list and compares their password before redirecting to the next page.
                string enteredUsername = txbUsernameLogIn.Text;
                Employee matchingEmployee = null;
                foreach (Employee employee in library.Employees)
                {
                    if (employee.Username == enteredUsername)
                    {
                        matchingEmployee = employee;
                        break; // Exit the loop when a match is found
                    }
                }


                // checks if the matching employee variable is null or not (found a match or not)
                if (matchingEmployee != null)
                {                    
                    string enteredPassword = pwbPasswordLogIn.Password;
                    if (matchingEmployee.PasswordAuthentication(enteredPassword)) // makes sure the right password was entered
                    {
                        MessageBox.Show("Employee login successful");
                        EmployeeWindow employees = new EmployeeWindow();
                        employees.Show();
                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid password", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Employee with the specified username not found", "User Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                
            }
            else if (ClientRadioButton.IsChecked == true)
            {
                //Checks if the user exist in the list and compares their password before redirecting to the next page.

                string enteredUsername = txbUsernameLogIn.Text;
                Client matchingClient = null;

                foreach (Client client in library.Client)
                {
                    if (client.Username == enteredUsername)
                    {
                        matchingClient = client;
                        break; // Exit the loop when a match is found
                    }
                }

                if (matchingClient != null)   // checks if the matching client variable is null or not (found a match or not)
                {
                    string enteredPassword = pwbPasswordLogIn.Password;
                    if (matchingClient.PasswordAuthentication(enteredPassword))  // makes sure the right password was entered
                    {
                        MessageBox.Show("Client login successful");
                        ClientWindow clients = new ClientWindow(matchingClient.Username);
                        clients.Show();
                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid password", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Client with the specified username not found", "User Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a user type", "User Type Missing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClink_SignIn(object sender, RoutedEventArgs e)
        {
            //Checks if the user input left no empty fields to log in

            if (string.IsNullOrEmpty(txbFirstNameSignIn.Text))
            {
                MessageBox.Show("Please enter a first name", "First Name Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txbLastNameSignIn.Text))
            {
                MessageBox.Show("Please enter a last name", "Last Name Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(pwbPasswordSignIn.Password))
            {
                MessageBox.Show("Please enter password", "Password Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (pwbPasswordSignIn.Password.Length<6)
            {
                MessageBox.Show("Password must be 6 characters or more", "Password must be 6 characters or more", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Client newClient = new Client(txbFirstNameSignIn.Text, txbLastNameSignIn.Text, pwbPasswordSignIn.Password);
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
                MessageBox.Show($"Client sign-in successful. Your username is {newClient.Username} and password {newClient.Password}");
                ClientWindow clients = new ClientWindow(newClient.Username);
                clients.Show();
                
            }
        }

        /// <summary>
        /// Checks if the generated username is alr in the client library
        /// </summary>
        /// <param name="generatedUsername">generatedusername</param>
        /// <returns>bool true or false</returns>
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
        /// Using a streamWriter to open the file and add the newclient for the next log in 
        /// </summary>
        /// <param name="client">client to add in the file</param>
        private void SaveClientToFile(Client client)
        {
            string filePath = "../../../TestFiles/Users.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    // Append the client information to the file
                    writer.WriteLine($"1,{client.FirstName},{client.LastName},{client.Username},{client.Password}");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Error happened while saving client to file: {ex.Message}", "File Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //private void HandleCheck(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
