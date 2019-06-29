using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
   public class BoardBaseExpert
    {
        public int BoardLengt { get; }
        public Player Player1 { get; }
        public Player Player2 { get; }
        public BoardItemExpert Board { get; private set; }
        public List<Node> Nodes { get; set; }
        public MatrixVertex<string[,]> GoalFound { get; private set; }

        List<int[]> GoalStates = new List<int[]>
        { new[] { 0, 1, 2 },new[] { 2, 7, 12 }, new[] { 0,5,10 }, new[] { 10,11,12 },
            new[] { 2, 6,10 }, new[] { 0, 6, 12} };


        public BoardBaseExpert(int boardLenghat)
        {
            BoardLengt = boardLenghat;
            Nodes = new List<Node>();

        }

    }
}
