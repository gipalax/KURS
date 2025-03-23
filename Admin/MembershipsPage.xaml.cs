using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class MembershipsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user;

        public MembershipsPage(User user)
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

        // Заполняем поля при выборе элемента в таблице
        private void MembershipsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MembershipsDataGrid.SelectedItem != null)
            {
                dynamic selectedMembership = MembershipsDataGrid.SelectedItem;
                NameBox.Text = selectedMembership.Name;
                PriceBox.Text = selectedMembership.Price.ToString();
                DurationBox.Text = selectedMembership.DurationDays.ToString();
            }
        }

        // Добавление абонемента
        private void AddMembership_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newMembership = new Membership
                {
                    Name = NameBox.Text,
                    Price = decimal.Parse(PriceBox.Text),
                    DurationDays = int.Parse(DurationBox.Text)
                };

                _db.Memberships.Add(newMembership);
                _db.SaveChanges();
                LoadMemberships();

                MessageBox.Show("Абонемент добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Редактирование абонемента
        private void EditMembership_Click(object sender, RoutedEventArgs e)
        {
            if (MembershipsDataGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedMembership = MembershipsDataGrid.SelectedItem;
                    int id = selectedMembership.MembershipID;
                    var membership = _db.Memberships.FirstOrDefault(m => m.MembershipID == id);

                    if (membership != null)
                    {
                        membership.Name = NameBox.Text;
                        membership.Price = decimal.Parse(PriceBox.Text);
                        membership.DurationDays = int.Parse(DurationBox.Text);

                        _db.SaveChanges();
                        LoadMemberships();
                        MessageBox.Show("Абонемент обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка редактирования: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
       
        // Удаление абонемента
        private void DeleteMembership_Click(object sender, RoutedEventArgs e)
        {
            if (MembershipsDataGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedMembership = MembershipsDataGrid.SelectedItem;
                    int id = selectedMembership.MembershipID;
                    var membership = _db.Memberships.FirstOrDefault(m => m.MembershipID == id);

                    if (membership != null)
                    {
                        _db.Memberships.Remove(membership);
                        _db.SaveChanges();
                        LoadMemberships();
                        MessageBox.Show("Абонемент удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Назад в админку
        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage(_user));
        }
    }
}
