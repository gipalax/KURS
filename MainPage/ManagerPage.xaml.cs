using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ManagerPage : Page
    {
        private User _user;

        public ManagerPage(User user) 
        {
            InitializeComponent();
            _user = user;
        }

        private void ClientsPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerClientsPage(_user));
        }

        private void ManagerMembershipSalesPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerMembershipSalesPage(_user));
        }

        private void TrainingsPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerTrainingsPage(_user));
        }

        private void FeedbackPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerFeedbackPage(_user));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutharizationPage());
        }
    }
}
