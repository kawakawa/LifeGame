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

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void buttonRandom_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            



        }
    }
}
