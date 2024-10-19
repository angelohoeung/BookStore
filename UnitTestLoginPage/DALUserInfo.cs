using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BookStoreLIB {
    internal class DALUserInfo {
        public int LogIn(string userName, string password) {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            try {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT UserId FROM UserData WHERE UserName = @UserName AND Password = @Password";
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();
                int userId = (int)cmd.ExecuteScalar();
                if (userId > 0) { return userId; } else return -1;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                return -1;
            }
            finally {
                if (conn.State == System.Data.ConnectionState.Open) {
                    conn.Close();
                }
            }
        }
    }
}