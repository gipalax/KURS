using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KP
{
    public partial class TrainerAddEditTrainingPage : Page
    {
        private IndividualTraining _currentTraining;
        private User _user;

        public TrainerAddEditTrainingPage(IndividualTraining training, User user)
        {
            InitializeComponent();
            _user = user;
            _currentTraining = training ?? new IndividualTraining();

            // Заполнение ComboBox тренеров
            TrainerComboBox.ItemsSource = Entities1.Getcontext().Employees
                .Where(e => e.Position == "Тренер")
                .Select(e => new
                {
                    e.EmployeeID,
                    FullName = e.FirstName + " " + e.LastName
                }).ToList();

            TrainerComboBox.DisplayMemberPath = "FullName";
            TrainerComboBox.SelectedValuePath = "EmployeeID";

            // Если редактируем существующую тренировку — подставить значения
            if (training != null)
            {
                TrainerComboBox.SelectedValue = _currentTraining.TrainerID;
                ClientIDBox.Text = _currentTraining.ClientID.ToString();
                DatePicker.SelectedDate = _currentTraining.DateTime.Date;
                TimeBox.Text = _currentTraining.DateTime.ToString("HH:mm");
                PeopleCountBox.Text = _currentTraining.PeopleCount.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка выбора тренера
            if (TrainerComboBox.SelectedValue == null)
                errors.AppendLine("Выберите тренера.");

            // Проверка ID клиента
            if (!int.TryParse(ClientIDBox.Text, out int clientId))
                errors.AppendLine("Некорректный ID клиента.");

            // Проверка даты
            var selectedDate = DatePicker.SelectedDate;
            if (selectedDate == null)
                errors.AppendLine("Выберите дату.");

            // Проверка времени
            TimeSpan time = default;
            if (string.IsNullOrWhiteSpace(TimeBox.Text) || !TimeSpan.TryParse(TimeBox.Text, out time))
                errors.AppendLine("Введите корректное время в формате ЧЧ:ММ.");

            // Проверка количества человек
            if (!int.TryParse(PeopleCountBox.Text, out int count))
                errors.AppendLine("Введите количество человек.");

            // Если есть ошибки — показать и выйти
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка");
                return;
            }

            // Заполнение полей тренировки
            _currentTraining.TrainerID = (int)TrainerComboBox.SelectedValue;
            _currentTraining.ClientID = clientId;
            _currentTraining.DateTime = selectedDate.Value.Date + time;
            _currentTraining.PeopleCount = count;

            // Добавление новой тренировки, если ID = 0
            if (_currentTraining.TrainingID == 0)
                Entities1.Getcontext().IndividualTrainings.Add(_currentTraining);

            // Сохранение изменений
            try
            {
                Entities1.Getcontext().SaveChanges();
                MessageBox.Show("Сохранено!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
