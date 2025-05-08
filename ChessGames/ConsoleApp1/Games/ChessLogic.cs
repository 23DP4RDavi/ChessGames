using System;
using System.Collections.Generic;

namespace ConsoleApp1.Games
{
    public class ChessLogic
    {
        private string[,] board;
        public string[,] Board => board;
        private const int GridSize = 8;
        public (int x, int y) WhiteKingPosition { get; private set; }
        public (int x, int y) BlackKingPosition { get; private set; }
        private bool isWhiteTurn = true;
        private (int x, int y)? lastMove;

        // Castling flags
        private bool whiteKingMoved = false, blackKingMoved = false;
        private bool whiteKingsideRookMoved = false, whiteQueensideRookMoved = false;
        private bool blackKingsideRookMoved = false, blackQueensideRookMoved = false;

        // Threefold repetition
        private Dictionary<string, int> positionCounts = new Dictionary<string, int>();

        public ChessLogic()
        {
            InitializeBoard();
            UpdatePositionCounts();
        }

        private void InitializeBoard()
        {
            board = new string[GridSize, GridSize];

            for (int i = 0; i < GridSize; i++)
            {
                board[1, i] = "BPawn";
                board[6, i] = "WPawn";
            }

            board[0, 0] = "BRook"; board[0, 7] = "BRook";
            board[7, 0] = "WRook"; board[7, 7] = "WRook";
            board[0, 1] = "BKnight"; board[0, 6] = "BKnight";
            board[7, 1] = "WKnight"; board[7, 6] = "WKnight";
            board[0, 2] = "BBishop"; board[0, 5] = "BBishop";
            board[7, 2] = "WBishop"; board[7, 5] = "WBishop";
            board[0, 3] = "BQueen"; board[7, 3] = "WQueen";
            board[0, 4] = "BKing"; board[7, 4] = "WKing";

            WhiteKingPosition = (7, 4);
            BlackKingPosition = (0, 4);

            whiteKingMoved = blackKingMoved = false;
            whiteKingsideRookMoved = whiteQueensideRookMoved = false;
            blackKingsideRookMoved = blackQueensideRookMoved = false;
            positionCounts.Clear();
        }

        public bool MovePiece(int startX, int startY, int endX, int endY)
        {
            if (!IsValidMove(board[startX, startY], startX, startY, endX, endY))
                return false;

            string piece = board[startX, startY];
            string captured = board[endX, endY];

            // Castling
            if (piece == "WKing" && startX == 7 && startY == 4)
            {
                if (endX == 7 && endY == 6) // Kingside
                {
                    board[7, 5] = board[7, 7];
                    board[7, 7] = null;
                    whiteKingsideRookMoved = true;
                }
                else if (endX == 7 && endY == 2) // Queenside
                {
                    board[7, 3] = board[7, 0];
                    board[7, 0] = null;
                    whiteQueensideRookMoved = true;
                }
                whiteKingMoved = true;
            }
            else if (piece == "BKing" && startX == 0 && startY == 4)
            {
                if (endX == 0 && endY == 6) // Kingside
                {
                    board[0, 5] = board[0, 7];
                    board[0, 7] = null;
                    blackKingsideRookMoved = true;
                }
                else if (endX == 0 && endY == 2) // Queenside
                {
                    board[0, 3] = board[0, 0];
                    board[0, 0] = null;
                    blackQueensideRookMoved = true;
                }
                blackKingMoved = true;
            }

            // Update rook moved flags
            if (piece == "WRook")
            {
                if (startX == 7 && startY == 0) whiteQueensideRookMoved = true;
                if (startX == 7 && startY == 7) whiteKingsideRookMoved = true;
            }
            if (piece == "BRook")
            {
                if (startX == 0 && startY == 0) blackQueensideRookMoved = true;
                if (startX == 0 && startY == 7) blackKingsideRookMoved = true;
            }

            // En passant
            if (piece.EndsWith("Pawn") && Math.Abs(endY - startY) == 1 && board[endX, endY] == null)
            {
                int direction = piece.StartsWith("W") ? 1 : -1;
                board[endX + direction, endY] = null;
            }

            board[endX, endY] = piece;
            board[startX, startY] = null;

            if (piece == "WKing") WhiteKingPosition = (endX, endY);
            if (piece == "BKing") BlackKingPosition = (endX, endY);

            lastMove = (endX, endY);
            isWhiteTurn = !isWhiteTurn;
            UpdatePositionCounts();
            return true;
        }

