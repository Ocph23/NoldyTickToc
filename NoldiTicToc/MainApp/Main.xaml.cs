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
using System.Windows.Shapes;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var inputPlayer = new InputPlayer();
            inputPlayer.ShowDialog();
            if(!string.IsNullOrEmpty(inputPlayer.PlayerName) && inputPlayer.StartPlay)
            {
                this.Hide();
                if(inputPlayer.IsEasy)
                {
                    var game = new MainWindow(inputPlayer.PlayerName);
                    game.ShowDialog();
                }else
                {
                    var game = new ExpertView(inputPlayer.PlayerName);
                    game.ShowDialog();
                }
                this.Show();
             
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var game = new Aturan();
            game.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var inputPlayer = new InputPlayer2();
            inputPlayer.ShowDialog();
            if (!string.IsNullOrEmpty(inputPlayer.PlayerName) && !string.IsNullOrEmpty(inputPlayer.PlayerName2) && inputPlayer.StartPlay)
            {
                this.Hide();
                if (inputPlayer.IsEasy)
                {
                    var game = new MainWindow(inputPlayer.PlayerName, inputPlayer.PlayerName2);
                    game.ShowDialog();
                }
                else
                {
                    var game = new ExpertView(inputPlayer.PlayerName, inputPlayer.PlayerName2);
                    game.ShowDialog();
                }
                this.Show();

            }


        }

        private void score_click(object sender, RoutedEventArgs e)
        {
            var scoreView = new ScoreView();
            scoreView.ShowDialog();
        }
    }
}
