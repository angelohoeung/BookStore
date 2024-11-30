using BookStoreLIB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for Wishlist.xaml
    /// </summary>
    /// 

    public partial class Wishlist : Window
    {
        private ObservableCollection<WishlistItem> wishlist;
        private DALWishlist dalWishlist;

        public Wishlist(ObservableCollection<WishlistItem> wishlist)
        {
            InitializeComponent();
            this.wishlist = wishlist;
            this.dalWishlist = new DALWishlist();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            UpdateWindow();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            //this.dalWishlist.addItemWishlistItemToShoppingCart(this.wishlistTextBlock.)

            this.UpdateWindow();

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.UpdateWindow();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateWindow()
        {
            int total = 0;
            StringBuilder orderSummaryBuilder = new StringBuilder();

            foreach (WishlistItem item in this.wishlist)
            {
                total++;
                orderSummaryBuilder.AppendLine($" {item.BookName} (ISBN: {item.Isbn})\n");
            }
            wishlistTextBlock.Text = orderSummaryBuilder.ToString();
            totalItems.Text = $"Total items: {total}";
        }
    }
}
