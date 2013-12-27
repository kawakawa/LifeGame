using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LifeGame.Model.Piece;

namespace LifeGame.Model
{
    /// <summary>
    /// 罫線の種類
    /// </summary>

    public enum BoardType
    {
        Go,
        Chess
    }

    // Boardおよび駒(PIece)の表示を担当する
    // BoardオブジェクトからChangeイベントを受け取ると、変更されたセルのPieceを描画する。
    // その他Board/Pieceを描画するための各種メソッドを用意。
    // PanelにUiElementを動的に追加削除することで描画を行っている。
    public class BoardCanvas
    {
        /// <summary>
        /// １つのCellの幅
        /// </summary>
        protected double CellWidth { get; private set; }

        /// <summary>
        /// １つのCellの高さ
        /// </summary>
        protected double CellHeight { get; private set; }

        /// <summary>
        /// 縦方向のCell数
        /// </summary>
        protected int Ysize { get; private set; }

        /// <summary>
        /// 横方向のCell数
        /// </summary>
        protected int Xsize { get; private set; }

        /// <summary>
        /// 罫線の種類（基盤かチェス盤）
        /// </summary>
        protected BoardType BoardType { get; private set; }


        /// <summary>
        /// 対象となるPanelオブジェクト
        /// </summary>
        protected Panel Panel { get; private set; }

        /// <summary>
        /// 対象となるBoardオブジェクト
        /// </summary>
        protected Board Board { get; private set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="board"></param>
        public BoardCanvas(Panel panel, Board board)
        {
            this.Panel = panel;
            this.Board = board;

            Ysize = board.Ysize;
            Xsize = board.Xsize;

            this.CellWidth = (panel.ActualWidth - 1)/Xsize;
            this.CellHeight = (panel.ActualHeight - 1)/Ysize;

            foreach (var loc in board.GetValidLocations())
            {
                UpdatePiece(loc, board[loc]);
            }

            this.Board.Changed += board_Changed;

            _synchronize = true;
        }

        /// <summary>
        /// 同期用オブジェクト
        /// </summary>
        private bool _synchronize;


        /// <summary>
        /// Boardオブジェクトと同期するか否か （初期値：同期する）
        /// </summary>
        public bool Synchronize
        {
            get { return this._synchronize; }
            set
            {
                if (value == true)
                {
                    if (_synchronize == false)
                    {
                        this.Board.Changed += board_Changed;
                        _synchronize = true;
                    }
                }
                else
                {
                    if (_synchronize == true)
                    {
                        this.Board.Changed -=board_Changed;
                        _synchronize = false;
                    }
                }
            }
        }



        public void ChangeBoard(Board board)
        {
            this.Board = board;
        }


        /// <summary>
        /// 罫線を引く
        /// </summary>
        /// <param name="lineType"></param>
        public void DrawRuledLines(BoardType lineType)
        {
            this.BoardType = lineType;
            int startx = (lineType == BoardType.Chess)
                ? 0
                : (int) (CellHeight/2);

            for (double i = startx; i <= Panel.ActualHeight; i += CellHeight)
            {
                DrawLine(0, i, Panel.ActualWidth, i);
            }

            
            int starty = (lineType == BoardType.Chess)
                ? 0
                : (int) (CellWidth/2);

            for (double i = starty; i <= Panel.ActualWidth; i += CellWidth)
            {
                DrawLine(i, 0, i, Panel.ActualHeight);
            }

        }

        /// <summary>
        /// 線を引く
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            var line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush(Colors.LightGray)
            };

            Panel.Children.Add(line);
        }

        /// <summary>
        /// 円オブジェクトを生成する （Pieceのデフォルト表示を担当)
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Ellipse CreateEllipse(Location loc, Color color)
        {
            var eli = new Ellipse();
            eli.Name = PieceName(loc);
            eli.Height = CellWidth*0.85;
            eli.Width = CellHeight*0.85;
            eli.Fill=new SolidColorBrush(color);
            eli.Stroke = new SolidColorBrush(Colors.DarkGray);

            var pt = ToPoint(loc);
            Canvas.SetLeft(eli,pt.X+CellWidth/2-eli.ActualWidth/2);
            Canvas.SetTop(eli,pt.Y+CellHeight/2-eli.ActualHeight/2);

            return eli;
        }

        /// <summary>
        /// Pieceオブジェクトの名前を生成する
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        protected string PieceName(Location loc)
        {
            return string.Format("x{0}y{1}", loc.X, loc.Y);
        }

