using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;

namespace KP
{
    public partial class TrainerAddEditServiceRegistrationPage : Page
    {
        private ProvidedService _currentService;
        private User _user;

        public TrainerAddEditServiceRegistrationPage(ProvidedService service, User user)
        {
            InitializeComponent();
            _user = user;
            _currentService = service ?? new ProvidedService();

            // Загрузка списка тренеров
            TrainerComboBox.ItemsSource = Entities1.Getcontext().Employees
                .Where(e => e.Position == "Тренер")
                .Select(e => new
                {
                    e.EmployeeID,
                    FullName = e.FirstName + " " + e.LastName
                })
                .ToList();
            TrainerComboBox.DisplayMemberPath = "FullName";
            TrainerComboBox.SelectedValuePath = "EmployeeID";

            // Загрузка списка клиентов
            ClientComboBox.ItemsSource = Entities1.Getcontext().Clients
                .Select(c => new
                {
                    c.ClientID,
                    FullName = c.FirstName + " " + c.LastName
                })
                .ToList();
            ClientComboBox.DisplayMemberPath = "FullName";
            ClientComboBox.SelectedValuePath = "ClientID";

            // Загрузка списка услуг
            ServiceComboBox.ItemsSource = Entities1.Getcontext().ClubServices
                .Select(s => new
                {
                    s.ServiceID,
                    s.Name
                })
                .ToList();
            ServiceComboBox.DisplayMemberPath = "Name";
            ServiceComboBox.SelectedValuePath = "ServiceID";

            // Если редактируем существующую запись
            if (service != null)
            {
                TrainerComboBox.SelectedValue = _currentService.EmployeeID;
                ClientComboBox.SelectedValue = _currentService.ClientID;
                ServiceComboBox.SelectedValue = _currentService.ServiceID;
                DatePicker.SelectedDate = _currentService.DateTime.Date;
                TimeBox.Text = _currentService.DateTime.ToString("HH:mm");
            }
            else
            {
                TimeBox.Text = "ЧЧ:ММ";
                TimeBox.Foreground = Brushes.Gray;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка выбора тренера
            if (TrainerComboBox.SelectedValue == null)
                errors.AppendLine("Выберите тренера.");

            // Проверка выбора клиента
            if (ClientComboBox.SelectedValue == null)
                errors.AppendLine("Выберите клиента.");

            // Проверка выбора услуги
            if (ServiceComboBox.SelectedValue == null)
                errors.AppendLine("Выберите услугу.");

            // Проверка даты
            if (DatePicker.SelectedDate == null)
                errors.AppendLine("Выберите дату.");

            // Проверка времени (теперь с объявлением переменной)
            if (string.IsNullOrWhiteSpace(TimeBox.Text) || TimeBox.Text == "ЧЧ:ММ" || !TimeSpan.TryParse(TimeBox.Text, out TimeSpan time))
            {
                errors.AppendLine("Введите корректное время в формате ЧЧ:ММ.");
            }
            else if (DatePicker.SelectedDate != null) // Добавляем дату только если время корректное
            {
                // Заполнение полей (теперь time точно имеет значение)
                _currentService.EmployeeID = (int)TrainerComboBox.SelectedValue;
                _currentService.ClientID = (int)ClientComboBox.SelectedValue;
                _currentService.ServiceID = (int)ServiceComboBox.SelectedValue;
                _currentService.DateTime = DatePicker.SelectedDate.Value.Date + time;
            }

            // Если есть ошибки - показать и выйти
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка");
                return;
            }

            // Добавление новой записи, если ID = 0
            if (_currentService.ProvidedServiceID == 0)
                Entities1.Getcontext().ProvidedServices.Add(_currentService);

            // Сохранение изменений
            try
            {
                Entities1.Getcontext().SaveChanges();
                MessageBox.Show("Запись сохранена!", "Успех");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка");
            }
        }

        private void TimeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TimeBox.Text == "ЧЧ:ММ")
            {
                TimeBox.Text = "";
                TimeBox.Foreground = Brushes.Black;
            }
        }

        private void TimeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TimeBox.Text))
            {
                TimeBox.Text = "ЧЧ:ММ";
                TimeBox.Foreground = Brushes.Gray;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}