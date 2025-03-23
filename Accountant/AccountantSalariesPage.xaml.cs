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
                    LastNameBox.Text = _selectedEmployee.LastName;
                    FirstNameBox.Text = _selectedEmployee.FirstName;
                    PositionBox.Text = _selectedEmployee.Position;
                    SalaryBox.Text = _selectedEmployee.Salary.ToString();
                    ClubIDBox.Text = _selectedEmployee.ClubID?.ToString() ?? ""; // Если null, ставим пустую строку
                }
            }
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newEmployee = new Employee
                {
                    LastName = LastNameBox.Text,
                    FirstName = FirstNameBox.Text,
                    Position = PositionBox.Text,
                    Salary = decimal.Parse(SalaryBox.Text),
                    ClubID = string.IsNullOrWhiteSpace(ClubIDBox.Text) ? (int?)null : int.Parse(ClubIDBox.Text)
                };

                _db.Employees.Add(newEmployee);
                _db.SaveChanges();
                LoadSalaries();
                MessageBox.Show("Сотрудник добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee != null)
            {
                try
                {
                    _selectedEmployee.LastName = LastNameBox.Text;
                    _selectedEmployee.FirstName = FirstNameBox.Text;
                    _selectedEmployee.Position = PositionBox.Text;
                    _selectedEmployee.Salary = decimal.Parse(SalaryBox.Text);
                    _selectedEmployee.ClubID = string.IsNullOrWhiteSpace(ClubIDBox.Text) ? (int?)null : int.Parse(ClubIDBox.Text);

                    _db.SaveChanges();
                    LoadSalaries();
                    MessageBox.Show("Данные сотрудника обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка редактирования: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee != null)
            {
                try
                {
                    _db.Employees.Remove(_selectedEmployee);
                    _db.SaveChanges();
                    LoadSalaries();
                    MessageBox.Show("Сотрудник удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountantPage(_user)); // Вернуться в меню бухгалтера
        }
    }
}
