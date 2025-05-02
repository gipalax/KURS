using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class TrainerClientReviewsPage : Page
    {
        private User _user;

        public TrainerClientReviewsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadReviews();
        }

        private void LoadReviews()
        {
            try
            {
                var context = Entities1.Getcontext();

                var reviews = context.ClientReviews
                    .ToList()
                    .Select(r => new
                    {
                        r.ReviewID,
                        ClientFullName = context.Clients
                            .Where(c => c.ClientID == r.ClientID)
                            .Select(c => c.FirstName + " " + c.LastName)
                            .FirstOrDefault(),
                        r.ClubID,
                        r.ReviewText,
                        r.ReviewDate
                    })
                    .ToList();

                ReviewsDataGrid.ItemsSource = reviews;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки отзывов: " + ex.Message, "Ошибка");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var context = Entities1.Getcontext();
                string filter = SearchBox.Text.ToLower();

                var reviews = context.ClientReviews
                    .ToList()
                    .Select(r => new
                    {
                        r.ReviewID,
                        ClientFullName = context.Clients
                            .Where(c => c.ClientID == r.ClientID)
                            .Select(c => c.FirstName + " " + c.LastName)
                            .FirstOrDefault(),
                        r.ClubID,
                        r.ReviewText,
                        r.ReviewDate
                    })
                    .Where(r =>
                        (!string.IsNullOrEmpty(r.ReviewText) && r.ReviewText.ToLower().Contains(filter)) ||
                        (!string.IsNullOrEmpty(r.ClientFullName) && r.ClientFullName.ToLower().Contains(filter)))
                    .ToList();

                ReviewsDataGrid.ItemsSource = reviews;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка фильтрации: " + ex.Message, "Ошибка");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
