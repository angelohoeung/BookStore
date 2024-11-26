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
    class DALBookCatalog
    {
        SqlConnection conn;
        DataSet dsBooks;
        public DALBookCatalog()
        {
            conn = new SqlConnection(Properties.Settings.Default.Connection);
        }
        public DataSet GetBookInfo()
        {
            try
            {
                String strSQL = "Select CategoryID, Name, Description from Category";
                SqlCommand cmdSelCategory = new SqlCommand(strSQL, conn);
                SqlDataAdapter daCatagory = new SqlDataAdapter(cmdSelCategory);
                dsBooks = new DataSet("Books");
                daCatagory.Fill(dsBooks, "Category");            //Get category info

                String strSQL2 = "Select ISBN, CategoryID, Title," +
                    "Author, Price, Year, Edition, Publisher from BookData";
                SqlCommand cmdSelBook = new SqlCommand(strSQL2, conn);
                SqlDataAdapter daBook = new SqlDataAdapter(cmdSelBook);
                daBook.Fill(dsBooks, "Books");                  //Get Books info

                DataRelation drCat_Book = new DataRelation ("drCat_Book",
                dsBooks.Tables["Category"].Columns["CategoryID"],
                dsBooks.Tables["Books"].Columns["CategoryID"], false);
                dsBooks.Relations.Add(drCat_Book);       //Set up the table relation
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return dsBooks;
        }

        public DataSet GetBooks(bool showAll) {
            try {
                String strSQLCategory = "Select CategoryID, Name, Description from Category";
                SqlCommand cmdSelCategory = new SqlCommand(strSQLCategory, conn);
                SqlDataAdapter daCategory = new SqlDataAdapter(cmdSelCategory);
                dsBooks = new DataSet("Books");
                daCategory.Fill(dsBooks, "Category");

                // get books info with InStock filtering if user is not a manager
                String strSQLBooks = "Select ISBN, CategoryID, Title, Author, Price, Year, Edition, Publisher, InStock from BookData";
                if (!showAll) {
                    strSQLBooks += " WHERE InStock > 0";
                }
                SqlCommand cmdSelBooks = new SqlCommand(strSQLBooks, conn);
                SqlDataAdapter daBooks = new SqlDataAdapter(cmdSelBooks);
                daBooks.Fill(dsBooks, "Books");

                // set up table relations
                DataRelation drCat_Book = new DataRelation("drCat_Book",
                    dsBooks.Tables["Category"].Columns["CategoryID"],
                    dsBooks.Tables["Books"].Columns["CategoryID"], false);
                dsBooks.Relations.Add(drCat_Book);

                return dsBooks;
            }
            catch (Exception ex) {
                Debug.WriteLine($"Error fetching books with filter: {ex.Message}");
                return null;
            }
        }

        public bool UpdateBook(string isbn, string title, string author, decimal price, string publisher) {
            try {
                string updateQuery = "UPDATE BookData SET Title = @Title, Author = @Author, Price = @Price, Publisher = @Publisher WHERE ISBN = @ISBN";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@ISBN", isbn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Author", author);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Publisher", publisher);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex) {
                Debug.WriteLine($"Error updating book: {ex.Message}");
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }
        }

        public bool SetBookOutOfStock(string isbn) {
            try {
                string updateQuery = "UPDATE BookData SET InStock = 0 WHERE ISBN = @ISBN";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@ISBN", isbn);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex) {
                Debug.WriteLine($"Error marking book out of stock: {ex.Message}");
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }
        }
    }
}
