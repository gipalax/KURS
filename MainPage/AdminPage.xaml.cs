using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class AdminPage : Page
    {
        private User _user;

        public AdminPage(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void BackToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutharizationPage());
        }

        private void Clients_Click(object sender, RoutedEventArgs e)=> NavigationService.Navigate(new MyClientsPage(_user)); // Передаем _user
        private void Inventory_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new InventoryPage(_user));
        private void Memberships_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new MembershipsPage(_user));
        private void Sales_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new MembershipSalesPage(_user));
        private void Services_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ServicesPage(_user));
        private void Clubs_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ClubsPage(_user));
        private void ClubServices_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ClubServicesPage(_user));
        private void Employees_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new EmployeesPage(_user));
        private void GroupTrainings_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new GroupTrainingsPage(_user));
        private void IndividualTrainings_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new IndividualTrainingsPage(_user));
    }
}
