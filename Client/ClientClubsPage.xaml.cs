using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ClientClubsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user; // Храним текущего пользователя

        public ClientClubsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadClubs();
        }

        private void LoadClubs()
        {
            try
            {
                ClubsDataGrid.ItemsSource = _db.Clubs
                    .Select(c => new
                    {
                        c.ClubID,
                        c.Name,
                        c.Address,
                        c.PhoneNumber
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
