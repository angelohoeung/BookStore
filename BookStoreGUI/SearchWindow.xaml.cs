using System.Windows;
using BookStoreGUI.Views;

namespace BookStoreGUI
{
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Search());
        }
    }
} 