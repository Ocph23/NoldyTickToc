using GameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MainApp
{
    public class ActualSize:BaseNotify
    {
        private Canvas mainCanvas;

        public ActualSize(Canvas mainCanvas)
        {
            this.mainCanvas = mainCanvas;
            Width = mainCanvas.ActualWidth;
            Height = mainCanvas.ActualHeight;
            HalfHeight = ((mainCanvas.ActualHeight) / 2);
            HalfWidth= ((mainCanvas.ActualWidth) / 2);
        }


        private double y1;
        private double y2;
        private double _xmidle;
        private double _ymidle;
        private double x1;
        private double x2;

        public double Width
        {
            get { return x1; }
            set { SetProperty(ref x1, value); }
        }


        public double Height
        {
            get { return x2; }
            set { SetProperty(ref x2, value); }
        }

        public double HalfHeight
        {
            get { return _xmidle; }
            set { SetProperty(ref _xmidle, value); }
        }

        public double HalfWidth
        {
            get { return _ymidle; }
            set { SetProperty(ref _ymidle, value); }
        }
    }
}
