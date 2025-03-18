using System;
using ChessGames.UI.Views;
using System.Windows.Forms;

namespace ConsoleApp1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainView mainView = new MainView();
            mainView.DisplayWelcomeMessage();
            mainView.DisplayMenu();
            Application.Run(new Form());
        }
    }
}