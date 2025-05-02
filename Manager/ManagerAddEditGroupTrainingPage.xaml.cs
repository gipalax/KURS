using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class ManagerAddEditGroupTrainingPage : Page
    {
        private GroupTraining _currentTraining = new GroupTraining();
        private User _user;

        public ManagerAddEditGroupTrainingPage(GroupTraining training = null)
        {
            InitializeComponent();

            TrainerComboBox.ItemsSource = Entities1.Getcontext().Employees
                .Where(e => e.Position == "Тренер")
                .Select(e => new { e.EmployeeID, FullName = e.FirstName + " " + e.LastName })
                .ToList();

            if (training != null)
            {
                _currentTraining = training;
                TrainerComboBox.SelectedValue = _currentTraining.TrainerID;
                TrainingTypeBox.Text = _currentTraining.TrainingTypeID.ToString();
                DatePicker.SelectedDate = _currentTraining.DateTime.Date;
                TimeBox.Text = _currentTraining.DateTime.ToString("HH:mm");
                PeopleCountBox.Text = _currentTraining.PeopleCount.ToString();
            }

            DataContext = _currentTraining;
        }
        public ManagerAddEditGroupTrainingPage(GroupTraining training, User user) : this(training)
        {
            _user = user;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            TimeSpan time = default;

            if (TrainerComboBox.SelectedValue == null)
                errors.AppendLine("Выберите тренера.");
            if (!int.TryParse(TrainingTypeBox.Text, out int trainingTypeId))
                errors.AppendLine("Некорректный ID типа тренировки.");
            if (DatePicker.SelectedDate == null)
                errors.AppendLine("Выберите дату.");
            if (!TimeSpan.TryParse(TimeBox.Text, out time))
                errors.AppendLine("Введите корректное время в формате ЧЧ:ММ.");
            if (!int.TryParse(PeopleCountBox.Text, out int count))
                errors.AppendLine("Введите количество человек.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка");
                return;
            }

            _currentTraining.TrainerID = (int)TrainerComboBox.SelectedValue;
            _currentTraining.TrainingTypeID = trainingTypeId;
            _currentTraining.DateTime = DatePicker.SelectedDate.Value.Date + time;
            _currentTraining.PeopleCount = count;

            if (_currentTraining.TrainingID == 0)
                Entities1.Getcontext().GroupTrainings.Add(_currentTraining);

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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
