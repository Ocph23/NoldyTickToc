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
    public class GreedyExpert:IEngine
    {
        private BoardBase boardBase;
        public GreedyExpert(BoardBase board)
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

            foreach (var pilihan in childs1.OrderByDescending(O => O.Number))
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

  


}
