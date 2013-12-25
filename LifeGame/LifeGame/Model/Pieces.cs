using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame.Model
{
    /// <summary>
    /// 良く利用する駒たち
    /// </summary>
    public static class Pieces
    {
        public static readonly IPiece Black = new BlackPiece();
        public static readonly IPiece White = new WhitePiece();
        public static readonly IPiece Empty = new EmptyPiece();
        public static readonly IPiece Guard = new GuardPiece();

    }
}
