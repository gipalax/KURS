using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class ManagerClientsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _currentUser;
        private int _selectedClientId = -1;

        public ManagerClientsPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                ClientsGrid.ItemsSource = _db.Clients.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}");
            }
        }

        private void SearchClient_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            ClientsGrid.ItemsSource = _db.Clients
                .Where(c => c.FirstName.ToLower().Contains(searchText) ||
                           c.LastName.ToLower().Contains(searchText) ||
                           c.PhoneNumber.Contains(searchText))
                .ToList();
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientsGrid.SelectedItem is Client selectedClient)
            {
                _selectedClientId = selectedClient.ClientID;
                FirstNameBox.Text = selectedClient.FirstName;
                LastNameBox.Text = selectedClient.LastName;
                PhoneBox.Text = selectedClient.PhoneNumber;
                EmailBox.Text = selectedClient.Email;
            }
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newClient = new Client
                {
                    FirstName = FirstNameBox.Text,
                    LastName = LastNameBox.Text,
                    PhoneNumber = PhoneBox.Text,
                    Email = EmailBox.Text,
                    RoleID = 4 // Предполагаем, что 4 - это ID роли "Клиент"
                };

                _db.Clients.Add(newClient);
                _db.SaveChanges();
                LoadClients();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}");
            }
        }

        private void UpdateClient_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClientId == -1) return;

            try
            {
                var client = _db.Clients.First(c => c.ClientID == _selectedClientId);
                client.FirstName = FirstNameBox.Text;
                client.LastName = LastNameBox.Text;
                client.PhoneNumber = PhoneBox.Text;
                client.Email = EmailBox.Text;

                _db.SaveChanges();
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}");
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClientId == -1) return;

            try
            {
                var client = _db.Clients.First(c => c.ClientID == _selectedClientId);
                _db.Clients.Remove(client);
                _db.SaveChanges();
                LoadClients();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        private void ClearFields()
        {
            FirstNameBox.Text = "";
            LastNameBox.Text = "";
            PhoneBox.Text = "";
            EmailBox.Text = "";
            _selectedClientId = -1;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerPage(_currentUser));
        }
    }
}