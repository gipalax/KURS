using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace KP
{
    public partial class EmployeesPage : Page
    {
        private Entities1 _db; // Контекст базы данных
        private User _user; // Текущий пользователь

        public EmployeesPage(User user)
        {
            InitializeComponent();

            try
            {
                _db = new Entities1(); // Initialize database context
                _user = user ?? throw new ArgumentNullException(nameof(user));
                SearchEmployeeName.Text = "Поиск по имени";
                SearchEmployeeName.Foreground = Brushes.Gray;
                UpdateEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchEmployeeName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchEmployeeName.Text == "Поиск по имени")
            {
                SearchEmployeeName.Text = "";
                SearchEmployeeName.Foreground = Brushes.Black;
            }
        }

        private void SearchEmployeeName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchEmployeeName.Text))
            {
                SearchEmployeeName.Text = "Поиск по имени";
                SearchEmployeeName.Foreground = Brushes.Gray;
            }
        }

        private void SearchEmployeeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateEmployees();
        }

        private void UpdateEmployees()
        {
            try
            {
                if (_db == null) return;

                var employees = _db.Employees.AsQueryable();

                if (!string.IsNullOrWhiteSpace(SearchEmployeeName.Text) &&
                    SearchEmployeeName.Text != "Поиск по имени")
                {
                    string searchText = SearchEmployeeName.Text.ToLower();
                    employees = employees.Where(e =>
                        e.FirstName.ToLower().Contains(searchText) ||
                        e.LastName.ToLower().Contains(searchText));
                }

                EmployeesDataGrid.ItemsSource = employees
    .Select(e => new
    {
        e.EmployeeID,
        e.FirstName,
        e.LastName,
        e.Position,
        e.Email,
        e.PhoneNumber,
        e.Address,
        Salary = e.Salary, // Оставляем как decimal
        ClubID = e.ClubID.HasValue ? e.ClubID.Value.ToString() : ""
    })
    .AsEnumerable() // Переносим обработку в память
    .Select(e => new
    {
        e.EmployeeID,
        e.FirstName,
        e.LastName,
        e.Position,
        e.Email,
        e.PhoneNumber,
        e.Address,
        Salary = e.Salary.ToString("N2"), // Теперь форматируем в памяти
        e.ClubID
    })
    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
                    string.IsNullOrWhiteSpace(LastNameBox.Text) ||
                    string.IsNullOrWhiteSpace(PositionBox.Text))
                {
                    MessageBox.Show("Заполните обязательные поля (Имя, Фамилия, Должность)",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(SalaryBox.Text, out decimal salary))
                {
                    MessageBox.Show("Введите корректное значение зарплаты",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newEmployee = new Employee
                {
                    FirstName = FirstNameBox.Text.Trim(),
                    LastName = LastNameBox.Text.Trim(),
                    Position = PositionBox.Text.Trim(),
                    Email = EmailBox.Text.Trim(),
                    PhoneNumber = PhoneBox.Text.Trim(),
                    Address = AddressBox.Text.Trim(),
                    Salary = salary,
                    ClubID = string.IsNullOrWhiteSpace(ClubIDBox.Text) ?
                        null : int.TryParse(ClubIDBox.Text, out int clubId) ? (int?)clubId : null
                };

                _db.Employees.Add(newEmployee);
                _db.SaveChanges();

                // Clear fields after successful add
                FirstNameBox.Text = "";
                LastNameBox.Text = "";
                PositionBox.Text = "";
                EmailBox.Text = "";
                PhoneBox.Text = "";
                AddressBox.Text = "";
                SalaryBox.Text = "";
                ClubIDBox.Text = "";

                UpdateEmployees();
                MessageBox.Show("Сотрудник успешно добавлен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника для редактирования",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                dynamic selectedItem = EmployeesDataGrid.SelectedItem;
                int id = selectedItem.EmployeeID;
                var employee = _db.Employees.FirstOrDefault(emp => emp.EmployeeID == id);

                if (employee == null)
                {
                    MessageBox.Show("Сотрудник не найден в базе данных",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Validate required fields
                if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
                    string.IsNullOrWhiteSpace(LastNameBox.Text) ||
                    string.IsNullOrWhiteSpace(PositionBox.Text))
                {
                    MessageBox.Show("Заполните обязательные поля (Имя, Фамилия, Должность)",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(SalaryBox.Text, out decimal salary))
                {
                    MessageBox.Show("Введите корректное значение зарплаты",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                employee.FirstName = FirstNameBox.Text.Trim();
                employee.LastName = LastNameBox.Text.Trim();
                employee.Position = PositionBox.Text.Trim();
                employee.Email = EmailBox.Text.Trim();
                employee.PhoneNumber = PhoneBox.Text.Trim();
                employee.Address = AddressBox.Text.Trim();
                employee.Salary = salary;
                employee.ClubID = string.IsNullOrWhiteSpace(ClubIDBox.Text) ?
                    null : int.TryParse(ClubIDBox.Text, out int clubId) ? (int?)clubId : null;

                _db.SaveChanges();
                UpdateEmployees();
                MessageBox.Show("Данные сотрудника обновлены!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                dynamic selectedItem = EmployeesDataGrid.SelectedItem;
                int id = selectedItem.EmployeeID;
                var employee = _db.Employees.FirstOrDefault(emp => emp.EmployeeID == id);

                if (employee == null)
                {
                    MessageBox.Show("Сотрудник не найден в базе данных",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _db.Employees.Remove(employee);
                _db.SaveChanges();

                // Clear fields after deletion
                FirstNameBox.Text = "";
                LastNameBox.Text = "";
                PositionBox.Text = "";
                EmailBox.Text = "";
                PhoneBox.Text = "";
                AddressBox.Text = "";
                SalaryBox.Text = "";
                ClubIDBox.Text = "";

                UpdateEmployees();
                MessageBox.Show("Сотрудник успешно удален!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EmployeesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem == null) return;

            try
            {
                dynamic selectedItem = EmployeesDataGrid.SelectedItem;
                FirstNameBox.Text = selectedItem.FirstName ?? "";
                LastNameBox.Text = selectedItem.LastName ?? "";
                PositionBox.Text = selectedItem.Position ?? "";
                EmailBox.Text = selectedItem.Email ?? "";
                PhoneBox.Text = selectedItem.PhoneNumber ?? "";
                AddressBox.Text = selectedItem.Address ?? "";
                SalaryBox.Text = selectedItem.Salary ?? "";
                ClubIDBox.Text = selectedItem.ClubID ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных сотрудника: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_db != null)
                {
                    _db.Dispose();
                }
                NavigationService?.Navigate(new AdminPage(_user));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переходе: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}