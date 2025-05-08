using System;
using ConsoleApp1.Saving;
using ConsoleApp1.Games;
using ChessGames.UI;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Menus
{
    public class LoadGameMenu
    {
        public static void Show()
        {
            GameSaving gameSaving = new GameSaving();
            GameState state = gameSaving.LoadGame();

            if (state == null)
            {
                Console.WriteLine("No saved game found.");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
                return;
            }

            object gameLogic;
            if (state.GameType == "Chess")
            {
                var chess = new ChessLogic();
                ApplyBoard(chess.Board, state.Board);
                gameLogic = chess;
            }
            else if (state.GameType == "Checkers")
            {
                var checkers = new CheckersLogic();
                ApplyBoard(checkers.Board, state.Board);
                gameLogic = checkers;
            }
            else
            {
                Console.WriteLine("Unknown game type in save.");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
                return;
            }

            // Launch the WinForms game board with loaded state
            Application.Run(new GameBoardForm(
                gameLogic,
                state.WhitePlayerName,
                state.BlackPlayerName,
                Math.Max(state.WhiteTime, state.BlackTime) / 60 // fallback: use max time as minutes
            ));
        }

        private static void ApplyBoard(string[,] board, List<List<string>> saved)
        {
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                    board[i, j] = saved[i][j];
        }
    }
}