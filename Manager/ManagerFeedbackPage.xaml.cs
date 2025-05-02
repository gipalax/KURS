using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class ManagerFeedbackPage : Page
    {
        private User _user;

        public ManagerFeedbackPage(User user)
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

        private void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ReviewsDataGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите отзыв для удаления.");
                return;
            }

            int reviewId = (int)selectedItem.GetType().GetProperty("ReviewID").GetValue(selectedItem, null);
            var review = Entities1.Getcontext().ClientReviews.FirstOrDefault(r => r.ReviewID == reviewId);

            if (review != null)
            {
                if (MessageBox.Show("Удалить отзыв?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Entities1.Getcontext().ClientReviews.Remove(review);
                    Entities1.Getcontext().SaveChanges();
                    LoadReviews();
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
