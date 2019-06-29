using GameLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewmodel;

        public PionVIew SelectedPion { get; private set; }
        Stopwatch stopWatch = new Stopwatch();

        public List<string> Solutions { get; set; } = new List<string>();
        public Node NodeSelected { get; private set; }
        public BoardBase board= new BoardBase(
                    new List<int[]> { new[] { 0, 4, 8 }, new[] { 2, 4, 6 },
                    new[] { 1, 4, 7 }, new[] { 3, 4, 5 }, new[] { 0, 3, 6 },
                    new[] { 2, 5, 8 }, new[] { 0, 1, 2 }, new[] { 6, 7, 8 } }, 3,true);

        public MainWindow(string playerName)
        {
            InitializeComponent();
            DataContext =viewmodel= new MainWindowViewModel();
            viewmodel.Player1 = new PlayerView("M", PlayerType.Human);
            viewmodel.Player1.PlayerName = playerName;
            viewmodel.Player2 = new PlayerView("H", PlayerType.Computer);
            viewmodel.Player2.PlayerName = "Computer";
            Loaded += ExpertView_Loaded;

        }


        public MainWindow(string playerName, string playerName2)
        {
            InitializeComponent();
            DataContext = viewmodel = new MainWindowViewModel();
            viewmodel.Player1 = new PlayerView("H", PlayerType.Human);
            viewmodel.Player1.PlayerName = playerName;
            viewmodel.Player2 = new PlayerView("M", PlayerType.Human);
            viewmodel.Player2.PlayerName = playerName2;
            Loaded += ExpertView_Loaded;

        }

        private void ExpertView_Loaded(object sender, RoutedEventArgs e)
        {
            DrawBoard();
            SetNewPionPosition();
           
        }

        private void ChangeNode()
        {
            ActualSize size = new ActualSize(mainCanvas);
            var ctx = DataContext as MainWindowViewModel;
            ctx.SetLine(size);
            foreach (var item in mainCanvas.Children.OfType<NodeView>())
            {
               // var vm = DataContext as ExpertViewModel;
                switch (item.Base.NodeName)
                {
                    case 0:
                        item.Base.X = 0;
                        item.Base.Y = size.Height;
                        break;
                    case 1:
                        item.Base.X = size.HalfWidth;
                        item.Base.Y = size.Height;
                        break;
                    case 2:
                        item.Base.X = size.Width;
                        item.Base.Y = size.Height;
                        break;
                    case 3:
                        item.Base.Y = size.HalfHeight;
                        item.Base.X = 0;
                        break;
                    case 4:
                        item.Base.Y = size.HalfHeight;
                        item.Base.X = size.HalfWidth;
                        break;
                    case 5:
                        item.Base.X = size.Width;
                        item.Base.Y = size.HalfHeight;
                        break;
                    case 6:
                        item.Base.Y = 0;
                        item.Base.X = 0;
                        break;
                    case 7:
                        item.Base.Y = 0;
                        item.Base.X = size.HalfWidth;
                        break;
                    case 8:
                        item.Base.Y = 0;
                        item.Base.X = size.Width;
                        break;
                    default:
                        break;
                }
            }

        }

        private async void DrawBoard()
        {
            await Task.Delay(500);
            var size = new ActualSize(mainCanvas);
            size.Width = mainCanvas.ActualWidth;
            size.Height = mainCanvas.ActualHeight;
            size.HalfHeight = (mainCanvas.ActualHeight / 2);
            size.HalfWidth = (mainCanvas.ActualWidth / 2);

            var w =40;
          
            mainCanvas.Children.Add(new NodeView(w, 0, new Position(0, 0)));
            mainCanvas.Children.Add(new NodeView(w, 1, new Position(0, 1)));
            mainCanvas.Children.Add(new NodeView(w, 2, new Position(0, 2)));
            mainCanvas.Children.Add(new NodeView(w, 3, new Position(1, 0)));
            mainCanvas.Children.Add(new NodeView(w, 4, new Position(1, 1)));
            mainCanvas.Children.Add(new NodeView(w, 5, new Position(1, 2)));
            mainCanvas.Children.Add(new NodeView(w, 6, new Position(2, 0)));
            mainCanvas.Children.Add(new NodeView(w, 7, new Position(2, 1)));
            mainCanvas.Children.Add(new NodeView(w, 8, new Position(2, 2)));

            ChangeNode();

          //Create Pion
            for (int i = 0; i < 3; i++)
            {
                Brush brush = Brushes.Red;
                var pion = new PionVIew(i, viewmodel.Player1.Code, Brushes.Red, 50);
                pion.Base.OnNode = i;
                pion.OnSelected += Pion_OnSelected;
                viewmodel.Player1.AddPion(pion);
                viewmodel.Player1.FirstState.Add(pion.Base.OnNode);
                mainCanvas.Children.Add(pion);

                var pion2 = new PionVIew(i,viewmodel.Player2.Code, Brushes.Green, 50);
                pion2.Base.OnNode = i+6;
                pion2.OnSelected += Pion_OnSelected;
                viewmodel.Player2.FirstState.Add(pion2.Base.OnNode);
                viewmodel.Player2.AddPion(pion2);
                mainCanvas.Children.Add(pion2);
            }

            viewmodel.Player1.IsPlay = true;
            board.SetPlayers(viewmodel.Player1, viewmodel.Player2);

            foreach (var item in mainCanvas.Children.OfType<NodeView>())
            {
                item.OnSelected += Item_OnSelected;
                board.Nodes.Add(item.Base);
            }

            SetNewPionPosition();
        }

        private  void SetNewPionPosition()
        {
            foreach (var pion in mainCanvas.Children.OfType<PionVIew>())
            {
                var item = mainCanvas.Children.OfType<NodeView>().Where(O => O.Base.NodeName == pion.Base.OnNode).FirstOrDefault();
                if (item != null)
                {
                    pion.SetPosition(item.Base);

                    
                }
            }

            stopWatch.Start();
          
        }

        private bool CanMove(Position pion, Position node)
        {
            if (pion.Column == node.Column && pion.Row == node.Row)
                return false;


            if (pion.Row < node.Row && pion.Row + 1 < node.Row)
                return false;

            if (pion.Row > node.Row && pion.Row - 1 > node.Row)
                return false;

            if (pion.Column < node.Column && node.Column > pion.Column+1)
                return false;

            if (pion.Column > node.Column && node.Column < pion.Column - 1  )
                return false;


            if (mainCanvas.Children.OfType<PionVIew>().Where(O => O.Base.Position.Column == node.Column && O.Base.Position.Row == node.Row).FirstOrDefault() != null)
                return false;

            if (pion.Column != node.Column && pion.Row != node.Row)
            {
                if((node.Row == 1 && node.Column == 1)||pion.Row==1 && pion.Column==1)
                {
                    return true;
                }else
                {
                    return false;
                }
            }
                
            return true;

        }

        private async void SwitchPlayer()
        {
            SelectedPion = null;
            viewmodel.Player1.IsPlay = !viewmodel.Player1.IsPlay;
            viewmodel.Player2.IsPlay = !viewmodel.Player2.IsPlay;
            var willplay = viewmodel.Player1.IsPlay ? viewmodel.Player1 : viewmodel.Player2;
            if(willplay.PlayerType== PlayerType.Computer)
               await AutoPlay(willplay, MethodType.DFS);
        }

        private async Task<int> AutoPlay(PlayerView player, MethodType type)
        {
            Step step = null;
            board.SetEngine(type);

            var resultData =await board.Get(GetBoard(), player);
            step = resultData.Item1;
            if(player.PlayerType== PlayerType.Human)
                SetStepsToConsole(resultData, player);

            
            var pionselected = player.Pions.Where(O => O.Base.Position.Row == step.From.Item1 && O.Base.Position.Column == step.From.Item2).FirstOrDefault();
            Pion_OnSelected(pionselected);
            var nodeSelected = mainCanvas.Children.OfType<NodeView>().Where(O => O.Base.Position.Row == step.To.Item1 && O.Base.Position.Column == step.To.Item2).FirstOrDefault();
            Item_OnSelected(null, nodeSelected.Base);
            return 0;
        }
                             
         private void ClearConsole()
        {
            console.Text=string.Empty;
            dfs.IsChecked = false;
            greedy.IsChecked = false;
        }

        private void SetStepsToConsole(Tuple<Step,ArrayList> step, PlayerView player)
        {
            var item1 = step.Item1;
            var item2 = step.Item2;
            if (dfs.IsChecked == true || greedy.IsChecked == true)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format(" {0} Langkah -> {1} det", item2.Count, item1.TimeAnalisys.TotalSeconds));
                foreach (Step result in item2)
                {
                    var nodeSelected = mainCanvas.Children.OfType<NodeView>().Where(O => O.Base.Position.Row == result.To.Item1 && O.Base.Position.Column == result.To.Item2).FirstOrDefault();
                    if (nodeSelected != null && result.FromNode.Substring(0,1)==player.Code)
                    {
                        sb.AppendLine(string.Format("Move {0} To Node {1}", result.FromNode, nodeSelected.Base.NodeName));
                    }
                }
                console.Text = sb.ToString();
            }
            
        }

        private async void Item_OnSelected (object sender, Node node)
        {
            await Task.Delay(0);
            NodeSelected = node;
            Point p = Mouse.GetPosition(mainCanvas);
            var text = p.ToString();

            PlayerView player = GetPlayerIsPlay();

            if (SelectedPion != null && CanMove(SelectedPion.Base.Position, NodeSelected.Position))
            {
                var pion = mainCanvas.Children.OfType<PionVIew>().Where(O => O.Base.NameView == SelectedPion.Base.NameView).FirstOrDefault();
                player.Play(node, pion);
           
                await Task.Delay(2000);
                if(board.IsWinner(GetBoard(),player))
                {
                    stopWatch.Stop();
                    player.Win += 1;
                    MessageBox.Show(player.PlayerName + "  Win !");
                    try
                    {
                        TimeSpan ts = stopWatch.Elapsed;
                        using (var db = new OcphDbContext())
                        {
                            var item = new Score { };
                            item.Player2 = "1P";
                            item.Player2 = "2P";
                            if (viewmodel.Player1.PlayerType == PlayerType.Human)
                                item.Player1 = viewmodel.Player1.PlayerName;

                            if (viewmodel.Player2.PlayerType == PlayerType.Human)
                                item.Player2 = viewmodel.Player2.PlayerName;
                            item.Player1Win = viewmodel.Player1.Win;
                            item.Player2Win = viewmodel.Player2.Win;
                            item.Solutions = GetSulution();
                            item.GameType = "Mudah";
                            item.Time = ts.ToString();
                            db.Scores.InsertAndGetLastID(item);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Data Not Saved");
                    }
                }
                else
                SwitchPlayer();

            }
        }

        private string GetSulution()
        {
            string s = string.Empty;
            foreach(var item in Solutions)
            {
                if (string.IsNullOrEmpty(s))
                    s = item;
                else
                    s = s + ", " + item;
            }
            return s;
        }

        private PlayerView GetPlayerIsPlay()
        {
            if (viewmodel.Player1.IsPlay)
                return viewmodel.Player1;
            return viewmodel.Player2;
        }

        private void Pion_OnSelected(PionVIew pion)
        {
            if (SelectedPion != null)
            {
                if (SelectedPion == pion)
                {
                    pion.Stopanimation();
                    SelectedPion = null;
                }

            }
            else
            {
                if ((viewmodel.Player1.IsPlay && viewmodel.Player1.IsPion(pion)) || (viewmodel.Player2.IsPlay && viewmodel.Player2.IsPion(pion)))
                {
                    SelectedPion = pion;
                    SelectedPion.PlayAnimation();
                }

            }

        }

        public string[,] GetBoard()
        {
            var board = new string[3, 3];
            foreach (var pion in mainCanvas.Children.OfType<PionVIew>())
            {
                board[pion.Base.Position.Row, pion.Base.Position.Column] = pion.Base.NameView;
            }
            return board;
        }

        private async void Replay_Click(object sender, RoutedEventArgs e)
        {
            ClearConsole();
            mainCanvas.Children.Clear();
            viewmodel.Player1.Pions.Clear();
            viewmodel.Player2.Pions.Clear();
            viewmodel.Player1.Win=0;
            viewmodel.Player2.Win=0;
            DrawBoard();
            await Task.Delay(5000);
            viewmodel.Player2.IsPlay = true;
            viewmodel.Player1.IsPlay = false;
            Solutions.Clear();
             if(stopWatch.IsRunning)
            {
                stopWatch.Stop();
            }
            else
            {
                stopWatch.Start();
            }
            
            SwitchPlayer();

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Dfs_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);
            var willplay = viewmodel.Player1.IsPlay ? viewmodel.Player1 : viewmodel.Player2;

            if (dfs.IsChecked == true)
            {
                var dfsExist = Solutions.Where(O => O == MethodType.DFS.ToString()).FirstOrDefault()!=null;
                if (!dfsExist)
                    Solutions.Add(MethodType.DFS.ToString());
              await  AutoPlay(willplay, MethodType.DFS);
            }

        }

        private async void Greedy_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);
            var willplay = viewmodel.Player1.IsPlay ? viewmodel.Player1 : viewmodel.Player2;

            if (greedy.IsChecked == true)
            {
                var dfsExist = Solutions.Where(O => O == MethodType.Greedy.ToString()).FirstOrDefault() != null;
                if (!dfsExist)
                    Solutions.Add(MethodType.Greedy.ToString());
                await AutoPlay(willplay, MethodType.Greedy);
            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.ClearConsole();
        }
    }

    public class MainWindowViewModel : BaseNotify
    {
        public MainWindowViewModel()
        {
        }
        public PlayerView Player1 { get; set; }
        public PlayerView Player2 { get; set; }
        private double y1;
        private double y2;
        private double _xmidle;
        private double _ymidle;
        private double x1;
        private double x2;

        public double X1
        {
            get { return x1; }
            set { SetProperty(ref x1, value); }
        }

        public double X2
        {
            get { return x2; }
            set { SetProperty(ref x2, value); }
        }

        public double XMidle
        {
            get { return _xmidle; }
            set { SetProperty(ref _xmidle, value); }
        }

        public double YMidle
        {
            get { return _ymidle; }
            set { SetProperty(ref _ymidle, value); }
        }

        public double Y1
        {
            get { return y1; }
            set { SetProperty(ref y1, value); }
        }

        public double Y2
        {
            get { return y2; }
            set { SetProperty(ref y2, value); }
        }

        internal void SetLine(ActualSize size)
        {
            this.X1 = 0;
            this.Y1 = 0;
            this.X2 = size.Width;
            this.Y2 = size.Height;
            this.XMidle = size.HalfWidth;
            this.YMidle = size.HalfHeight;
        }
    }
}
