using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class TrainerClubServicesPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user;

        public TrainerClubServicesPage(User user)
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

        private void AddClubService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newService = new ClubService
                {
                    Name = NameBox.Text,
                    Price = decimal.Parse(PriceBox.Text)
                };

                _db.ClubServices.Add(newService);
                _db.SaveChanges();
                LoadClubServices();

                MessageBox.Show("Услуга добавлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message);
            }
        }

        private void EditClubService_Click(object sender, RoutedEventArgs e)
        {
            if (ClubServicesDataGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedItem = ClubServicesDataGrid.SelectedItem;
                    int id = selectedItem.ServiceID;
                    var service = _db.ClubServices.FirstOrDefault(s => s.ServiceID == id);

                    if (service != null)
                    {
                        service.Name = NameBox.Text;
                        service.Price = decimal.Parse(PriceBox.Text);
                        _db.SaveChanges();
                        LoadClubServices();
                        MessageBox.Show("Услуга обновлена!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка редактирования: " + ex.Message);
                }
            }
        }

        private void DeleteClubService_Click(object sender, RoutedEventArgs e)
        {
            if (ClubServicesDataGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedItem = ClubServicesDataGrid.SelectedItem;
                    int id = selectedItem.ServiceID;
                    var service = _db.ClubServices.FirstOrDefault(s => s.ServiceID == id);

                    if (service != null)
                    {
                        _db.ClubServices.Remove(service);
                        _db.SaveChanges();
                        LoadClubServices();
                        MessageBox.Show("Услуга удалена!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }
            }
        }

        private void ClubServicesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClubServicesDataGrid.SelectedItem != null)
            {
                dynamic selectedItem = ClubServicesDataGrid.SelectedItem;
                NameBox.Text = selectedItem.Name;
                PriceBox.Text = selectedItem.Price.ToString();
            }
        }

        private void BackToTrainer_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
