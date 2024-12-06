using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BookStoreLIB
{
    [TestClass]
    public class ReviewTest
    {
        BookReviews bookReviews = new BookReviews();
        string testTitle;
        int testUserId;
        string testContent;

        /// <summary>
        /// Retrieves the ISBN for a given title from BookData.
        /// </summary>
        private string GetISBNForTitle(string title)
        {
            using (var conn = new SqlConnection(Properties.Settings.Default.Connection))
            {
                var cmd = new SqlCommand("SELECT ISBN FROM BookData WHERE Title = @Title", conn);
                cmd.Parameters.AddWithValue("@Title", title);
                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();
                return result?.ToString();
            }
        }

        [TestMethod]
        public void GetReviews_ValidTitleWithReviews_ReturnsNonEmptyDataTable()
        {
            // "xyz" must exist in BookData and must have at least one review in Review.
            testTitle = "xyz";
            string isbn = GetISBNForTitle(testTitle);
            Assert.IsNotNull(isbn, "Could not find ISBN for 'xyz'. Ensure 'xyz' is present in BookData.");

            DataTable result = bookReviews.GetReviews(testTitle);

            Assert.IsNotNull(result, "Expected a DataTable for 'xyz'.");
            Assert.IsTrue(result.Rows.Count > 0, "Expected at least one review for 'xyz'. Ensure the Review table has a row with ISBN corresponding to 'xyz'.");
        }

        [TestMethod]
        public void GetReviews_UnknownTitle_ReturnsEmptyDataTable()
        {
            testTitle = "NonExistentBookTitle999";
            string isbn = GetISBNForTitle(testTitle);
            Assert.IsNull(isbn, "'NonExistentBookTitle999' should not exist in BookData.");

            DataTable result = bookReviews.GetReviews(testTitle);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Rows.Count, "Expected no reviews for a non-existent title.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetReviews_EmptyTitle_ThrowsArgumentException()
        {
            testTitle = string.Empty;
            bookReviews.GetReviews(testTitle);
        }

        [TestMethod]
        

        [ExpectedException(typeof(ArgumentException))]
        public void SetReview_EmptyTitle_ThrowsArgumentException()
        {
            testTitle = "";
            testUserId = 1;
            testContent = "Valid content";
            bookReviews.SetReview(testTitle, testUserId, testContent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReview_EmptyContent_ThrowsArgumentException()
        {
            testTitle = "xyz";
            testUserId = 1;
            testContent = "";
            bookReviews.SetReview(testTitle, testUserId, testContent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetReview_NegativeUserId_ThrowsArgumentOutOfRangeException()
        {
            testTitle = "xyz";
            testUserId = -1;
            testContent = "Some content";
            bookReviews.SetReview(testTitle, testUserId, testContent);
        }

        [TestMethod]
        public void GetReviews_TitleWithNoReviews_ReturnsEmptyDataTable()
        {
            // "A Good Book" must exist in BookData but have no entries in Review.
            testTitle = "A Good Book";
            string isbn = GetISBNForTitle(testTitle);
            Assert.IsNotNull(isbn, "Could not find ISBN for 'A Good Book'. Ensure it exists in BookData with no reviews.");

            DataTable result = bookReviews.GetReviews(testTitle);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Rows.Count, "Expected zero reviews for 'A Good Book'. If this fails, remove any review entries for its ISBN.");
        }

        [TestMethod]
        public void SetReview_ExistingReviewTitle_DoesNotThrow()
        {
            // Arrange
            testTitle = "xyz";
            testUserId = 1;
            testContent = "Another review for xyz.";

            string isbn = GetISBNForTitle(testTitle);
            Assert.IsNotNull(isbn, "Could not find ISBN for 'xyz'.");

            // Clean up any existing record to avoid PK violation
            using (var conn = new SqlConnection(Properties.Settings.Default.Connection))
            {
                var deleteCmd = new SqlCommand("DELETE FROM Review WHERE ISBN = @ISBN AND UserID = @UserID", conn);
                deleteCmd.Parameters.AddWithValue("@ISBN", isbn);
                deleteCmd.Parameters.AddWithValue("@UserID", testUserId);

                conn.Open();
                deleteCmd.ExecuteNonQuery();
                conn.Close();
            }

            bool actualReturn = false;

            // Act
            try
            {
                actualReturn = bookReviews.SetReview(testTitle, testUserId, testContent);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, got: " + ex.Message);
            }

            // Assert
            Assert.IsTrue(actualReturn, "Could not insert another review for 'xyz'. Check that UserId=1 exists and no constraints block insertion.");
        }



        [TestMethod]
        

        [ExpectedException(typeof(ArgumentException))]
        public void SetReview_WhitespaceTitle_ThrowsArgumentException()
        {
            testTitle = "   ";
            testUserId = 1;
            testContent = "Valid content here";
            bookReviews.SetReview(testTitle, testUserId, testContent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReview_WhitespaceContent_ThrowsArgumentException()
        {
            testTitle = "xyz";
            testUserId = 1;
            testContent = "   ";
            bookReviews.SetReview(testTitle, testUserId, testContent);
        }

        [TestMethod]
        public void GetReviews_WhitespaceTitle_ThrowsArgumentException()
        {
            try
            {
                bookReviews.GetReviews("   ");
                Assert.Fail("Expected ArgumentException for whitespace title.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Title cannot be empty");
            }
        }
        [TestMethod]
        public void SetReview_ExcessivelyLongTitle_ThrowsExceptionOrFailsGracefully()
        {
            // Arrange
            string longTitle = new string('X', 5000); // 5000 chars, adjust if needed
            int userId = 1;
            string content = "Normal content";

            // Act & Assert
            // Expect either an ArgumentException if you've capped input length, or false return if DAL fails gracefully.
            try
            {
                bool result = bookReviews.SetReview(longTitle, userId, content);
                // Depending on your logic, you might expect false rather than an exception.
                Assert.IsFalse(result, "Expected SetReview to fail or return false for overly long title.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Title");
            }
        }
        [TestMethod]
        public void SetReview_ExcessivelyLongContent_ThrowsExceptionOrFailsGracefully()
        {
            // Arrange
            string title = "xyz"; // Ensure xyz exists and has ISBN
            int userId = 1;
            string longContent = new string('C', 10000); // 10,000 chars of content

            // Act & Assert
            try
            {
                bool result = bookReviews.SetReview(title, userId, longContent);
                Assert.IsFalse(result, "Expected insertion to fail or return false for overly long content.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Content");
            }
        }
       




    }
}
