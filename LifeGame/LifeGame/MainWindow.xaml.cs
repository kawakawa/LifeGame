using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LifeGame.Model;

namespace LifeGame
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private LifeBoard board;
        private Drawer drawer;



        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Canvasがロードされた。初期化をする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas1_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick+=new EventHandler(timer_Tick);

            board=new LifeBoard(30);
            
            drawer=new Drawer(Canvas1,board);
            drawer.DrawRuledLines(BoardType.Chess);
        }

        /// <summary>
        /// Startボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            start();
        }

        /// <summary>
        /// Stopボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            stop();
        }

        /// <summary>
        /// Clearボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            board.ClearAll();
        }


        private bool _mouseDown = false;
        private Location _prevLoc;


        /// <summary>
        /// マウスが押された（Cellの状態を反転させる）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var loc = drawer.ToLocation(e.GetPosition(this.Canvas1 as UIElement));
            board.Reverse(loc);
            _mouseDown = true;
            _prevLoc = loc;

        }

        /// <summary>
        /// ドラッグ終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = false;
        }

        /// <summary>
        /// マウスがドラッグされた （Cellの状態を反転させる）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown == true)
            {
                var loc = drawer.ToLocation(e.GetPosition(this.Canvas1 as UIElement));
                if (_prevLoc != null && (_prevLoc.X != loc.X || _prevLoc.Y != loc.Y))
                {
                    board.Reverse(loc);
                    _prevLoc = loc;
                }
            }
        }

        /// <summary>
        /// Randomボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRandom_Click(object sender, RoutedEventArgs e)
        {
            SetRandom();
        }

        /// <summary>
        /// 世代を進める。（一定間隔で呼び出される）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            var count = board.Survive();
            if (count == 0)
            {
                stop();
            }
        }
        
        /// <summary>
        /// 開始
        /// </summary>
        private void start()
        {
            timer.Start();
            ButtonStart.IsEnabled = false;
            ButtonClear.IsEnabled = false;
        }

        /// <summary>
        /// 終了
        /// </summary>
        private void stop()
        {
            timer.Stop();
            ButtonStart.IsEnabled = true;
            ButtonClear.IsEnabled = true;
        }


        /// <summary>
        /// Random配置
        /// </summary>
        private void SetRandom()
        {
            var rnd = new Random();
            var count = rnd.Next(100, 150);

            while (count > 0)
            {
                int x = rnd.Next(5, board.Xsize - 5);
                int y = rnd.Next(5, board.Ysize - 5);
                var loc = new Location(x, y);
                board.Reverse(loc);
                count--;
            }
        }

    }
}
