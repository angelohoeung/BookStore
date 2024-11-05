using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using BookStoreLIB;

namespace BookStoreGUI.Views
{
    public partial class Search : Page
    {
        private DataSet bookDataSet;
        private BookCatalog bookCatalog;

        public Search()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            try
            {
                bookCatalog = new BookCatalog();
                bookDataSet = bookCatalog.GetBookInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Database Error");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                PerformSearch();
            }
        }

        private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                PerformSearch();
            }
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                PerformSearch();
            }
        }

        private void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                PerformSearch();
            }
        }

        private void PerformSearch()
        {
            if (bookDataSet?.Tables[0] == null) return;

            var searchText = SearchBox.Text?.Trim().ToLower() ?? "";
            var dt = bookDataSet.Tables[0];
            DataView dv = dt.DefaultView;

            // Apply search filter
            if (!string.IsNullOrEmpty(searchText))
            {
                var searchType = ((ComboBoxItem)SearchTypeComboBox.SelectedItem)?.Content.ToString();
                switch (searchType)
                {
                    case "Title":
                        dv.RowFilter = $"LOWER(Title) LIKE '%{searchText}%'";
                        break;
                    case "Author":
                        dv.RowFilter = $"LOWER(Author) LIKE '%{searchText}%'";
                        break;
                    case "Genre":
                        dv.RowFilter = $"LOWER(Genre) LIKE '%{searchText}%'";
                        break;
                    case "Keywords":
                        dv.RowFilter = $"LOWER(Title) LIKE '%{searchText}%' OR LOWER(Author) LIKE '%{searchText}%' OR LOWER(Genre) LIKE '%{searchText}%'";
                        break;
                    default:
                        dv.RowFilter = "";
                        break;
                }
            }
            else
            {
                dv.RowFilter = "";
            }

            // Apply sorting
            var sortBy = ((ComboBoxItem)SortByComboBox.SelectedItem)?.Content.ToString();
            var sortOrder = ((ComboBoxItem)SortOrderComboBox.SelectedItem)?.Content.ToString() == "Increasing" ? "ASC" : "DESC";

            switch (sortBy)
            {
                case "Price":
                    dv.Sort = $"Price {sortOrder}";
                    break;
                case "Rating":
                    dv.Sort = $"Rating {sortOrder}";
                    break;
                case "Release Date":
                    dv.Sort = $"PublishDate {sortOrder}";
                    break;
                default:
                    dv.Sort = "";
                    break;
            }

            // Update UI
            SearchResultsListView.ItemsSource = dv;
            UpdateResultsCount(dv.Count);
        }

        private void UpdateResultsCount(int count)
        {
            ResultsCount.Text = $"Showing {count} results";
            NoResultsMessage.Visibility = count > 0 ? Visibility.Collapsed : Visibility.Visible;
            SearchResultsListView.Visibility = count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}