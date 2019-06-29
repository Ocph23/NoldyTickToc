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
    /// Interaction logic for BoardLevel.xaml
    /// </summary>
    public partial class BoardLevel : Window
    {
        public BoardLevel()
        {
            InitializeComponent();
            IsEasy = true;
            easy.BorderThickness = new Thickness(6);
        }

        public bool IsEasy { get;  set; }
        public bool StartPlay { get; set; }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            easy.BorderThickness = new Thickness(6);
            hard.BorderThickness = new Thickness(0);
            string name = (sender as Border).Name;
            if (name == "easy")
                IsEasy = true;
        }

        private void hard_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            easy.BorderThickness = new Thickness(0);
            hard.BorderThickness = new Thickness(6);
            string name = (sender as Border).Name;
            if (name == "hard")
                IsEasy = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartPlay = true;
            this.Close();
        }
    }
}
