using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class TrainerAddEditTrainingTypesPage : Page
    {
        private TrainingType _currentTrainingType;
        private User _user;

        public TrainerAddEditTrainingTypesPage(TrainingType trainingType, User user)
        {
            InitializeComponent();
            _user = user;
            _currentTrainingType = trainingType ?? new TrainingType();

            // Если редактируем существующую запись
            if (trainingType != null)
            {
                NameTextBox.Text = _currentTrainingType.Name;
                DescriptionTextBox.Text = _currentTrainingType.Description;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка названия
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                errors.AppendLine("Введите название типа тренировки.");

            // Проверка описания
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                errors.AppendLine("Введите описание типа тренировки.");

            // Если есть ошибки - показать и выйти
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка");
                return;
            }

            // Заполнение полей
            _currentTrainingType.Name = NameTextBox.Text;
            _currentTrainingType.Description = DescriptionTextBox.Text;

            // Добавление новой записи, если ID = 0
            if (_currentTrainingType.TrainingTypeID == 0)
                Entities1.Getcontext().TrainingTypes.Add(_currentTrainingType);

            // Сохранение изменений
            try
            {
                Entities1.Getcontext().SaveChanges();
                MessageBox.Show("Тип тренировки сохранён!", "Успех");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}