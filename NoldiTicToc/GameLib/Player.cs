using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
   public class Player:BaseNotify
    {
        public Player(string code, PlayerType type)
        {
            this.Code = code; this.PlayerType = type;
        }
        public Player()
        {

        }

        private bool _isPlay;
        private PlayerType _playerType;

        public bool IsPlay { get { return _isPlay; } set { SetProperty(ref _isPlay, value); } }

         public PlayerType PlayerType {
            get { return _playerType; }
            set { SetProperty(ref _playerType, value); }
        }


        public ArrayList FirstState { get; set; } = new ArrayList();

       

        private string _code;
        private int[] _state;

        public string Code
        {
            get { return _code; }
            set {SetProperty(ref _code ,value); }
        }

        public int[] State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private string name;

        public string PlayerName
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }




    }
}
