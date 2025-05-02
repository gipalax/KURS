using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using KP;

namespace KP
{
    public partial class ManagerAddEditClientPage : Page
    {
        private Client _currentClient;

        private User _user;

        public ManagerAddEditClientPage(Client selectedClient, User user)
        {
            InitializeComponent();
            _user = user;
            _currentClient = selectedClient ?? new Client();

            if (selectedClient != null)
            {
                FirstNameBox.Text = _currentClient.FirstName;
                LastNameBox.Text = _currentClient.LastName;
                EmailBox.Text = _currentClient.Email;
                PhoneBox.Text = _currentClient.PhoneNumber;
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(FirstNameBox.Text))
                errors.AppendLine("Введите имя.");
            if (string.IsNullOrWhiteSpace(LastNameBox.Text))
                errors.AppendLine("Введите фамилию.");
            if (string.IsNullOrWhiteSpace(EmailBox.Text))
                errors.AppendLine("Введите email.");
            if (string.IsNullOrWhiteSpace(PhoneBox.Text))
                errors.AppendLine("Введите номер телефона.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка");
                return;
            }

            try
            {
                using (var db = new Entities1()) // создаём новый контекст
                {
                    if (_currentClient.ClientID == 0)
                    {
                        // Новый клиент — просто заполняем и добавляем
                        var newClient = new Client
                        {
                            FirstName = FirstNameBox.Text,
                            LastName = LastNameBox.Text,
                            Email = EmailBox.Text,
                            PhoneNumber = PhoneBox.Text,
                            RoleID = 5 // Предположим, что 5 — это клиентская роль
                        };

                        db.Clients.Add(newClient);
                    }
                    else
                    {
                        // Существующий клиент — ищем по ID в контексте
                        var existingClient = db.Clients.FirstOrDefault(c => c.ClientID == _currentClient.ClientID);
                        if (existingClient != null)
                        {
                            existingClient.FirstName = FirstNameBox.Text;
                            existingClient.LastName = LastNameBox.Text;
                            existingClient.Email = EmailBox.Text;
                            existingClient.PhoneNumber = PhoneBox.Text;
                            // existingClient.RoleID = ... если надо
                        }
                    }

                    db.SaveChanges();
                }

                MessageBox.Show("Данные успешно сохранены!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
