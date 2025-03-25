using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGames.UI.Models
{
    public class Queen : ChessPiece
    {
        public Queen(Image image, Point location) : base(image, location) { }

        protected override void OnClick(object sender, EventArgs e)
        {
            PictureBox Queen = sender as PictureBox;
            MessageBox.Show($"Queen clicked at location: {Queen.Location}");
        }
    }
}