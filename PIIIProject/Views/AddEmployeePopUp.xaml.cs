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
    /// Interaction logic for AddEmployeePopUp.xaml
    /// </summary>
    public partial class AddEmployeePopUp : Window
    {
        //data-members:
        private Library library = new Library();

        //methods:

        /// <summary>
        ///  Will fetch the infos from the txt files in the bin
        /// </summary>
        public void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();
        }

        /// <summary>
        /// Initializes a new instance of the addemployeepopup class.
        /// </summary>
        public AddEmployeePopUp()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();
        }

        /// <summary>
        /// Validates the click on the add employee, by verifying if the fields are null or empty.
        /// </summary>
        /// <param name="sender">the object that triggered the event</param>
        /// <param name="e">the event</param>
        private void Btn_AddEmployee(object sender, RoutedEventArgs e)
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

            Employee newEmployee = new Employee(txbFirstName.Text, txbLastName.Text, txbPassword.Text);
            Client newEmployeeClient = new Client(txbFirstName.Text, txbLastName.Text, txbPassword.Text);
            string generatedUsername = newEmployee.Username;


            // Validate if the generated username already exists in the library
            bool usernameExists = ExistUsername(generatedUsername);


            // If a client with the generated username exists, show an error message
            if (usernameExists)
            {
                MessageBox.Show("An Employee with this username already exists", "Username Exists", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                library.Employees.Add(newEmployee);
                library.Client.Add(newEmployeeClient);
                SaveEmployeeToFile(newEmployee);
                MessageBox.Show($"Employee added successfuly. Username is {newEmployee.Username} and password {newEmployee.Password}");
                Close();
            }
        }

        /// <summary>
        /// Will update the user txt file by adding the new employee to it.
        /// </summary>
        /// <param name="newEmployee">the new employee object</param>
        private void SaveEmployeeToFile(Employee newEmployee)
        {
            string filePath = "../../../TestFiles/Users.txt";

           
            string employeeLine = $"2,{newEmployee.FirstName},{newEmployee.LastName},{newEmployee.Username},{newEmployee.Password},{newEmployee.EmployeeId}";

            try
            {
                // Append the new client line to the existing file or create a new file if it doesn't exist
                File.AppendAllText(filePath, employeeLine + Environment.NewLine);

                MessageBox.Show("Employee information saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving employe information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if a generated username already exists among the employee in the library.
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
        /// Closes the current window.
        /// </summary>
        private void Btn_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
