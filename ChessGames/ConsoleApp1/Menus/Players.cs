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
            string[] MainMenu = { "See players", "Add player", "Go Back" };
            int selectedIndex = 0;

            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Players Menu:");
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
            } while (!(key == ConsoleKey.Enter && selectedIndex == 2));
        }

        static void HandleMenuSelection(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    Console.Clear();
                    PlayerSaving playerSaving = new PlayerSaving();
                    playerSaving.DisplayPlayers();
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
                    Console.WriteLine("Returning to the main menu...");
                    break;
            }
        }
    }
}