        private bool IsValidMove(string piece, int startX, int startY, int endX, int endY)
        {
            if (piece == null) return false;
            if (startX < 0 || startX >= GridSize || startY < 0 || startY >= GridSize ||
                endX < 0 || endX >= GridSize || endY < 0 || endY >= GridSize)
                return false;
            if (isWhiteTurn && piece.StartsWith("B")) return false;
            if (!isWhiteTurn && piece.StartsWith("W")) return false;
            if (board[endX, endY] != null && board[endX, endY].StartsWith(piece[0].ToString()))
                return false;

            string pieceType = piece.Substring(1);
            bool valid = false;
            switch (pieceType)
            {
                case "Pawn": valid = IsValidPawnMove(piece, startX, startY, endX, endY); break;
                case "Rook": valid = IsValidRookMove(startX, startY, endX, endY); break;
                case "Knight": valid = IsValidKnightMove(startX, startY, endX, endY); break;
                case "Bishop": valid = IsValidBishopMove(startX, startY, endX, endY); break;
                case "Queen": valid = IsValidQueenMove(startX, startY, endX, endY); break;
                case "King": valid = IsValidKingMove(startX, startY, endX, endY); break;
                default: valid = false; break;
            }
            if (!valid) return false;

            // Check if move leaves own king in check
            string originalTarget = board[endX, endY];
            board[endX, endY] = piece;
            board[startX, startY] = null;
            // Update king position if moving king
            (int oldKingX, int oldKingY) = isWhiteTurn ? WhiteKingPosition : BlackKingPosition;
            bool movedKing = pieceType == "King";
            if (movedKing)
            {
                if (isWhiteTurn) WhiteKingPosition = (endX, endY);
                else BlackKingPosition = (endX, endY);
            }
            bool inCheck = IsInCheck(isWhiteTurn);
            // Undo move
            board[startX, startY] = piece;
            board[endX, endY] = originalTarget;
            if (movedKing)
            {
                if (isWhiteTurn) WhiteKingPosition = (oldKingX, oldKingY);
                else BlackKingPosition = (oldKingX, oldKingY);
            }
            return !inCheck;
        }

        private bool IsValidMove(string piece, int startRow, int startCol, int endRow, int endCol, bool ignoreTurn)
        {
            if (piece == null) return false;
            if (startRow < 0 || startRow >= GridSize || startCol < 0 || startCol >= GridSize ||
                endRow < 0 || endRow >= GridSize || endCol < 0 || endCol >= GridSize)
                return false;
            // Only skip turn check if ignoreTurn is true
            if (!ignoreTurn)
            {
                if (isWhiteTurn && piece.StartsWith("B")) return false;
                if (!isWhiteTurn && piece.StartsWith("W")) return false;
            }
            if (board[endRow, endCol] != null && board[endRow, endCol].StartsWith(piece[0].ToString()))
                return false;

            string pieceType = piece.Substring(1);
            switch (pieceType)
            {
                case "Pawn": return IsValidPawnMove(piece, startRow, startCol, endRow, endCol);
                case "Rook": return IsValidRookMove(startRow, startCol, endRow, endCol);
                case "Knight": return IsValidKnightMove(startRow, startCol, endRow, endCol);
                case "Bishop": return IsValidBishopMove(startRow, startCol, endRow, endCol);
                case "Queen": return IsValidQueenMove(startRow, startCol, endRow, endCol);
                case "King": return IsValidKingMove(startRow, startCol, endRow, endCol);
                default: return false;
            }
        }

