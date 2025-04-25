using System;
using ConsoleApp1.Menus;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] MainMenu = { "New Game", "Load Game", "Players", "Exit" };
            int selectedIndex = 0;

            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to Chess Games");
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
            } while (key != ConsoleKey.Enter || selectedIndex != 3); // Fixed exit condition
        }

        static void HandleMenuSelection(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    Console.WriteLine("New Game selected...");
                    // Implement NewGame.StartNewGame() or replace with actual logic
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    break;
                case 1:
                    Console.WriteLine("Load Game selected...");
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    break;
                case 2:
                    PlayersMenuHandler.ShowPlayersMenu(); // Fixed method call
                    break;
                case 3:
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}