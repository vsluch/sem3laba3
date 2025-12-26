using System;
using System.Windows;

namespace sem3laba3
{
    public partial class LoadGameWindow : Window
    {
        private SaveManager _saveManager;

        public LoadGameWindow()
        {
            InitializeComponent();
            _saveManager = new SaveManager();
            LoadSaveFiles();
        }

        private void LoadSaveFiles()
        {
            try
            {
                var saveFiles = _saveManager.GetSaveFiles();
                SavesListView.ItemsSource = saveFiles;

                if (saveFiles.Count > 0)
                {
                    SavesListView.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка сохранений: {ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSave = SavesListView.SelectedItem as SaveFileInfo;
            if (selectedSave == null)
            {
                MessageBox.Show("Выберите сохранение для удаления",
                               "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить сохранение '{selectedSave.FileName}'?",
                                        "Подтверждение",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_saveManager.DeleteSave(selectedSave.FilePath))
                {
                    LoadSaveFiles();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить сохранение",
                                   "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSaveFiles();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSave = SavesListView.SelectedItem as SaveFileInfo;
            if (selectedSave == null)
            {
                MessageBox.Show("Выберите сохранение для загрузки",
                               "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Game loadedGame = _saveManager.LoadGame(selectedSave.FilePath);

                if (loadedGame == null)
                {
                    MessageBox.Show("Не удалось загрузить выбранное сохранение",
                                   "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MainWindow mainWindow = new MainWindow(loadedGame);
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке игры: {ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}