using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Controlers
{
  using System.Windows;
  using System.Windows.Media;
    using Go.Controllers;
    using Go.Models;

    public class MoveController
    {
        

        public MoveResult Move(Player player, Token token, List<Token> tokensList)
        {
           PlayStateController _playStateController = new PlayStateController(tokensList);
            MoveResult result = new MoveResult();
            result = _playStateController.ValidateRules(player.PlayerState, token, tokensList);
            if (result.IsNormalMoveSuccess || result.IsMoveKill)
            {
                token.TokenState = player.PlayerState;
                token.Opacity = 100;
                if (token.TokenState == TokenState.BLACK)
                {
                    token.Background = Brushes.Black;
                }
                else
                {
                    token.Background = Brushes.White;
                }
                player.MoveCount++;
                if (result.IsMoveKill)
                    {
                    foreach (Chain deadChain in result.DeadChains)
                    {
                        foreach (Token deadToken in tokensList.Where(x => x.Chain == deadChain))
                        {
                            deadToken.TokenState = TokenState.NONE;
                            deadToken.Chain = null;
                            deadToken.Opacity = 0;
                        }
                    }
                }
            }
            else
            {
              MessageBox.Show("Spielzug konnte nicht ausgeführt werden.\nSie haben eine Regel verletzt.", "Regelverletzung", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            return result;
        }

        public void ShowToken(Token token)
        {
            token.Opacity = 100;
            if (token.TokenState == TokenState.BLACK)
            {
                token.Background = Brushes.Black;
            }
            else
            {
                token.Background = Brushes.White;
            }
        }
    }
}
