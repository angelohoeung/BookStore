using BookStoreLIB;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditBookDialog.xaml
    /// </summary>
    public partial class EditBookDialog : Window {
        public EditBookDialog() {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) {
            BookCatalog bookCatalog = new BookCatalog();

            // validate and update the book
            string errorMessage = bookCatalog.UpdateBook(
                ISBNTextBox.Text,
                TitleTextBox.Text,
                AuthorTextBox.Text,
                decimal.TryParse(PriceTextBox.Text, out var price) ? price : -1,
                YearTextBox.Text,
                PublisherTextBox.Text,
                CategoryComboBox.SelectedValue is int categoryId ? categoryId : -1,
                SupplierComboBox.SelectedValue is int supplierId ? supplierId : -1,
                int.TryParse(InStockTextBox.Text, out var inStock) ? inStock : -1,
                EditionTextBox.Text
            );

            if (!string.IsNullOrEmpty(errorMessage)) {
                // display error message
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // close the dialog if successful
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }
    }
}
