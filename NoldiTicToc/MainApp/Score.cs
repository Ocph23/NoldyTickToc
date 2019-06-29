using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp
{
    [TableName("Score")]
    public class Score
    {
        [DbColumn("Id")]
        public int? Id { get; set; }

        [DbColumn("Player1")]
        public string Player1 { get; set; }

        [DbColumn("Player2")]
        public string Player2 { get; set; }

        [DbColumn("GameType")]
        public string GameType { get; set; }

        [DbColumn("Player1Win")]
        public int Player1Win { get; set; }

        [DbColumn("Player2Win")]
        public int Player2Win { get; set; }

        [DbColumn("Solutions")]
        public string Solutions { get; set; }

        [DbColumn("Time")]
        public string Time { get; set; }

    }
}
