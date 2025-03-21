using System;
using ChessGames.UI;
using System.Windows.Forms;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] menuItems = { "New Game", "Load Game", "Exit"};
            int selectedIndex = 0;

            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to Chess Games");
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine("- " + menuItems[i]);
                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? menuItems.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == menuItems.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Enter:
                        HandleMenuSelection(selectedIndex);
                        break;
                }
            } while (key != ConsoleKey.Enter);
        }

        static void HandleMenuSelection(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    ChessBoardForm form = new ChessBoardForm();
                    form.ShowDialog();
                    break;
                case 1:
                    Console.WriteLine("Load Game selected");
                    // Add logic to load a game
                    break;
                case 2:
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                    break;
            }
        }
    }
}