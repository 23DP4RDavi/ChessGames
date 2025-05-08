using System;

namespace ConsoleApp1.Games
{
    public class CheckersLogic
    {
        private const int BoardSize = 8;
        private string[,] board;
        private bool isWhiteTurn = true;

        // For chain jumps
        public bool MustContinueJump { get; private set; } = false;
        public (int row, int col)? JumpingPiece { get; private set; } = null;

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
                    else
                    {
                        board[row, col] = null;
                    }
                }
            }
        }

        public string[,] Board => board;

        public bool MovePiece(int startRow, int startCol, int endRow, int endCol)
        {
            if (!IsValidMove(startRow, startCol, endRow, endCol))
                return false;

            string piece = board[startRow, startCol];

            // Handle jump (capture)
            bool wasJump = Math.Abs(endRow - startRow) == 2;
            if (wasJump)
            {
                int midRow = (startRow + endRow) / 2;
                int midCol = (startCol + endCol) / 2;
                board[midRow, midCol] = null; // Remove captured piece
            }

            board[endRow, endCol] = piece;
            board[startRow, startCol] = null;

            // Promote to king
            if (piece == "W" && endRow == 0)
                board[endRow, endCol] = "WK";
            else if (piece == "B" && endRow == BoardSize - 1)
                board[endRow, endCol] = "BK";

            // Chain jump logic
            if (wasJump && CanJumpAgain(endRow, endCol))
            {
                MustContinueJump = true;
                JumpingPiece = (endRow, endCol);
            }
            else
            {
                MustContinueJump = false;
                JumpingPiece = null;
                isWhiteTurn = !isWhiteTurn;
            }

            return true;
        }

        private bool CanJumpAgain(int row, int col)
        {
            string piece = board[row, col];
            if (piece == null) return false;
            int[] dr = { -2, -2, 2, 2 };
            int[] dc = { -2, 2, -2, 2 };

            for (int i = 0; i < 4; i++)
            {
                int newRow = row + dr[i];
                int newCol = col + dc[i];
                if (IsValidMove(row, col, newRow, newCol))
                    return true;
            }
            return false;
        }

        private bool IsValidMove(int startRow, int startCol, int endRow, int endCol)
        {
            if (!IsWithinBounds(startRow, startCol) || !IsWithinBounds(endRow, endCol))
                return false;

            // Only allow moves on black tiles
            if ((endRow + endCol) % 2 == 0)
                return false;

            string piece = board[startRow, startCol];
            if (piece == null || board[endRow, endCol] != null)
                return false;

            // If chain jump is required, only allow moves for that piece
            if (MustContinueJump && JumpingPiece != null && (startRow != JumpingPiece.Value.row || startCol != JumpingPiece.Value.col))
                return false;

            int rowDiff = endRow - startRow;
            int colDiff = Math.Abs(endCol - startCol);
            bool isKing = piece == "WK" || piece == "BK";

            if (colDiff != Math.Abs(rowDiff) || (Math.Abs(rowDiff) != 1 && Math.Abs(rowDiff) != 2))
                return false;

            // Regular move
            if (Math.Abs(rowDiff) == 1)
            {
                if (MustContinueJump) return false; // Can't make a regular move if must continue jumping
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

        public bool IsWhiteTurn => isWhiteTurn;

        public bool IsWin(out string winner)
        {
            bool whiteExists = false, blackExists = false;
            bool whiteCanMove = false, blackCanMove = false;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    string piece = board[row, col];
                    if (piece == null) continue;
                    if (piece.StartsWith("W"))
                    {
                        whiteExists = true;
                        if (!whiteCanMove && HasAnyValidMove(row, col))
                            whiteCanMove = true;
                    }
                    else if (piece.StartsWith("B"))
                    {
                        blackExists = true;
                        if (!blackCanMove && HasAnyValidMove(row, col))
                            blackCanMove = true;
                    }
                }
            }

            if (!whiteExists || !whiteCanMove)
            {
                winner = "B";
                return true;
            }
            if (!blackExists || !blackCanMove)
            {
                winner = "W";
                return true;
            }
            winner = null;
            return false;
        }

        private bool HasAnyValidMove(int row, int col)
        {
            for (int dr = -2; dr <= 2; dr++)
            {
                for (int dc = -2; dc <= 2; dc++)
                {
                    if (dr == 0 && dc == 0) continue;
                    int newRow = row + dr;
                    int newCol = col + dc;
                    if (IsValidMove(row, col, newRow, newCol))
                        return true;
                }
            }
            return false;
        }
    }
}
