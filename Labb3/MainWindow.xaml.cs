using System.ComponentModel;
using System.Windows;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;

            LoginTab.Visibility = Visibility.Visible;
            AdminTab.Visibility = Visibility.Collapsed;
            ShopTab.Visibility = Visibility.Collapsed;
        }

        private void UserManager_CurrentUserChanged()
        {
            if (UserManager.IsAdminLoggedIn)
            {
                LoginViewTab.Visibility = Visibility.Collapsed;
                AdminViewTab.Visibility = Visibility.Visible;
                AdminTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Visible;
                LoginTab.Visibility = Visibility.Collapsed;
                AdminTab.IsSelected = true;
            }
            else if (UserManager.IsCustomerLoggedIn)
            {
                LoginViewTab.Visibility = Visibility.Collapsed;
                ShopViewTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Visible;
                AdminTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Collapsed;
                ShopTab.IsSelected = true;
            }
            else if (UserManager.CurrentUser is null)
            {
                LoginViewTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Collapsed;
                AdminTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Visible;
                LoginTab.IsSelected = true;
            }
        }

        private async void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            await UserManager.SaveUsersToFile();
        }
    }
}
