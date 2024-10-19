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
            inputName = "hutzz";
            inputPassword = "zh123456";
            bool actualReturn = userData.LogIn(inputName, inputPassword);
            bool expectedReturn = true;
            int expectedUserId = 9;
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
    }
}

// 6 character password starting with letter and at least one number (valid)
// 7 character password starting with letter and at least one number (valid)
// 5 character password starting with letter and at least one number (invalid)
// 6+ character password containing letters and numbers, but starting with a number (invalid)
// 6+ character password containing only letters (invalid)
// 6+ character password containing only numbers (invalid)
// blank password (invalid)
// 6+ character password starting with letter and at least one number that also contains at least one non-letter/non-numeric character (invalid)