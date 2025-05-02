using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class TrainerGroupTrainingsPage : Page
    {
        private User _user;
        private List<GroupTraining> _trainings;
        private List<Employee> _employees;
        private List<TrainingType> _trainingTypes;

        public TrainerGroupTrainingsPage(User user)
        {
            InitializeComponent();
            _user = user;

            _trainings = Entities1.Getcontext().GroupTrainings.ToList();
            _employees = Entities1.Getcontext().Employees.ToList();
            _trainingTypes = Entities1.Getcontext().TrainingTypes.ToList();

            LoadTrainings();
        }

        private void LoadTrainings()
        {
            try
            {
                TrainingsDataGrid.ItemsSource = _trainings
                    .Select(t => new
                    {
                        t.TrainingID,
                        TrainingTypeName = _trainingTypes
                            .FirstOrDefault(tt => tt.TrainingTypeID == t.TrainingTypeID)?.Name ?? "Неизвестно",
                        TrainerName = t.TrainerID != null
                            ? _employees
                                .Where(e => e.EmployeeID == t.TrainerID)
                                .Select(e => e.FirstName + " " + e.LastName)
                                .FirstOrDefault()
                            : "Не назначен",
                        t.DateTime,
                        t.PeopleCount
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchBox.Text.ToLower();

            var filteredTrainings = _trainings
                .Where(t =>
                {
                    // Проверяем ID тренировки
                    if (t.TrainingID.ToString().Contains(filter))
                        return true;

                    // Проверяем название типа тренировки
                    var trainingType = _trainingTypes.FirstOrDefault(tt => tt.TrainingTypeID == t.TrainingTypeID);
                    if (trainingType?.Name?.ToLower().Contains(filter) == true)
                        return true;

                    // Проверяем имя и фамилию тренера
                    if (t.TrainerID != null)
                    {
                        var trainer = _employees.FirstOrDefault(emp => emp.EmployeeID == t.TrainerID);
                        if (trainer != null)
                        {
                            if (trainer.FirstName?.ToLower().Contains(filter) == true ||
                                trainer.LastName?.ToLower().Contains(filter) == true)
                                return true;
                        }
                    }

                    // Проверяем дату и время
                    if (t.DateTime.ToString().ToLower().Contains(filter))
                        return true;

                    return false;
                })
                .Select(t => new
                {
                    t.TrainingID,
                    TrainingTypeName = _trainingTypes
                        .FirstOrDefault(tt => tt.TrainingTypeID == t.TrainingTypeID)?.Name ?? "Неизвестно",
                    TrainerName = t.TrainerID != null
                        ? _employees
                            .Where(emp => emp.EmployeeID == t.TrainerID)
                            .Select(emp => emp.FirstName + " " + emp.LastName)
                            .FirstOrDefault()
                        : "Не назначен",
                    t.DateTime,
                    t.PeopleCount
                })
                .ToList();

            TrainingsDataGrid.ItemsSource = filteredTrainings;
        }

        private void AddTraining_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerAddEditGroupTrainingPage(null, _user));
        }

        private void EditTraining_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = TrainingsDataGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите тренировку для редактирования.");
                return;
            }

            int trainingId = (int)selectedItem.GetType().GetProperty("TrainingID").GetValue(selectedItem, null);
            var training = _trainings.FirstOrDefault(t => t.TrainingID == trainingId);

            if (training != null)
                NavigationService.Navigate(new ManagerAddEditGroupTrainingPage(training, _user));
        }

        private void DeleteTraining_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = TrainingsDataGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите тренировку для удаления.");
                return;
            }

            int trainingId = (int)selectedItem.GetType().GetProperty("TrainingID").GetValue(selectedItem, null);
            var training = Entities1.Getcontext().GroupTrainings.FirstOrDefault(t => t.TrainingID == trainingId);

            if (training != null)
            {
                Entities1.Getcontext().GroupTrainings.Remove(training);
                Entities1.Getcontext().SaveChanges();

                // Обновляем локальные списки после удаления
                _trainings = Entities1.Getcontext().GroupTrainings.ToList();
                LoadTrainings();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}