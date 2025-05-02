using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class ManagerMembershipSalesPage : Page
    {
        private User _user;

        public ManagerMembershipSalesPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadSales();
        }

        private void LoadSales()
        {
            try
            {
                SalesDataGrid.ItemsSource = Entities1.Getcontext().MembershipSales
                    .Select(s => new
                    {
                        s.SaleID,
                        s.ClientID,
                        s.MembershipID,
                        s.SaleDate
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
            SalesDataGrid.ItemsSource = Entities1.Getcontext().MembershipSales
                .Where(s =>
                    s.ClientID.ToString().Contains(filter) ||
                    s.MembershipID.ToString().Contains(filter))
                .Select(s => new
                {
                    s.SaleID,
                    s.ClientID,
                    s.MembershipID,
                    s.SaleDate
                })
                .ToList();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerAddEditMembershipSalePage(null, _user));
        }

        private void DeleteSale_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = SalesDataGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            int saleId = (int)selectedItem.GetType().GetProperty("SaleID").GetValue(selectedItem, null);
            var sale = Entities1.Getcontext().MembershipSales.FirstOrDefault(s => s.SaleID == saleId);
            if (sale != null)
            {
                Entities1.Getcontext().MembershipSales.Remove(sale);
                Entities1.Getcontext().SaveChanges();
                LoadSales();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
