using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using ConsoleApp1.Games;

namespace CheckersGames.UI
{
    public class CheckersBoardForm : Form
    {
        private const int TileSize = 60;
        private const int GridSize = 8;
        private Panel[,] tiles = new Panel[GridSize, GridSize];
        private Dictionary<string, Image> pieceImages;
        private Image whiteTileBackground;
        private Image blackTileBackground;

        public CheckersBoardForm()
        {
            pieceImages = new Dictionary<string, Image>();
            InitializeComponent();
            LoadTileBackgrounds();
            LoadPieceImages();
            CreateCheckersBoard();
            RenderCheckersPieces();
        }

        private void InitializeComponent()
        {
            this.Text = "Checkers Board";
            this.BackColor = Color.Black;
            this.Size = new Size(TileSize * GridSize + 16, TileSize * GridSize + 39);
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
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UI", "Image", "Checkers");
            try
            {
                pieceImages["WhitePiece"] = Image.FromFile(Path.Combine(imagePath, "w_puck.png"));
                pieceImages["BlackPiece"] = Image.FromFile(Path.Combine(imagePath, "b_puck.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading piece images: {ex.Message}");
            }
        }

        private void CreateCheckersBoard()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Panel tile = CreateTile(row, col);
                    tiles[row, col] = tile;
                    this.Controls.Add(tile);
                }
            }
        }

        private Panel CreateTile(int row, int col)
        {
            Panel tile = new Panel
            {
                Size = new Size(TileSize, TileSize),
                Location = new Point(col * TileSize, row * TileSize),
                BackgroundImage = (row + col) % 2 == 0 ? whiteTileBackground : blackTileBackground,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            tile.Click += (sender, e) => Tile_Click(row, col);
            return tile;
        }

        private void Tile_Click(int row, int col)
        {
            // Add logic to handle checkers game moves
        }

        private void RenderCheckersPieces()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Image tileBackground = (row + col) % 2 == 0 ? whiteTileBackground : blackTileBackground;

                    // Determine if a piece should be placed on this tile
                    string piece = GetCheckersPiece(row, col);
                    if (piece != null && pieceImages.ContainsKey(piece))
                    {
                        Bitmap combinedImage = new Bitmap(TileSize, TileSize);
                        using (Graphics g = Graphics.FromImage(combinedImage))
                        {
                            // Draw the tile background
                            g.DrawImage(tileBackground, 0, 0, TileSize, TileSize);

                            // Calculate the size and position to center the piece image
                            Image pieceImage = pieceImages[piece];
                            int pieceWidth = pieceImage.Width;
                            int pieceHeight = pieceImage.Height;

                            // Maintain aspect ratio
                            float scale = Math.Min((float)TileSize / pieceWidth, (float)TileSize / pieceHeight);
                            int scaledWidth = (int)(pieceWidth * scale);
                            int scaledHeight = (int)(pieceHeight * scale);

                            int offsetX = (TileSize - scaledWidth) / 2;
                            int offsetY = (TileSize - scaledHeight) / 2;

                            // Draw the piece image centered
                            g.DrawImage(pieceImage, offsetX, offsetY, scaledWidth, scaledHeight);
                        }

                        tiles[row, col].BackgroundImage = combinedImage;
                    }
                    else
                    {
                        tiles[row, col].BackgroundImage = tileBackground;
                    }

                    tiles[row, col].BackgroundImageLayout = ImageLayout.None; // Prevent stretching
                }
            }
        }

        private string GetCheckersPiece(int row, int col)
        {
            if (row < 3 && (row + col) % 2 != 0)
            {
                return "BlackPiece";
            }
            else if (row > 4 && (row + col) % 2 != 0)
            {
                return "WhitePiece";
            }
            return null;
        }
    }
}
