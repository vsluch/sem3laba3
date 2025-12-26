using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace sem3laba3
{
    public class SaveManager
    {
        private readonly string _savesDirectory;

        public SaveManager()
        {
            _savesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saves");

            if (!Directory.Exists(_savesDirectory))
            {
                Directory.CreateDirectory(_savesDirectory);
            }
        }

        public List<SaveFileInfo> GetSaveFiles()
        {
            var saveFiles = new List<SaveFileInfo>();

            if (!Directory.Exists(_savesDirectory))
                return saveFiles;

            var files = Directory.GetFiles(_savesDirectory, "*.json");

            foreach (var file in files)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    var saveInfo = new SaveFileInfo
                    {
                        FilePath = file,
                        FileName = Path.GetFileNameWithoutExtension(file),
                        LastModified = fileInfo.LastWriteTime,
                        Size = fileInfo.Length
                    };

                    // Читаем базовую информацию о сохранении без полной загрузки
                    var json = File.ReadAllText(file);
                    var gameState = JsonConvert.DeserializeObject<GameState>(json);

                    if (gameState != null)
                    {
                        saveInfo.TurnNumber = gameState.TurnNumber;
                        saveInfo.Player1Name = $"Игрок 1 ({gameState.Player1State.Hand.Count} карт)";
                        saveInfo.Player2Name = $"Игрок 2 ({gameState.Player2State.Hand.Count} карт)";
                    }

                    saveFiles.Add(saveInfo);
                }
                catch
                {
                    // Пропускаем поврежденные файлы
                }
            }

            return saveFiles.OrderByDescending(s => s.LastModified).ToList();
        }

        public void SaveGame(Game game, string fileName)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = $"save_{DateTime.Now:yyyyMMdd_HHmmss}";

            if (!fileName.EndsWith(".json"))
                fileName += ".json";

            string filePath = Path.Combine(_savesDirectory, fileName);
            game.SaveGame(filePath);
        }

        public Game LoadGame(string filePath)
        {
            return Game.LoadGame(filePath);
        }

        public bool DeleteSave(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    public class SaveFileInfo
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime LastModified { get; set; }
        public long Size { get; set; }
        public int TurnNumber { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public string DisplayName => $"{FileName} (Ход: {TurnNumber})";
        public string SizeFormatted => $"{Size / 1024} KB";
    }
}