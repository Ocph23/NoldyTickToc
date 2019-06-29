namespace GameLib
{
    public class Position:BaseNotify
    {
        private int _row;
        private int _column;

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Position(int index,int row, int column)
        {
            Index = index;
            Row = row;
            Column = column;
        }

        public int Index { get; set; }

        public int Row {
            get { return _row; }
            set
            {
                SetProperty(ref _row, value);
            }
        }

        public int Column
        {
            get { return _column; }
            set
            {
                SetProperty(ref _column, value);
            }
        }
    }
}