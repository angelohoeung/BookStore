using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace BookStoreLIB
{
    public class DALWishlist
    {
        public void addItemToWishlist(int UserId, DataRowView selectedItem)
        {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            //int newWishlistItemId = -1;

            try {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;

                    cmd.CommandText = "INSERT INTO Wishlist (UserId, Isbn) " +
                                      "VALUES (@UserId, @Isbn)";
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@Isbn", selectedItem["Isbn"].ToString());
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                //return -1;
            }
            finally {
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
            }

            //return newWishlistItemId;
        }

        public void deleteItemFromWishlist(int userId, int id) {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            try
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "DELETE FROM Wishlist WHERE UserId = @userId AND WishlistItemId = @WishlistItemId";
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@WishlistItemId", id);
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
            }
        } 

        public bool addItemWishlistItemToShoppingCart(int UserId, WishlistItem item)
        {
            DALShoppingCart cart = new DALShoppingCart();
            return cart.AddCartItem(UserId, new OrderItem(item.Isbn, item.BookName, item.Price, 1));
        }

        public List<WishlistItem> GetWishlistItems(int userId)
        {
            if (userId < 1)
                throw new ArgumentException($"Invalid ID passed in DALShoppingCart.GetCartItems({userId})");

            List<WishlistItem> items = new List<WishlistItem>();

            string query = @"
                            SELECT 
                                W.WishlistItemId as Id,
                                W.Isbn AS BookID, 
                                BD.Title AS BookTitle, 
                                BD.Price as Price
                               
                            FROM 
                                Wishlist W
                            INNER JOIN 
                                BookData BD ON W.Isbn = BD.ISBN
                            WHERE 
                                W.UserId = @LoggedInUserID;";

            try {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Connection))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LoggedInUserID", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int wishlistItemId = reader.GetInt32(reader.GetOrdinal("Id"));
                                string bookId = reader.GetString(reader.GetOrdinal("BookID"));
                                string bookTitle = reader.GetString(reader.GetOrdinal("BookTitle"));
                                double price = (double)reader.GetDecimal(reader.GetOrdinal("Price"));
                                var item = new WishlistItem(bookId, bookTitle, price);
                                item.WishlistItemId = wishlistItemId;
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            return items;
        }
        public int GetCurrentWishlistItemId() {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            int currentId = 0;

            try {
                conn.Open();
                string query = "SELECT IDENT_CURRENT('Wishlist')";

                using (SqlCommand command = new SqlCommand(query, conn)) {
                    object result = command.ExecuteScalar();

                    if (result != DBNull.Value) {
                        currentId = Convert.ToInt32(result); 
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine($"Error fetching current identity value: {ex.ToString()}");
            }
            finally {
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
            }

            return currentId;
        }
    }
}
