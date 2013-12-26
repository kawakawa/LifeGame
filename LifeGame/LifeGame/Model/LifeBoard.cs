using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame.Model
{
    /// <summary>
    /// Cellsを管理します
    /// </summary>
    /// <remarks>
    /// ライフゲームの空間（盤）を表すクラスで、Boardクラスを継承しています。
    /// このクラスは複数のCellオブジェクトを管理します。
    /// 一つの升目にひとつのCellオブジェクトが割り当てられます。
    /// 初期値はすべてのCellが死んだ状態です。
    /// このクラスにもSurviveメソッドがあります。このメソッドは、すべてのCellオブジェクトに対して、
    /// 周りの生存数をカウントし、その値を引数にして、Cell.Survive メソッドを呼び出した後に、
    /// CellごとにNextStageを呼び出しています。
    /// なお、状態が変化した場合、変化したCellの数分、Changedイベントが発行されます。
    /// これにより、UI上にCellが変化したことを通知し、描画をさせています。
    /// 
    /// </remarks>
    public class LifeBoard:Board
    {
        //コンストラクタ
        public LifeBoard(int size) : base(size, size)
        {
            foreach (var loc in this.GetValidLocations())
            {
                this[loc]=new Cell();
            }
        }

        //反転する
        public void Reverse(Location loc)
        {
            var cell = this[loc] as Cell;
            cell.Toggle();
            OnChanged(loc,cell);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            foreach (var loc in this.GetValidLocations())
            {
                this[loc]=new Cell();
            }
        }

        /// <summary>
        /// 周りの生存者の数を数える
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        protected int CountAround(Location loc)
        {
            int[] directions =
            {
                -this.Xsize-3,-this.Xsize-2,-this.Xsize-1,
                -1,
                +1,
                this.Xsize+1,
                this.Xsize+2,
                this.Xsize+3
            };

            int count = 0;
            var index = ToIndex(loc);
            foreach (var d in directions)
            {
                var nix = (index + d);
                var loc2 = this.ToLocation(index + d);
                var cell = this[loc2] as Cell;

                if (cell != null && cell.IsAlive)
                {
                    count++;
                }
            }

            return count;
        }




    }
}
