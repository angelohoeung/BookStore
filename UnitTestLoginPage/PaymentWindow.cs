using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB {
    public class PaymentWindow {
        public bool IsCardNumberValid(string cardNumber) {
            return cardNumber.Length == 16 && cardNumber.All(char.IsDigit);
        }

        public bool IsExpirationDateValid(string expirationDate) {
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

        public bool IsSecurityCodeValid(string securityCode) {
            return securityCode.Length == 3 && securityCode.All(char.IsDigit);
        }
    }
}