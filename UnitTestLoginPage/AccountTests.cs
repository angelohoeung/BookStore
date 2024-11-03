using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BookStoreLIB {
    [TestClass]
    public class AccountTests {
        UserData userData;
        DALAccount dalAccount;
        int testUserId;
        int existingUserId;

        [TestInitialize]
        public void Setup() {
            userData = new UserData();
            dalAccount = new DALAccount();
            CreateTestUser();
        }

        [TestCleanup]
        public void Cleanup() {
            DeleteTestUser();
        }

        private void CreateTestUser() {
            string insertQuery = @"
        IF NOT EXISTS (SELECT * FROM UserData WHERE UserID = 1000)
        BEGIN
            INSERT INTO UserData (UserID, UserName, Password, Type, Manager, FullName) 
            VALUES (1000, 'testuser', 'tu1234', 'RG', 0, 'Test User')
        END";

            using (var conn = new SqlConnection(Properties.Settings.Default.Connection))
            using (var command = new SqlCommand(insertQuery, conn)) {
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        private void DeleteTestUser() {
            string deleteQuery = "DELETE FROM UserData WHERE UserID = 1000";
            using (var conn = new SqlConnection(Properties.Settings.Default.Connection))
            using (var command = new SqlCommand(deleteQuery, conn)) {
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void GetAccountInfo_ValidUserId_ShouldReturnAccountInfo() {
            var response = userData.GetAccountInfo(1000);
            Assert.IsFalse(response.err);
            Assert.AreEqual("Fetched successfully", response.message);
            Assert.IsNotNull(response.data);
            Assert.AreEqual(1, response.data.Tables["Accounts"].Rows.Count);
        }

        [TestMethod]
        public void GetAccountInfo_InvalidUserId_ShouldReturnError() {
            var response = userData.GetAccountInfo(2000);
            Assert.IsTrue(response.err);
            Assert.AreEqual("Some thing has happens during the fetching account data process", response.message);
            Assert.IsNull(response.data);
        }

        [TestMethod]
        public void UpdateAccount_InvalidUserId_ShouldReturnError() {
            var response = userData.UpdateAccount(2000, "newuser", "tu1234", "Test User", false);
            Assert.IsTrue(response.err);
            Assert.AreEqual("No account found with the specified UserID", response.message);
        }

        [TestMethod]
        public void GetAccountInfoByName_ValidUsername_ShouldReturnAccountInfo() {
            var response = dalAccount.GetAccountInfoByName("testuser");
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Tables["Accounts"].Rows.Count);
            Assert.AreEqual("testuser", response.Tables["Accounts"].Rows[0]["UserName"]);
        }

        [TestMethod]
        public void GetAccountInfoByName_InvalidUsername_ShouldReturnNull() {
            var response = dalAccount.GetAccountInfoByName("nonexistentuser");
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Tables["Accounts"].Rows.Count);
        }

        [TestMethod]
        public void UpdateAccount_FullNameLessThanSixCharacters_ShouldReturnError() {
            var response = userData.UpdateAccount(1000, "testuser", "tu1234", "John", false);
            Assert.IsTrue(response.err);
            Assert.AreEqual("Username and password does not satisfy all conditions", response.message);
        }

        [TestMethod]
        public void UpdateAccount_FullNameGreaterThanSixCharacters_ShouldUpdateSuccessfully() {
            var response = userData.UpdateAccount(1000, "testuser", "tu1234", "Test User Updated", false);
            Assert.IsFalse(response.err);
            Assert.AreEqual("Updated successfully", response.message);
        }

        [TestMethod]
        public void UpdateAccount_ExistingUsername_ShouldReturnError() {
            var response = userData.UpdateAccount(2, "jsmith", "js1234", "Jone Smith", true);
            Assert.IsTrue(response.err);
            Assert.AreEqual("The username has already existed", response.message);
        }

        [TestMethod]
        public void UpdateAccount_ValidUsername_ShouldUpdateSuccessfully() {
            var response = userData.UpdateAccount(1000, "newtestuser", "tu1234", "Test User Updated", true);
            Assert.IsFalse(response.err);
            Assert.AreEqual("Updated successfully", response.message);
        }

        [TestMethod]
        public void UpdateAccount_BlankPassword_ShouldReturnError() {
            var response = userData.UpdateAccount(1000, "newtestuser", "", "Test User Updated", false);
            Assert.IsTrue(response.err);
            Assert.AreEqual("Username and password does not satisfy all conditions", response.message);
        }

        [TestMethod]
        public void UpdateAccount_PasswordStartsWithNonLetter_ShouldReturnError() {
            var response = userData.UpdateAccount(1000, "newtestuser", "1abcde", "Test User Updated", false);
            Assert.IsTrue(response.err);
            Assert.AreEqual("Username and password does not satisfy all conditions", response.message);
        }

        [TestMethod]
        public void UpdateAccount_PasswordLessThanSixCharacters_ShouldReturnError() {
            var response = userData.UpdateAccount(1000, "newtestuser", "abc12", "Test User Updated", false);
            Assert.IsTrue(response.err);
            Assert.AreEqual("Username and password does not satisfy all conditions", response.message);
        }

        [TestMethod]
        public void UpdateAccount_OnlyNumbersPassword_ShouldReturnError() {
            var response = userData.UpdateAccount(1000, "newtestuser", "123456", "Test User Updated", false);
            Assert.IsTrue(response.err);
            Assert.AreEqual("Username and password does not satisfy all conditions", response.message);
        }

        [TestMethod]
        public void UpdateAccount_ValidPassword_ShouldUpdateSuccessfully() {
            var response = userData.UpdateAccount(1000, "newtestuser", "a12345", "Test User Updated", false);
            Assert.IsFalse(response.err);
            Assert.AreEqual("Updated successfully", response.message);
        }

        [TestMethod]
        public void DeleteAccount_ValidUserId_ShouldDeleteAccount() {
            var response = userData.DeletedAccount(1000);
            Assert.IsFalse(response.err);
            Assert.AreEqual("Deleted account", response.message);
        }

        [TestMethod]
        public void DeleteAccount_InvalidUserId_ShouldReturnError() {
            var response = userData.DeletedAccount(-1);
            Assert.IsTrue(response.err);
            Assert.AreEqual("Something happens during the process", response.message);
        }
    }
}
