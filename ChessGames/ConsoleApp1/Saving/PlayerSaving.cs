using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConsoleApp1.Saving
{
    public class PlayerSaving
    {
        // Use an explicit absolute path for the Saves folder
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saving", "Saves", "players.json");
        private List<Player> players;

        public PlayerSaving()
        {
            players = new List<Player>();
            EnsureSavesDirectoryExists();
            LoadPlayers();
        }

        public void AddPlayer(string name)
        {
            if (players.Exists(p => p.Name == name))
            {
                Console.WriteLine("A player with this name or nickname already exists.");
                return;
            }

            players.Add(new Player { Name = name, ChessWins = 0, CheckersWins = 0 });
            SavePlayers();
            Console.WriteLine("Player added successfully.");
        }

        public void RecordWin(string name, string gameMode)
        {
            var player = players.Find(p => p.Name == name);
            if (player == null)
            {
                Console.WriteLine("Player not found.");
                return;
            }

            if (gameMode.ToLower() == "chess")
            {
                player.ChessWins++;
            }
            else if (gameMode.ToLower() == "checkers")
            {
                player.CheckersWins++;
            }
            else
            {
                Console.WriteLine("Invalid game mode.");
                return;
            }

            SavePlayers();
            Console.WriteLine("Win recorded successfully.");
        }

        public void DisplayPlayers()
        {
            Console.WriteLine("Players:");
            foreach (var player in players)
            {
                Console.WriteLine($"Name: {player.Name}, Chess Wins: {player.ChessWins}, Checkers Wins: {player.CheckersWins}");
            }
        }

        private void LoadPlayers()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    players = JsonSerializer.Deserialize<List<Player>>(json) ?? new List<Player>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while loading players: {ex.Message}");
                    players = new List<Player>();
                }
            }
            else
            {
                players = new List<Player>();
            }
        }

        private void SavePlayers()
        {
            try
            {
                string json = JsonSerializer.Serialize(players, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving players: {ex.Message}");
            }
        }

        private void EnsureSavesDirectoryExists()
        {
            string directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private class Player
        {
            public string Name { get; set; }
            public int ChessWins { get; set; }
            public int CheckersWins { get; set; }
        }
    }
}