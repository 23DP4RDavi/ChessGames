using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ChessGames.UI
{
    public class ChessBoardForm : Form
    {
        private const int TileSize = 60;
        private const int GridSize = 8;
        private Panel[,] tiles = new Panel[GridSize, GridSize];
        private Image whiteTileImage;
        private Image blackTileImage;

        public ChessBoardForm()
        {
            InitializeComponent();
            LoadImages();
            CreateChessBoard();
        }

        private void InitializeComponent()
        {
            this.Text = "Chess Board";
            this.BackColor = Color.Black;
            this.Size = new Size(TileSize * GridSize + 16, TileSize * GridSize + 39);
        }

        private void LoadImages()
        {
            try
            {
                whiteTileImage = Image.FromFile(@"..\\UI\\Views\\Image\\white.png");
                blackTileImage = Image.FromFile(@"..\\UI\\Views\\Image\\black.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
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
                BackgroundImage = (row + col) % 2 == 0 ? whiteTileImage : blackTileImage,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            tile.Click += Tile_Click;
            return tile;
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            Panel tile = sender as Panel;
            MessageBox.Show($"Tile clicked at location: {tile.Location}");
        }
    }
}