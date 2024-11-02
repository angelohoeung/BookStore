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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PaymentWindow : Window {
        public PaymentWindow() {
            InitializeComponent();
        }

        public void SubmitButton_Click(object sender, RoutedEventArgs e) {
            var cardNumber = cardNumberTextBox.Text;
            var expirationDate = expirationDateTextBox.Text;
            var securityCode = securityCodeTextBox.Text;
            if (IsCardNumberValid(cardNumber) && IsExpirationDateValid(expirationDate) && IsSecurityCodeValid(securityCode)) {
                this.DialogResult = true;
            } else {
                this.DialogResult = false;
            }
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private bool IsCardNumberValid(string cardNumber) {
            return cardNumber.Length == 16 && cardNumber.All(char.IsDigit);
        }

        private bool IsExpirationDateValid(string expirationDate) {
            if (expirationDate == null || expirationDate == "" || !expirationDate.Contains("/") || expirationDate.Length != 5) {
                return false;
            }

            string[] parts = expirationDate.Split('/');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int month) || !int.TryParse(parts[1], out int year)) {
                return false;
            }

            year += 2000;
            DateTime currentDate = DateTime.Now;
            DateTime expiration = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

            return expiration >= currentDate;
        }

        private bool IsSecurityCodeValid(string securityCode) {
            return securityCode.Length == 3 && securityCode.All(char.IsDigit);
        }

    }
}
