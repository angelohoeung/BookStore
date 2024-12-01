using System.Data.SqlClient;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace BookStoreLIB {
    internal class DALDashboardManager {
        public BookSales TopBook { get; set; }
        public List<Inventory> BookInventory { get; set; }

        public List<BookSales> Display(int userId) {

            var conn = new SqlConnection(Properties.Settings.Default.Connection);
            try {
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = "SELECT Manager FROM UserData WHERE UserID = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);
                conn.Open();
                object isAdminResult = command.ExecuteScalar();

                if (isAdminResult != null && !(bool)isAdminResult) {
                    Debug.WriteLine("User is not Admin.");
                    return null;
                }
                List<BookSales> bookSales = new List<BookSales>();

                command.CommandText = "SELECT BD.Title AS BookName, SUM(OI.Quantity) AS NumberSold FROM Orders AS O JOIN OrderItem AS OI ON O.OrderID = OI.OrderID JOIN BookData AS BD ON OI.ISBN = BD.ISBN WHERE O.OrderDate >= DATEADD(DAY, -7, GETDATE()) GROUP BY BD.Title ORDER BY NumberSold DESC";

                SqlDataReader getWeeklyBooksReader = command.ExecuteReader();
                while (getWeeklyBooksReader.Read()) {
                    bookSales.Add(new BookSales {
                        Book = getWeeklyBooksReader.GetString(0),
                        NumberSold = getWeeklyBooksReader.GetInt32(1)
                    });
                }
                getWeeklyBooksReader.Close();

                command.CommandText = "SELECT TOP 1 BD.Title AS BookName, SUM(OI.Quantity) AS TotalSold FROM OrderItem OI INNER JOIN BookData BD ON OI.ISBN = BD.ISBN GROUP BY BD.Title ORDER BY SUM(OI.Quantity) DESC;";

                SqlDataReader getTopBookReader = command.ExecuteReader();
                while (getTopBookReader.Read()) {
                    TopBook = new BookSales {
                        Book = getTopBookReader.GetString(0),
                        NumberSold = getTopBookReader.GetInt32(1)
                    };
                }
                getTopBookReader.Close();

                command.CommandText = "SELECT Title, InStock FROM Bookdata";

                List<Inventory> bookInventory = new List<Inventory>();
                SqlDataReader getInventoryReader = command.ExecuteReader();
                while (getInventoryReader.Read()) {
                    bookInventory.Add(new Inventory {
                        Book = getInventoryReader.GetString(0),
                        Stock = getInventoryReader.GetInt32(1)
                    });
                }
                getInventoryReader.Close();
                BookInventory = bookInventory;

                return bookSales;

            } catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                return null;
            } finally {
                if (conn.State == System.Data.ConnectionState.Open) {
                    conn.Close();
                }
            }
        }
    }
}