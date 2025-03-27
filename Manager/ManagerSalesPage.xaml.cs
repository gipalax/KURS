using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;

namespace KP
{
    public partial class ManagerSalesPage : Page
    {
        private readonly Entities1 _db = new Entities1();
        private User _currentUser;

        public ManagerSalesPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadData();
            MembershipCombo.SelectionChanged += MembershipCombo_SelectionChanged;
        }

        private void LoadData()
        {
            try
            {
                // Загрузка продаж с объединением имени и фамилии клиента
                var sales = _db.MembershipSales
                    .Include("Client")
                    .Include("Membership")
                    .Select(s => new
                    {
                        s.SaleID,
                        ClientName = s.Client.FirstName + " " + s.Client.LastName,
                        MembershipName = s.Membership.Name,
                        s.SaleDate,
                        Price = s.Membership.Price,
                        s.ClientID,
                        s.MembershipID
                    }).ToList();

                SalesGrid.ItemsSource = sales;

                // Загрузка клиентов с объединением имени и фамилии
                ClientCombo.ItemsSource = _db.Clients
                    .Select(c => new
                    {
                        c.ClientID,
                        FullName = c.FirstName + " " + c.LastName
                    }).ToList();
                ClientCombo.DisplayMemberPath = "FullName";
                ClientCombo.SelectedValuePath = "ClientID";

                MembershipCombo.ItemsSource = _db.Memberships.ToList();
                MembershipCombo.DisplayMemberPath = "Name";
                MembershipCombo.SelectedValuePath = "MembershipID";

                SaleDatePicker.SelectedDate = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void SalesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SalesGrid.SelectedItem != null)
            {
                dynamic selectedItem = SalesGrid.SelectedItem;
                ClientCombo.SelectedValue = selectedItem.ClientID;
                MembershipCombo.SelectedValue = selectedItem.MembershipID;
                SaleDatePicker.SelectedDate = selectedItem.SaleDate;
            }
        }

        private void SearchSale_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();
            SalesGrid.ItemsSource = _db.MembershipSales
                .Include("Client")
                .Where(s => s.Client.FirstName.ToLower().Contains(searchText) ||
                           s.Client.LastName.ToLower().Contains(searchText))
                .Select(s => new
                {
                    s.SaleID,
                    ClientName = s.Client.FirstName + " " + s.Client.LastName,
                    MembershipName = s.Membership.Name,
                    s.SaleDate,
                    Price = s.Membership.Price
                }).ToList();
        }

        private void MembershipCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MembershipCombo.SelectedItem is Membership selectedMembership)
            {
                PriceBox.Text = selectedMembership.Price.ToString("C");
            }
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            if (ClientCombo.SelectedItem == null || MembershipCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента и абонемент!");
                return;
            }

            try
            {
                var newSale = new MembershipSale
                {
                    ClientID = (int)ClientCombo.SelectedValue,
                    MembershipID = (int)MembershipCombo.SelectedValue,
                    SaleDate = SaleDatePicker.SelectedDate ?? DateTime.Today
                };

                _db.MembershipSales.Add(newSale);
                _db.SaveChanges();
                LoadData();
                ClearForm();
                MessageBox.Show("Продажа успешно оформлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка оформления продажи: {ex.Message}");
            }
        }

        private void ClearForm_Click(object sender, RoutedEventArgs e) => ClearForm();

        private void ClearForm()
        {
            ClientCombo.SelectedIndex = -1;
            MembershipCombo.SelectedIndex = -1;
            PriceBox.Text = "";
            SaleDatePicker.SelectedDate = DateTime.Today;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagerPage(_currentUser));
        }
    }
}