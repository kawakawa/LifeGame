using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace LifeGame.Model.Piece
{
    public struct BlackPiece:IColorPiece
    {
        public Color Color
        {
            get { return Color.FromArgb(255, 128, 128, 128); }
        }
    }
}
