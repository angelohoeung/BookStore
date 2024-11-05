using System.Data;
using System.Windows;
using System.Windows.Controls;
using BookStoreLIB;

namespace BookStoreGUI.Views
{
    public partial class Search : Window
    {
        private SearchBooks searchBooks;

        public Search()
        {
            InitializeComponent();
            searchBooks = new SearchBooks();
            LoadInitialData();
        }

        private void LoadInitialData()
        {
            DataTable books = searchBooks.Search("", "Title");
            ResultsGrid.ItemsSource = books.DefaultView;
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
            DataTable books = searchBooks.Search(searchQuery, searchType);
            ResultsGrid.ItemsSource = books.DefaultView;
        }

        private void SearchBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            // Implement real-time search if needed
        }
    }
}