using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGames.UI
{
    public class ChessBoardForm : Form
    {
        private const int TileSize = 60;
        private const int GridSize = 8;
        private Panel[,] tiles = new Panel[GridSize, GridSize];

        public ChessBoardForm()
        {
            InitializeComponent();
            CreateChessBoard();
        }

        private void InitializeComponent()
        {
            this.Text = "Chess Board";
            this.Size = new Size(TileSize * GridSize + 16, TileSize * GridSize + 39);
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
                        BackColor = (row + col) % 2 == 0 ? Color.White : Color.Gray
                    };
                    tiles[row, col] = tile;
                    this.Controls.Add(tile);
                }
            }
        }
    }
}