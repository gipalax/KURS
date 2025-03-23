using System.Windows;
using System.Windows.Controls;
using KP;

namespace KP
{
    public partial class ManagerPage : Page
    {
        private User _user;

        public ManagerPage(User user)
        {
            InitializeComponent();
            _user = user;
            // Логика для менеджера
        }

        private void BackToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutharizationPage());
        }
    }
}