        private bool IsValidPawnMove(string piece, int startX, int startY, int endX, int endY)
        {
            int direction = piece.StartsWith("W") ? -1 : 1;
            int startRow = piece.StartsWith("W") ? 6 : 1;

            // Move forward
            if (startY == endY && board[endX, endY] == null)
            {
                if (endX == startX + direction) return true;
                if (endX == startX + 2 * direction && startX == startRow && board[startX + direction, startY] == null)
                    return true;
            }

            // Capture
            if (Math.Abs(endY - startY) == 1 && endX == startX + direction)
            {
                if (board[endX, endY] != null) return true;

            }
            return false;
        }

        private bool IsValidRookMove(int startX, int startY, int endX, int endY)
        {
            if (startX != endX && startY != endY) return false;
            if (startX == endX)
            {
                int step = startY < endY ? 1 : -1;
                for (int y = startY + step; y != endY; y += step)
                    if (board[startX, y] != null) return false;
            }
            else
            {
                int step = startX < endX ? 1 : -1;
                for (int x = startX + step; x != endX; x += step)
                    if (board[x, startY] != null) return false;
            }
            return true;
        }

        private bool IsValidKnightMove(int startX, int startY, int endX, int endY)
        {
            int dx = Math.Abs(endX - startX);
            int dy = Math.Abs(endY - startY);
            return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
        }

        private bool IsValidBishopMove(int startX, int startY, int endX, int endY)
        {
            if (Math.Abs(endX - startX) != Math.Abs(endY - startY)) return false;
            int stepX = endX > startX ? 1 : -1;
            int stepY = endY > startY ? 1 : -1;
            int x = startX + stepX, y = startY + stepY;
            while (x != endX && y != endY)
            {
                if (board[x, y] != null) return false;
                x += stepX; y += stepY;
            }
            return true;
        }

        private bool IsValidQueenMove(int startX, int startY, int endX, int endY)
        {
            return IsValidRookMove(startX, startY, endX, endY) || IsValidBishopMove(startX, startY, endX, endY);
        }

        private bool IsValidKingMove(int startX, int startY, int endX, int endY)
        {
            int dx = Math.Abs(endX - startX);
            int dy = Math.Abs(endY - startY);
            if (dx <= 1 && dy <= 1) return true;

            // Castling
            if (startX == endX && dx == 0 && dy == 2)
            {
                bool isWhite = isWhiteTurn;
                if (isWhite)
                {
                    if (whiteKingMoved) return false;
                    // Kingside
                    if (endY == 6 && !whiteKingsideRookMoved &&
                        board[7, 5] == null && board[7, 6] == null &&
                        board[7, 7] == "WRook" && !IsInCheck(true) &&
                        !WouldBeInCheck(7, 4, 7, 5) && !WouldBeInCheck(7, 4, 7, 6))
                        return true;
                    // Queenside
                    if (endY == 2 && !whiteQueensideRookMoved &&
                        board[7, 1] == null && board[7, 2] == null && board[7, 3] == null &&
                        board[7, 0] == "WRook" && !IsInCheck(true) &&
                        !WouldBeInCheck(7, 4, 7, 3) && !WouldBeInCheck(7, 4, 7, 2))
                        return true;
                }
                else
                {
                    if (blackKingMoved) return false;
                    // Kingside
                    if (endY == 6 && !blackKingsideRookMoved &&
                        board[0, 5] == null && board[0, 6] == null &&
                        board[0, 7] == "BRook" && !IsInCheck(false) &&
                        !WouldBeInCheck(0, 4, 0, 5) && !WouldBeInCheck(0, 4, 0, 6))
                        return true;
                    // Queenside
                    if (endY == 2 && !blackQueensideRookMoved &&
                        board[0, 1] == null && board[0, 2] == null && board[0, 3] == null &&
                        board[0, 0] == "BRook" && !IsInCheck(false) &&
                        !WouldBeInCheck(0, 4, 0, 3) && !WouldBeInCheck(0, 4, 0, 2))
                        return true;
                }
            }
            return false;
        }

