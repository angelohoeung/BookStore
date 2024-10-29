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
    class DALAccount
    {
        SqlConnection conn;
        DataSet dsAccountInfo;
        public DALAccount()
        {
            conn = new SqlConnection(Properties.Settings.Default.Connection);
        }
        public DataSet GetAccountInfo()
        {
            try
            {
                String strSQL = "Select UserID, UserName, Password, Fullname from UserData";
                SqlCommand cmdGetAccountInfo = new SqlCommand(strSQL, conn);
                SqlDataAdapter daAccountInfo = new SqlDataAdapter(cmdGetAccountInfo);
                dsAccountInfo = new DataSet("Books");
                daAccountInfo.Fill(dsAccountInfo, "Category");            //Get category info
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return dsAccountInfo;
        }
    }
}
