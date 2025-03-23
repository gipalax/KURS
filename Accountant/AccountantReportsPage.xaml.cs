using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KP
{
    public partial class AccountantReportsPage : Page
    {
        private readonly Entities1 _db = new Entities1(); // Контекст базы данных
        private User _user;

        public AccountantReportsPage(User user)
        {
            InitializeComponent();
            _user = user;
            LoadReports();
        }

        private void LoadReports()
        {
            try
            {
                // 1️⃣ Выручка от продаж абонементов
                decimal totalMembershipSales = _db.MembershipSales
                    .Join(_db.Memberships,
                        sale => sale.MembershipID,
                        membership => membership.MembershipID,
                        (sale, membership) => membership.Price)
                    .Sum();

                // 2️⃣ Выручка от оказанных услуг
                decimal totalServicesRevenue = _db.ProvidedServices
                    .Join(_db.ClubServices,
                        service => service.ServiceID,
                        clubService => clubService.ServiceID,
                        (service, clubService) => clubService.Price)
                    .Sum();

                // 3️⃣ Общие затраты на зарплату
                decimal totalSalaries = _db.Employees.Sum(emp => emp.Salary);

                // 4️⃣ Чистая прибыль
                decimal netProfit = (totalMembershipSales + totalServicesRevenue) - totalSalaries;

                // Формируем список отчетов
                var reports = new[]
                {
                    new { Title = "Выручка от абонементов", Amount = totalMembershipSales },
                    new { Title = "Выручка от услуг клуба", Amount = totalServicesRevenue },
                    new { Title = "Общие зарплатные расходы", Amount = totalSalaries },
                    new { Title = "Чистая прибыль", Amount = netProfit }
                };

                ReportsListView.ItemsSource = reports;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountantPage(_user)); // Вернуться в меню бухгалтера
        }
    }
}
