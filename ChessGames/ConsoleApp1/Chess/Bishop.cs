using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGames.UI.Models
{
    public class Bishop : ChessPiece
    {
        public Bishop(Image image, Point location) : base(image, location) { }

        protected override void OnClick(object sender, EventArgs e)
        {
            PictureBox Bishop = sender as PictureBox;
            MessageBox.Show($"Bishop clicked at location: {Bishop.Location}");
        }
    }
}