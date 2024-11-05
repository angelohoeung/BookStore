using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using BookStoreLIB;
using BookStoreGUI.Views;

namespace BookStoreGUI
{
    public partial class MainWindow : Window
    {
        DataSet dsBookCat;
        UserData userData;
        BookOrder bookOrder;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                dsBookCat = new DataSet();
                userData = new UserData();
                bookOrder = new BookOrder();
                
                // Set initial status
                statusTextBlock.Text = "Please login before proceeding to checkout.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing application: " + ex.Message);
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // Login code here
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load code here
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // Add book code here
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove book code here
        }

        private void chechoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Checkout code here
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            Search searchWindow = new Search();
            searchWindow.Owner = this;
            searchWindow.ShowDialog();
        }
    }
} 