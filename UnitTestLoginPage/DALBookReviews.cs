using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    class DALBookReviews
    {
        private readonly SqlConnection conn;

        public DALBookReviews()
        {
            conn = new SqlConnection(Properties.Settings.Default.Connection);
        }

        public DataTable GetReviews(string title)
        {
            DataTable reviewsTable = new DataTable();
            try
            {
                // SQL query to join BookData and Review tables based on the title
                string query = @"
                    SELECT r.UserID, r.Content
                    FROM Review r
                    INNER JOIN BookData b ON r.ISBN = b.ISBN
                    WHERE b.Title = @Title";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);

                    // Open connection, execute the query, and fill the DataTable
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(reviewsTable);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log or rethrow)
                Console.WriteLine($"Error fetching reviews: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
            return reviewsTable;
        }

        public bool SetReview(string title, int userID, string content)
        {
            try
            {
                // SQL query to insert a new review
                string query = @"
                    INSERT INTO Review (ISBN, UserID, Content)
                    SELECT b.ISBN, @UserID, @Content
                    FROM BookData b
                    WHERE b.Title = @Title";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Content", content);

                    // Open connection and execute the query
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0; // Returns true if a row was inserted
                }
            }
            catch (Exception ex)
            {
                // Handle exception (log or rethrow)
                Console.WriteLine($"Error adding review: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
