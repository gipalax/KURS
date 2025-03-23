using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class AccountantSalesPage : Page
    {
        private readonly Entities1 _db = new Entities1(); // Контекст базы данных
        private User _user;

        public AccountantSalesPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadMembershipSales();
        }

        private void LoadMembershipSales()
        {
            MembershipSalesListView.ItemsSource = _db.MembershipSales
                .Select(s => new
                {
                    s.SaleID,
                    s.ClientID,    
                    s.MembershipID, 
                    s.SaleDate
                })
                .ToList();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountantPage(_user)); // Вернуться на главную страницу бухгалтера
        }
    }
}
