using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public class BoardBase
    {
        public int BoardLengt { get; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public IBoardItem BoardData { get; set; }
        public List<Node> Nodes { get; set; }
        public bool IsBeginer { get; }
        public MatrixVertex<string[,]> GoalFound { get; set; }
        public IEngine Engine { get; private set; }

        List<int[]> goalStates;

        public BoardBase(List<int[]> goals, int boardLenghat, bool beginer)
        {
            BoardLengt = boardLenghat;
            goalStates = goals;
            Nodes = new List<Node>();
            IsBeginer = beginer;

        }

        public void SetPlayers(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
        }

        public bool IsWinner(string[,] board, Player player)
        {
            var item = new MatrixVertex<string[,]>();
            item.Data = board;

            var playItem = GetItemsAsPositionName(item, player.Code).ToArray(typeof(int)) as int[];
            var opponent = GetOpponentItems(item, player.Code).ToArray(typeof(int)) as int[];
            if (!IsFirstState(playItem, player) &&IsGoal(playItem,opponent))
                return true;
            return false;




        }

        public ArrayList GetOpponentItems(MatrixVertex<string[,]> b, string code)
        {
            ArrayList foundItems = new ArrayList();
            for (var i = 0; i < BoardLengt; i++)
            {
                for (var j = 0; j < BoardLengt; j++)
                {
                    if (b.Data[i, j] != null && b.Data[i, j].Substring(0, 1) != code)
                    {
                        var n = Nodes.Where(O => O.Position.Row == i && O.Position.Column == j).FirstOrDefault();
                        if (n != null)
                            foundItems.Add(n.NodeName);

                    }
                }
            }

            return foundItems;

        }


        public ArrayList GetItemsAsPositionName(MatrixVertex<string[,]> b, string code)
        {
            ArrayList foundItems = new ArrayList();
            for (var i = 0; i < BoardLengt; i++)
            {
                for (var j = 0; j < BoardLengt; j++)
                {
                    if (b.Data[i, j] != null && b.Data[i, j].Substring(0, 1) == code)
                    {
                        var n = Nodes.Where(O => O.Position.Row == i && O.Position.Column == j).FirstOrDefault();
                        if (n != null)
                            foundItems.Add(n.NodeName);

                    }
                }
            }

            return foundItems;



        }

        public bool IsGoal(int[] data, int[] opponent)
        {
            var isfound = false;
            foreach (var item in goalStates)
            {
                var found = true;
                for (var i = 0; i < data.Length; i++)
                {
                    if (data[i] != item[i])
                    {
                        found = false;
                        break;
                    }
                }

                if (item.Length>3)
                {
                   foreach(var item1 in opponent)
                    {
                        if(item1==item[3]||item1==item[4])
                        {
                            found = false;
                            break;
                        }

                    }
                }
              

                if (found)
                {
                    isfound = true;
                    break;
                }
            }
            return isfound;
        }

        public bool IsGoal(int[] data)
        {
            var isfound = false;
            foreach (var item in goalStates)
            {
                var found = true;
                for (var i = 0; i < data.Length; i++)
                {
                    if (data[i] != item[i])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    isfound = true;
                    break;
                }
            }
            return isfound;
        }

        public bool IsFirstState(int[] data, Player player)
        {
            int[] state = player.FirstState.ToArray(typeof(int)) as int[];
            var found = true;
            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] != state[i])
                {
                    found = false;
                    break;
                }
            }

            return found;
        }

        public void SetEngine(MethodType engine)
        {
            if(IsBeginer)
            {
                if (engine == MethodType.DFS)
                    Engine = new DFS(this);
                else
                    Engine = new Greedy(this);
            }   else
            {
                if (engine == MethodType.DFS)
                    Engine = new DFSExpert(this);
                else
                    Engine = new GreedyExpert(this);
            }
           
        }

        public Task<Tuple<Step, ArrayList>> Get(string[,] board, Player player)
        {
            return Engine.Analysis(board, player);
        }


    }
}
