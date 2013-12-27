using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LifeGame.Model
{
    /// <summary>
    /// 描画を受け持つクラス
    /// </summary>
    public class Drawer:BoardCanvas
    {
        public Drawer(Panel panel1, Board board)
            : base(panel1, board) { }


        /// <summary>
        /// ひとつのセルをその状態によって描画
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="piece"></param>
        public override　void DrawPiece(Location loc, IPiece piece)
        {
            var cell = piece as Cell;
            if (cell.IsAlive == true)
            {
                DrawLife(loc, Colors.Gray);
            }
            else
            {
                RemovePiece(loc);
            }
        }
    }
}
