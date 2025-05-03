using System;

namespace ConsoleApp1.Games
{
    public class CheckersLogic
    {
        private const int BoardSize = 8;
        private string[,] board;
        private bool isWhiteTurn = true; // White starts first

        public CheckersLogic()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            board = new string[BoardSize, BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if ((row + col) % 2 != 0)
                    {
                        if (row < 3)
                            board[row, col] = "B";
                        else if (row > 4)
                            board[row, col] = "W";
                        else
                            board[row, col] = null;
                    }
                }
            }
        }

        public string GetPiece(int row, int col)
        {
            if (!IsWithinBounds(row, col)) return null;
            return board[row, col];
        }

        public void SetPiece(int row, int col, string piece)
        {
            if (IsWithinBounds(row, col))
            {
                board[row, col] = piece;
            }
        }

        public bool MovePiece(int startRow, int startCol, int endRow, int endCol)
        {
            if (!IsValidMove(startRow, startCol, endRow, endCol))
                return false;

            board[endRow, endCol] = board[startRow, startCol];
            board[startRow, startCol] = null;

            // Toggle the turn after a successful move
            isWhiteTurn = !isWhiteTurn;

            return true;
        }

        private bool IsValidMove(int startRow, int startCol, int endRow, int endCol)
        {
            if (!IsWithinBounds(startRow, startCol) || !IsWithinBounds(endRow, endCol))
                return false;

            string piece = board[startRow, startCol];
            if (piece == null || board[endRow, endCol] != null)
                return false;

            int rowDiff = endRow - startRow;
            int colDiff = Math.Abs(endCol - startCol);
            bool isKing = piece == "WK" || piece == "BK";

            if (colDiff != Math.Abs(rowDiff) || (Math.Abs(rowDiff) != 1 && Math.Abs(rowDiff) != 2))
                return false;

            // Regular move
            if (Math.Abs(rowDiff) == 1)
            {
                if (!isKing)
                {
                    if (piece == "W" && rowDiff >= 0) return false;
                    if (piece == "B" && rowDiff <= 0) return false;
                }
                return true;
            }

            // Jump move
            if (Math.Abs(rowDiff) == 2)
            {
                int midRow = (startRow + endRow) / 2;
                int midCol = (startCol + endCol) / 2;
                string midPiece = board[midRow, midCol];

                if (midPiece == null)
                    return false;

                if ((piece.StartsWith("W") && midPiece.StartsWith("B")) ||
                    (piece.StartsWith("B") && midPiece.StartsWith("W")))
                    return true;
            }

            return false;
        }

        private bool IsWithinBounds(int row, int col)
        {
            return row >= 0 && row < BoardSize && col >= 0 && col < BoardSize;
        }

        public string[,] GetBoard()
        {
            return (string[,])board.Clone(); // optional helper
        }
    }
}
