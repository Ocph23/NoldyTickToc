using System;

namespace GameLib
{
    public class Pion : BaseNotify,ICloneable
    {

       

        private Position _position = new Position(0,0);
        private string nama;
        private int _id;

        public int Id {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        public string Name
        {
            get
            {
                return nama;
            }
            set
            {
                SetProperty(ref nama, value);
            }
        }


        public string NameView
        {
            get
            {
                return Name + Id;
            }
        }


     

        public Position Position
        {
            get { return _position; }
            set
            {
                SetProperty(ref _position, value);
            }
        }

        private int onNOde;
        private int size;

        public int OnNode
        {
            get { return onNOde; }
            set {SetProperty(ref onNOde ,value); }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Pion Copy()
        {
            return (Pion)Clone();
        }

        public int Size
        {
            get { return size; }
            set { SetProperty(ref size, value); }
        }

       
    }

}