using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
