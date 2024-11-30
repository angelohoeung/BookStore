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
 
            //try
            //{
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
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.ToString());
            //    //return -1;
            //}
            //finally
            //{
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        conn.Close();
            //    }
            //}

            //return newWishlistItemId;
        }

        public void deleteItemFromWishlist(int userId, WishlistItem selectedItem) {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            try
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "DELETE FROM Wishlist WHERE UserId = @userId AND Isbn = @Isbn";
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Isbn", selectedItem.Isbn);
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
                                W.Isbn AS BookID, 
                                BD.Title AS BookTitle, 
                                BD.Price as Price
                               
                            FROM 
                                Wishlist W
                            INNER JOIN 
                                BookData BD ON W.Isbn = BD.ISBN
                            WHERE 
                                W.UserId = @LoggedInUserID;";

            //try
            //{
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
                                string bookId = reader.GetString(reader.GetOrdinal("BookID"));
                                string bookTitle = reader.GetString(reader.GetOrdinal("BookTitle"));
                                double price = (double)reader.GetDecimal(reader.GetOrdinal("Price"));
                                items.Add(new WishlistItem(bookId, bookTitle, price));
                            }
                        }
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            return items;
        }
    }
}
