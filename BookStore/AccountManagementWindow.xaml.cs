using BookStoreLIB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStoreGUI {
    /// <summary>
    /// Interaction logic for AccountManagementWindow.xaml
    /// </summary>
    public partial class AccountManagementWindow : Window {
        private UserData _userData;
        private DALAccount _dalAccount;
        private DataSet _accountDataSet;

        public AccountManagementWindow(UserData userData) {
            InitializeComponent();
            _userData = userData;
            _dalAccount = new DALAccount();
            LoadUserInfo();
        }

        private void LoadUserInfo() {
            _accountDataSet = _dalAccount.GetAccountInfo(_userData.UserId);

            if (_accountDataSet != null && _accountDataSet.Tables["Accounts"].Rows.Count > 0) {
                DataRow accountInfo = _accountDataSet.Tables["Accounts"].Rows[0];

                nameTextBlock.Text = accountInfo["FullName"].ToString();
                usernameTextBlock.Text = accountInfo["UserName"].ToString();
                passwordTextBlock.Text = "*****";  // Masked for security
            }
            else {
                MessageBox.Show("Error loading account information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditUsername_Click(object sender, RoutedEventArgs e) {
            EditDialog editDialog = new EditDialog();
            editDialog.Title = "Edit Username";
            if (editDialog.ShowDialog() == true) {
                string newUsername = editDialog.InputText;
                var response = _userData.UpdateAccount(_userData.UserId, newUsername, _userData.Password, nameTextBlock.Text);
                if (!response.err) {
                    usernameTextBlock.Text = newUsername;
                    MessageBox.Show("Username updated successfully.");
                }
                else {
                    MessageBox.Show(response.message, "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditPassword_Click(object sender, RoutedEventArgs e) {
            EditDialog editDialog = new EditDialog();
            editDialog.Title = "Edit Password";
            if (editDialog.ShowDialog() == true) {
                string newPassword = editDialog.InputText;
                var response = _userData.UpdateAccount(_userData.UserId, usernameTextBlock.Text, newPassword, nameTextBlock.Text);
                if (!response.err) {
                    MessageBox.Show("Password updated successfully.");
                }
                else {
                    MessageBox.Show(response.message, "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this account?",
                                                      "Delete Account", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) {
                // Implement delete functionality if needed
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
