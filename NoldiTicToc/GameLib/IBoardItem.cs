using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public interface IBoardItem
    {
        Step GetStep(string[,] board, string[,] newdata);
        List<MatrixVertex<string[,]>> GetChilds(string[,] board, string code);
        void PrintBoard(string[,] item, string v);
        bool EqualBoard(string[,] item, string[,] board);
        bool StartPosition(string[,] board);
    }
}
