using System;

namespace ConsoleApp1.Games
{
    public class ChessLogic
    {
        public string[,] Board { get; private set; }
        private const int GridSize = 8;
        public (int x, int y) WhiteKingPosition { get; private set; }
        public (int x, int y) BlackKingPosition { get; private set; }
        private bool isWhiteTurn = true; // Track whose turn it is (true = White's turn, false = Black's turn)
        public event Action<string, int, int> OnPawnPromotion; // Event for pawn promotion

        public ChessLogic()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            Board = new string[GridSize, GridSize];

            // Place pawns
            for (int i = 0; i < GridSize; i++)
            {
                Board[1, i] = "WPawn";
                Board[6, i] = "BPawn";
            }

            // Place other pieces
            string[] whitePieces = { "WRook", "WKnight", "WBishop", "WQueen", "WKing", "WBishop", "WKnight", "WRook" };
            string[] blackPieces = { "BRook", "BKnight", "BBishop", "BQueen", "BKing", "BBishop", "BKnight", "BRook" };

            for (int i = 0; i < GridSize; i++)
            {
                Board[0, i] = whitePieces[i];
                Board[7, i] = blackPieces[i];
            }

            WhiteKingPosition = (0, 4);
            BlackKingPosition = (7, 4);
        }

        public bool MovePiece(int startX, int startY, int endX, int endY)
        {
            // Validate the move
            if (!IsValidMove(Board[startX, startY], startX, startY, endX, endY))
            {
                return false;
            }

            // Perform the move
            string piece = Board[startX, startY];
            Board[endX, endY] = piece;
            Board[startX, startY] = null;

            // Check for pawn promotion
            if (piece == "WPawn" && endX == GridSize - 1) // White promotes at the bottom
            {
                OnPawnPromotion?.Invoke("W", endX, endY);
            }
            else if (piece == "BPawn" && endX == 0) // Black promotes at the top
            {
                OnPawnPromotion?.Invoke("B", endX, endY);
            }

            return true;
        }

        private bool IsValidMove(string piece, int startX, int startY, int endX, int endY)
        {
            Console.WriteLine($"Validating move for {piece} from ({startX}, {startY}) to ({endX}, {endY})");

            int direction = piece.StartsWith("W") ? 1 : -1; // White moves down, Black moves up

            if (piece.Contains("Pawn"))
            {
                return IsValidPawnMove(piece, startX, startY, endX, endY, direction);
            }

            if (piece.Contains("Rook"))
            {
                return IsValidRookMove(startX, startY, endX, endY);
            }

            if (piece.Contains("Bishop"))
            {
                return IsValidBishopMove(startX, startY, endX, endY);
            }

            if (piece.Contains("Queen"))
            {
                return IsValidQueenMove(startX, startY, endX, endY);
            }

            if (piece.Contains("King"))
            {
                return IsValidKingMove(startX, startY, endX, endY);
            }

            if (piece.Contains("Knight"))
            {
                return IsValidKnightMove(startX, startY, endX, endY);
            }

            Console.WriteLine("Invalid move: Unknown piece.");
            return false;
        }

        private bool IsValidPawnMove(string piece, int startX, int startY, int endX, int endY, int direction)
        {
            // Forward movement
            if (startY == endY && Board[endX, endY] == null)
            {
                // Single step
                if (endX == startX + direction) return true;

                // Double step (only from starting position)
                if ((piece.StartsWith("W") && startX == 1 || piece.StartsWith("B") && startX == 6) &&
                    endX == startX + 2 * direction && Board[startX + direction, startY] == null)
                {
                    return true;
                }
            }

            // Diagonal capture
            if (Math.Abs(startY - endY) == 1 && endX == startX + direction && Board[endX, endY] != null)
            {
                return true;
            }

            return false;
        }

        private bool IsValidRookMove(int startX, int startY, int endX, int endY)
        {
            if (startX == endX || startY == endY)
            {
                return IsPathClear(startX, startY, endX, endY);
            }
            return false;
        }

        private bool IsValidBishopMove(int startX, int startY, int endX, int endY)
        {
            if (Math.Abs(startX - endX) == Math.Abs(startY - endY))
            {
                return IsPathClear(startX, startY, endX, endY);
            }
            return false;
        }

        private bool IsValidQueenMove(int startX, int startY, int endX, int endY)
        {
            return IsValidRookMove(startX, startY, endX, endY) || IsValidBishopMove(startX, startY, endX, endY);
        }

        private bool IsValidKingMove(int startX, int startY, int endX, int endY)
        {
            return Math.Abs(startX - endX) <= 1 && Math.Abs(startY - endY) <= 1;
        }

        private bool IsValidKnightMove(int startX, int startY, int endX, int endY)
        {
            return (Math.Abs(startX - endX) == 2 && Math.Abs(startY - endY) == 1) ||
                   (Math.Abs(startX - endX) == 1 && Math.Abs(startY - endY) == 2);
        }

        private bool IsPathClear(int startX, int startY, int endX, int endY)
        {
            int deltaX = Math.Sign(endX - startX);
            int deltaY = Math.Sign(endY - startY);

            int currentX = startX + deltaX;
            int currentY = startY + deltaY;

            while (currentX != endX || currentY != endY)
            {
                if (Board[currentX, currentY] != null)
                {
                    return false;
                }

                currentX += deltaX;
                currentY += deltaY;
            }

            return true;
        }

        private bool IsKingInCheck(string king, int kingX, int kingY)
        {
            string opponentPrefix = king.StartsWith("W") ? "B" : "W";

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    string piece = Board[x, y];
                    if (piece != null && piece.StartsWith(opponentPrefix))
                    {
                        if (IsValidMove(piece, x, y, kingX, kingY))
                        {
                            return true; // King is in check
                        }
                    }
                }
            }

            return false; // King is safe
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < GridSize && y >= 0 && y < GridSize;
        }
    }
}