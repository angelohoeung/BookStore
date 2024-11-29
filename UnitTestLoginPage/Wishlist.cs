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
        public int addItemToWishlist(int UserId, int ItemId)
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

                    cmd.CommandText = "INSERT INTO WishlistItems (UserId, ItemId) " +
                                      "OUTPUT INSERTED.OrderId " +
                                      "VALUES (@UserId, @ItemId)";

                   

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
    }

   
}
