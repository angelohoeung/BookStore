/* **********************************************************************************
 * For use by students taking 60-422 (Fall, 2014) to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
 * **********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                if (userData.LogIn(dlg.nameTextBox.Text, dlg.passwordTextBox.Password) == true)
                    this.statusTextBlock.Text = "You are logged in as User #" +
                    userData.UserId;
                else
                    this.statusTextBlock.Text = "Your login failed. Please try again.";
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
        public MainWindow() { InitializeComponent(); }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookCatalog bookCat = new BookCatalog();
            dsBookCat = bookCat.GetBookInfo();
            this.DataContext = dsBookCat.Tables["Category"];
            bookOrder = new BookOrder();
            userData = new UserData();
            this.orderListView.ItemsSource = bookOrder.OrderItemList;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;
            selectedRow = (DataRowView)this.ProductsDataGrid.SelectedItems[0];
            orderItemDialog.isbnTextBox.Text = selectedRow.Row.ItemArray[0].ToString();
            orderItemDialog.titleTextBox.Text = selectedRow.Row.ItemArray[2].ToString();
            orderItemDialog.priceTextBox.Text = selectedRow.Row.ItemArray[4].ToString();
            orderItemDialog.Owner = this;
            orderItemDialog.ShowDialog();
            if (orderItemDialog.DialogResult == true) {
                string isbn = orderItemDialog.isbnTextBox.Text;
                string title = orderItemDialog.titleTextBox.Text;
                double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
            }
            UpdateTotal();
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
                        if(quantity > 0)
                        {
                            bookOrder.SetQuantity((selectedItem as OrderItem), quantity);
                        }
                        else if(quantity == 0)
                        {
                            bookOrder.RemoveItem((selectedItem as OrderItem)?.BookID);
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
    }
}
