using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class MembershipSalesPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user; // Храним текущего пользователя

        public MembershipSalesPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadMembershipSales();
        }

        private void LoadMembershipSales()
        {
            try
            {
                MembershipSalesDataGrid.ItemsSource = _db.MembershipSales
                    .Select(s => new
                    {
                        s.SaleID,
                        s.ClientID,
                        s.MembershipID,
                        s.SaleDate
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_user.RoleID == 1) // Допустим, 1 — это администратор
                NavigationService.Navigate(new AdminPage(_user));
            else if (_user.RoleID == 2) // 2 — бухгалтер
                NavigationService.Navigate(new AccountantPage(_user));
        }
    }
}