        // --- Special rules and helpers ---

        public bool IsInCheck(bool white)
        {
            // Find king position
            string king = white ? "WKing" : "BKing";
            int kingRow = -1, kingCol = -1;
            for (int i = 0; i < GridSize; i++)
                for (int j = 0; j < GridSize; j++)
                    if (board[i, j] == king)
                    {
                        kingRow = i;
                        kingCol = j;
                        break;
                    }

            if (kingRow == -1) return false; // King not found

            // Check all opponent pieces for attack on king
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    string piece = board[i, j];
                    if (piece == null) continue;
                    if (white && piece.StartsWith("B") || !white && piece.StartsWith("W"))
                    {
                        if (IsValidMove(piece, i, j, kingRow, kingCol, ignoreTurn: true))
                            return true;
                    }
                }
            }
            return false;
        }

        private bool WouldBeInCheck(int kingX, int kingY, int newX, int newY)
        {
            string temp = board[newX, newY];
            board[newX, newY] = board[kingX, kingY];
            board[kingX, kingY] = null;
            var king = board[newX, newY];
            var pos = (newX, newY);
            bool inCheck = false;
            for (int x = 0; x < GridSize; x++)
                for (int y = 0; y < GridSize; y++)
                    if (board[x, y] != null && board[x, y][0] != king[0])
                        if (IsValidMove(board[x, y], x, y, pos.Item1, pos.Item2))
                            inCheck = true;
            board[kingX, kingY] = board[newX, newY];
            board[newX, newY] = temp;
            return inCheck;
        }

        public bool IsStalemate()
        {
            if (IsInCheck(isWhiteTurn)) return false;
            for (int x = 0; x < GridSize; x++)
                for (int y = 0; y < GridSize; y++)
                    if (board[x, y] != null && ((isWhiteTurn && board[x, y].StartsWith("W")) || (!isWhiteTurn && board[x, y].StartsWith("B"))))
                        for (int i = 0; i < GridSize; i++)
                            for (int j = 0; j < GridSize; j++)
                                if (IsValidMove(board[x, y], x, y, i, j))
                                    return false;
            return true;
        }

        public bool IsCheckmate()
        {
            bool white = isWhiteTurn;
            if (!IsInCheck(white))
                return false;

            // Try every possible move for the current player
            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    string piece = board[x, y];
                    if (piece == null) continue;
                    if ((white && piece.StartsWith("W")) || (!white && piece.StartsWith("B")))
                    {
                        for (int i = 0; i < GridSize; i++)
                        {
                            for (int j = 0; j < GridSize; j++)
                            {
                                if (IsValidMove(piece, x, y, i, j))
                                {
                                    // Make the move temporarily
                                    string captured = board[i, j];
                                    board[i, j] = piece;
                                    board[x, y] = null;
                                    bool stillInCheck = IsInCheck(white);
                                    // Undo move
                                    board[x, y] = piece;
                                    board[i, j] = captured;
                                    if (!stillInCheck)
                                        return false;
                                }
                            }
                        }
                    }
                }
            }
            // No legal moves to escape check
            return true;
        }

        private string GetBoardKey()
        {
            var key = "";
            for (int x = 0; x < GridSize; x++)
                for (int y = 0; y < GridSize; y++)
                    key += board[x, y] ?? ".";
            key += isWhiteTurn ? "W" : "B";
            return key;
        }

        private void UpdatePositionCounts()
        {
            var key = GetBoardKey();
            if (!positionCounts.ContainsKey(key))
                positionCounts[key] = 0;
            positionCounts[key]++;
        }

        public bool IsThreefoldRepetition()
        {
            var key = GetBoardKey();
            return positionCounts.ContainsKey(key) && positionCounts[key] >= 3;
        }
    }
}