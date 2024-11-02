using BookStoreLIB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace BookStoreGUI {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OrderConfirmation : Window {
        private readonly ObservableCollection<OrderItem> items;
        public OrderConfirmation(ObservableCollection<OrderItem> items) {
            InitializeComponent();
            this.items = items;
        }
        private void WindowLoaded(object sender, RoutedEventArgs e) {
            UpdateWindow(this.items);
        }
        private void OkButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            this.Close(); 
        }

        private void UpdateWindow(ObservableCollection<OrderItem> items) {
            double total = 0;
            StringBuilder orderSummaryBuilder = new StringBuilder();

            foreach (OrderItem item in items) {
                total += item.SubTotal;
                orderSummaryBuilder.AppendLine($"{item.Quantity}x {item.BookTitle} (ISBN: {item.BookID})\n");
            }
            orderSummaryTextBlock.Text = orderSummaryBuilder.ToString();
            bookTotal.Text = $"Total: ${total}";
        }
    }
}
