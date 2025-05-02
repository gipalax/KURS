using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class CoachPage : Page
    {
        private User _user;

        public CoachPage(User user)
        {
            InitializeComponent();
            _user = user;
        }

        private void BackToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutharizationPage());
        }

        private void Clients_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new TrainerClientsPage(_user));
        private void ClubServices_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new TrainerClubServicesPage(_user));
        private void ServiceRegistrations_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new TrainerServiceRegistrationsPage(_user));
        private void TrainingsPage_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new TrainerTrainingsPage(_user));
        
        private void ClientReviews_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new TrainerClientReviewsPage(_user));
        private void TrainingTypes_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new TrainerTrainingTypesPage(_user));
    }
}
