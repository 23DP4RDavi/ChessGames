using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Games
{
    public class ChessLogic
    {
        public string[,] Board { get; private set; }
        private const int GridSize = 8;

        public ChessLogic()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            Board = new string[GridSize, GridSize];

            // Initialize pawns
            for (int i = 0; i < GridSize; i++)
            {
                Board[1, i] = "WPawn"; // White pawns
                Board[6, i] = "BPawn"; // Black pawns
            }

            // Initialize other pieces
            string[] whitePieces = { "WRook", "WKnight", "WBishop", "WQueen", "WKing", "WBishop", "WKnight", "WRook" };
            string[] blackPieces = { "BRook", "BKnight", "BBishop", "BQueen", "BKing", "BBishop", "BKnight", "BRook" };

            for (int i = 0; i < GridSize; i++)
            {
                Board[0, i] = whitePieces[i]; // White pieces
                Board[7, i] = blackPieces[i]; // Black pieces
            }
        }

        public bool MovePiece(int startX, int startY, int endX, int endY)
        {
            if (IsMoveValid(startX, startY, endX, endY))
            {
                Board[endX, endY] = Board[startX, startY];
                Board[startX, startY] = null;
                return true;
            }
            return false;
        }

        private bool IsMoveValid(int startX, int startY, int endX, int endY)
        {
            // Basic validation: Ensure the move is within bounds and not moving to the same spot
            if (startX < 0 || startX >= GridSize || startY < 0 || startY >= GridSize ||
                endX < 0 || endX >= GridSize || endY < 0 || endY >= GridSize ||
                (startX == endX && startY == endY))
            {
                return false;
            }

            // Add more complex chess rules here (e.g., piece-specific movement)
            return Board[startX, startY] != null;
        }
    }
}