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
    /// Interaction logic for ManagerDashboard.xaml
    /// </summary>
    public partial class ManagerDashboard : Window {
        private DashboardManager dashboardManager;
        int userId;
        public ManagerDashboard(int userId) {
            InitializeComponent();
            this.userId = userId;
            LoadData();
        }

        private void LoadData() {
            dashboardManager = new DashboardManager();
            if (dashboardManager.Display(userId)) {
                TopDataGrid.ItemsSource = new List<BookSales> { dashboardManager.TopBook };
                SoldDataGrid.ItemsSource = dashboardManager.BookInfo;
                StockDataGrid.ItemsSource = dashboardManager.BookInventory;
            } else {
                MessageBox.Show("Failed to load Dashboard. Are you a Manager?");
            }
        }

        private void closeButtonClick(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }
    }
}
