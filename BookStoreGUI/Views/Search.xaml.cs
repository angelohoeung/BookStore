using System.Data;
using System.Windows;
using System.Windows.Controls;
using BookStoreLIB;

namespace BookStoreGUI.Views
{
    public partial class Search : Window
    {
        private BookCatalog bookCatalog;

        public Search()
        {
            InitializeComponent();
            bookCatalog = new BookCatalog();
            LoadInitialData();
        }

        private void LoadInitialData()
        {
            DataSet bookDataSet = bookCatalog.GetBookInfo();
            if (bookDataSet != null && bookDataSet.Tables.Contains("Books"))
            {
                ResultsGrid.ItemsSource = bookDataSet.Tables["Books"].DefaultView;
            }
        }

        private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedOption = selectedItem.Content.ToString();
                // Update search logic based on type
            }
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedOption = selectedItem.Content.ToString();
                // Update sorting logic
            }
        }

        private void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedOption = selectedItem.Content.ToString();
                // Update sort order
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = SearchBox.Text.Trim();
            string searchType = (SearchTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            DataSet bookDataSet = bookCatalog.GetBookInfo();
            if (bookDataSet.Tables.Contains("Books"))
            {
                DataTable booksTable = bookDataSet.Tables["Books"];
                string filterExpression = string.IsNullOrEmpty(searchQuery) ?
                    "" : $"{searchType} LIKE '%{searchQuery}%'";

                booksTable.DefaultView.RowFilter = filterExpression;
                ResultsGrid.ItemsSource = booksTable.DefaultView;
            }
        }

        private void SearchBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            // Implement real-time search if needed
        }
    }
} 