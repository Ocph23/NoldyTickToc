using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{

    public class Node : BaseNotify
    {
        private int width;
        public event EventHandler<double> ChangeLeftPosition;
        public event EventHandler<double> ChangeTopPosition;
        public Position Position { get; set; }
        public int Width
        {
            get { return width; }
            set { SetProperty(ref width, value); }
        }

        private int height;

        public Node(int width)
        {
            Width = width;
            Height = width;
        }
        public Node(int n, Position p)
        {
            NodeName = n;
            Position = p;
        }

        public int Height
        {
            get { return height; }
            set { SetProperty(ref height, value); }
        }


        private double top;

        public double Y
        {
            get { return top; }
            set
            {
                SetProperty(ref top, value);
                ChangeTopPosition.Invoke(this,value);
            }
        }

        private double left;

        public double X
        {
            get { return left; }
            set { SetProperty(ref left, value); ChangeLeftPosition.Invoke(this,value); }
        }


        private int nodeName;

        public int NodeName
        {
            get { return nodeName; }
            set { nodeName = value; }
        }



    }
}
