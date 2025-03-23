using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class AccountantServicesPage : Page
    {
        private readonly Entities1 _db = new Entities1(); // Контекст базы данных
        private User _user;

        public AccountantServicesPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadProvidedServices();
        }

        private void LoadProvidedServices()
        {
            ProvidedServicesListView.ItemsSource = _db.ProvidedServices
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountantPage(_user)); // Вернуться на главную страницу бухгалтера
        }
    }
}
