﻿using System;
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
    /// Interaction logic for EditDialog.xaml
    /// </summary>
    public partial class EditDialog : Window {
        public string InputText => inputTextBox.Text;

        public EditDialog() {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
            this.Close();
        }
    }
}
