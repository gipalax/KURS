using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class ManagerAddEditMembershipSalePage : Page
    {
        private MembershipSale _currentSale;
        private bool _isEdit;
        private User _user;

        public ManagerAddEditMembershipSalePage(MembershipSale sale = null, User user = null)
        {
            InitializeComponent();

            _user = user;
            _currentSale = sale ?? new MembershipSale();
            _isEdit = sale != null;

            if (_isEdit)
            {
                ClientIDBox.Text = _currentSale.ClientID.ToString();
                MembershipIDBox.Text = _currentSale.MembershipID.ToString();
                SaleDatePicker.SelectedDate = _currentSale.SaleDate;
            }
            else
            {
                SaleDatePicker.SelectedDate = DateTime.Now;
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (!int.TryParse(ClientIDBox.Text, out int clientId))
                errors.AppendLine("Введите корректный ID клиента.");

            if (!int.TryParse(MembershipIDBox.Text, out int membershipId))
                errors.AppendLine("Введите корректный ID абонемента.");

            if (!SaleDatePicker.SelectedDate.HasValue)
                errors.AppendLine("Выберите дату продажи.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _currentSale.ClientID = clientId;
            _currentSale.MembershipID = membershipId;
            _currentSale.SaleDate = SaleDatePicker.SelectedDate.Value;

            try
            {
                var context = Entities1.Getcontext();
                if (!_isEdit)
                {
                    context.MembershipSales.Add(_currentSale);
                }

                context.SaveChanges();
                MessageBox.Show("Продажа успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + (ex.InnerException?.Message ?? ex.Message), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
