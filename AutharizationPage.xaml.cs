using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
    /// Логика взаимодействия для AutharizationPage.xaml
    /// </summary>
    public partial class AutharizationPage : Page
    {
        public AutharizationPage()
        {
            InitializeComponent();
        }
        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginTextBox.Text) ||
                string.IsNullOrEmpty(passwordText.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            string passwordHash = GetHash(passwordText.Password);

            using (var db = new Entities1())
            {
                var user = db.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Username == loginTextBox.Text &&
                                       u.Password == passwordHash);

                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден!");
                    return;
                }

                Page newWindow;
                switch (user.RoleID)
                {
                    case 1:
                        newWindow = new AdminPage(user);
                        break;
                    case 2:
                        newWindow = new AccountantPage(user);
                        break;
                    case 3:
                        newWindow = new ManagerPage(user);
                        break;
                    case 4:
                        newWindow = new CoachPage(user);
                        break;
                    case 5:
                        newWindow = new ClientPage(user);
                        break;
                    default:
                        MessageBox.Show("Неизвестная роль пользователя!");
                        return;
                }

                NavigationService.Navigate(newWindow);
            }
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }

        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(password))
                    .Select(x => x.ToString("X2")));
            }
        }
    }
}
