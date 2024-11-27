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
        private ObservableCollection<OrderItem> wishlist;

        public Wishlist(ObservableCollection<OrderItem> wishlist)
        {
            InitializeComponent();
            this.wishlist = wishlist;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            UpdateWindow(wishlist);
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateWindow(ObservableCollection<OrderItem> items)
        {
            int total = 0;
            StringBuilder orderSummaryBuilder = new StringBuilder();

            foreach (OrderItem item in items)
            {
                total++;
                orderSummaryBuilder.AppendLine($"{item.Quantity}x {item.BookTitle} (ISBN: {item.BookID})\n");
            }
            wishlistTextBlock.Text = orderSummaryBuilder.ToString();
            totalItems.Text = $"Total items: {total}";
        }
    }
}
