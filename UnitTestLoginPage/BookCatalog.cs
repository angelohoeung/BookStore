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

        public string UpdateBook(string isbn, string title, string author, decimal price, string year, string publisher, int categoryId, int supplierId, int inStock, string edition) {
            // validate inputs
            if (string.IsNullOrWhiteSpace(isbn) || isbn.Length > 10)
                return "ISBN must be 10 characters or less.";
            if (string.IsNullOrWhiteSpace(title) || title.Length > 80)
                return "Title cannot be empty and must be 80 characters or less.";
            if (string.IsNullOrWhiteSpace(author) || author.Length > 255)
                return "Author cannot be empty and must be 255 characters or less.";
            if (price < 0 || price > 99999999.99M)
                return "Price must be between 0 and 99999999.99.";
            if (string.IsNullOrWhiteSpace(year) || year.Length > 4)
                return "Year must be 4 characters or less.";
            if (string.IsNullOrWhiteSpace(edition) || edition.Length > 2)
                return "Edition must be 2 characters or less.";
            if (string.IsNullOrWhiteSpace(publisher) || publisher.Length > 50)
                return "Publisher cannot be empty and must be 50 characters or less.";
            if (categoryId <= 0)
                return "CategoryID must be a valid positive integer.";
            if (supplierId <= 0)
                return "SupplierID must be a valid positive integer.";
            if (inStock < 0)
                return "InStock cannot be negative.";

            // pass validated inputs to DAL
            DALBookCatalog dalBookCatalog = new DALBookCatalog();
            bool success = dalBookCatalog.UpdateBook(isbn, title, author, price, year, publisher, categoryId, supplierId, inStock, edition);

            return success ? "" : "Failed to update the book in the database.";
        }

        public string SetBookOutOfStock(string isbn) {
            // validate ISBN
            if (string.IsNullOrWhiteSpace(isbn) || isbn.Length > 10)
                return "ISBN must be 10 characters or less.";

            DALBookCatalog dalBookCatalog = new DALBookCatalog();
            bool success = dalBookCatalog.SetBookOutOfStock(isbn);

            return success ? "" : "Failed to mark the book as out of stock.";
        }
    }
}
