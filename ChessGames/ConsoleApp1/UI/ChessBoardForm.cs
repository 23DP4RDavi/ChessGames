using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using ConsoleApp1.Games;

namespace ChessGames.UI
{
    public class ChessBoardForm : Form
    {
        private const int TileSize = 60;
        private const int GridSize = 8;
        private Panel[,] tiles = new Panel[GridSize, GridSize];
        private ChessLogic chessLogic;
        private Dictionary<string, Image> pieceImages;
        private Image whiteTileBackground;
        private Image blackTileBackground;
        private (int row, int col)? selectedTile = null;
        private Label player1Label;
        private Label player2Label;
        private Label whiteTimerLabel;
        private Label blackTimerLabel;
        private Panel capturedWhitePanel;
        private Panel capturedBlackPanel;
        private List<string> capturedWhitePieces = new List<string>();
        private List<string> capturedBlackPieces = new List<string>();
        private bool isWhiteTurn = true;
        private int whiteTimeRemaining = 600000; // 10 minutes in milliseconds
        private int blackTimeRemaining = 600000; // 10 minutes in milliseconds
        private Timer whiteTimer;
        private Timer blackTimer;

        public ChessBoardForm()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeComponent()
        {
            this.Text = "Chess Game";
            this.Size = new Size(TileSize * GridSize + 300, TileSize * GridSize + 100);
            this.BackColor = Color.Black;

            // Player labels
            player1Label = CreateLabel("White Player", new Point(TileSize * GridSize + 20, 10));
            player2Label = CreateLabel("Black Player", new Point(TileSize * GridSize + 20, 200));

            // Timers
            whiteTimerLabel = CreateLabel("10:00:000", new Point(TileSize * GridSize + 20, 50));
            blackTimerLabel = CreateLabel("10:00:000", new Point(TileSize * GridSize + 20, 240));

            this.Controls.Add(player1Label);
            this.Controls.Add(player2Label);
            this.Controls.Add(whiteTimerLabel);
            this.Controls.Add(blackTimerLabel);

            // Captured pieces panels
            capturedWhitePanel = CreateCapturedPanel(new Point(TileSize * GridSize + 20, 100));
            capturedBlackPanel = CreateCapturedPanel(new Point(TileSize * GridSize + 20, 290));

            this.Controls.Add(capturedWhitePanel);
            this.Controls.Add(capturedBlackPanel);
        }

        private void InitializeGame()
        {
            chessLogic = new ChessLogic();
            pieceImages = new Dictionary<string, Image>();
            LoadTileBackgrounds();
            LoadPieceImages();
            CreateChessBoard();
            RenderPieces();
            InitializeTimers();
        }

