using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Models
{
    public class Chain
    {
        private TokenState _state;

        public TokenState TokenChainState
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }
    }
}
