using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class GroupTrainingsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user; // Текущий пользователь

        public GroupTrainingsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadGroupTrainings();
        }

        private void LoadGroupTrainings()
        {
            try
            {
                GroupTrainingsDataGrid.ItemsSource = _db.GroupTrainings
                    .Select(gt => new
                    {
                        gt.TrainingID,
                        gt.TrainerID,
                        gt.TrainingTypeID,
                        gt.DateTime,
                        gt.PeopleCount
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
