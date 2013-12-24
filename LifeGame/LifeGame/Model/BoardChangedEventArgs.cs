using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame.Model
{
    /// <summary>
    /// Changeイベントで使われる EventArgs
    /// </summary>
    public class BoardChangedEventArgs:EventArgs
    {
        public Location Location { get; internal set; }
        public IPiece Piece { get; internal set; }
    }
}
