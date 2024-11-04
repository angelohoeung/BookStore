using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BookStoreLIB
{
    class DALOrder
    {
        // legacy code; leaving here just in case
        public int Proceed2Order(string xmlOrder)
        {
            SqlConnection cn = new SqlConnection(
                Properties.Settings.Default.Connection);
            try
            {
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "down_PlaceOrder";
                SqlParameter inParameter = new SqlParameter();
                inParameter.ParameterName = "@xmlOrder";
                inParameter.Value = xmlOrder;
                inParameter.DbType = DbType.String;
                inParameter.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(inParameter);
                SqlParameter ReturnParameter = new SqlParameter();
                ReturnParameter.ParameterName = "@OrderID";
                ReturnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(ReturnParameter);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                return (int)cmd.Parameters["@OrderID"].Value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return 0;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }
        public int CreateOrder(int userId, ObservableCollection<OrderItem> orderItemList) {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            int newOrderId = -1;

            try {
                conn.Open();
                using (var transaction = conn.BeginTransaction()) {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;

                    cmd.CommandText = "INSERT INTO Orders (UserId, OrderDate, Status, DiscountPercent) " +
                                      "OUTPUT INSERTED.OrderId " +
                                      "VALUES (@UserId, @OrderDate, @Status, @DiscountPercent)";

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Status", 'P');
                    cmd.Parameters.AddWithValue("@DiscountPercent", 0);

                    newOrderId = (int)cmd.ExecuteScalar();

                    // iterate over order items and insert
                    foreach (var item in orderItemList) {
                        int result = CreateOrderItem(newOrderId, item, cmd);
                        if (result == -1) {
                            // rollback transaction if any insert fails
                            transaction.Rollback();
                            DeleteOrder(newOrderId, cmd);
                            return -1;
                        }
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
            finally {
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
            }

            return newOrderId;
        }
        public int CreateOrderItem(int orderId, OrderItem item, SqlCommand cmd) {
            try {
                cmd.CommandText = "INSERT INTO OrderItem (OrderId, ISBN, Quantity) VALUES (@OrderId, @ISBN, @Quantity)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@ISBN", item.BookID);
                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);

                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
        }
        private void DeleteOrder(int orderId, SqlCommand cmd) {
            cmd.CommandText = "DELETE FROM OrderItem WHERE OrderId = @OrderId";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "DELETE FROM Orders WHERE OrderId = @OrderId";
            cmd.ExecuteNonQuery();
        }
    }
}
