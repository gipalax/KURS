using System.Windows;
using System.Windows.Controls;
using KP;

namespace KP
{
    public partial class CoachPage : Page
    {
        private User _user;

        public CoachPage(User user)
        {
            InitializeComponent();
            _user = user;
            // Логика для тренера
        }

        private void BackToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutharizationPage());
        }
    }
}