using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
  public  class Step
    {
        public Tuple<int,int> From { get; set; }
        public string FromNode { get; set; }
        public Tuple<int, int> To { get; set; }
        public string ToNode { get; set; }
        public string[,] FromBoard { get; internal set; }
        public string[,] ToBoard { get; internal set; }
        public TimeSpan TimeAnalisys { get;  set; }
    }
}
