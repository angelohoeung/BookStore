/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using BookStoreGUI.Views;
using BookStoreLIB;

namespace BookStoreGUI {
    /// Interaction logic for MainWindow.xaml
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        DataSet dsBookCat;
        UserData userData;
        BookOrder bookOrder;

        private decimal totalAmount;
        public decimal TotalAmount
        {
            get { return totalAmount; }
            set
            {
                if (totalAmount != value)
                {
                    totalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateTotal()
        {
           
            TotalAmount = bookOrder.OrderItemList.Sum(item => (decimal)item.SubTotal);
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginDialog dlg = new LoginDialog();
            dlg.Owner = this;
            dlg.ShowDialog();
            // Process data entered by user if dialog box is accepted
            if (dlg.DialogResult == true) {
                if (userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password) == true) {
                    this.statusTextBlock.Text = "You are logged in as User #" +
                    userData.UserId;
                    RefreshBooks();

                    //Load shopping cart
                    DALShoppingCart cart = new DALShoppingCart();
                    List<OrderItem> cartItems = cart.GetCartItems(userData.UserId);

                    Console.WriteLine("Cart items count: " + cartItems.Count());
                    foreach(var item in cartItems)
                    {
                        bookOrder.AddItem(item);
                    }
                    UpdateTotal();
                }
                else
                    this.statusTextBlock.Text = "Your login failed. Please try again.";
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
        public MainWindow() { InitializeComponent(); }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            userData = new UserData();
            bookOrder = new BookOrder();
            BookCatalog bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBooks(userData.IsManager);
            this.DataContext = dsBookCat.Tables["Category"];
            this.orderListView.ItemsSource = bookOrder.OrderItemList;
            Debug.WriteLine($"User Manager Status: {userData.IsManager}");
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            
            if(userData.UserId > 0)
            {
                if (ProductsDataGrid.SelectedItems.Count > 0)
                {
                    OrderItemDialog orderItemDialog = new OrderItemDialog();
                    DataRowView selectedRow;
                    selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];
                    orderItemDialog.isbnTextBox.Text = selectedRow["ISBN"].ToString();
                    orderItemDialog.titleTextBox.Text = selectedRow["Title"].ToString();
                    orderItemDialog.priceTextBox.Text = selectedRow["Price"].ToString();
                    orderItemDialog.Owner = this;
                    orderItemDialog.ShowDialog();
                    if (orderItemDialog.DialogResult == true)
                    {
                        string isbn = orderItemDialog.isbnTextBox.Text;
                        string title = orderItemDialog.titleTextBox.Text;
                        double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                        int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                        OrderItem orderItem = new OrderItem(isbn, title, unitPrice, quantity);
                        bookOrder.AddItem(orderItem);
                        DALShoppingCart cart = new DALShoppingCart();
                        cart.AddCartItem(userData.UserId, orderItem);

                    }
                    UpdateTotal();
                }
            }
            else
            {
                MessageBox.Show("You must be logged in");
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.orderListView.SelectedItem != null)
            {
                var selectedOrderItem = this.orderListView.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);
            }
            UpdateTotal();
        }
        private void chechoutButton_Click(object sender, RoutedEventArgs e) {
            if (userData.UserId > 0 && bookOrder.OrderItemList.Count() > 0) {
                PaymentWindow pw = new PaymentWindow() { Owner = this };
                pw.ShowDialog();
                if (pw.DialogResult == true) {
                    OrderConfirmation conf = new OrderConfirmation(bookOrder.OrderItemList) { Owner = this };
                    conf.ShowDialog();
                    if (conf.DialogResult == true) {
                        var orderId = bookOrder.PlaceOrder(userData.UserId);
                        this.statusTextBlock.Text = $"Order placed successfully. ID: {orderId}";
                    }
                }
            }

            //int orderId;
            //orderId = bookOrder.PlaceOrder(userData.UserId);
            //MessageBox.Show("Your order has been placed. Your order id is " +
            //orderId.ToString());
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e) {
            if (userData.UserId > 0) {
                AccountManagementWindow accountWindow = new AccountManagementWindow(userData);
                accountWindow.Owner = this;
                accountWindow.ShowDialog();
            }
            else {
                MessageBox.Show("You are not logged in. Please log in to access account management.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (orderListView.SelectedItem != null)
            {
                var selectedItem = orderListView.SelectedItem;
                //MessageBox.Show($"Editing item with ISBN: {(selectedItem as OrderItem)?.BookID}");
                EditOrderItemDialog editOrderdialog = new EditOrderItemDialog();

                editOrderdialog.isbnTextBox.Text = (selectedItem as OrderItem)?.BookID;
                editOrderdialog.titleTextBox.Text = (selectedItem as OrderItem)?.BookTitle;
                editOrderdialog.priceTextBox.Text = (selectedItem as OrderItem)?.UnitPrice.ToString();
                editOrderdialog.quantityTextBox.Text = (selectedItem as OrderItem)?.Quantity.ToString();
                editOrderdialog.Owner = this;
                editOrderdialog.ShowDialog();
                
                if(editOrderdialog.DialogResult == true)
                {
                    if(Int32.TryParse(editOrderdialog.quantityTextBox.Text, out int quantity))
                    {
                        DALShoppingCart cart = new DALShoppingCart();
                        if (quantity > 0)
                        {
                            bookOrder.SetQuantity((selectedItem as OrderItem), quantity);
                            
                            cart.EditCartItem(userData.UserId, selectedItem  as OrderItem);
                        }
                        else if(quantity == 0)
                        {
                            bookOrder.RemoveItem((selectedItem as OrderItem)?.BookID);
                            cart.RemoveCartItem(userData.UserId, selectedItem as OrderItem);
                        }
                        else
                        {
                            MessageBox.Show("Please enter a non-negative quantity");
                        }
                    }
                    else
                    {
                        //Remove item
                        MessageBox.Show("Please enter a valid quantity");
                    }
                }

            }
            UpdateTotal();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (orderListView.SelectedItem != null)
            {
                var selectedItem = orderListView.SelectedItem;

                bookOrder.RemoveItem((selectedItem as OrderItem)?.BookID);
                //MessageBox.Show($"Deleting item with ISBN: {(selectedItem as OrderItem)?.BookID}");

                DALShoppingCart cart = new DALShoppingCart();
                cart.RemoveCartItem(userData.UserId, selectedItem as OrderItem);
            }
            UpdateTotal();
            
        }

        private void signUpButtonClick(object sender, RoutedEventArgs e) {
            SignUp signUp = new SignUp();
            signUp.Owner = this;
            signUp.ShowDialog();
            if (signUp.DialogResult == true) {
                if (userData.SignUp(signUp.usernameTextBox.Text, signUp.passwordTextBox.Password, signUp.confirmPasswordTextBox.Password, signUp.fullNameTextBox.Text) == true) {
                    this.statusTextBlock.Text = "You have successfully signed up";
                } else {
                    this.statusTextBlock.Text = "Sign Up failed. Please try again.";
                }
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            Search searchWindow = new Search();
            searchWindow.Owner = this;
            searchWindow.ShowDialog();
        }

        private void wishlistButton_Click(Object sender, RoutedEventArgs e)
        {
            if (orderListView.SelectedItem != null)
            {
                DALWishlist wishlist = new DALWishlist();
                wishlist.addItemToWishlist(userData.UserId, orderListView.SelectedItem as OrderItem);
            }
        }

        private void RefreshBooks() {
            BookCatalog bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBooks(userData.IsManager);
            this.DataContext = dsBookCat.Tables["Category"];
        }

        private void EditBookMenuItem_Click(object sender, RoutedEventArgs e) {
            if (!userData.IsManager) {
                MessageBox.Show("You do not have permission to edit books.");
                return;
            }

            if (ProductsDataGrid.SelectedItem != null) {
                DataRowView selectedRow = (DataRowView)ProductsDataGrid.SelectedItem;
                EditBookDialog editDialog = new EditBookDialog();

                // populate dialog with book details
                editDialog.ISBNTextBox.Text = selectedRow["ISBN"].ToString();
                editDialog.TitleTextBox.Text = selectedRow["Title"].ToString();
                editDialog.AuthorTextBox.Text = selectedRow["Author"].ToString();
                editDialog.PriceTextBox.Text = selectedRow["Price"].ToString();
                editDialog.PublisherTextBox.Text = selectedRow["Publisher"].ToString();
                editDialog.YearTextBox.Text = selectedRow["Year"].ToString();
                editDialog.EditionTextBox.Text = selectedRow["Edition"].ToString();
                editDialog.InStockTextBox.Text = selectedRow["InStock"].ToString();

                // load categories and suppliers
                BookCatalog bookCatalog = new BookCatalog();
                DataSet dsBookData = bookCatalog.GetBooks(userData.IsManager);
                editDialog.CategoryComboBox.ItemsSource = dsBookData.Tables["Category"].DefaultView;
                editDialog.CategoryComboBox.SelectedValue = selectedRow["CategoryID"];

                editDialog.SupplierComboBox.ItemsSource = dsBookData.Tables["Supplier"].DefaultView;
                editDialog.SupplierComboBox.SelectedValue = selectedRow["SupplierId"];

                editDialog.Owner = this;
                if (editDialog.ShowDialog() == true) {
                    string errorMessage = bookCatalog.UpdateBook(
                        editDialog.ISBNTextBox.Text,
                        editDialog.TitleTextBox.Text,
                        editDialog.AuthorTextBox.Text,
                        decimal.Parse(editDialog.PriceTextBox.Text),
                        editDialog.YearTextBox.Text,
                        editDialog.PublisherTextBox.Text,
                        int.Parse(editDialog.CategoryComboBox.SelectedValue.ToString()),
                        int.Parse(editDialog.SupplierComboBox.SelectedValue.ToString()),
                        int.Parse(editDialog.InStockTextBox.Text),
                        editDialog.EditionTextBox.Text
                    );

                    if (!string.IsNullOrEmpty(errorMessage)) {
                        MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else {
                        MessageBox.Show("Book updated successfully.");
                        RefreshBooks();
                    }
                }
            }
        }

        private void DeleteBookMenuItem_Click(object sender, RoutedEventArgs e) {
            if (!userData.IsManager) {
                MessageBox.Show("You do not have permission to delete books.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ProductsDataGrid.SelectedItem != null) {
                DataRowView selectedRow = (DataRowView)ProductsDataGrid.SelectedItem;
                string isbn = selectedRow["ISBN"].ToString();

                BookCatalog bookCatalog = new BookCatalog();
                string errorMessage = bookCatalog.SetBookOutOfStock(isbn);

                if (string.IsNullOrEmpty(errorMessage)) {
                    MessageBox.Show("Book marked as out of stock.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshBooks();
                }
                else {
                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ViewWishlistButton_Click(object sender, RoutedEventArgs e)
        {
            if (userData.UserId > 0)
            {
                Wishlist wishlistWindow = new Wishlist();
                
                wishlistWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not logged in. Please log in to access account management.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool IsManager {
            get { return userData.IsManager; }
        }
    }
}
