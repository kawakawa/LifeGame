using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LifeGame.Model.Piece
{
    public struct WhitePiece:IColorPiece
    {
        public Color Color
        {
            get { return Color.FromArgb(255, 255, 255, 255); }
        }
    }
}
