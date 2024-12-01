using System.Data.SqlClient;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace BookStoreLIB {
    public class DashboardManager {
        public List<BookSales> BookInfo { get; set; }
        public BookSales TopBook { get; set; }
        public List<Inventory> BookInventory { get; set; }

        public bool Display(int userId) {
            if (userId < 1) {
                Debug.WriteLine("User not logged in.");
                return false;
            }

            DALDashboardManager dalDash = new DALDashboardManager();
            List<BookSales> result = dalDash.Display(userId);
            if (result != null) {
                Debug.Write("Sucessful Display");
                BookInfo = result;
                TopBook = dalDash.TopBook;
                BookInventory = dalDash.BookInventory;
                return true;
            } else {
                return false;
            }
        }
    }
}