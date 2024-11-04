using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BookStoreLIB {
    [TestClass]
    public class SignUpUnitTests {
        UserData userData = new UserData();
        string inputName, inputPassword, inputConfirmPassword, inputFullName;
        int actualUserId;
        [TestMethod]
        public void TestMethod1() {
            inputName = "jhenry";
            inputPassword = "jh1234";
            inputConfirmPassword = "jh1234";
            inputFullName = "John Henry";

            // Remove entry from database before testing validity
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            try {
                SqlCommand deleteExsitingUser = new SqlCommand();
                deleteExsitingUser.Connection = conn;
                deleteExsitingUser.CommandText = "DELETE FROM UserData WHERE UserName = @UserName";
                deleteExsitingUser.Parameters.AddWithValue("@UserName", inputName);
                conn.Open();
                deleteExsitingUser.ExecuteNonQuery();

            } catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
            } finally {
                if (conn.State == System.Data.ConnectionState.Open) {
                    conn.Close();
                }
            }

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = true;
            int notExpectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreNotEqual(notExpectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod2() {
            inputName = "";
            inputPassword = "anything1";
            inputConfirmPassword = "anything1";
            inputFullName = "Ryan Kuehfuss";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod3() {
            inputName = "jsmith";
            inputPassword = "anything1";
            inputConfirmPassword = "anything1";
            inputFullName = "Niko Jones";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod4() {
            inputName = "yes";
            inputPassword = "anything1";
            inputConfirmPassword = "anything1";
            inputFullName = "Ryan Kuehfuss";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod5() {
            inputName = "njones";
            inputPassword = "";
            inputConfirmPassword = "anything1";
            inputFullName = "Ryan Kuehfuss";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod6() {
            inputName = "njones";
            inputPassword = "nj123";
            inputConfirmPassword = "anything1";
            inputFullName = "Niko Jones";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod7() {
            inputName = "njones";
            inputPassword = "1rk1234";
            inputConfirmPassword = "1rk1234";
            inputFullName = "Niko Jones";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod8() {
            inputName = "rkuehfuss";
            inputPassword = "testing";
            inputConfirmPassword = "testing";
            inputFullName = "Ryan Kuehfuss";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod9() {
            inputName = "rkuehfuss";
            inputPassword = "rk1234!";
            inputConfirmPassword = "rk1234!";
            inputFullName = "Ryan Kuehfuss";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod10() {
            inputName = "njones";
            inputPassword = "nj1234";
            inputConfirmPassword = "rk1234";
            inputFullName = "Niko Jones";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod11() {
            inputName = "rkuehfuss";
            inputPassword = "rk1234";
            inputConfirmPassword = "rk1234";
            inputFullName = "Ryan Kuehfuss!";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod12() {
            inputName = "rkuehfuss";
            inputPassword = "rk1234";
            inputConfirmPassword = "rk1234";
            inputFullName = "1Ryan Kuehfuss";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod13() {
            inputName = "rkuehfuss";
            inputPassword = "rk1234";
            inputConfirmPassword = "rk1234";
            inputFullName = "";

            bool actualReturn = userData.SignUp(inputName, inputPassword, inputConfirmPassword, inputFullName);
            bool expectedReturn = false;
            int expectedUserId = -1;
            actualUserId = userData.UserId;
            Assert.AreEqual(expectedUserId, actualUserId);
            Assert.AreEqual(expectedReturn, actualReturn);
        }
    }
}
