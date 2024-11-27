using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class BookCatalog
    {
        public DataSet GetBookInfo()
        {
            //perform any business logic befor passing to client.
            // None needed at this time.
            DALBookCatalog bookCatalog = new DALBookCatalog();
            return bookCatalog.GetBookInfo();
        }
        public DataSet GetBooks(bool showAll) {
            DALBookCatalog bookCatalog = new DALBookCatalog();
            return bookCatalog.GetBooks(showAll);
        }

        public bool UpdateBook(string isbn, string title, string author, decimal price, string year, string publisher, int categoryId, int supplierId, int inStock, string edition) {
            DALBookCatalog dalBookCatalog = new DALBookCatalog();
            return dalBookCatalog.UpdateBook(isbn, title, author, price, year, publisher, categoryId, supplierId, inStock, edition);
        }

        public bool SetBookOutOfStock(string isbn) {
            DALBookCatalog dalBookCatalog = new DALBookCatalog();
            return dalBookCatalog.SetBookOutOfStock(isbn);
        }
    }
}
