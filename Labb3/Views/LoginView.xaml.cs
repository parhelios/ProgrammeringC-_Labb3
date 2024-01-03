using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            UserManager.UserListChanged += UserManagerOnUserListChanged;
        }

        private void UserManagerOnUserListChanged()
        {
            MessageBox.Show("Logged in!");
        }

        private void UserManager_CurrentUserChanged()
        {
            //Städar bort them pesky textfält.

            LoginName.Text = string.Empty;
            LoginPwd.Password = string.Empty;
            RegisterName.Text = string.Empty;
            RegisterPwd.Password = string.Empty;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = LoginName.Text.ToLower();
            var selectedPass = LoginPwd.Password;

            if (selectedUser == null || selectedPass == null)
            {
                return;
            }

            if (UserManager.Users.Any(u => u.Name.ToLower() == selectedUser))
            {
                var usertype = UserManager.Users.FirstOrDefault(d => d.Name.ToLower() == LoginName.Text.ToLower()).Type;

                foreach (var user in UserManager.Users)
                {
                    if (user.Name == selectedUser && user.Authenticate(selectedPass))
                    {
                        UserManager.ChangeCurrentUser(selectedUser, selectedPass, usertype);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Error, incorrect username or password.");
            }
        }

        private void RegisterAdminBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!LoginCheck())
            {
                return;
            }

            var tempUser = new Admin(RegisterName.Text, RegisterPwd.Password);
            UserManager.AddUser(tempUser);
            MessageBox.Show($"Admin {RegisterName.Text} has been registered.");

            RegisterName.Text = string.Empty;
            RegisterPwd.Password = string.Empty;
        }

        private void RegisterCustomerBtn_OnClickmerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!LoginCheck())
            {
                return;
            }

            var tempUser = new Customer(RegisterName.Text, RegisterPwd.Password);
            UserManager.AddUser(tempUser);
            MessageBox.Show($"Customer {RegisterName.Text} has been registered.");

            RegisterName.Text = string.Empty;
            RegisterPwd.Password = string.Empty;
        }

        private bool LoginCheck()
        {
            //En metod som checkar diverse kriterier för registrering av användare.

            if (RegisterName.Text == "" || RegisterPwd.Password == "")
            {
                MessageBox.Show("Invalid input, no password found.");
                return false;
            }

            if (Regex.IsMatch(RegisterName.Text, @"^\d+$"))
            {
                MessageBox.Show("The username may only contain letters.");
                return false;
            }

            if (RegisterPwd.Password.Length < 3 || RegisterName.Text.Length < 3)
            {
                MessageBox.Show("The username and the password has to contain at least 3 characters.");
                return false;
            }

            if (UserManager.Users.Any(c => c.Name == RegisterName.Text))
            {
                MessageBox.Show("User already exists.");
                return false;
            }

            return true;
        }
    }
}
