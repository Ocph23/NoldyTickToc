using GameLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for NodeView.xaml
    /// </summary>
    /// 

    public partial class NodeView : UserControl,INotifyPropertyChanged
    {
        private Node _base;

        public event EventHandler<Node> OnSelected;
        public event PropertyChangedEventHandler PropertyChanged;

        public int NodeName
        {
            get { return Base.NodeName; }
            set {
                Base.NodeName = value;
            }
        }


        public NodeView()
        {
            InitializeComponent();
            Base = new Node(40);
            
            Base.Position = new Position(2,2);
            Base.ChangeLeftPosition += Node_ChangeLeftPosition;
            Base.ChangeTopPosition += Node_ChangeTopPosition;
            DataContext = Base;
        }

        public NodeView(int width, int name,Position postion)
        {
            InitializeComponent();
            Base = new Node(width);
            Base.NodeName=name;
            Base.Position = postion;
            Base.ChangeLeftPosition += Node_ChangeLeftPosition;
            Base.ChangeTopPosition += Node_ChangeTopPosition;
            DataContext = Base;
        }

        private void Node_ChangeTopPosition(object sender, double value)
        {
            Canvas.SetTop(this, value-(Base.Width/2));
        }

        private void Node_ChangeLeftPosition(object sender, double value)
        {
            Canvas.SetLeft(this, value - (Base.Width/ 2));
        }

        private void SetPosition(double l,double t)
        {
           Canvas.SetLeft(this, l);
            Canvas.SetTop(this, t);
        }

        public Node Base {
            get { return _base; }
            set { SetProperty(ref _base, value); }
        }
     

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnSelected?.Invoke(this,Base);
        }





        public bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName]string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        #region INotifyPropertyChanged



        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion



    }




}
