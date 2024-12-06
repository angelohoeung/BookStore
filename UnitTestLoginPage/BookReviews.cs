using System;
using System.Data;

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
            // Validation
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            // Fetch reviews from DAL
            return dalBookReviews.GetReviews(title);
        }

        public bool SetReview(string title, int userID, string content)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content cannot be empty.", nameof(content));

            if (userID < 0)
                throw new ArgumentOutOfRangeException(nameof(userID), "User ID cannot be negative.");

            // Insert review via DAL
            return dalBookReviews.SetReview(title, userID, content);
        }
    }
}
