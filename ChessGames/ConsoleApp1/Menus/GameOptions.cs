using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using ConsoleApp1.Games;
using ChessGames.UI;
using System.Windows.Forms;

namespace ConsoleApp1.Menus
{
    public static class GameOptions
    {
        private static readonly PlayerManager _playerManager = new PlayerManager("../../../Saving/Saves/players.json");
        private static readonly GameConfiguration _gameConfiguration = new GameConfiguration();

        public static void ConfigureGame()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Game Options!");

            // Load players
            List<Player> players = _playerManager.LoadPlayers();
            if (players.Count == 0)
            {
                Console.WriteLine("No players available. Please add players and try again.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            Player player1 = _playerManager.SelectPlayer(players, "Select Player 1:");
            Player player2 = _playerManager.SelectPlayer(players, "Select Player 2:");

            // Configure game settings
            int timeInMinutes = _gameConfiguration.GetTimePerPlayer();
            string selectedGame = SelectGameWithArrowKeys();

            // Display configuration
            Console.Clear();
            Console.WriteLine("Game Configuration:");
            Console.WriteLine($"Player 1: {player1.Name}");
            Console.WriteLine($"Player 2: {player2.Name}");
            Console.WriteLine($"Time per player: {timeInMinutes} minutes");
            Console.WriteLine($"Selected Game: {selectedGame}");

            Console.WriteLine("\nPress any key to start the game...");
            Console.ReadKey();

            // Launch the selected game
            LaunchGame(selectedGame, timeInMinutes, player1, player2);
        }

        private static string SelectGameWithArrowKeys()
        {
            string[] games = { "Chess", "Checkers" };
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("Select a game:");
                for (int i = 0; i < games.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine($"- {games[i]}");
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? games.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == games.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                }
            } while (key != ConsoleKey.Enter);

            return games[selectedIndex];
        }

        private static void LaunchGame(string selectedGame, int timeInMinutes, Player player1, Player player2)
        {
            object gameLogic;

            if (selectedGame == "Chess")
            {
                gameLogic = new ChessLogic();
            }
            else if (selectedGame == "Checkers")
            {
                gameLogic = new CheckersLogic();
            }
            else
            {
                Console.WriteLine("Invalid game selected.");
                return;
            }

            // Launch the WinForms game board
            Application.Run(new GameBoardForm(gameLogic, player1.Name, player2.Name, timeInMinutes));
        }
    }

    public class PlayerManager
    {
        private readonly string _filePath;

        public PlayerManager(string filePath)
        {
            _filePath = filePath;
        }

        public List<Player> LoadPlayers()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"JSON file '{_filePath}' not found. No players loaded.");
                return new List<Player>();
            }

            try
            {
                string jsonContent = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Player>>(jsonContent) ?? new List<Player>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return new List<Player>();
            }
        }

        public Player SelectPlayer(List<Player> players, string prompt)
        {
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine(prompt);
                Console.WriteLine("Use Up/Down arrows to select a player and press Enter:");

                for (int i = 0; i < players.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine($"- {players[i].Name}");
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? players.Count - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == players.Count - 1) ? 0 : selectedIndex + 1;
                        break;
                }
            } while (key != ConsoleKey.Enter);

            return players[selectedIndex];
        }
    }

    public class GameConfiguration
    {
        public int GetTimePerPlayer()
        {
            Console.Write("Enter time (in minutes) for each player: ");
            int timeInMinutes;
            while (!int.TryParse(Console.ReadLine(), out timeInMinutes) || timeInMinutes <= 0)
            {
                Console.Write("Invalid input. Please enter a positive number: ");
            }
            return timeInMinutes;
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public int ChessWins { get; set; }
        public int CheckersWins { get; set; }
    }
}