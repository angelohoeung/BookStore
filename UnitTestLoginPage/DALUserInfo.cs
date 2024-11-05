using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BookStoreLIB {
    internal class DALUserInfo {
        public int LogIn(string username, string password) {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            try {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT UserId FROM UserData WHERE UserName = @UserName AND Password = @Password";
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int userId)) {
                    return userId;
                } else {
                    return -1;
                }
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

        public int SignUp(string username, string password, string fullName) {
            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            try {
                SqlCommand checkExsitingUser = new SqlCommand();
                checkExsitingUser.Connection = conn;
                checkExsitingUser.CommandText = "SELECT COUNT(*) FROM UserData WHERE UserName = @UserName";
                checkExsitingUser.Parameters.AddWithValue("@UserName", username);
                conn.Open();

                int userCount = (int)checkExsitingUser.ExecuteScalar();
                if (userCount > 0) {
                    Debug.WriteLine("Username already exists.");
                    return -1;
                }

                SqlCommand maxIdCmd = new SqlCommand("SELECT ISNULL(MAX(UserID), 0) FROM UserData", conn);
                int newUserId = (int)maxIdCmd.ExecuteScalar() + 1;

                SqlCommand addUser = new SqlCommand();
                addUser.Connection = conn;
                addUser.CommandText = "INSERT INTO UserData (UserID, UserName, Password, Type, Manager, FullName) VALUES (@UserID, @UserName, @Password, @Type, @Manager, @FullName)";
                addUser.Parameters.AddWithValue("@UserID", newUserId);
                addUser.Parameters.AddWithValue("@UserName", username);
                addUser.Parameters.AddWithValue("@Password", password);
                addUser.Parameters.AddWithValue("@Type", "CM");
                addUser.Parameters.AddWithValue("@Manager", false);
                addUser.Parameters.AddWithValue("@FullName", fullName);
                addUser.ExecuteNonQuery();

                SqlCommand checkSignUp = new SqlCommand();
                checkSignUp.Connection = conn;
                checkSignUp.CommandText = "SELECT UserId FROM UserData WHERE UserName = @UserName AND Password = @Password";
                checkSignUp.Parameters.AddWithValue("@UserName", username);
                checkSignUp.Parameters.AddWithValue("@Password", password);
                object result = checkSignUp.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int userId)) {
                    return userId;
                } else {
                    Debug.WriteLine("User not added to database");
                    return -1;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                return -1;
            } finally {
                if (conn.State == System.Data.ConnectionState.Open) {
                    conn.Close();
                }
            }
        }
    }
}