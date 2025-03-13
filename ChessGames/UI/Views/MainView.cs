using System;

namespace ChessGames.UI.Views
{
    public class MainView
    {
        public void DisplayWelcomeMessage()
        {
            Console.WriteLine("Welcome to the Chess Games Application!");
        }

        public void DisplayMenu()
        {
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Start a new game");
            Console.WriteLine("2. Load a saved game");
            Console.WriteLine("3. Exit");
        }

        public void GetUserInput()
        {
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();
            HandleUserChoice(choice);
        }

        private void HandleUserChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Starting a new game...");
                    // Logic to start a new game
                    break;
                case "2":
                    Console.WriteLine("Loading a saved game...");
                    // Logic to load a saved game
                    break;
                case "3":
                    Console.WriteLine("Exiting the application...");
                    // Logic to exit the application
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    GetUserInput();
                    break;
            }
        }
    }
}