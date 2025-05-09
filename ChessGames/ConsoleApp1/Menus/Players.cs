using System;
using ConsoleApp1.Menus.Players;
using ConsoleApp1.Saving;
using ConsoleApp1;

namespace ConsoleApp1.Menus
{
    public class PlayersMenuHandler
    {
        public static void ShowPlayersMenu()
        {
            string[] MainMenu = { "See players", "Add player", "Delete player", "Go Back" };
            int selectedIndex = 0;

            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine(@"    
    ____  __    _____  ____________     __  __________   ____  __    
   / __ \/ /   /   \ \/ / ____/ __ \   /  |/  / ____/ | / / / / /  _ 
  / /_/ / /   / /| |\  / __/ / /_/ /  / /|_/ / __/ /  |/ / / / /  (_)
 / ____/ /___/ ___ |/ / /___/ _, _/  / /  / / /___/ /|  / /_/ /  _   
/_/   /_____/_/  |_/_/_____/_/ |_|  /_/  /_/_____/_/ |_/\____/  (_) 
___________________________________________________________________
");
                for (int i = 0; i < MainMenu.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine("- " + MainMenu[i]);
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? MainMenu.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == MainMenu.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        HandleMenuSelection(selectedIndex);
                        break;
                }
            } while (!(key == ConsoleKey.Enter && selectedIndex == MainMenu.Length - 1));
        }

        static void HandleMenuSelection(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    Console.Clear();
                    ShowPlayersWithFilter();
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("Add player selected...");
                    PlayerCreate.CreatePlayer();
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                    break;
                case 2:
                    DeletePlayerByArrowKeys();
                    break;
                case 3:
                    Console.WriteLine("Returning to the main menu...");
                    break;
            }
        }

        static void ShowPlayersWithFilter()
        {
            string[] filters = { "Alphabetical", "Chess Wins", "Checkers Wins" };
            int selectedFilter = 0;
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Select a filter for displaying players:\n");
                for (int i = 0; i < filters.Length; i++)
                {
                    if (i == selectedFilter)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(filters[i]);
                    Console.ResetColor();
                }
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedFilter = (selectedFilter == 0) ? filters.Length - 1 : selectedFilter - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedFilter = (selectedFilter == filters.Length - 1) ? 0 : selectedFilter + 1;
                        break;
                    case ConsoleKey.Enter:
                        DisplayFilteredPlayers(selectedFilter);
                        return;
                }
            } while (true);
        }

        static void DisplayFilteredPlayers(int filterIndex)
        {
            PlayerSaving playerSaving = new PlayerSaving();
            var players = playerSaving.GetAllPlayers();

            switch (filterIndex)
            {
                case 0: // Alphabetical
                    players.Sort((a, b) => a.Name.CompareTo(b.Name));
                    break;
                case 1: // Chess Wins
                    players.Sort((a, b) => b.ChessWins.CompareTo(a.ChessWins));
                    break;
                case 2: // Checkers Wins
                    players.Sort((a, b) => b.CheckersWins.CompareTo(a.CheckersWins));
                    break;
            }

            Console.WriteLine("Players:");
            foreach (var player in players)
            {
                Console.WriteLine($"Name: {player.Name}, Chess Wins: {player.ChessWins}, Checkers Wins: {player.CheckersWins}");
            }
        }

        static void DeletePlayerByArrowKeys()
        {
            PlayerSaving playerSaving = new PlayerSaving();
            var players = playerSaving.GetPlayerNames();
            if (players.Count == 0)
            {
                Console.WriteLine("No players to delete.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                return;
            }

            int selected = 0;
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Select a player to delete (Enter to confirm, Esc to cancel):\n");
                for (int i = 0; i < players.Count; i++)
                {
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(players[i]);
                    Console.ResetColor();
                }
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selected = (selected == 0) ? players.Count - 1 : selected - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selected = (selected == players.Count - 1) ? 0 : selected + 1;
                        break;
                    case ConsoleKey.Enter:
                        playerSaving.DeletePlayer(players[selected]);
                        Console.WriteLine($"Player '{players[selected]}' deleted. Press any key to return...");
                        Console.ReadKey();
                        return;
                    case ConsoleKey.Escape:
                        return;
                }
            } while (true);
        }
    }
}