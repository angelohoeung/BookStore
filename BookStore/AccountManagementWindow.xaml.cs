using System;
using System.Collections.Generic;
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
        public AccountManagementWindow() {
            InitializeComponent();
        }

        private void EditUsername_Click(object sender, RoutedEventArgs e) {
            EditDialog editDialog = new EditDialog();
            editDialog.Title = "Edit Username";
            if (editDialog.ShowDialog() == true) {
                string newUsername = editDialog.InputText;
                // Logic to update the username
            }
        }

        private void EditPassword_Click(object sender, RoutedEventArgs e) {
            EditDialog editDialog = new EditDialog();
            editDialog.Title = "Edit Password";
            if (editDialog.ShowDialog() == true) {
                string newPassword = editDialog.InputText;
                // Logic to update the password
            }
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this account?",
                                                      "Delete Account", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) {
                // Logic to delete the account
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
