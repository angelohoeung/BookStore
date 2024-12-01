using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace BookStoreLIB
{
    internal class Wishlist
    {
        public int addItemToWishlist(int UserId, string Isbn)
        {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            int newWishlistItemId = -1;
 
            try
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;

                    cmd.CommandText = "INSERT INTO WishlistItems (UserId, Isbn) " +
                                      "OUTPUT INSERTED.OrderId " +
                                      "VALUES (@UserId, @Isbn)";
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@Isbn", Isbn);
                    newWishlistItemId = (int)cmd.ExecuteScalar();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return newWishlistItemId;
        }


        public int addItemWishlistItemToOrder(int OrderId, WishlistItem item)
        {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            int newOrderItemId = -1;
            DALOrder order = new DALOrder();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO OrderItem (OrderId, ISBN, Quantity) VALUES (@OrderId, @ISBN, @Quantity)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderId", OrderId);
                cmd.Parameters.AddWithValue("@ISBN", item.Isbn);
                cmd.Parameters.AddWithValue("@Quantity", 1);

                cmd.ExecuteNonQuery();
                return newOrderItemId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }

            return newOrderItemId;
        }
    }

    private int checkBookStock(string Isbn)
    {
        DALBookCatalog catalog = new DALBookCatalog();

    }

    public Observable
}
