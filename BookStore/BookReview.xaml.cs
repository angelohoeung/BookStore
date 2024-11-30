using BookStoreLIB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for BookReview.xaml
    /// </summary>
    public partial class BookReview : Window
    {
        UserData userData;
        BookReviews bookReviews;
        public BookReview(UserData user)
        {
            InitializeComponent();
            this.userData = user;
            this.bookReviews = new BookReviews();
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleSearchBox.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a book title to search.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Fetch reviews using DALBookReviews
                DataTable reviews = bookReviews.GetReviews(title);

                if (reviews.Rows.Count == 0)
                {
                    MessageBox.Show("No reviews found for this book.", "No Results", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Bind reviews to the ListBox
                ReviewsFeed.ItemsSource = reviews.AsEnumerable().Select(row => new
                {
                    User = $"User ID: {row["UserID"]}", // Display UserID; adjust as needed
                    Content = row["Content"].ToString()
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching reviews: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SubmitReviewButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleSearchBox.Text.Trim();
            string content = ReviewContentBox.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                MessageBox.Show("Both the book title and review content are required.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Submit the review using BookReviews
                bool success = bookReviews.SetReview(title, userData.UserId, content);

                if (success)
                {
                    MessageBox.Show("Your review has been submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear the input box and refresh the reviews feed
                    ReviewContentBox.Clear();
                    SearchButton_Click(null, null); // Refresh feed
                }
                else
                {
                    MessageBox.Show("Failed to submit the review. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while submitting your review: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
