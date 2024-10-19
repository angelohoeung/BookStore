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
    }
}
