using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class IndividualTrainingsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user; // Текущий пользователь

        public IndividualTrainingsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadIndividualTrainings();
        }

        private void LoadIndividualTrainings()
        {
            try
            {
                IndividualTrainingsDataGrid.ItemsSource = _db.IndividualTrainings
                    .Select(it => new
                    {
                        it.TrainingID,
                        it.TrainerID,
                        it.ClientID,
                        it.DateTime,
                        it.PeopleCount
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage(_user));
        }
    }
}
