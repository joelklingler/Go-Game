using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Models
{
    public class Player
    {
        private TokenState _playerState;

        public Player()
        {
            this.Score = 0;
            this.MoveCount = 0;
        }

        public string PlayerName { get; set; }

        public int Score { get; set; }

        public bool IsActive { get; set; }

        public int MoveCount { get; set; }

        public TokenState PlayerState
        {
            get
            {
                return _playerState;
            }
            set
            {
                _playerState = value;
            }
        }
    }
}
