using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class WishlistItem
    {
        public string Isbn { get; set; }
        public string BookName { get; set; }
        public double Price { get; set; }

        public WishlistItem(string Isbn, string BookName, double Price)
        {
            this.Isbn = Isbn;
            this.BookName = BookName;
            this.Price = Price;
        }
    }
}
