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
    public partial class ClientSearch : Window
    {
        private Library library = new Library();

        public void GetData()
        {
            library.Books = Data.GetBooksData();
            library.Employees = Data.GetEmployeeData();
            library.Client = Data.GetClientData();

        }
        public ClientSearch()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            GetData();

        }

        /// <summary>
        ///  When employees enters 3 or more characters in the search box and presses the search button, the client username starting with those characters are displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SearchClientEmployee(object sender, RoutedEventArgs e)
        {

            string searchText = txtSearch.Text.ToLower();

            if (searchText.Length < 3) // makes sure the user entered at least 3 characters
            {
                MessageBox.Show("Search text must be at least 3 characters long.");
                return;
            }

            List<Client> filteredClients = new List<Client>();

            foreach (Client client in library.Client) // finds client username containing letters from the search
            {
                if (client.Username.ToLower().Contains(searchText))
                {
                    filteredClients.Add(client);// adds them to the list
                }
            }

            
            lbxClients.ItemsSource = filteredClients; // displays them
        }

        /// <summary>
        /// Displays information about the client (password, username, fullname, borrowed books)
        /// </summary>
        /// <param name="client"></param>
        private void DisplayInfo(Client client)
        {

            if (lbxClients.SelectedItem is Client selectedClient)
            {
                // Display details of the selected client in the TextBlock
                StringBuilder detailsBuilder = new StringBuilder(selectedClient.ToString()); // gets the client info (full name, username, password)
                detailsBuilder.AppendLine(); // Add a line break

                // Append borrowed books information
                detailsBuilder.AppendLine("Borrowed Books:");
                foreach (Book book in library.Books)
                {
                    if (book.Borrower == selectedClient.Username)
                    {
                        detailsBuilder.AppendLine(book.Title);
                    }
                }

                // displays all of the info
                txtSearchDetails.Text = detailsBuilder.ToString();
            }

        }

        
        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxClients.SelectedItem is Client selectedClient)
            {
                // Display details of the selected book in the TextBlock using ToString
                txtSearchDetails.Text = selectedClient.ToString();
                DisplayInfo(selectedClient);

            }

           
        }

        /// <summary>
        /// Deletes the selected client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_DeleteSelectedClient(object sender, RoutedEventArgs e)
        {

            if (lbxClients.SelectedItem is Client selectedClient)
            {
                // Confirm with the user before deleting the book
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the client:\n{selectedClient.FirstName} {selectedClient.LastName}?",
                                                          "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question); // confirms the client is to be deleted

                if (result == MessageBoxResult.Yes)
                {
                    // Delete the selected client from the library.Books collection
                    library.Client.Remove(selectedClient);

                    // save changes to a file
                    SaveClientsToFile();

                    // Update the ListBox
                    lbxClients.ItemsSource = null;
                    lbxClients.ItemsSource = library.Client;

                    //provide a message to indicate successful deletion
                    MessageBox.Show("Book deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Saves the changes made to the data file
        /// </summary>
        private void SaveClientsToFile()
        {
            string filePath = "../../../TestFiles/Users.txt";
            // Read existing content from the file
            List<string> lines = File.ReadAllLines(filePath).ToList();
            // Get the selected client for deletion
            Client selectedClient = lbxClients.SelectedItem as Client;
            List<string> newLines = new List<string>();

            // Iterate through each line and exclude the line corresponding to the selected client
            foreach (string line in lines)
            {
               
                if (!line.Contains(selectedClient.Username))
                {
                    newLines.Add(line);
                }
            }

            // Write the modified content back to the file
            File.WriteAllLines(filePath, newLines);

            // provide a message to indicate successful save
            MessageBox.Show("Client deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //returns to the previous page
        private void Btn_ClientSearchDone(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //brings to a new page that allows to add new client
        private void Btn_AddClient(object sender, RoutedEventArgs e)
        {
            AddClientPopUp addClient= new AddClientPopUp();
            addClient.Show();
        }
    }
}
