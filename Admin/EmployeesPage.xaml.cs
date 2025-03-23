using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class EmployeesPage : Page
    {
        private Entities1 _db = new Entities1(); // Контекст базы данных
        private User _user; // Текущий пользователь

        public EmployeesPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadEmployees();
        }

        // Загрузка данных из БД
        private void LoadEmployees()
        {
            try
            {
                EmployeesDataGrid.ItemsSource = _db.Employees
                    .Select(e => new
                    {
                        e.EmployeeID,
                        e.FirstName,
                        e.LastName,
                        e.Position,
                        e.Email,
                        e.PhoneNumber,
                        e.Address,
                        e.Salary,
                        e.ClubID
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Добавление нового сотрудника
        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newEmployee = new Employee
                {
                    FirstName = FirstNameBox.Text,
                    LastName = LastNameBox.Text,
                    Position = PositionBox.Text,
                    Email = EmailBox.Text,
                    PhoneNumber = PhoneBox.Text,
                    Address = AddressBox.Text,
                    Salary = decimal.Parse(SalaryBox.Text),
                    ClubID = string.IsNullOrWhiteSpace(ClubIDBox.Text) ? (int?)null : int.Parse(ClubIDBox.Text)
                };

                _db.Employees.Add(newEmployee);
                _db.SaveChanges();
                LoadEmployees();

                MessageBox.Show("Сотрудник добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Изменение данных о сотруднике
        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedItem = EmployeesDataGrid.SelectedItem;
                    int id = selectedItem.EmployeeID;
                    var employee = _db.Employees.FirstOrDefault(emp => emp.EmployeeID == id);
                    if (employee != null)
                    {
                        employee.FirstName = FirstNameBox.Text;
                        employee.LastName = LastNameBox.Text;
                        employee.Position = PositionBox.Text;
                        employee.Email = EmailBox.Text;
                        employee.PhoneNumber = PhoneBox.Text;
                        employee.Address = AddressBox.Text;
                        employee.Salary = decimal.Parse(SalaryBox.Text);
                        employee.ClubID = string.IsNullOrWhiteSpace(ClubIDBox.Text) ? (int?)null : int.Parse(ClubIDBox.Text);

                        _db.SaveChanges();
                        LoadEmployees();
                        MessageBox.Show("Сотрудник обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка редактирования: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Удаление сотрудника
        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedItem = EmployeesDataGrid.SelectedItem;
                    int id = selectedItem.EmployeeID;
                    var employee = _db.Employees.FirstOrDefault(emp => emp.EmployeeID == id);

                    if (employee != null)
                    {
                        _db.Employees.Remove(employee);
                        _db.SaveChanges();
                        LoadEmployees();
                        MessageBox.Show("Сотрудник удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Заполнение полей при выборе элемента в таблице
        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                dynamic selectedItem = EmployeesDataGrid.SelectedItem;
                FirstNameBox.Text = selectedItem.FirstName;
                LastNameBox.Text = selectedItem.LastName;
                PositionBox.Text = selectedItem.Position;
                EmailBox.Text = selectedItem.Email;
                PhoneBox.Text = selectedItem.PhoneNumber;
                AddressBox.Text = selectedItem.Address;
                SalaryBox.Text = selectedItem.Salary.ToString();
                ClubIDBox.Text = selectedItem.ClubID?.ToString();
            }
        }

        // Возврат к странице администратора
        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage(_user));
        }
    }
}
