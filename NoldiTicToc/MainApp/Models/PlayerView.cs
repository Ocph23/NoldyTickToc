using MainApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GameLib
{
    public class PlayerView:Player
    {
        public List<PionVIew> Pions { get; set; } = new List<PionVIew>();
        public int Win { get;  set; }

        public PlayerView(string name,PlayerType playerType)
        {
            PlayerType = playerType;
            Code = name;
            
          
        }

        public void AddPion(PionVIew pion)
        {
            Pions.Add(pion);
            SetState();

        }

        internal bool IsPion(PionVIew pion)
        {
            return Pions.Contains(pion);
        }

        private void SetState()
        {
            List<int> list = new List<int>();
            foreach(var item in Pions.OrderBy(O=>O.Base.OnNode))
            {
                list.Add(item.Base.OnNode);
            }
            State= list.ToArray();
        }

        public async void Play(Node node, PionVIew pion)
        {
           
            MoveAnimation(new Point(node.X, node.Y), pion);
            await Task.Delay(TimeSpan.FromSeconds(1));
            pion.SetPosition(node);
            SetState();
            pion.Stopanimation();
        }

        private void MoveAnimation(Point p, PionVIew pion)
        {
            var storyboard = new Storyboard();

          
           // var margin = new Thickness(p.X - (pion.Base.Size / 2), p.Y - (pion.Base.Size / 2), pion.Margin.Right,pion.Margin.Bottom);
           // margin.Left = p.X;
         //   ThicknessAnimation xAnimationx = new ThicknessAnimation(margin, TimeSpan.FromSeconds(1));

        DoubleAnimation xAnimation = new DoubleAnimation(p.X - (pion.Base.Size / 2), TimeSpan.FromSeconds(1));

        DoubleAnimation yAnimation = new DoubleAnimation(p.Y - (pion.Base.Size / 2), TimeSpan.FromSeconds(1));

            pion.BeginAnimation(Canvas.LeftProperty, xAnimation);
            pion.BeginAnimation(Canvas.TopProperty, yAnimation);
        }

    }
}
