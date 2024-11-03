using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            bool expectedCardNumberResult = true;
            bool expectedSecurityCodeValidResult = true;
            bool expectedExpirationValidResult = true;
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
            bool expectedCardNumberResult = true;
            bool expectedSecurityCodeValidResult = true;
            bool expectedExpirationValidResult = true;
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
            bool expectedCardNumberResult = true;
            bool expectedSecurityCodeValidResult = true;
            bool expectedExpirationValidResult = true;
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
            bool expectedCardNumberResult = true;
            bool expectedSecurityCodeValidResult = true;
            bool expectedExpirationValidResult = true;
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

    }
}