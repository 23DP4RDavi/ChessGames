using System;

namespace ChessGames.UI.Views
{
    public class MainView
    {
        public void DisplayWelcomeMessage()
        {
            Console.WriteLine("Welcome to Chess Games!");
        }

        public void DisplayMenu()
        {
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. Exit");
        }
    }
}