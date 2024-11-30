using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class BookReviews
    {
        private DALBookReviews dalBookReviews;
        public BookReviews()
        {
            this.dalBookReviews = new DALBookReviews();
        }
        public DataTable GetReviews(string title)
        {
            return dalBookReviews.GetReviews(title);
        }

        public bool SetReview(string title, int userID, string content)
        {
            return dalBookReviews.SetReview(title, userID, content);
        }
    }
}
