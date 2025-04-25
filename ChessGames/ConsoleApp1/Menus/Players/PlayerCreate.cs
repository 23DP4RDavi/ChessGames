using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp1.Saving;

namespace ConsoleApp1.Menus.Players
{
    public class PlayerCreate
    {
        public static void CreatePlayer()
        {
            Console.WriteLine("Enter player name:");
            string name = Console.ReadLine();
            
            PlayerSaving playerSaving = new PlayerSaving();
            playerSaving.AddPlayer(name);

            Console.WriteLine("Player created successfully. Press any key to return to the menu.");
            Console.ReadKey();
        }
    }
}