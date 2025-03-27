using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class AccountantSalariesPage : Page
    {
        private readonly Entities1 _db = new Entities1(); // Контекст базы данных
        private User _user;
        private Employee _selectedEmployee; // Выбранный сотрудник

        public AccountantSalariesPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadSalaries();
        }

        private void LoadSalaries()
        {
            SalariesListView.ItemsSource = _db.Employees
                .Select(emp => new
                {
                    emp.EmployeeID,
                    emp.LastName,
                    emp.FirstName,
                    emp.Position,
                    emp.Salary,
                    emp.ClubID
                })
                .ToList();
        }

        private void SalariesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SalariesListView.SelectedItem != null)
            {
                dynamic selected = SalariesListView.SelectedItem;
                int selectedID = (int)selected.EmployeeID;

                _selectedEmployee = _db.Employees.FirstOrDefault(emp => emp.EmployeeID == selectedID);

                if (_selectedEmployee != null)
                {
                    SalaryBox.Text = _selectedEmployee.Salary.ToString();
                }
            }
        }

        private void EditSalary_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee != null)
            {
                try
                {
                    if (decimal.TryParse(SalaryBox.Text, out decimal newSalary))
                    {
                        _selectedEmployee.Salary = newSalary;
                        _db.SaveChanges();
                        LoadSalaries();
                        MessageBox.Show("Зарплата обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Введите корректное значение зарплаты!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountantPage(_user)); // Вернуться в меню бухгалтера
        }
    }
}
