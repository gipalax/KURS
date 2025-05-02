using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;

namespace KP
{
    public partial class TrainerServiceRegistrationsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user;
        private List<ProvidedServiceViewModel> _services;

        public class ProvidedServiceViewModel
        {
            public int ProvidedServiceID { get; set; }
            public string ClientName { get; set; }
            public string ServiceName { get; set; }
            public string EmployeeName { get; set; }
            public string DateTime { get; set; }
        }

        public TrainerServiceRegistrationsPage(User user)
        {
            InitializeComponent();
            _user = user;
            UpdateProvidedServices();
        }

        private void UpdateProvidedServices()
        {
            // Сначала получаем данные без форматирования даты
            var query = _db.ProvidedServices
                .Select(ps => new
                {
                    ps.ProvidedServiceID,
                    ClientFirstName = ps.Client.FirstName,
                    ClientLastName = ps.Client.LastName,
                    ServiceName = ps.ClubService.Name,
                    EmployeeFirstName = ps.Employee.FirstName,
                    EmployeeLastName = ps.Employee.LastName,
                    ps.DateTime
                })
                .AsEnumerable(); // Переключаемся на LINQ to Objects

            // Теперь выполняем преобразование в памяти
            _services = query.Select(x => new ProvidedServiceViewModel
            {
                ProvidedServiceID = x.ProvidedServiceID,
                ClientName = x.ClientFirstName + " " + x.ClientLastName,
                ServiceName = x.ServiceName,
                EmployeeName = x.EmployeeFirstName + " " + x.EmployeeLastName,
                DateTime = x.DateTime.ToString("g") // Теперь это выполняется в памяти
            })
                .ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string searchText = SearchBox.Text.ToLower();
                _services = _services.Where(s =>
                    (s.ClientName != null && s.ClientName.ToLower().Contains(searchText)) ||
                    (s.ServiceName != null && s.ServiceName.ToLower().Contains(searchText)) ||
                    (s.EmployeeName != null && s.EmployeeName.ToLower().Contains(searchText))
                ).ToList();
            }

            ProvidedServicesDataGrid.ItemsSource = _services;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProvidedServices();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainerAddEditServiceRegistrationPage(null, _user));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = ProvidedServicesDataGrid.SelectedItem as ProvidedServiceViewModel;
            if (selected != null)
            {
                // Получаем полный объект ProvidedService из базы данных
                var service = _db.ProvidedServices.FirstOrDefault(ps => ps.ProvidedServiceID == selected.ProvidedServiceID);

                if (service != null)
                {
                    NavigationService.Navigate(new TrainerAddEditServiceRegistrationPage(service, _user));
                }
                else
                {
                    MessageBox.Show("Запись не найдена в базе данных.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = ProvidedServicesDataGrid.SelectedItem as ProvidedServiceViewModel;
            if (selected != null)
            {
                if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var service = _db.ProvidedServices.FirstOrDefault(p => p.ProvidedServiceID == selected.ProvidedServiceID);
                        if (service != null)
                        {
                            _db.ProvidedServices.Remove(service);
                            _db.SaveChanges();
                            UpdateProvidedServices();
                            MessageBox.Show("Запись успешно удалена.", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.InnerException?.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}