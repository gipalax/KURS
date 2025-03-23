using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KP;

namespace KP
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginText.Text.Length == 0)
            {
                MessageBox.Show("Укажите логин!");
                return;
            }

            using (var db = new Entities1())
            {
                var existingUser = db.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Username == loginText.Text);

                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь уже существует!");
                    return;
                }

                if (passwordText.Password != confirmPasswordText.Password)
                {
                    MessageBox.Show("Пароли не совпадают!");
                    return;
                }

                bool hasLetter = false;
                bool hasNumber = false;
                foreach (char c in passwordText.Password)
                {
                    if (char.IsLetter(c)) hasLetter = true;
                    if (char.IsDigit(c)) hasNumber = true;
                }

                var phoneRegex = new Regex(@"^\+7\d{10}$");
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

                StringBuilder errors = new StringBuilder();

                if (passwordText.Password.Length < 6)
                    errors.AppendLine("Пароль должен быть длиннее 6 символов");
                if (!phoneRegex.IsMatch(phoneText.Text))
                    errors.AppendLine("Номер телефона должен быть в формате +7XXXXXXXXXX");
                if (!hasLetter)
                    errors.AppendLine("Пароль должен содержать буквы");
                if (!hasNumber)
                    errors.AppendLine("Пароль должен содержать цифры");
                if (!emailRegex.IsMatch(emailText.Text))
                    errors.AppendLine("Неверный формат email");

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }

                var newClient = new Client
                {
                    FirstName = nameText.Text,
                    LastName = surnameText.Text,
                    Email = emailText.Text,
                    PhoneNumber = phoneText.Text,
                    RoleID = 5
                };

                db.Clients.Add(newClient);
                db.SaveChanges();

                var newUser = new User
                {
                    Username = loginText.Text,
                    Password = AutharizationPage.GetHash(passwordText.Password),
                    RoleID = 5,
                    UserID = newClient.ClientID
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                MessageBox.Show("Регистрация успешна!");
                NavigationService.Navigate(new AutharizationPage());
            }
        }
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }

}
