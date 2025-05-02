using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;
using KP;

namespace KP
{
    public partial class TrainerTrainingTypesPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user;
        private List<TrainingType> _trainingTypes;

        public TrainerTrainingTypesPage(User user)
        {
            InitializeComponent();
            _user = user;
            UpdateTrainingTypes();
        }

        private void UpdateTrainingTypes()
        {
            _trainingTypes = _db.TrainingTypes.ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                string searchText = SearchBox.Text.ToLower();
                _trainingTypes = _trainingTypes.Where(t =>
                    (t.Name != null && t.Name.ToLower().Contains(searchText)) ||
                    (t.Description != null && t.Description.ToLower().Contains(searchText))
                ).ToList();
            }

            TrainingTypesDataGrid.ItemsSource = _trainingTypes;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTrainingTypes();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TrainerAddEditTrainingTypesPage(null, _user));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = TrainingTypesDataGrid.SelectedItem as TrainingType;
            if (selected != null)
            {
                NavigationService.Navigate(new TrainerAddEditTrainingTypesPage(selected, _user));
            }
            else
            {
                MessageBox.Show("Выберите тип тренировки для редактирования.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = TrainingTypesDataGrid.SelectedItem as TrainingType;
            if (selected != null)
            {
                if (MessageBox.Show("Удалить выбранный тип тренировки?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var trainingType = _db.TrainingTypes.FirstOrDefault(t => t.TrainingTypeID == selected.TrainingTypeID);
                        if (trainingType != null)
                        {
                            _db.TrainingTypes.Remove(trainingType);
                            _db.SaveChanges();
                            UpdateTrainingTypes();
                            MessageBox.Show("Тип тренировки успешно удалён.", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.InnerException?.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите тип тренировки для удаления.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}