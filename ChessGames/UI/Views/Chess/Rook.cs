using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGames.UI.Models
{
    public class Rook : ChessPiece
    {
        public Rook(Image image, Point location) : base(image, location) { }

        protected override void OnClick(object sender, EventArgs e)
        {
            PictureBox Rook = sender as PictureBox;
            MessageBox.Show($"Rook clicked at location: {Rook.Location}");
        }
    }
}