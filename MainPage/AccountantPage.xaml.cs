using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class AccountantPage : Page
    {
        private User _user;

        public AccountantPage(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void BackToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutharizationPage());
        }

        private void MembershipSales_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new AccountantSalesPage(_user));
        private void ProvidedServices_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new AccountantServicesPage(_user));
        private void Salaries_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new AccountantSalariesPage(_user));
        private void FinancialReports_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new AccountantReportsPage(_user));
    }
}
