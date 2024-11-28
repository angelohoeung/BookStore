using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;

namespace BookStoreLIB {
    [TestClass]
    public class EditBookListingTests {
        private const string TestISBN = "TESTISBN12";
        private readonly string connectionString = Properties.Settings.Default.Connection;
        private int testManagerId;
        private int testNonManagerId;

        [TestInitialize]
        public void Setup() {
            FindExistingUsers();
            CreateTestBook();
        }

        [TestCleanup]
        public void Cleanup() {
            DeleteTestBook();
        }

        private void FindExistingUsers() {
            string findManagerQuery = "SELECT TOP 1 UserID FROM UserData WHERE Manager = 1";
            string findNonManagerQuery = "SELECT TOP 1 UserID FROM UserData WHERE Manager = 0";

            using (var conn = new SqlConnection(connectionString)) {
                conn.Open();

                // find manager
                using (var cmd = new SqlCommand(findManagerQuery, conn)) {
                    var result = cmd.ExecuteScalar();
                    if (result != null) {
                        testManagerId = (int)result;
                    }
                    else {
                        throw new Exception("No manager user found in the database.");
                    }
                }

                // find non manager
                using (var cmd = new SqlCommand(findNonManagerQuery, conn)) {
                    var result = cmd.ExecuteScalar();
                    if (result != null) {
                        testNonManagerId = (int)result;
                    }
                    else {
                        throw new Exception("No non-manager user found in the database.");
                    }
                }
            }
        }

        private void CreateTestBook() {
            string insertQuery = @"
            IF NOT EXISTS (SELECT * FROM BookData WHERE ISBN = @ISBN)
            BEGIN
                INSERT INTO BookData (ISBN, CategoryID, Title, Author, Price, SupplierId, Year, Edition, Publisher, InStock) 
                VALUES (@ISBN, 1, 'Test Book', 'Test Author', 19.99, 1, '2023', '1', 'Test Publisher', 10)
            END";

            using (var conn = new SqlConnection(connectionString))
            using (var command = new SqlCommand(insertQuery, conn)) {
                command.Parameters.AddWithValue("@ISBN", TestISBN);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        private void DeleteTestBook() {
            string deleteQuery = "DELETE FROM BookData WHERE ISBN = @ISBN";
            using (var conn = new SqlConnection(connectionString))
            using (var command = new SqlCommand(deleteQuery, conn)) {
                command.Parameters.AddWithValue("@ISBN", TestISBN);
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void GetManagerStatus_ShouldReturnTrue_ForManager() {
            DALUserInfo userInfo = new DALUserInfo();
            bool isManager = userInfo.GetManagerStatus(testManagerId);

            Assert.IsTrue(isManager, $"Expected UserID {testManagerId} to be a manager.");
        }

        [TestMethod]
        public void GetManagerStatus_ShouldReturnFalse_ForNonManager() {
            DALUserInfo userInfo = new DALUserInfo();
            bool isManager = userInfo.GetManagerStatus(testNonManagerId);

            Assert.IsFalse(isManager, $"Expected UserID {testNonManagerId} to be a non-manager.");
        }

        [TestMethod]
        public void UpdateBook_ValidInput_ShouldReturnSuccess() {
            BookCatalog catalog = new BookCatalog();
            string newTitle = "Updated Test Book";
            string newAuthor = "Updated Author";
            decimal newPrice = 29.99M;
            string newPublisher = "Updated Publisher";
            int newCategoryId = 2;
            int newSupplierId = 2;
            int newInStock = 20;
            string newEdition = "2";

            string result = catalog.UpdateBook(
                TestISBN, newTitle, newAuthor, newPrice, "2024", newPublisher, newCategoryId, newSupplierId, newInStock, newEdition
            );

            Assert.AreEqual("", result, "Expected no error message for valid input.");
        }

        [TestMethod]
        public void UpdateBook_InvalidISBN_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                "INVALIDISBN", "New Title", "New Author", 29.99M, "2024", "New Publisher", 2, 2, 20, "2"
            );

            Assert.AreEqual("ISBN must be 10 characters or less.", result);
        }

        [TestMethod]
        public void UpdateBook_InvalidPrice_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", -5.0M, "2024", "New Publisher", 2, 2, 20, "2"
            );

            Assert.AreEqual("Price must be between 0 and 99999999.99.", result);
        }

        [TestMethod]
        public void UpdateBook_InvalidEdition_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", 29.99M, "2024", "New Publisher", 2, 2, 20, "TOOLONG"
            );

            Assert.AreEqual("Edition must be 2 characters or less.", result);
        }

        [TestMethod]
        public void SetBookOutOfStock_ValidISBN_ShouldReturnSuccess() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.SetBookOutOfStock(TestISBN);

            Assert.AreEqual("", result, "Expected no error message for valid ISBN.");
        }

        [TestMethod]
        public void SetBookOutOfStock_InvalidISBN_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.SetBookOutOfStock("INVALIDISBN");

            Assert.AreEqual("ISBN must be 10 characters or less.", result);
        }

        [TestMethod]
        public void UpdateBook_InvalidCategoryID_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", 29.99M, "2024", "New Publisher", -1, 2, 20, "2"
            );

            Assert.AreEqual("CategoryID must be a valid positive integer.", result);
        }

        [TestMethod]
        public void UpdateBook_InvalidSupplierID_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", 29.99M, "2024", "New Publisher", 2, -5, 20, "2"
            );

            Assert.AreEqual("SupplierID must be a valid positive integer.", result);
        }

        [TestMethod]
        public void UpdateBook_EmptyTitle_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "", "New Author", 29.99M, "2024", "New Publisher", 2, 2, 20, "2"
            );

            Assert.AreEqual("Title cannot be empty and must be 80 characters or less.", result);
        }

        [TestMethod]
        public void UpdateBook_EmptyAuthor_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "", 29.99M, "2024", "New Publisher", 2, 2, 20, "2"
            );

            Assert.AreEqual("Author cannot be empty and must be 255 characters or less.", result);
        }

        [TestMethod]
        public void UpdateBook_EmptyYear_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", 29.99M, "", "New Publisher", 2, 2, 20, "2"
            );

            Assert.AreEqual("Year must be 4 characters or less.", result);
        }

        [TestMethod]
        public void UpdateBook_EmptyEdition_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", 29.99M, "2024", "New Publisher", 2, 2, 20, ""
            );

            Assert.AreEqual("Edition must be 2 characters or less.", result);
        }

        [TestMethod]
        public void UpdateBook_EmptyPublisher_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", 29.99M, "2024", "", 2, 2, 20, "2"
            );

            Assert.AreEqual("Publisher cannot be empty and must be 50 characters or less.", result);
        }

        [TestMethod]
        public void UpdateBook_NegativeInStock_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", 29.99M, "2024", "New Publisher", 2, 2, -10, "2"
            );

            Assert.AreEqual("InStock cannot be negative.", result);
        }

        [TestMethod]
        public void UpdateBook_EmptyPrice_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            // when price is empty, it is set to -1
            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", -1, "2024", "New Publisher", 2, 2, 20, "2"
            );

            Assert.AreEqual("Price must be between 0 and 99999999.99.", result);
        }

        [TestMethod]
        public void UpdateBook_EmptyInStock_ShouldReturnErrorMessage() {
            BookCatalog catalog = new BookCatalog();

            // when inStock is empty, it is set to -1
            string result = catalog.UpdateBook(
                TestISBN, "New Title", "New Author", 29.99M, "2024", "New Publisher", 2, 2, -1, "2"
            );

            Assert.AreEqual("InStock cannot be negative.", result);
        }
    }
}
