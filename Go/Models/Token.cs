using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Models
{
    using System.Windows;
    using System.Windows.Controls;
    
    public enum TokenState
    {
        NONE,

        BLACK,

        WHITE
    };

    public class Token : Button
    {
        public Point Point { get; set; }

        private TokenState _state;

        public TokenState TokenState
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

        public Chain Chain { get; set; }

        public List<Token> SurroundingTokens { get; set; }

        public string TokenInformation
        {
            get
            {
                StringBuilder stb = new StringBuilder();
                return stb.AppendFormat("{0},{1},{2}", Point.X, Point.Y, TokenState).ToString();
            }
            set
            {
                TokenInformation = value;
            }
        }
    }
}
