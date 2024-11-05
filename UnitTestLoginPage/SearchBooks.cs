using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class SearchBooks
    {
        private BookCatalog bookCatalog;
        public SearchBooks() 
        {
            bookCatalog = new BookCatalog();
        }
        public DataTable Search(String searchQuery, String searchType)
        {
            DataSet bookDataSet = bookCatalog.GetBookInfo();
            if (bookDataSet.Tables.Contains("Books") && searchType != "")
            {
                DataTable booksTable = bookDataSet.Tables["Books"];
                string filterExpression = string.IsNullOrEmpty(searchQuery) ?
                    "" : $"{searchType} LIKE '%{searchQuery}%'";

                booksTable.DefaultView.RowFilter = filterExpression;
                return booksTable;

            }
            return null;
        }
    }
}
