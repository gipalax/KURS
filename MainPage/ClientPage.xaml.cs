using System.Windows;
using System.Windows.Controls;
using KP;

namespace KP
{
    public partial class ClientPage : Page
    {
        private User _user;

        public ClientPage(User user)
        {
            InitializeComponent();
            _user = user;
            // Логика для клиента
        }

        private void BackToAuthButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AutharizationPage());
        }
    }
}