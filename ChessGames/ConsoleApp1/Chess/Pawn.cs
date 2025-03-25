using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGames.UI.Models
{
    public class Pawn : ChessPiece
    {
        public Pawn(Image image, Point location) : base(image, location) { }

        protected override void OnClick(object sender, EventArgs e)
        {
            PictureBox Pawn = sender as PictureBox;
            MessageBox.Show($"Pawn clicked at location: {Pawn.Location}");
        }
    }
}