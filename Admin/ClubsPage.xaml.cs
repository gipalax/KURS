using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class ClubsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user; // Храним текущего пользователя

        public ClubsPage(User user)
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

        private void ClubsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClubsDataGrid.SelectedItem != null)
            {
                dynamic selectedClub = ClubsDataGrid.SelectedItem;
                NameBox.Text = selectedClub.Name;
                AddressBox.Text = selectedClub.Address;
                PhoneBox.Text = selectedClub.PhoneNumber;
            }
        }

        private void AddClub_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newClub = new Club
                {
                    Name = NameBox.Text,
                    Address = AddressBox.Text,
                    PhoneNumber = PhoneBox.Text
                };

                _db.Clubs.Add(newClub);
                _db.SaveChanges();
                LoadClubs();
                MessageBox.Show("Клуб добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditClub_Click(object sender, RoutedEventArgs e)
        {
            if (ClubsDataGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedClub = ClubsDataGrid.SelectedItem;
                    int clubID = selectedClub.ClubID;
                    var club = _db.Clubs.FirstOrDefault(c => c.ClubID == clubID);

                    if (club != null)
                    {
                        club.Name = NameBox.Text;
                        club.Address = AddressBox.Text;
                        club.PhoneNumber = PhoneBox.Text;

                        _db.SaveChanges();
                        LoadClubs();
                        MessageBox.Show("Клуб обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка редактирования: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteClub_Click(object sender, RoutedEventArgs e)
        {
            if (ClubsDataGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedClub = ClubsDataGrid.SelectedItem;
                    int clubID = selectedClub.ClubID;
                    var club = _db.Clubs.FirstOrDefault(c => c.ClubID == clubID);

                    if (club != null)
                    {
                        _db.Clubs.Remove(club);
                        _db.SaveChanges();
                        LoadClubs();
                        MessageBox.Show("Клуб удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage(_user)); // Возвращаемся в админку
        }
    }
}
