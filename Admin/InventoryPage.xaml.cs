using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP
{
    public partial class InventoryPage : Page
    {
        private Entities1 _db = new Entities1();
        private User _user;
        private int selectedInventoryID = -1; // Храним ID выбранного элемента

        public InventoryPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadInventory();
        }

        private void LoadInventory()
        {
            try
            {
                InventoryDataGrid.ItemsSource = _db.Inventories
                    .Select(i => new
                    {
                        i.InventoryID,
                        i.Name,
                        i.Quantity,
                        i.ClubID
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Заполнение полей при выборе строки
        private void InventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InventoryDataGrid.SelectedItem != null)
            {
                dynamic selectedItem = InventoryDataGrid.SelectedItem;
                selectedInventoryID = selectedItem.InventoryID;
                NameTextBox.Text = selectedItem.Name;
                QuantityTextBox.Text = selectedItem.Quantity.ToString();
                ClubIDTextBox.Text = selectedItem.ClubID.ToString();
            }
        }

        // Добавление нового инвентаря
        private void AddInventory_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || !int.TryParse(ClubIDTextBox.Text, out int clubID))
            {
                MessageBox.Show("Введите корректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newInventory = new Inventory
            {
                Name = NameTextBox.Text,
                Quantity = quantity,
                ClubID = clubID
            };

            _db.Inventories.Add(newInventory);
            _db.SaveChanges();
            LoadInventory();
            ClearFields();
        }

        // Изменение существующего инвентаря
        private void UpdateInventory_Click(object sender, RoutedEventArgs e)
        {
            if (selectedInventoryID == -1)
            {
                MessageBox.Show("Выберите элемент для изменения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var inventory = _db.Inventories.FirstOrDefault(i => i.InventoryID == selectedInventoryID);
            if (inventory != null)
            {
                inventory.Name = NameTextBox.Text;
                inventory.Quantity = int.Parse(QuantityTextBox.Text);
                inventory.ClubID = int.Parse(ClubIDTextBox.Text);

                _db.SaveChanges();
                LoadInventory();
                ClearFields();
            }
        }

        // Удаление инвентаря
        private void DeleteInventory_Click(object sender, RoutedEventArgs e)
        {
            if (selectedInventoryID == -1)
            {
                MessageBox.Show("Выберите элемент для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var inventory = _db.Inventories.FirstOrDefault(i => i.InventoryID == selectedInventoryID);
            if (inventory != null)
            {
                _db.Inventories.Remove(inventory);
                _db.SaveChanges();
                LoadInventory();
                ClearFields();
            }
        }

        private void ClearFields()
        {
            NameTextBox.Text = "";
            QuantityTextBox.Text = "";
            ClubIDTextBox.Text = "";
            selectedInventoryID = -1;
        }

        private void BackToAdmin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage(_user));
        }
    }
}
