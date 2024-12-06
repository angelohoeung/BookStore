using System;
using System.Data;
using System.Data.SqlClient;

namespace BookStoreLIB
{
    class DALBookReviews
    {
        private readonly SqlConnection conn;

        public DALBookReviews()
        {
            conn = new SqlConnection(Properties.Settings.Default.Connection);
        }

        private string GetISBNForTitle(string title)
        {
            string isbn = null;
            try
            {
                string query = "SELECT ISBN FROM BookData WHERE Title = @Title";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();

                    if (result != null)
                        isbn = result.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting ISBN for title: " + ex.Message);
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return isbn;
        }

        public DataTable GetReviews(string title)
        {
            DataTable reviewsTable = new DataTable();
            try
            {
                string isbn = GetISBNForTitle(title);
                if (isbn == null)
                {
                    // If no book found with that title, return empty table
                    return reviewsTable;
                }

                string query = "SELECT UserID, Content FROM Review WHERE ISBN = @ISBN";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(reviewsTable);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching reviews: {ex.Message}");
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return reviewsTable;
        }

        public bool SetReview(string title, int userID, string content)
        {
            try
            {
                string isbn = GetISBNForTitle(title);
                if (isbn == null)
                {
                    // Book does not exist
                    return false;
                }

                string query = "INSERT INTO Review (ISBN, UserID, Content) VALUES (@ISBN, @UserID, @Content)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Content", content);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding review: {ex.Message}");
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }
        }
    }
}
