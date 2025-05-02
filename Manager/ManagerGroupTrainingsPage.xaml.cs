using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class ManagerGroupTrainingsPage : Page
    {
        private User _user;
        private List<GroupTraining> _trainings;
        private List<Employee> _employees;

        public ManagerGroupTrainingsPage(User user)
        {
            InitializeComponent();
            _user = user;

            _trainings = Entities1.Getcontext().GroupTrainings.ToList();
            _employees = Entities1.Getcontext().Employees.ToList();

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
                        TrainerName = t.TrainerID != null
                            ? _employees
                                .Where(e => e.EmployeeID == t.TrainerID)
                                .Select(e => e.FirstName + " " + e.LastName)
                                .FirstOrDefault()
                            : "Не назначен",
                        t.TrainingTypeID,
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
                    t.TrainerID.ToString().Contains(filter) ||
                    t.TrainingTypeID.ToString().Contains(filter) ||
                    (_employees.FirstOrDefault(emp => emp.EmployeeID == t.TrainerID)?.FirstName + " " +
                     _employees.FirstOrDefault(emp => emp.EmployeeID == t.TrainerID)?.LastName)
                    .ToLower().Contains(filter))
                .Select(t => new
                {
                    t.TrainingID,
                    TrainerName = t.TrainerID != null
    ? _employees
        .Where(emp => emp.EmployeeID == t.TrainerID)
        .Select(emp => emp.FirstName + " " + emp.LastName)
        .FirstOrDefault()
    : "Не назначен",

                    t.TrainingTypeID,
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

        private void GoToIndividual_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerTrainingsPage(_user));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
