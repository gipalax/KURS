using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class TrainerAddEditGroupTrainingPage : Page
    {
        private GroupTraining _currentTraining;
        private Employee _trainer;

        public TrainerAddEditGroupTrainingPage(GroupTraining training, Employee trainer)
        {
            InitializeComponent();

            _currentTraining = training ?? new GroupTraining();
            _trainer = trainer;

            TrainingTypeComboBox.ItemsSource = Entities1.Getcontext().TrainingTypes.ToList();

            if (training != null)
            {
                TrainingTypeComboBox.SelectedValue = _currentTraining.TrainingTypeID;
                DatePicker.SelectedDate = _currentTraining.DateTime;
                TimeBox.Text = _currentTraining.DateTime.ToString("HH:mm");
                PeopleCountBox.Text = _currentTraining.PeopleCount.ToString();
            }

            DataContext = _currentTraining;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (TrainingTypeComboBox.SelectedValue == null)
                errors.AppendLine("Выберите тип тренировки");
            if (DatePicker.SelectedDate == null)
                errors.AppendLine("Выберите дату");
            if (!TimeSpan.TryParse(TimeBox.Text, out TimeSpan time))
                errors.AppendLine("Введите корректное время");
            if (!int.TryParse(PeopleCountBox.Text, out int peopleCount) || peopleCount <= 0)
                errors.AppendLine("Введите корректное количество участников");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка");
                return;
            }

            _currentTraining.TrainingTypeID = (int)TrainingTypeComboBox.SelectedValue;
            _currentTraining.DateTime = DatePicker.SelectedDate.Value.Date + time;
            _currentTraining.PeopleCount = peopleCount;
            _currentTraining.TrainerID = _trainer.EmployeeID;

            if (_currentTraining.TrainingID == 0)
                Entities1.Getcontext().GroupTrainings.Add(_currentTraining);

            try
            {
                Entities1.Getcontext().SaveChanges();
                MessageBox.Show("Сохранено");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}