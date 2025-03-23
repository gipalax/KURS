using System.Windows;
using KP;

namespace KP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Навигация на страницу авторизации при запуске
            MainFrame.Navigate(new AutharizationPage());
        }
    }
}