using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ClientPage : Page
    {
        private User _user;

        public ClientPage(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void BackToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutharizationPage());
        }

        private void GroupTrainings_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ClientGroupTrainingsPage(_user));
        private void ClubServices_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ClientClubServicesPage(_user));
        private void Reviews_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ClientReviewsPage(_user));
        private void Memberships_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ClientMembershipsPage(_user));
        private void Clubs_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ClientClubsPage(_user));
    }
}
