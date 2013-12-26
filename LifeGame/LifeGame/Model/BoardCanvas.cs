using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        protected double CellWidth { get;private set:}

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
        protected Board Board { get;private set:}


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

            this.Board.Changed += EventHandler<BoardChangedEventArgs>(board_Changed);

            _synchronize = true;
        }



















    }
}
