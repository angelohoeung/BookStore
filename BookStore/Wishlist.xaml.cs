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
        private UserData user;
        private MainWindow mainWindow;

        public Wishlist(List<WishlistItem> wishlist, UserData user, MainWindow mainWindow)
        {
            InitializeComponent();
            this.wishlist = new ObservableCollection<WishlistItem>(wishlist);
            this.dalWishlist = new DALWishlist();
            this.user = user;
            this.mainWindow = mainWindow;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            wishlistListView.ItemsSource = this.wishlist;
            UpdateWindow();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (wishlistListView.SelectedItem is WishlistItem selectedItem)
            {
                this.dalWishlist.addItemWishlistItemToShoppingCart(user.UserId, selectedItem);
                this.dalWishlist.deleteItemFromWishlist(this.user.UserId, selectedItem);
                this.wishlist.Remove(selectedItem);
                UpdateWindow();
                this.mainWindow.refreshShoppingCart(selectedItem);
            }
            else
            {
                MessageBox.Show("Please select an item to add.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (wishlistListView.SelectedItem is WishlistItem selectedItem)
            {
                this.dalWishlist.deleteItemFromWishlist(this.user.UserId, selectedItem);
                this.wishlist.Remove(selectedItem);
                UpdateWindow();
            }
            else
            {
                MessageBox.Show("Please select an item to remove.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateWindow()
        {
            wishlistListView.ItemsSource = null;
            wishlistListView.ItemsSource = this.wishlist;
            totalItems.Text = $"Total Items: {this.wishlist.Count}";
        }

    }
}
