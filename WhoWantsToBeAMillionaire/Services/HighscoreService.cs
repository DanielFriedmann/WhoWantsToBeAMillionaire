using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

using WhoWantsToBeAMillionaire.Models;

namespace WhoWantsToBeAMillionaire.Services
{
    public class HighscoreService
    {
        private readonly string _filePath = "Data/highscores.json";

        public List<HighScoreEntry> LoadHighScores()
        {
            if (!File.Exists(_filePath))
            {
                return new List<HighScoreEntry>();
            }
            string jsonString = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<HighScoreEntry>>(jsonString) ?? new List<HighScoreEntry>();
        }

        public void SaveHighscore(HighScoreEntry entry)
        {
            List<HighScoreEntry> highscores = LoadHighScores();
            highscores.Add(entry);

            highscores = highscores
                         .OrderByDescending(h => h.Level)
                         .Take(10)
                         .ToList();

            string jsonString = JsonSerializer.Serialize(highscores, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonString);
        }
    }
}
