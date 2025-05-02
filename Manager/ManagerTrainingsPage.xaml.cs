using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class ManagerTrainingsPage : Page
    {
        private User _user;

        public ManagerTrainingsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadTrainings();
        }

        private void LoadTrainings()
        {
            try
            {
                TrainingsDataGrid.ItemsSource = Entities1.Getcontext().IndividualTrainings
                    .Select(t => new
                    {
                        t.TrainingID,
                        TrainerName = t.Employee.FirstName + " " + t.Employee.LastName,
                        t.ClientID,
                        t.DateTime,
                        t.PeopleCount
                    }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message, "Ошибка");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = SearchBox.Text.ToLower();

            TrainingsDataGrid.ItemsSource = Entities1.Getcontext().IndividualTrainings
                .Where(t =>
                    t.Employee.FirstName.ToLower().Contains(filter) ||
                    t.Employee.LastName.ToLower().Contains(filter) ||
                    t.ClientID.ToString().Contains(filter))
                .Select(t => new
                {
                    t.TrainingID,
                    TrainerName = t.Employee.FirstName + " " + t.Employee.LastName,
                    t.ClientID,
                    t.DateTime,
                    t.PeopleCount
                }).ToList();
        }

        private void AddTraining_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerAddEditIndividualTrainingPage(null, _user));
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
            var training = Entities1.Getcontext().IndividualTrainings.FirstOrDefault(t => t.TrainingID == trainingId);

            if (training != null)
            {
                NavigationService.Navigate(new ManagerAddEditIndividualTrainingPage(training, _user));
            }
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
            var training = Entities1.Getcontext().IndividualTrainings.FirstOrDefault(t => t.TrainingID == trainingId);

            if (training != null)
            {
                Entities1.Getcontext().IndividualTrainings.Remove(training);
                Entities1.Getcontext().SaveChanges();
                LoadTrainings();
            }
        }

        private void GroupTrainings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerGroupTrainingsPage(_user));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
