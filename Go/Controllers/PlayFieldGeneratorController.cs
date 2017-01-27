using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Go.Controlers
{
    using System.Windows;

    using Go.Models;

    class PlayFieldGeneratorController
    {
        public List<Token> CreateButtons(int lines)
        {
            List<Token> tokensList = new List<Token>();

            for (int x = 0; x < lines; x++)
            {
                for (int y = 0; y < lines; y++)
                {
                    Token button = new Token();
                    button.Width = 30;
                    button.Height = 30;
                    button.Opacity = 0;
                    button.Point = new Point(x, y);
                    button.SetValue(Grid.ColumnProperty, x);
                    button.SetValue(Grid.RowProperty, y);
                    button.Chain = new Chain();
                    tokensList.Add(button);
                }
            }
            foreach (Token token in tokensList)
            {
                token.SurroundingTokens = SetSurroundingTokens(tokensList, token);
            }
            return tokensList;
        }

        public List<Token> GetButtons(string[] informationString, List<Token> tokensList)
        {
            double x = Convert.ToDouble(informationString[0]);
            double y = Convert.ToDouble(informationString[1]);
            TokenState tokenState = TokenState.NONE;
            switch (informationString[2])
            {
                case "NONE":
                    tokenState = TokenState.NONE;
                    break;
                case "WHITE":
                    tokenState = TokenState.WHITE;
                    break;
                case "BLACK":
                    tokenState = TokenState.BLACK;
                    break;
            }
            Token button = new Token();
            button.TokenState = tokenState;
            button.Width = 30;
            button.Height = 30;
            button.Opacity = 0;
            button.Point = new Point(x, y);
            button.SetValue(Grid.ColumnProperty, Convert.ToInt32(x));
            button.SetValue(Grid.RowProperty, Convert.ToInt32(y));
            button.Chain = new Chain() {TokenChainState = tokenState};
            tokensList.Add(button);
            return tokensList;
        }

        public List<Token> SetSurroundingTokens(List<Token> tokensList, Token token)
        {
            List<Token> surroundingTokensList = new List<Token>();
            if (token.Point.Y == 0)
            {
                // Ist ganz oben
                if (token.Point.X == 0)
                {
                    // Ist ganz oben - Am Rand(links)
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X + 1 && x.Point.Y == token.Point.Y));
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y + 1 && x.Point.X == token.Point.X));
                }
                else if (token.Point.X == 18)
                {
                    // Ist ganz oben - Am Rand (rechts)
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X - 1 && x.Point.Y == token.Point.Y));
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y + 1 && x.Point.X == token.Point.X));
                }
                else
                {
                    // Ist ganz oben - Irgendwo
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X - 1 && x.Point.Y == token.Point.Y));
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X + 1 && x.Point.Y == token.Point.Y));
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y + 1 && x.Point.X == token.Point.X));
                }
            }
            else if (token.Point.Y == 18)
            {
                // Ist ganz unten
                if (token.Point.X == 0)
                {
                    // Ist ganz unten - Am Rand(links)
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X + 1 && x.Point.Y == token.Point.Y));
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y - 1 && x.Point.X == token.Point.X));
                }
                else if (token.Point.X == 18)
                {
                    // Ist ganz unten - Am Rand (rechts)
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X - 1 && x.Point.Y == token.Point.Y));
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y - 1 && x.Point.X == token.Point.X));
                }
                else
                {
                    // Ist ganz unten - Irgendwo
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X - 1 && x.Point.Y == token.Point.Y));
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X + 1 && x.Point.Y == token.Point.Y));
                    surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y - 1 && x.Point.X == token.Point.X));
                }
            }
            else if (token.Point.X == 0)
            {
                // Ist ganz links am Rand - Irgendwo
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X + 1 && x.Point.Y == token.Point.Y));
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y + 1 && x.Point.X == token.Point.X));
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y - 1 && x.Point.X == token.Point.X));
            }
            else if (token.Point.X == 18)
            {
                // Ist ganz rechts am Rand - Irgendwo
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y + 1 && x.Point.X == token.Point.X));
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y - 1 && x.Point.X == token.Point.X));
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X - 1 && x.Point.Y == token.Point.Y));
            }
            else
            {
                // Ist irgendwo
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X + 1 && x.Point.Y == token.Point.Y));
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.X == token.Point.X - 1 && x.Point.Y == token.Point.Y));
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y - 1 && x.Point.X == token.Point.X));
                surroundingTokensList.Add(tokensList.FirstOrDefault(x => x.Point.Y == token.Point.Y + 1 && x.Point.X == token.Point.X));
            }
            return surroundingTokensList;
        }

        public List<ColumnDefinition> GetColumns(int size)
        {
            List<ColumnDefinition> colList = new List<ColumnDefinition>();
            for (int i = 0; i < size - 1; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(39.5);
                colList.Add(column);
            }
            return colList;
        }

        public List<RowDefinition> GetRows(int size)
        {
            List<RowDefinition> rowList = new List<RowDefinition>();
            for (int i = 0; i < size - 1; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(39.5);
                rowList.Add(row);
            }
            return rowList;
        }
    }
}
