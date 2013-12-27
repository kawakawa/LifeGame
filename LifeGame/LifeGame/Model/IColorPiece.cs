using LifeGame.Model.Piece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LifeGame.Model
{
    /// <summary>
    /// 色をもつ駒
    /// </summary>
    interface IColorPiece:IPiece
    {
        Color Color { get; }
    
    }
}
