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

    }
}
