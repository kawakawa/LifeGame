using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LifeGame.Model
{
    /// <summary>
    /// 盤データクラス
    /// </summary>
    public class Board
    {
        // 駒が配置される１次元配列 （周辺には番兵が置かれる）
        private IPiece[] _pieces;

        // 番兵以外の有効な位置(１次元のインデックス）が格納される
        private readonly int[] _validIndexes;

        // 盤の行（縦方向）数
        public int Ysize { get; private set; }

        // 盤のカラム（横方向）数
        public int Xsize { get; private set; }

        // _pieces配列に変更があるとChangeイベントが発生する。
        private event EventHandler<BoardChangedEventArgs> Changed;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="xsize"></param>
        /// <param name="ysize"></param>
        public Board(int xsize, int ysize)
        {
            this.Xsize = xsize;
            this.Ysize = ysize;

            // 盤データの初期化 （周りは番兵(Guard)をセットしておく）
            this._pieces = new IPiece[(xsize+2)*(ysize+2)];

            for (var i = 0; i < this._pieces.Length; i++)
            {
                if (IsOnBoard(ToLocation(i)) == true)
                {
                    this._pieces[i] = Pieces.Empty;
                }
                else
                {
                    this._pieces[i] = Pieces.Guard;
                }
            }

            //毎回求めるのではなく、最初に求めておく
            this._validIndexes = Enumerable.Range(0, this._pieces.Length)
                                            .Where(n => _pieces[n] == Pieces.Empty)
                                            .ToArray();
        }

        /// <summary>
        /// コンストラクタ（Cloneとしても利用できる）
        /// </summary>
        /// <param name="board"></param>
        public Board(Board board)
        {
            this.Ysize = board.Ysize;
            this.Xsize = board.Xsize;
            this._validIndexes = board._validIndexes;
            this._pieces = board._pieces;
        }
        


        //イベント発行
        protected void OnChanged(Location loc, IPiece piece)
        {
            if (Changed != null)
            {
                var args=new BoardChangedEventArgs
                {
                    Location = loc,
                    Piece=piece
                };
                Changed(this, args);
            }
        }

        /// <summary>
        /// (x,y)から、＿Pieceへのインデックスを求める
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int ToIndex(int x, int y)
        {
            var result = x + y*(Xsize + 2);
            return result;
        }

        /// <summary>
        /// Locationから_Pieceのインデックスを求める
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public int ToIndex(Location loc)
        {
            return ToIndex(loc.X, loc.Y);
        }



        /// <summary>
        /// 本来のボード上の位置かどうかを調べる
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private bool IsOnBoard(Location location)
        {
            int x = location.X;
            int y = location.Y;

            var checkX = (1 <= x && x <= Xsize);
            var checkY = (1 <= y && y <= Ysize);

            return checkX && checkY;
        }

        /// <summary>
        /// IndexからLocationを求める
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Location ToLocation(int index)
        {
            var lacation = new Location(index%(Xsize + 2), index/(Ysize + 2));
            return lacation;
        }

    }
}


