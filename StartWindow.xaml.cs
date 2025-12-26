using System;
using System.Windows;
using Microsoft.Win32; // Добавьте эту директиву

namespace sem3laba3
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            MarketWindow marketWindow = new MarketWindow();
            marketWindow.Show();
            this.Close();
        }

        private void LoadGameButton_Click(object sender, RoutedEventArgs e)
        {
            LoadGameWindow loadWindow = new LoadGameWindow();
            loadWindow.Owner = this;
            loadWindow.ShowDialog();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}