using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class TrainerTrainingsPage : Page
    {
        private User _user;
        private List<IndividualTraining> _trainings;
        private List<Employee> _employees;
        private List<Client> _clients;

        public TrainerTrainingsPage(User user)
        {
            InitializeComponent();
            _user = user;

            _trainings = Entities1.Getcontext().IndividualTrainings.ToList();
            _employees = Entities1.Getcontext().Employees.ToList();
            _clients = Entities1.Getcontext().Clients.ToList();

            LoadTrainings();
        }

        private void LoadTrainings()
        {
            TrainingsDataGrid.ItemsSource = _trainings.Select(t => new
            {
                t.TrainingID,
                TrainerName = _employees.FirstOrDefault(e => e.EmployeeID == t.TrainerID)?.FirstName + " " +
                              _employees.FirstOrDefault(e => e.EmployeeID == t.TrainerID)?.LastName,
                ClientName = _clients.FirstOrDefault(c => c.ClientID == t.ClientID)?.FirstName + " " +
                             _clients.FirstOrDefault(c => c.ClientID == t.ClientID)?.LastName,
                t.DateTime
            }).ToList();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchBox.Text.ToLower();

            var filtered = _trainings.Where(t =>
                (_employees.FirstOrDefault(emp => emp.EmployeeID == t.TrainerID)?.FirstName + " " +
                 _employees.FirstOrDefault(emp => emp.EmployeeID == t.TrainerID)?.LastName).ToLower().Contains(filter) ||
                (_clients.FirstOrDefault(cli => cli.ClientID == t.ClientID)?.FirstName + " " +
                 _clients.FirstOrDefault(cli => cli.ClientID == t.ClientID)?.LastName).ToLower().Contains(filter)
            ).Select(t => new
            {
                t.TrainingID,
                TrainerName = _employees.FirstOrDefault(emp => emp.EmployeeID == t.TrainerID)?.FirstName + " " +
                              _employees.FirstOrDefault(emp => emp.EmployeeID == t.TrainerID)?.LastName,
                ClientName = _clients.FirstOrDefault(cli => cli.ClientID == t.ClientID)?.FirstName + " " +
                             _clients.FirstOrDefault(cli => cli.ClientID == t.ClientID)?.LastName,
                t.DateTime
            }).ToList();

            TrainingsDataGrid.ItemsSource = filtered;
        }


        private void AddTraining_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainerAddEditTrainingPage(null, _user));
        }

        private void EditTraining_Click(object sender, RoutedEventArgs e)
        {
            if (TrainingsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите тренировку для редактирования.");
                return;
            }

            int id = (int)TrainingsDataGrid.SelectedItem.GetType().GetProperty("TrainingID").GetValue(TrainingsDataGrid.SelectedItem);
            var training = _trainings.FirstOrDefault(t => t.TrainingID == id);
            NavigationService.Navigate(new TrainerAddEditTrainingPage(training, _user));
        }

        private void DeleteTraining_Click(object sender, RoutedEventArgs e)
        {
            if (TrainingsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите тренировку для удаления.");
                return;
            }

            int id = (int)TrainingsDataGrid.SelectedItem.GetType().GetProperty("TrainingID").GetValue(TrainingsDataGrid.SelectedItem);
            var training = Entities1.Getcontext().IndividualTrainings.FirstOrDefault(t => t.TrainingID == id);

            if (training != null)
            {
                Entities1.Getcontext().IndividualTrainings.Remove(training);
                Entities1.Getcontext().SaveChanges();
                _trainings = Entities1.Getcontext().IndividualTrainings.ToList();
                LoadTrainings();
            }
        }

        private void GoToGroup_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainerGroupTrainingsPage(_user));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
