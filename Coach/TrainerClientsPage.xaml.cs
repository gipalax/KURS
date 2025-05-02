using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;

namespace KP
{
    public partial class TrainerClientsPage : Page
    {
        private User _user;
        private List<Client> _clientsList; // Кэшируем полный список для поиска

        public TrainerClientsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadClients();
        }

        private void LoadClients()
        {
            _clientsList = Entities1.Getcontext().Clients.ToList();
            ClientsDataGrid.ItemsSource = _clientsList;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            var filtered = _clientsList
                .Where(c => (c.FirstName != null && c.FirstName.ToLower().Contains(searchText)) ||
                            (c.LastName != null && c.LastName.ToLower().Contains(searchText)))
                .ToList();

            ClientsDataGrid.ItemsSource = filtered;
        }


        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            ClientsDataGrid.ItemsSource = _clientsList;
        }

        // Handler for the Back button
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // This will navigate back to the previous page
            NavigationService.GoBack();
        }
    }
}
