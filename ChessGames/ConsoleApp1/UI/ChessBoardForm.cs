using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
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

        public ChessBoardForm()
        {
            chessLogic = new ChessLogic();
            pieceImages = new Dictionary<string, Image>();
            InitializeComponent();
            LoadTileBackgrounds();
            LoadPieceImages();
            CreateChessBoard();
            RenderPieces();
        }

        private void InitializeComponent()
        {
            this.Text = "Chess Board";
            this.BackColor = Color.Black;
            this.Size = new Size(TileSize * GridSize + 16, TileSize * GridSize + 39);
        }

        private void LoadTileBackgrounds()
        {
            string imagePath = @"..\\ConsoleApp1\\UI\\Image\\";
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
            string imagePath = @"..\\ConsoleApp1\\UI\\Image\\Chess\\";
            try
            {
                pieceImages["WPawn"] = Image.FromFile(Path.Combine(imagePath, "pawn.png"));
                pieceImages["BPawn"] = Image.FromFile(Path.Combine(imagePath, "bpawn.png"));
                pieceImages["WRook"] = Image.FromFile(Path.Combine(imagePath, "rook.png"));
                pieceImages["BRook"] = Image.FromFile(Path.Combine(imagePath, "brook.png"));
                pieceImages["WKnight"] = Image.FromFile(Path.Combine(imagePath, "knight.png"));
                pieceImages["BKnight"] = Image.FromFile(Path.Combine(imagePath, "bknight.png"));
                pieceImages["WBishop"] = Image.FromFile(Path.Combine(imagePath, "bishop.png"));
                pieceImages["BBishop"] = Image.FromFile(Path.Combine(imagePath, "bbishop.png"));
                pieceImages["WQueen"] = Image.FromFile(Path.Combine(imagePath, "queen.png"));
                pieceImages["BQueen"] = Image.FromFile(Path.Combine(imagePath, "bqueen.png"));
                pieceImages["WKing"] = Image.FromFile(Path.Combine(imagePath, "king.png"));
                pieceImages["BKing"] = Image.FromFile(Path.Combine(imagePath, "bking.png"));
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
            MessageBox.Show($"Tile clicked at row {row}, column {col}");
        }

        private void RenderPieces()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Image tileBackground = (row + col) % 2 == 0 ? whiteTileBackground : blackTileBackground;

                    string piece = chessLogic.Board[row, col];
                    if (piece != null && pieceImages.ContainsKey(piece))
                    {
                        Bitmap combinedImage = new Bitmap(TileSize, TileSize);
                        using (Graphics g = Graphics.FromImage(combinedImage))
                        {
                            g.DrawImage(tileBackground, 0, 0, TileSize, TileSize);
                            g.DrawImage(pieceImages[piece], 0, 0, TileSize, TileSize);
                        }

                        tiles[row, col].BackgroundImage = combinedImage;
                    }
                    else
                    {
                        tiles[row, col].BackgroundImage = tileBackground;
                    }

                    tiles[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }
    }
}