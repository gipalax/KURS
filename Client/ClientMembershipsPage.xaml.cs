using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ClientMembershipsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user;

        public ClientMembershipsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadMemberships();
        }

        // Загрузка данных в DataGrid
        private void LoadMemberships()
        {
            try
            {
                MembershipsDataGrid.ItemsSource = _db.Memberships
                    .Select(m => new
                    {
                        m.MembershipID,
                        m.Name,
                        m.Price,
                        m.DurationDays
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
            NavigationService.GoBack();
        }
    }
}
