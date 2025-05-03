using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleApp1.Saving
{
    public class GameSaving
    {
        private const string SaveFilePath = "../../../Saving/Saves/game.json";

        public void SaveGame(GameState gameState)
        {
            string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SaveFilePath, json);
        }

        public GameState LoadGame()
        {
            if (!File.Exists(SaveFilePath)) return null;
            string json = File.ReadAllText(SaveFilePath);
            return JsonSerializer.Deserialize<GameState>(json);
        }
    }

    public class GameState
    {
        public string[,] Board { get; set; }
        public List<string> WhiteCaptured { get; set; }
        public List<string> BlackCaptured { get; set; }
        public int WhiteTime { get; set; }
        public int BlackTime { get; set; }
        public bool IsWhiteTurn { get; set; }
        public string WhitePlayerName { get; set; }
        public string BlackPlayerName { get; set; }
        public string GameType { get; set; } // "Chess" or "Checkers"
    }
}