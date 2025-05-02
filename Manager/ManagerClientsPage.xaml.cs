using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ManagerClientsPage : Page
    {
        private User _user;

        public ManagerClientsPage(User user)
        {
            InitializeComponent();
            _user = user;
            UpdateClients();
        }

        private void UpdateClients()
        {
            var clients = Entities1.Getcontext().Clients.ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string searchText = SearchBox.Text.ToLower();
                clients = clients.Where(c =>
                    (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(c.LastName) && c.LastName.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(c.PhoneNumber) && c.PhoneNumber.Contains(searchText))
                ).ToList();
            }

            ClientsDataGrid.ItemsSource = clients; // Обратите внимание: ClientsDataGrid вместо ClientsGrid
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClients();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerAddEditClientPage(null, _user));
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selected)
            {
                NavigationService.Navigate(new ManagerAddEditClientPage(selected, _user));
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования.");
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDataGrid.SelectedItem is Client selected)
            {
                if (MessageBox.Show($"Удалить клиента {selected.FirstName} {selected.LastName}?",
                    "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Entities1.Getcontext().Clients.Remove(selected);
                        Entities1.Getcontext().SaveChanges();
                        UpdateClients();
                        MessageBox.Show("Клиент удалён.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении: " + ex.InnerException?.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления.");
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}