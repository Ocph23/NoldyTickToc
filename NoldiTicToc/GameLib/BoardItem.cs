using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public class BoardItem:IBoardItem
    {
        private string[,] BaseBoard;
        Dictionary<Vertex<string[,]>, int> index = new Dictionary<Vertex<string[,]>, int>();
        public List<BoardItem> Children { get; set; } = new List<BoardItem>();
        private string code;
        private int level;

        public BoardItem(int lenght)
        {
            BoardLength = lenght;
        }

        public BoardItem Parent { get; }
        public int BoardLength { get; }

        public Step GetStep(string[,] board, string[,] newdata)
        {
            ArrayList notsame = new ArrayList();
            Step step = new Step();
            for (var i = 0; i < BoardLength; i++)
            {
                for (var j = 0; j < BoardLength; j++)
                {
                    if (board[i, j] != newdata[i, j])
                        notsame.Add(new Tuple<int, int>(i, j));
                }
            }

            foreach (Tuple<int, int> item in notsame)
            {
                if (board[item.Item1, item.Item2] != null)
                {
                    step.From = item;
                }

                if (newdata[item.Item1, item.Item2] != null)
                {
                    step.To = item;
                }
            }

            return step;

        }
        public List<MatrixVertex<string[,]>> GetChilds(string[,] board, string code)
        {
            var items = CreateChilds(board, code);
            var list = new List<MatrixVertex<string[,]>>();
            var i = 0;
            foreach (var item in items)
            {
                i++;
                list.Add(new MatrixVertex<string[,]>() { Data = item.ToBoard, Step = item, Number=i });
            }
            return list;
        }

        private List<Step> CreateChilds(string[,] board, string xcode)
        {
            List<Step> childs = new List<Step>();
            for (int i = 0; i < BoardLength; i++)
            {
                for (int j = 0; j < BoardLength; j++)
                {
                    if (board[i, j] != null && board[i, j].Substring(0, 1) == xcode)
                    {
                        var items = GetChild(board, new Position(i, j), board[i, j]);
                        int count = 0;

                        foreach (var item in items)
                        {
                            //count++;
                            //if (!CheckedIfExists(item))
                            var step = GetStep(board, item);
                            step.FromNode = board[i, j];
                            step.FromBoard = board;
                            step.ToBoard = item;

                            childs.Add(step);
                        }
                    }
                }
            }

            return childs;
        }

        private bool CheckedIfExists(string[,] item)
        {
            var isExist = false;

            if (Parent != null)
            {
                if (EqualBoard(item, Parent.BaseBoard))
                    isExist = true;
                else if (Parent.Parent != null)
                {
                    foreach (var board in Parent.Parent.Children)
                    {
                        PrintBoard(board.BaseBoard, "parent");
                        if (EqualBoard(item, board.BaseBoard))
                        {
                            isExist = true;
                            break;
                        }

                        if (!isExist && board.Parent != null)
                        {
                            if (!EqualBoard(item, board.Parent.BaseBoard))
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("this Level : {0} To Parent Level :{1}", level, Parent.level));
                                isExist = board.CheckedIfExists(item);
                            }
                            else
                            {
                                isExist = true;
                            }
                        }

                    }
                }

            }

            System.Diagnostics.Debug.WriteLine("if Exixt On Level : " + level + " " + isExist);

            return isExist;
        }


        public void PrintBoard(string[,] item, string v)
        {
            //     System.Diagnostics.Debug.ForegroundColor = System.Diagnostics.DebugColor.Red;
            System.Diagnostics.Debug.WriteLine("\r");
            System.Diagnostics.Debug.WriteLine(v);
            for (int i = 0; i < BoardLength; i++)
            {
                for (int j = 0; j < BoardLength; j++)
                {
                    if (item[i, j] == null)
                        System.Diagnostics.Debug.Write(" - ");
                    else
                        System.Diagnostics.Debug.Write(item[i, j].Substring(0, 1) + "X ");

                }
                System.Diagnostics.Debug.Write("\r");
            }
            System.Diagnostics.Debug.WriteLine("\r");
        }


        public bool EqualBoard(string[,] item, string[,] board)
        {
            bool isequal = true;
            for (int i = 0; i < BoardLength; i++)
            {
                for (int j = 0; j < BoardLength; j++)
                {
                    if (item[i, j] != board[i, j])
                    {
                        isequal = false;
                    }
                    else if (item[i, j] != null && board[i, j] != null)
                    {
                        if (item[i, j].Substring(0, 1) != board[i, j].Substring(0, 1))
                        {
                            isequal = false;

                        }
                    }

                    if (!isequal)
                        break;

                }
                if (!isequal)
                    break;
            }

            return isequal;
        }

        public bool StartPosition(string[,] board)
        {
            int count = 0;
            bool result = false;
            if (code == "H")
            {
                for (int i = 0; i < BoardLength; i++)
                {
                    if (BaseBoard[0, i] != null && BaseBoard[2, i].Substring(0, 1) == "H")
                        count++;
                }
                if (count == BoardLength)
                    result = true;
            }
            else
            {
                for (int i = 0; i < BoardLength; i++)
                {
                    if (BaseBoard[2, i] != null && BaseBoard[2, i].Substring(0, 1) == "M")
                        count++;
                }
                if (count == BoardLength)
                    result = true;
            }

            return result;


        }




        private bool FoundStateSolution(List<string[,]> childs)
        {
            bool result = false;
            foreach (var item in childs)
            {

                for (int i = 0; i < BoardLength; i++)
                {
                    int row = 0;
                    int column = 0;
                    for (int j = 0; j < BoardLength; j++)
                    {
                        if (item[i, j] != null && item[i, j].Substring(0, 1) == code)
                            row++;

                        if (item[j, i] != null && item[j, i].Substring(0, 1) == code)
                            column++;
                    }

                    if ((row == BoardLength || column == BoardLength) && !StartPosition(item))
                    {
                        PrintBoard(item, "Solution");
                        result = true;
                        break;
                    }
                }

                if (result)
                    break;


            }
            return result;
        }

        private List<string[,]> GetChild(string[,] board, Position position, string pion)
        {
            List<string[,]> Childs = new List<string[,]>();
            //i++
            int i = position.Row + 1;
            if (i < BoardLength && board[i, position.Column] == null)
            {
                Childs.Add(CreateBoard(board, new Position(i, position.Column), position, pion));
            }

            i = position.Row - 1;
            if (i >= 0 && board[i, position.Column] == null)
            {
                Childs.Add(CreateBoard(board, new Position(i, position.Column), position, pion));
            }


            //j++
            var j = position.Column + 1;
            if (j < BoardLength && board[position.Row, j] == null)
            {
                Childs.Add(CreateBoard(board, new Position(position.Row, j), position, pion));
            }


            //j--
            j = position.Column - 1;
            if (j >= 0 && board[position.Row, j] == null)
            {
                Childs.Add(CreateBoard(board, new Position(position.Row, j), position, pion));
            }





            //to right Horizontal
            i = position.Row + 1;
            j = position.Column + 1;
            if (i < BoardLength && j < BoardLength && board[i, j] == null)
            {
                if ((i != 1 && j != 1) && (position.Row == 1 && position.Column == 1))
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
                else if (position.Row == 1 && position.Column == 1)
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
            }


            //to right Horizontal
            i = position.Row - 1;
            j = position.Column - 1;
            if (i >= 0 && j >= 0 && board[i, j] == null)
            {
                if (position.Row == position.Column)
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
            }

            //to left Horizontal to botom
            i = position.Row + 1;
            j = position.Column - 1;
            if (i < BoardLength && j >= 0 && board[i, j] == null)
            {
                if ((i != 1 && j != 1) && (position.Row == 1 && position.Column == 1))
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
                else if (position.Row == 1 && position.Column == 1)
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));

            }

            //to left Horizontal to up
            i = position.Row - 1;
            j = position.Column + 1;
            if (i >= 0 && j < BoardLength && board[i, j] == null)
            {
                if ((i != 1 && j != 1) && (position.Row == 1 && position.Column == 1))
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
                else if (position.Row - 1 == 1 && position.Column + 1 == 1)
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
            }

            return Childs;

        }

        private string[,] CreateBoard(string[,] board, Position newPosition, Position oldPosition, string name)
        {
            var b = (string[,])board.Clone();

            b[newPosition.Row, newPosition.Column] = name;
            b[oldPosition.Row, oldPosition.Column] = null;
            return b;
        }


    }



}
