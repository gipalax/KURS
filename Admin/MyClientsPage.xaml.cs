using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class MyClientsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user; // Храним текущего пользователя

        public MyClientsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                ClientsDataGrid.ItemsSource = _db.Clients
                    .Select(c => new
                    {
                        c.ClientID,
                        c.LastName,  
                        c.FirstName, 
                        c.Email,
                        c.PhoneNumber,
                        c.RoleID
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage(_user)); // Передаем user обратно
        }
    }
}
