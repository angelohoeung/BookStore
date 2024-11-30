using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class DALAccount
    {
        SqlConnection conn;
        DataSet dsAccountInfo;

        public DALAccount()
        {
            conn = new SqlConnection(Properties.Settings.Default.Connection);
        }

        //-------------function to get order history by users
        public DataSet getOrderHistoryByUserID(int UserID)
        {
            try
            {
                String strSQL =
                    @"
                    SELECT 
                        BookData.Title, 
                        BookData.Author, 
                        BookData.Year, 
                        BookData.Edition, 
                        BookData.Publisher, 
                        BookData.Price, 
                        Orders.OrderDate
                    FROM 
                        Orders
                    INNER JOIN 
                        OrderItem 
                        ON Orders.OrderID = OrderItem.OrderID
                    INNER JOIN 
                        BookData 
                        ON OrderItem.ISBN = BookData.ISBN
                    WHERE 
                        Orders.UserID = @UserID";

                using (SqlCommand cmdGetOrder = new SqlCommand(strSQL, conn))
                {
                    cmdGetOrder.Parameters.AddWithValue("@UserID", UserID);

                    SqlDataAdapter daOrder = new SqlDataAdapter(cmdGetOrder);
                    dsAccountInfo = new DataSet("Orders");
                    daOrder.Fill(dsAccountInfo, "Orders");
                }
            }
            catch (Exception ex) { return null; }
            return dsAccountInfo;
        }

        public DataSet GetAccountInfo(int userId)
        {
            try
            {
                String strSQL = 
                    @"
                    SELECT 
                        UserID,
                        UserName, 
                        Password, 
                        Fullname 
                    FROM 
                        UserData 
                    WHERE 
                        UserID = @UserId";

                using (SqlCommand cmdGetAccountInfo = new SqlCommand(strSQL, conn))
                {
                cmdGetAccountInfo.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter daAccountInfo = new SqlDataAdapter(cmdGetAccountInfo);
                dsAccountInfo = new DataSet("Accounts");
                daAccountInfo.Fill(dsAccountInfo, "Accounts");          
                }
            }     
            catch (Exception ex) { return null; }
            return dsAccountInfo;
        }

        public DataSet GetAccountInfoByName(string userName)
        {
            try
            {
                String strSQL =
                    @"
                    SELECT 
                        UserID,
                        UserName, 
                        Password, 
                        Fullname 
                    FROM 
                        UserData 
                    WHERE 
                        UserName = @userName";

                using (SqlCommand cmdGetAccountInfo = new SqlCommand(strSQL, conn))
                {
                    cmdGetAccountInfo.Parameters.AddWithValue("@userName", userName);

                    SqlDataAdapter daAccountInfo = new SqlDataAdapter(cmdGetAccountInfo);
                    dsAccountInfo = new DataSet("Accounts");
                    daAccountInfo.Fill(dsAccountInfo, "Accounts");
                }
            }
            catch (Exception ex) { return null; }
            return dsAccountInfo;
        }

        public bool UpdateAccount(int userId, string userName, string password, string fullName)
        {
            string query = @"
            UPDATE UserData
            SET 
                UserName = @UserName, 
                Password = @Password, 
                FullName = @FullName
            WHERE 
                UserID = @UserId";


            using (SqlCommand command = new SqlCommand(query, conn))    
            {
                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@FullName", fullName);

                try
                {
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Return true if at least one row was updated
                    conn.Close();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    // Log or handle exception as needed
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }
            }
        }

        public bool DeleteAccount (int userId)
        {
            string query = @"
            DELETE FROM OrderItem
            WHERE OrderID IN (SELECT OrderID FROM Orders WHERE UserID = @UserId);

            DELETE FROM Orders
            WHERE UserID = @UserId;

            DELETE FROM UserData
            WHERE UserID = @UserId;
            ";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@UserId", userId);

                try
                {
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Return true if at least one row was updated
                    conn.Close();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    // Log or handle exception as needed
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return false;
                }
            }
        }

    }
}
