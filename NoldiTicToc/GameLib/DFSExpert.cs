using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public class DFSExpert:IEngine
    {
        private BoardBase boardBase;
        public DFSExpert(BoardBase board)
        {
            boardBase = board;
        }

        public Task<Tuple<Step, ArrayList>> Analysis(string[,] board, Player player)
        {
            Stopwatch stopWach = new Stopwatch();
            stopWach.Start();
            Option = new List<List<MatrixVertex<string[,]>>>();
            boardBase.BoardData = new BoardItemExpert(boardBase.BoardLengt);

            Player opponent = player.Code == boardBase.Player1.Code ? boardBase.Player2 : boardBase.Player1;

            var childs1 = boardBase.BoardData.GetChilds(board, player.Code);

            foreach (var pilihan in childs1)
            {
                List<int> vertices = new List<int>();
                List<Tuple<int, int>> edges = new List<Tuple<int, int>>();
                List<MatrixVertex<string[,]>> datas = new List<MatrixVertex<string[,]>>();
                bool complete = false;
                int vertex = 0;
                int index = 0;
                var mypos = boardBase.GetItemsAsPositionName(pilihan, player.Code).ToArray(typeof(int)) as int[];
                if (!boardBase.IsFirstState(mypos, player) && boardBase.IsGoal(mypos))
                {

                    pilihan.Index = index;
                    pilihan.Vertext = vertex;
                    vertices.Add(pilihan.Index);
                    edges.Add(new Tuple<int, int>(pilihan.Index, pilihan.Index));
                    datas.Add(pilihan);
                    boardBase.GoalFound = pilihan;

                }
                else
                {
                    pilihan.Vertext = vertex;
                    vertices.Add(0);
                    edges.Add(new Tuple<int, int>(0, 0));
                    datas.Add(pilihan);
                    while (!complete)
                    {
                        //opponen step
                        var d = datas.Where(O => O.Vertext == vertex).ToList();
                        vertex++;
                        foreach (var itemD in d)
                        {
                            System.Diagnostics.Debug.WriteLine("Langkah Lawan");
                            //if Not Goal open Opponent Child

                            var dataForAnalysis =  (string[,])itemD.Data.Clone();
                            CleanOpponentItem(dataForAnalysis,opponent.Code);
                            var opponentMove = boardBase.BoardData.GetChilds(dataForAnalysis, player.Code);
                            foreach (var a in opponentMove)
                            {
                                //var opp = GetItemsAsPositionName(a, opponent.Code).ToArray(typeof(int)) as int[];
                                var myp = boardBase.GetItemsAsPositionName(a, player.Code).ToArray(typeof(int)) as int[];

                                if(boardBase.IsGoal(myp) && !boardBase.IsFirstState(myp,player))
                                {
                                    index++;
                                    a.Index = index;
                                    a.Vertext = vertex;
                                    vertices.Add(a.Index);
                                    edges.Add(new Tuple<int, int>(itemD.Index, a.Index));
                                    datas.Add(a);
                                    boardBase.GoalFound = a;
                                    complete = true;
                                    break;
                                }  
                                else if (!IsExist(a, 0))
                                {
                                    index++;
                                    a.Index = index;
                                    a.Vertext = vertex;
                                    vertices.Add(a.Index);
                                    edges.Add(new Tuple<int, int>(itemD.Index, a.Index));
                                    datas.Add(a);

                                }

                            }

                        }

                         
                        if(datas.Count>1500)
                        {
                            ViewDatas(vertices, datas,edges);
                        }
                    }
                }



                bool IsExist(MatrixVertex<string[,]> b, int start)
                {
                    var graph = new Graph<int>(vertices, edges);
                    var algorithms = new Algorithms();

                    var path = new List<int>();
                    algorithms.Find(graph, start, v =>
                    {
                        var res = datas.Where(O => O.Index == v).FirstOrDefault();
                        if (res != null && boardBase.BoardData.EqualBoard(b.Data, res.Data))
                        {
                            path.Add(v);
                            algorithms.Found = true;

                        }
                    });

                    return path.Count > 0 ? true : false;
                }



                var graphs = new Graph<int>(vertices, edges);
                var algorithmss = new Algorithms();

                var paths = new List<int>();
                algorithmss.DFS(graphs, 0, v =>
                {
                    System.Diagnostics.Debug.WriteLine(v);
                    var res = datas.Where(O => O.Index == v).FirstOrDefault();
                    if (res != null && boardBase.BoardData.EqualBoard(boardBase.GoalFound.Data, res.Data))
                    {
                        paths.Add(v);
                    }
                });


                var x = paths.FirstOrDefault();
                var dataa = datas.Where(O => O.Index == x).FirstOrDefault();
                if (dataa != null)
                {
                    List<MatrixVertex<string[,]>> al = new List<MatrixVertex<string[,]>>();
                    al.Add(dataa);
                    while (x != 0)
                    {
                        var a = edges.Where(O => O.Item2 == x).FirstOrDefault();
                        x = a.Item1;
                        var data = datas.Where(O => O.Index == x).FirstOrDefault();
                        if (data != null)
                        {
                            al.Add(data);
                        }


                    }

                    Option.Add(al);
                }



            }



            var aa = Option.OrderBy(O => O.Count).FirstOrDefault();
            var xx = aa.OrderBy(O => O.Index).FirstOrDefault();


            ArrayList listResult = new ArrayList();
            var bbb = (string[,])board.Clone();
            foreach (var item in aa.OrderBy(O => O.Index))
            {
                listResult.Add(item.Step);
            }


            var result = boardBase.BoardData.GetStep(board, xx.Data);
            stopWach.Stop();
            result.TimeAnalisys = stopWach.Elapsed;
            return Task.FromResult(new Tuple<Step, ArrayList>(result, listResult));
        }

        private string GetStringData(string[,] data)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < boardBase.BoardLengt; i++)
            {
                for (var j = 0; j < boardBase.BoardLengt; j++)
                {
                    sb.Append(data[i, j] != null ? data[i, j] : "X");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        private void CleanOpponentItem(string[,] dataForAnalysis, string oppCode)
        {
            for(int i=0;i< boardBase.BoardLengt;i++)
            {
                for (int j = 0; j < boardBase.BoardLengt; j++)
                {
                      if(dataForAnalysis[i, j]!=null && dataForAnalysis[i,j].Contains(oppCode))
                    {
                        dataForAnalysis[i, j] = null;
                    }
                }
            }
        }

        private void ViewDatas(List<int> vertices, List<MatrixVertex<string[,]>> datas, List<Tuple<int, int>> edges)
        {

            ArrayList list = new ArrayList();
            foreach (var item in vertices)
            {
                var da = datas.Where(O => O.Index == item).FirstOrDefault();
                if (da != null)
                    list.Add(new
                    {
                        id = item,
                        label = GetStringData(da.Data),
                        shape = "box"
                    });
            }

            var ress = JsonConvert.SerializeObject(list);

            ArrayList edg = new ArrayList();
            foreach (var item in edges)
            {
                edg.Add(new { from = item.Item1, to = item.Item2 });
            }
            var ed = JsonConvert.SerializeObject(edg);
            var ddd = JsonConvert.SerializeObject(datas);
        }

      
        List<List<MatrixVertex<string[,]>>> Option = new List<List<MatrixVertex<string[,]>>>();
      

      

       
    }


    public class BoardItemExpert : IBoardItem
    {
        private string[,] BaseBoard;
        Dictionary<Vertex<string[,]>, int> index = new Dictionary<Vertex<string[,]>, int>();
        public List<BoardItemExpert> Children { get; set; } = new List<BoardItemExpert>();
        private string code;
        private int level;

        public BoardItemExpert(int lenght)
        {
            BoardLength = lenght;
        }

        public BoardItemExpert Parent { get; }
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
                list.Add(new MatrixVertex<string[,]>() { Data = item.ToBoard, Step = item, Number = i });
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
                    var itemValue = item[i, j]!=null? item[i, j].Substring(0,1):null;
                    var boardValue = board[i, j] != null ? board[i, j].Substring(0, 1) : null;
                    if (itemValue != boardValue)
                        isequal = false;

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
            //i++ Colomn Stay
            int i = position.Row +2;
            if ( i < BoardLength && board[i, position.Column] == null)
            {
                Childs.Add(CreateBoard(board, new Position(i, position.Column), position, pion));
            }

            i = position.Row - 2;
            if ((position.Row % 2 == 0)&& i >= 0 && board[i, position.Column] == null)
            {
                Childs.Add(CreateBoard(board, new Position(i, position.Column), position, pion));
            }


            //j++    row stay
            var j = position.Column + 2;
            if ((position.Column%2==0) && j < BoardLength && board[position.Row, j] == null)
            {
                Childs.Add(CreateBoard(board, new Position(position.Row, j), position, pion));
            }


            //j--
            j = position.Column - 2;
            if (j >= 0 && board[position.Row, j] == null)
            {
                Childs.Add(CreateBoard(board, new Position(position.Row, j), position, pion));
            }





            //to right Horizontal
            i = position.Row + 1;
            j = position.Column + 1;

            if (IsNode(i,j) &&   i < BoardLength && j < BoardLength && board[i, j] == null)
            {
                Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
            }


            //to right Horizontal
            i = position.Row - 1;
            j = position.Column - 1;
            if (IsNode(i, j) && i >= 0 && j >= 0 && board[i, j] == null)
            {
        
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
            }

            //to left Horizontal to botom
            i = position.Row + 1;
            j = position.Column - 1;
            if (IsNode(i, j) && i < BoardLength && j >= 0 && board[i, j] == null)
            {
              
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));

            }

            //to left Horizontal to up
            i = position.Row - 1;
            j = position.Column + 1;
            if (IsNode(i, j) && i >= 0 && j < BoardLength && board[i, j] == null)
            {
            
                    Childs.Add(CreateBoard(board, new Position(i, j), position, pion));
            }

            return Childs;

        }


        List<Tuple<int, int>> ListNotNode = new List<Tuple<int, int>> {
             new Tuple<int, int>(0,1),
              new Tuple<int, int>(0,3),

               new Tuple<int, int>(1,0),
                new Tuple<int, int>(1,2),
                 new Tuple<int, int>(1,4),

                  new Tuple<int, int>(2,1),
                   new Tuple<int, int>(2,3),

                    new Tuple<int, int>(3,0),
                     new Tuple<int, int>(3,2),
                     new Tuple<int, int>(3,4),

                     new Tuple<int, int>(4,1),
                     new Tuple<int, int>(4,3),
        };

        private bool IsNode(int i, int j)
        {
            var data = ListNotNode.Where(O => O.Item1 == i && O.Item2 == j).FirstOrDefault();
            return  data== null ? true : false;
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
