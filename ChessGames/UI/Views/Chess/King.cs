using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGames.UI.Models
{
    public class King : ChessPiece
    {
        public King(Image image, Point location) : base(image, location) { }

        protected override void OnClick(object sender, EventArgs e)
        {
            PictureBox King = sender as PictureBox;
            MessageBox.Show($"King clicked at location: {King.Location}");
        }
    }
}