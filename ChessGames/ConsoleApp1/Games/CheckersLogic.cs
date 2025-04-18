using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Games
{
    public class CheckersLogic
    {
        private const int BoardSize = 8;
        private string[,] board;

        public CheckersLogic()
        {
            InitializeBoard();
        }

        // Initialize the checkers board with pieces
        private void InitializeBoard()
        {
            board = new string[BoardSize, BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if ((row + col) % 2 != 0) // Only place pieces on dark tiles
                    {
                        if (row < 3)
                        {
                            board[row, col] = "B"; // Black pieces
                        }
                        else if (row > 4)
                        {
                            board[row, col] = "W"; // White pieces
                        }
                        else
                        {
                            board[row, col] = null; // Empty tile
                        }
                    }
                }
            }
        }

        // Display the board in the console (for debugging)
        public void DisplayBoard()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Console.Write(board[row, col] == null ? "." : board[row, col]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        // Move a piece from one position to another
        public bool MovePiece(int startRow, int startCol, int endRow, int endCol)
        {
            // Validate the move
            if (!IsValidMove(startRow, startCol, endRow, endCol))
            {
                return false;
            }

            // Perform the move
            board[endRow, endCol] = board[startRow, startCol];
            board[startRow, startCol] = null;

            // Check for captures
            if (Math.Abs(startRow - endRow) == 2) // A jump was made
            {
                int capturedRow = (startRow + endRow) / 2;
                int capturedCol = (startCol + endCol) / 2;
                board[capturedRow, capturedCol] = null; // Remove the captured piece
            }

            // Check for promotion to king
            if (endRow == 0 && board[endRow, endCol] == "W")
            {
                board[endRow, endCol] = "WK"; // White king
            }
            else if (endRow == BoardSize - 1 && board[endRow, endCol] == "B")
            {
                board[endRow, endCol] = "BK"; // Black king
            }

            return true;
        }

        // Validate if a move is legal
        private bool IsValidMove(int startRow, int startCol, int endRow, int endCol)
        {
            // Check if the start and end positions are within bounds
            if (!IsWithinBounds(startRow, startCol) || !IsWithinBounds(endRow, endCol))
            {
                return false;
            }

            // Check if the start position has a piece
            if (board[startRow, startCol] == null)
            {
                return false;
            }

            // Check if the end position is empty
            if (board[endRow, endCol] != null)
            {
                return false;
            }

            // Check if the move is diagonal
            int rowDiff = Math.Abs(startRow - endRow);
            int colDiff = Math.Abs(startCol - endCol);
            if (rowDiff != colDiff || (rowDiff != 1 && rowDiff != 2))
            {
                return false;
            }

            // Check for normal moves
            if (rowDiff == 1)
            {
                // Ensure the piece is moving forward
                if (board[startRow, startCol] == "W" && endRow >= startRow) return false;
                if (board[startRow, startCol] == "B" && endRow <= startRow) return false;
            }

            // Check for captures
            if (rowDiff == 2)
            {
                int capturedRow = (startRow + endRow) / 2;
                int capturedCol = (startCol + endCol) / 2;

                // Ensure there is an opponent's piece to capture
                if (board[startRow, startCol] == "W" && board[capturedRow, capturedCol] != "B" && board[capturedRow, capturedCol] != "BK")
                {
                    return false;
                }
                if (board[startRow, startCol] == "B" && board[capturedRow, capturedCol] != "W" && board[capturedRow, capturedCol] != "WK")
                {
                    return false;
                }
            }

            return true;
        }

        // Check if a position is within the bounds of the board
        private bool IsWithinBounds(int row, int col)
        {
            return row >= 0 && row < BoardSize && col >= 0 && col < BoardSize;
        }
    }
}