        private Label CreateLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent, // Optional: Set background to transparent or another color
                Location = location,
                AutoSize = true
            };
        }

        private Panel CreateCapturedPanel(Point location)
        {
            return new Panel
            {
                Location = location,
                Size = new Size(150, 100),
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private void LoadTileBackgrounds()
        {
            string imagePath = @"..\\..\\..\\UI\\Image\\";
            try
            {
                whiteTileBackground = Image.FromFile(Path.Combine(imagePath, "white.png"));
                blackTileBackground = Image.FromFile(Path.Combine(imagePath, "black.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tile backgrounds: {ex.Message}");
            }
        }

        private void LoadPieceImages()
        {
            string imagePath = @"..\\..\\..\\UI\\Image\\Chess\\";
            try
            {
                pieceImages["WPawn"] = Image.FromFile(Path.Combine(imagePath, "w_pawn.png"));
                pieceImages["BPawn"] = Image.FromFile(Path.Combine(imagePath, "b_pawn.png"));
                pieceImages["WRook"] = Image.FromFile(Path.Combine(imagePath, "w_rook.png"));
                pieceImages["BRook"] = Image.FromFile(Path.Combine(imagePath, "b_rook.png"));
                pieceImages["WKnight"] = Image.FromFile(Path.Combine(imagePath, "w_knight.png"));
                pieceImages["BKnight"] = Image.FromFile(Path.Combine(imagePath, "b_knight.png"));
                pieceImages["WBishop"] = Image.FromFile(Path.Combine(imagePath, "w_bishop.png"));
                pieceImages["BBishop"] = Image.FromFile(Path.Combine(imagePath, "b_bishop.png"));
                pieceImages["WQueen"] = Image.FromFile(Path.Combine(imagePath, "w_queen.png"));
                pieceImages["BQueen"] = Image.FromFile(Path.Combine(imagePath, "b_queen.png"));
                pieceImages["WKing"] = Image.FromFile(Path.Combine(imagePath, "w_king.png"));
                pieceImages["BKing"] = Image.FromFile(Path.Combine(imagePath, "b_king.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading piece images: {ex.Message}");
            }
        }

        private void CreateChessBoard()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Panel tile = new Panel
                    {
                        Size = new Size(TileSize, TileSize),
                        Location = new Point(col * TileSize, row * TileSize),
                        BackgroundImage = (row + col) % 2 == 0 ? whiteTileBackground : blackTileBackground,
                        BackgroundImageLayout = ImageLayout.Stretch
                    };

                    int capturedRow = row;
                    int capturedCol = col;
                    tile.Click += (sender, e) => Tile_Click(capturedRow, capturedCol);

                    tiles[row, col] = tile;
                    this.Controls.Add(tile);
                }
            }
        }

        private void Tile_Click(int row, int col)
        {
            // Debugging: Log the row and col values
            Console.WriteLine($"Tile_Click called with row: {row}, col: {col}");

            if (row < 0 || row >= GridSize || col < 0 || col >= GridSize)
            {
                MessageBox.Show("Invalid tile selected!");
                return;
            }

            if (selectedTile == null)
            {
                if (chessLogic.Board[row, col] != null)
                {
                    selectedTile = (row, col);
                }
            }
            else
            {
                var (startRow, startCol) = selectedTile.Value;

                if (startRow < 0 || startRow >= GridSize || startCol < 0 || startCol >= GridSize)
                {
                    MessageBox.Show("Invalid starting position!");
                    selectedTile = null;
                    return;
                }

                if (chessLogic.MovePiece(startRow, startCol, row, col))
                {
                    RenderPieces();
                    UpdateCapturedPieces();

                    isWhiteTurn = !isWhiteTurn;
                    if (isWhiteTurn)
                    {
                        blackTimer.Stop();
                        whiteTimer.Start();
                    }
                    else
                    {
                        whiteTimer.Stop();
                        blackTimer.Start();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid move!");
                }
                selectedTile = null;
            }
        }

        private void RenderPieces()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    string piece = chessLogic.Board[row, col];

                    // Set the tile's background image to the piece image or the tile background
                    if (piece != null && pieceImages.ContainsKey(piece))
                    {
                        tiles[row, col].BackgroundImage = pieceImages[piece];
                    }
                    else
                    {
                        tiles[row, col].BackgroundImage = (row + col) % 2 == 0 ? whiteTileBackground : blackTileBackground;
                    }

                    tiles[row, col].BackgroundImageLayout = ImageLayout.Stretch; // Ensure the image fits the tile
                }
            }
        }

        private void UpdateCapturedPieces()
        {
            capturedWhitePanel.Controls.Clear();
            capturedBlackPanel.Controls.Clear();

            foreach (string piece in capturedWhitePieces)
            {
                capturedWhitePanel.Controls.Add(new Label { Text = piece, AutoSize = true });
            }

            foreach (string piece in capturedBlackPieces)
            {
                capturedBlackPanel.Controls.Add(new Label { Text = piece, AutoSize = true });
            }
        }

        private void InitializeTimers()
        {
            whiteTimer = new Timer { Interval = 1 };
            blackTimer = new Timer { Interval = 1 };

            whiteTimer.Tick += (sender, e) =>
            {
                whiteTimeRemaining -= 1;
                UpdateTimerLabel(whiteTimerLabel, whiteTimeRemaining);

                if (whiteTimeRemaining <= 0)
                {
                    whiteTimer.Stop();
                    MessageBox.Show("White ran out of time! Black wins!");
                }
            };

            blackTimer.Tick += (sender, e) =>
            {
                blackTimeRemaining -= 1;
                UpdateTimerLabel(blackTimerLabel, blackTimeRemaining);

                if (blackTimeRemaining <= 0)
                {
                    blackTimer.Stop();
                    MessageBox.Show("Black ran out of time! White wins!");
                }
            };

            whiteTimer.Start(); // White starts first
        }

        private void UpdateTimerLabel(Label timerLabel, int timeRemaining)
        {
            int minutes = timeRemaining / 60000;
            int seconds = (timeRemaining % 60000) / 1000;
            int milliseconds = timeRemaining % 1000;

            timerLabel.Text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
        }
    }
}