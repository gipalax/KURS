using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ClientClubServicesPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user;

        public ClientClubServicesPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadClubServices();
        }

        private void LoadClubServices()
        {
            try
            {
                ClubServicesDataGrid.ItemsSource = _db.ClubServices
                    .Select(s => new
                    {
                        s.ServiceID,
                        s.Name,
                        s.Price
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
