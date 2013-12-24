using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LifeGame.Model
{
    /// <summary>
    /// Cell (ひとつの四角の領域）を表す。
    // IPeiseはマーカーインターフェース（実装すべきメソッド等は無い）
    /// </summary>
    /// <remarks>
    /// ひとつのセル上の生物を表すクラス。
    /// Surviceメソッドが、引数で周囲の「生きているセル」の数を受け取り、
    /// その数により次の世代の状態を決めます。このメソッドを呼び出しただけでは、実際の状態は更新されません。
    /// NextStageを呼び出されることで、Surviveで得られた状態に更新されます。
    /// 
    /// 使い方として、すべてのCellに対してSurviveを呼び出した後に、
    /// すべてのCellに対してNextStageを呼び出すことになります。
    /// 
    /// </remarks>
    public class Cell:IPiece
    {
        public bool IsAlive { get; private set; }

        private bool _nextStatus;




        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Cell()
        {
            IsAlive = false;
        }

        /// <summary>
        /// 生死を反転させる
        /// </summary>
        public void Toggle()
        {
            IsAlive = !IsAlive;
        }


        /// <summary>
        ///  trueならば生、falseならば死
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private bool Judge(int count)
        {
            if (IsAlive == true)
            {
                return (count == 2 | count == 3);
            }

            return (count == 3);
        }

        /// <summary>
        /// 次の世代の状態を決める。変化があるとtrueが返る。
        /// </summary>
        /// <param name="around"></param>
        /// <returns></returns>
        public bool Survive(int around)
        {
            this._nextStatus = this.Judge(around);
            return _nextStatus != IsAlive;
        }

        /// <summary>
        /// 次の状態にする
        /// </summary>
        /// <returns></returns>
        public bool NextStage()
        {
            var old = this.IsAlive;
            IsAlive = this._nextStatus;
            return IsAlive != old;
        }


    }
}
