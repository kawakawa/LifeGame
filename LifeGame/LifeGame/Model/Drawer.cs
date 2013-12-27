using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LifeGame.Model.Piece;

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

        /// <summary>
        /// 四角形を生成する
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="color"></param>
        private void DrawLife(Location loc, Color color)
        {
            Point p1 = ToPoint(loc);
            Point p2= new Point(p1.X+this.CellWidth-1,p1.Y+this.CellHeight-1);
            var rect = new Rectangle()
            {
                Name = this.PieceName(loc),
                Stroke = new SolidColorBrush(color),
                Fill = new SolidColorBrush(color),
                Margin = new Thickness(p1.X,p1.Y,p2.X,p2.Y),
                Width = p2.X-p1.X,
                Height = p2.Y-p1.Y,
            };

            this.Panel.Children.Add(rect);
            if (this.Panel.FindName(rect.Name) != null)
            {
                this.Panel.UnregisterName(rect.Name);
            }
            this.Panel.RegisterName(rect.Name,rect);
        }
    }
}
