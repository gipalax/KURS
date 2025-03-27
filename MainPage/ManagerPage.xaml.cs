using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ManagerPage : Page
    {
        private readonly User _currentUser;

        public ManagerPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        // Навигация
        private void Clients_Click(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ManagerClientsPage(_currentUser));

        private void Sales_Click(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ManagerSalesPage(_currentUser));

        /*private void Schedule_Click(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ManagerSchedulePage(_currentUser));

        private void Reports_Click(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new ManagerReportsPage(_currentUser));*/

        // Возврат на страницу авторизации
        private void BackToAuth_Click(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(new AutharizationPage());
    }
}