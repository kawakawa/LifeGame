using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        /// IndexからLocationを求める
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Location ToLocation(int index)
        {
            var lacation = new Location(index%(Xsize + 2), index/(Ysize + 2));
            return lacation;
        }

        /// <summary>
        /// 本来のボード上の位置かどうかを調べる
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        protected bool IsOnBoard(Location loc)
        {
            int x = loc.X;
            int y = loc.Y;

            var checkX = (1 <= x && x <= Xsize);
            var checkY = (1 <= y && y <= Ysize);

            return checkX && checkY;
        }

        /// <summary>
        /// 本来のボード上の位置(index)かどうかを調べる
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected bool IsOnBoard(int index)
        {
            if (0 <= index && index < _pieces.Length)
            {
                return this._pieces[index] != Pieces.Guard;
            }
            return false;
        }

        /// <summary>
        /// Pieceを置く_piecesの要素を変更するのはこのメソッドだけ（コンストラクタは除く）。
        /// override可能
        /// </summary>
        /// <param name="index"></param>
        /// <param name="piece"></param>
        protected virtual void PutPiece(int index, IPiece piece)
        {
            if (IsOnBoard(index) == true)
            {
                this[index] = piece;
                OnChanged(ToLocation(index),piece);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// インデクサ(x,y）の位置の要素へアクセスする
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IPiece this[int index]
        {
            get
            {
                return this._pieces[index];
            }
            set
            {
                PutPiece(index,value);
            }
        }

        /// <summary>
        /// インデクサ(x,y）の位置の要素へアクセスする
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IPiece this[int x,int y]
        {
            get
            {
                return this[ToIndex(x, y)];
            }
            set
            {
                 this[ToIndex(x, y)] = value; 
            }
        }

        /// <summary>
        /// インデクサ(x,y）の位置の要素へアクセスする
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public IPiece this[Location loc]
        {
            get
            {
                return this[loc.X, loc.Y];
            }
            set
            {
                this[loc.X, loc.X] = value;
            }
        }

        /// <summary>
        /// 全てのPieceをクリアする
        /// </summary>
        public virtual void ClearAll()
        {
            foreach (var ix in GetOccupiedIndexes())
            {
                ClearPiece(ToLocation(ix));
            }
        }


        /// <summary>
        /// x,yの位置をクリアする
        /// </summary>
        /// <param name="loc"></param>
        public virtual void ClearPiece(Location loc)
        {
            this[loc.X, loc.Y] = Pieces.Empty;
        }


        /// <summary>
        /// EmptyPieces以外の全てのPieceを列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPiece> GetAllPieces()
        {
            var query = this._validIndexes.Select(idx => this[idx])
                .Where(n => n != Pieces.Empty);

            return query;
        }


        /// <summary>
        /// 指定したIPieceが置いてあるLocationを列挙する
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public IEnumerable<Location> GetLocations(IPiece piece)
        {
            var type = piece.GetType();
            var query = this.GetValidLocations().Where(loc => this[loc].GetType() == type);
            return query;
        }

        /// <summary>
        /// 指定したIPieceがおいてあるIndexを列挙する
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public IEnumerable<int> GetIndexes(IPiece piece)
        {
            var type = piece.GetType();
            var query = this._validIndexes.Where(idx => this[idx].GetType() == type);
            return query;
        }

        /// <summary>
        /// 番兵部分を除いた有効なLocationを列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Location> GetValidLocations()
        {
            var query = _validIndexes.Select(n => ToLocation(n));
            return query;
        }

        /// <summary>
        /// 番兵部分を除いた有効なIndexを列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetValidIndexes()
        {
            return _validIndexes;
        }

        /// <summary>
        /// 駒が置かれているLocationを列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Location> GetOccupiedLocations()
        {
            var query = this.GetOccupiedIndexes()
                .Select(idx => ToLocation(idx));
            return query;
        }


        /// <summary>
        /// 駒が置かれているLocationを列挙する
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> GetOccupiedIndexes()
        {
            var que = this._validIndexes.Where(index => this[index] != Pieces.Empty);
            return que;
        }

        /// <summary>
        /// 何も置かれていないLocationを列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Location> GetVacantLocations()
        {
            var query = this._validIndexes.Select(idx => ToLocation(idx));
            return query;
        }

    }
}


