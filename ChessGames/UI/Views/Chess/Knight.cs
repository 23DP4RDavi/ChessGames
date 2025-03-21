using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGames.UI.Models
{
    public class Knight : ChessPiece
    {
        public Knight(Image image, Point location) : base(image, location) { }

        protected override void OnClick(object sender, EventArgs e)
        {
            PictureBox Knight = sender as PictureBox;
            MessageBox.Show($"Knight clicked at location: {Knight.Location}");
        }
    }
}