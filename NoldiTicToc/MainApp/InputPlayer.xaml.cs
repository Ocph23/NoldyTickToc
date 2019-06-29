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
    /// Interaction logic for InputPlayer.xaml
    /// </summary>
    public partial class InputPlayer : Window
    {
        public InputPlayer()
        {
            InitializeComponent();
        }

        public bool StartPlay { get; private set; }
        public bool IsEasy { get; private set; }
        public string PlayerName { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                var boardLevel = new BoardLevel();
                boardLevel.ShowDialog();
                if (boardLevel.StartPlay)
                {
                    StartPlay = boardLevel.StartPlay;
                    IsEasy = boardLevel.IsEasy;
                    PlayerName = txtName.Text;
                    this.Close();
                }
                 
            }

        

            

        }
    }
}
