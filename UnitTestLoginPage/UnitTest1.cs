using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Diagnostics;

namespace BookStoreLIB {
    [TestClass]
    public class UnitTest1 {
        UserData userData = new UserData();
        string inputName, inputPassword;
        int actualUserId;
        [TestMethod]
        public void TestMethod1() { // valid
            inputName = "hutz";
            inputPassword = "zh12345";
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            bool expectedReturn = true;
            int expectedUserId = 8;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        public void TestMethod2() { // blank pw
            inputName = "hutzz";
            inputPassword = "";
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        public void TestMethod3() { // starts with non-letter
            inputName = "hutzz";
            inputPassword = "1zh123456";
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }
        [TestMethod]
        public void TestMethod4() { // <6 characters
            inputName = "hutzz";
            inputPassword = "zh123";
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, actualUserId);
        }

        [TestMethod]
        public void TestPaymentWindowValid1()       //All valid inputs
        {
            string inputCardNumber = "4242424242424242";
            string securityCode = "333";
            string expiryDate = "02/26";
            PaymentWindow paymentWindow = new PaymentWindow();
            bool cardNumberValidResult = paymentWindow.IsCardNumberValid(inputCardNumber);
            bool securityCodeValidResult = paymentWindow.IsSecurityCodeValid(securityCode);
            bool expirationValidResult = paymentWindow.IsExpirationDateValid(expiryDate);
            Assert.IsTrue(cardNumberValidResult);
            Assert.IsTrue(securityCodeValidResult);
            Assert.IsTrue(expirationValidResult);
        }
        [TestMethod]
        public void TestPaymentWindowValid2()   //Invalid Card number
        {
            string inputCardNumber = "4242424242424242asfasd";
            string securityCode = "333";
            string expiryDate = "02/26";
            PaymentWindow paymentWindow = new PaymentWindow();
            bool cardNumberValidResult = paymentWindow.IsCardNumberValid(inputCardNumber);
            bool securityCodeValidResult = paymentWindow.IsSecurityCodeValid(securityCode);
            bool expirationValidResult = paymentWindow.IsExpirationDateValid(expiryDate);
            Assert.IsFalse(cardNumberValidResult);
            Assert.IsTrue(securityCodeValidResult);
            Assert.IsTrue(expirationValidResult);
        }
        [TestMethod]
        public void TestPaymentWindowValid3()   //Invalid security code
        {
            string inputCardNumber = "4242424242424242";
            string securityCode = "3533";
            string expiryDate = "02/26";
            PaymentWindow paymentWindow = new PaymentWindow();
            bool cardNumberValidResult = paymentWindow.IsCardNumberValid(inputCardNumber);
            bool securityCodeValidResult = paymentWindow.IsSecurityCodeValid(securityCode);
            bool expirationValidResult = paymentWindow.IsExpirationDateValid(expiryDate);
            Assert.IsTrue(cardNumberValidResult);
            Assert.IsFalse(securityCodeValidResult);
            Assert.IsTrue(expirationValidResult);
        }

        public void TestPaymentWindowValid4() {
            string inputCardNumber = "4242424242424242";
            string securityCode = "333";
            string expiryDate = "02/23";
            PaymentWindow paymentWindow = new PaymentWindow();
            bool cardNumberValidResult = paymentWindow.IsCardNumberValid(inputCardNumber);
            bool securityCodeValidResult = paymentWindow.IsSecurityCodeValid(securityCode);
            bool expirationValidResult = paymentWindow.IsExpirationDateValid(expiryDate);
            Assert.IsTrue(cardNumberValidResult);
            Assert.IsTrue(securityCodeValidResult);
            Assert.IsFalse(expirationValidResult);
        }
        [TestMethod]
        public void TestPlaceOrder1() {        //Valid
            BookOrder bookOrder = new BookOrder();
            bookOrder.AddItem(new OrderItem("0321278658", "Extreme Programming Explained: Embrace Change", 44.63, 1));
            var orderId = bookOrder.PlaceOrder(1);
            Assert.IsTrue(orderId > 0);
        }
        [TestMethod]
        public void TestAddToWishlist() {
            inputName = "hutz";
            inputPassword = "zh12345";
            userData.LogIn(inputName, inputPassword);

            DataTable table = new DataTable();
            table.Columns.Add("WishlistItemId", typeof(int));
            table.Columns.Add("UserId", typeof(int));
            table.Columns.Add("Isbn", typeof(string));

            DataRow row = table.NewRow();
            row["WishlistItemId"] = 1;
            row["UserId"] = userData.UserId;
            row["Isbn"] = "0135974445";

            table.Rows.Add(row);

            DataRowView rowView = table.DefaultView[0];

            DALWishlist wishlistDAL = new DALWishlist();
            wishlistDAL.addItemToWishlist(userData.UserId, rowView);
            var list = wishlistDAL.GetWishlistItems(userData.UserId);
            Assert.AreEqual(row["Isbn"], list[0].Isbn);
            wishlistDAL.deleteItemFromWishlist(userData.UserId, "0135974445");
        }

    }
}