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
    /// Interaction logic for ScoreView.xaml
    /// </summary>
    public partial class ScoreView : Window
    {
        public ScoreView()
        {
            InitializeComponent();
            DataContext = this;
            using (var db = new OcphDbContext())
            {
                var sources = db.Scores.Select();
                PlayerComputerSource = sources.Where(O => O.Player2.ToUpper() == "2P").OrderByDescending(O => O.Id).ToList();
              
                PlayersSource = sources.Where(O => O.Player2.ToUpper() != "2P").OrderByDescending(O => O.Id).ToList();
                  
            }
        }
        public List<Score> PlayerComputerSource { get; set; }
        public List<Score> PlayersSource { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Yakin membersihkan Score ?", "Clear Score", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if(result== MessageBoxResult.OK)
            {
                try
                {
                    using (var db = new OcphDbContext())
                    {
                        db.Scores.Delete(O => O.GameType == "Sulit" || O.GameType == "Mudah");
                        ClearSource();
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Data Tidak Berhasil Diberhiskan");
                }
            }
           
        }

        private async void ClearSource()
        {
           await Task.Delay(1000);
            PlayerComputerSource.Clear();
            PlayersSource.Clear();
            dg1.ItemsSource = null;
            dg2.ItemsSource = null;
        }
    }
}
