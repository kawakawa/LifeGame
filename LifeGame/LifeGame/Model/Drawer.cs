using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LifeGame.Model
{
    /// <summary>
    /// 描画を受け持つクラス
    /// </summary>
    public class Drawer:BoardCanvas
    {
        public Drawer(Panel panel1, Board board)
            : base(panel1, board) { }



    }
}
