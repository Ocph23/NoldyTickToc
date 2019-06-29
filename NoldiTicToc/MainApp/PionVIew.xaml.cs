using GameLib;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for PionVIew.xaml
    /// </summary>
    /// 
    public delegate void delOnSelectedPion(PionVIew pion);

    public partial class PionVIew : UserControl
    {
        public event delOnSelectedPion OnSelected;

        public PionViewModel Base { get; set; }

        public Position Position { get;  set; }



        public PionVIew(int id, string nama, Brush color,int size)
        {
            InitializeComponent();
            Name = nama;
            Base = new PionViewModel(id, nama, color, size);
            DataContext = Base;
            Base.Size = size;
            Base.ChangeLeftPosition += Pion_ChangeLeftPosition;
            Base.ChangeTopPosition += Pion_ChangeTopPosition;

        }


        private void Pion_ChangeTopPosition(object sender, double value)
        {
            Canvas.SetTop(this, value - (Base.Size / 2));
        }

        private void Pion_ChangeLeftPosition(object sender, double value)                                      
        {
            Canvas.SetLeft(this, value - (Base.Size / 2));
        }

        public void SetVisibility(Visibility visible)
        {
            Base.ShowLabel = Visibility.Visible;
        }


        private void Ellipse_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnSelected?.Invoke(this);
        }

        public void PlayAnimation()
        {
            var storyboardRight = (Storyboard)TryFindResource("Storyboard1");
            storyboardRight.Begin();
            Base.IsSelected = true;
        }

        public void Stopanimation()
        {
            var storyboardRight = (Storyboard)TryFindResource("Storyboard1");
            storyboardRight.Stop();
            Base.IsSelected = false;
        }

        public void SetPosition(Node e)
        {
           Base.X = e.X;
            Base.Y = e.Y;
            Base.Position = e.Position;
            Base.OnNode = e.NodeName;
        }
    }


    public class PionViewModel:Pion
    {
        public event EventHandler<double> ChangeLeftPosition;
        public event EventHandler<double> ChangeTopPosition;
        private Visibility _visible;
        private Brush color;
        private bool _selected;

        public PionViewModel(int id, string nama, Brush color, int size)
        {
            BackColor = color;
            Id = id;
            Name = nama;
            Size = size;
           
        }

        public Brush BackColor {

            get { return color; }
            set
            {
                SetProperty(ref color, value);
            }

        }
        public Visibility ShowLabel
        {
            get { return _visible; }
            set
            {
                _visible = value;
            }
        }

        public bool IsSelected
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
            }
        }

        private int size;

       

        private double x;

        public double X
        {
            get { return x; }
            set {SetProperty(ref x ,value);
                ChangeLeftPosition.Invoke(this,value); }
        }

        private double y;

        public double Y
        {
            get { return y; }
            set {SetProperty(ref y ,value);
                ChangeTopPosition.Invoke(this,value);
            }
        }

         
       

    }


}