        /// <summary>
        /// 矩形を描く
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="color"></param>
        public void DrawRectangle(Point p1, Point p2, Color color)
        {
            this.Panel.Children.Add(CreateRectangle(p1, p2, color));
        }


        /// <summary>
        /// 四角形を精製する
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Rectangle CreateRectangle(Point p1,Point p2,Color color)
        {
            var rect = new Rectangle();
            rect.Name = RectangleName(p1);
            rect.Stroke=new SolidColorBrush(color);

            double x1 = Math.Min(p1.X, p2.X);
            double y1 = Math.Min(p1.Y, p2.Y);
            double x2 = Math.Min(p1.X, p2.X);
            double y2 = Math.Min(p1.Y, p2.Y);

            rect.Margin= new Thickness(x1,y1,x2,y2);
            rect.Width = x2 - x1;
            rect.Height = y2 - y1;
            return rect;

        }


        /// <summary>
        /// 矩形の名前を得る
        /// </summary>
        /// <param name="p1"></param>
        /// <returns></returns>
        protected string RectangleName(Point p1)
        {
            var val = string.Format("r{0}{1}", (int) p1.X, (int) p1.Y);
            return val;
        }


        /// <summary>
        ///  Boardの内容に沿って、Pieceを描画しなおす
        ///  UIスレッドとは別スレッドで動作していた場合を考慮。
        /// </summary>
        public void Invalidate()
        {
            Panel.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var loc in Board.GetValidLocations())
                {
                    this.RemovePiece(loc);
                }

                foreach (var loc in Board.GetValidLocations())
                {
                    var piece = Board[loc];
                    UpdatePiece(loc, piece);
                }
            }));
        }

        /// <summary>
        /// UIスレッドとは別スレッドで動作させている時だけ意味を持つ。
        /// </summary>
        public TimeSpan UpdateInterval { get; set; }
    



        /// <summary>
        /// Boardオブジェクトが変更されたときに呼び出されるイベントハンドラ
        /// UIスレッドとは別スレッドで動作していた場合を考慮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void board_Changed(object sender, BoardChangedEventArgs e)
        {
 	        Thread.Sleep(UpdateInterval);

            Panel.Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdatePiece(e.Location, e.Piece);
            }));
        }


        /// <summary>
        /// Pieceの描画を更新する
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="piece"></param>
        private void UpdatePiece(Location loc,IPiece piece)
        {
            if (piece == null || piece is EmptyPiece)
            {
                RemovePiece(loc);
            }
            else
            {
                var name = PieceName(loc);
                object obj = Panel.FindName(name);

                if (obj != null)
                {
                    Panel.Children.Remove(obj as UIElement);
                }

                if (piece is EmptyPiece || piece is GuardPiece)
                {
                    return;
                }

                DrawPiece(loc, piece);
            }
        }


        /// <summary>
        /// Pieceを描く。 デフォルト実装は、IColorPieceのみに対応。
        /// 他のPiece型は、独自に当メソッドをoverrideする必要がある。
        /// なお、overrideした DrawPiece内では、DrawRectangleメソッドは利用できない。
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="piece"></param>
        public virtual void DrawPiece(Location loc,IPiece piece)
        {
            var colorPiece = piece as IColorPiece;
            if (colorPiece != null)
            {
                Panel.Children.Add(CreateEllipse(loc, colorPiece.Color));
            }
        }


        /// <summary>
        /// 指定した位置のPieceを取り除く
        /// </summary>
        /// <param name="loc"></param>
        protected void RemovePiece(Location loc)
        {
            var name = PieceName(loc);
            object obj = Panel.FindName(name);
            if (obj != null)
            {
                Panel.Children.Remove(obj as UIElement);
            }
        }





        /// <summary>
        /// Locationからグラフィックの座標であるPointへ変換
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public Point ToPoint(Location loc)
        {
            return new Point()
            {
                X=CellWidth*(loc.X-1),
                Y=CellHeight*(loc.Y-1)
            };
        }

        /// <summary>
        /// グラフィックの座標であるPointからLocationへ変換
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Location ToLocation(Point pt)
        {
            var x = pt.X;
            var y = pt.Y;

            int a = Math.Max(0, (int) (x/CellWidth));
            if (a >= Xsize)
            {
                a = Xsize - 1;
            }

            int b = Math.Max(0, (int) (y/CellHeight));
            if (b >= Ysize)
            {
                b = Ysize - 1;
            }

            return  new Location(a+1,b+1);
        }
    }
}
