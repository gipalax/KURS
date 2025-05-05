using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;
using KP;

namespace KP
{
    public partial class ClientGroupTrainingsPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user;
        private List<TrainingType> _trainingTypes;

        public ClientGroupTrainingsPage(User user)
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}