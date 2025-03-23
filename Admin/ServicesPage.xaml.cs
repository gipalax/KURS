using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ServicesPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user; // Храним текущего пользователя

        public ServicesPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadServices();
        }

        private void LoadServices()
        {
            try
            {
                ServicesDataGrid.ItemsSource = _db.ProvidedServices
                    .Select(s => new
                    {
                        s.ProvidedServiceID,
                        s.ClientID,
                        s.ServiceID,
                        s.EmployeeID,
                        s.DateTime
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
            NavigationService.Navigate(new AdminPage(_user)); // Возвращаемся в админку
        }
    }
}
