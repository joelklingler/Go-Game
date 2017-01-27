using System.Collections.Generic;
using System.Linq;
using Go.Models;

namespace Go.Controllers
{
  using System;
  using System.Windows;
  using System.Windows.Media.Animation;

  using Microsoft.Win32.SafeHandles;

  public class PlayStateController
    {
        #region Private Fields

        private List<Token> _tokensList;

        private List<Chain> _deadChains;

        #endregion

      public PlayStateController(List<Token> tokensList)
      {
        _tokensList = tokensList;
      }

        #region Methods

        public MoveResult ValidateRules(TokenState playerState, Token token, List<Token> tokensList)
        {
            _tokensList = tokensList;
            MoveResult result = new MoveResult() {IsNormalMoveSuccess = true};

            #region Stein Status
            // Validate PlayStone State (Wurde der Stein bereits angeklickt?)
            result = IsPlayStoneFree(token);
            #endregion

            #region Ketten
            result.KillCount = ValidateChains(playerState, token);
            if (result.KillCount > 0)
            {
                result.IsMoveKill = true;
                result.DeadChains = _deadChains;
            }
            #endregion

            #region Selbstmordregel
            // Validate PlayStone Liberties (Besitzt der Stein noch Freiheiten?)
            if (!ChainGotLiberties(token.Chain))
            {
                result.IsRuleConflict = true;
            }
            #endregion

            return result;
        }

        private MoveResult IsPlayStoneFree(Token token)
      {
        MoveResult result = new MoveResult() { IsNormalMoveSuccess = true };
        if (token.TokenState != TokenState.NONE)
        {
          result.IsRuleConflict = true;
          return result;
        }
        return result;
      }

        private int ValidateChains(TokenState state, Token playToken)
        {
            int i = 0;
            FindAndSetChain(state, playToken);
            if (CheckIfKilled(state, playToken))
            {

                foreach (Chain deadChain in _deadChains)
                {
                    foreach (Token deadToken in _tokensList.Where(x=>x.Chain == deadChain))
                    {
                        i++;
                    }
                }
            }
            return i;
        }

        private bool CheckIfKilled(TokenState state, Token playToken)
        {
            List<Chain> deadChainsList = new List<Chain>();
            int i = 0;
            List<Chain> enemyChainsList = new List<Chain>();
            foreach (Token enemyToken in playToken.SurroundingTokens.Where(x=>x.TokenState != TokenState.NONE && x.TokenState != state))
            {
                enemyChainsList.Add(enemyToken.Chain);
            }
            foreach (Chain enemyChain in enemyChainsList)
            {
                if (ChainGotLiberties(enemyChain))
                {
                    i++;
                }
                else
                {
                    deadChainsList.Add(enemyChain);
                }
            }
            _deadChains = deadChainsList;
            if (enemyChainsList.Count == 0)
            {
                return false;
            }
            else
            {
                return i == 0;
            }
        }

        public void FindAndSetChain(TokenState state, Token playToken)
        {
            List<Token> surroundingFriendlys = playToken.SurroundingTokens.Where(x => x.TokenState == state).ToList();
            if (surroundingFriendlys.Count == 0)
            {
                return;
            }
            if (surroundingFriendlys.Count == 1)
            {
                Token singleFriendlyToken = surroundingFriendlys.First();
                Chain chain = new Chain { TokenChainState = state };
                _tokensList.Remove(singleFriendlyToken);
                _tokensList.Remove(playToken);
                singleFriendlyToken.Chain = chain;
                playToken.Chain = chain;
                _tokensList.Add(singleFriendlyToken);
                _tokensList.Add(playToken);
            }
            else
            {
                List<Chain> chainsList = new List<Chain>();
                foreach (Token token in surroundingFriendlys)
                {
                    chainsList.Add(token.Chain);
                }
                if(chainsList.Count < 1)
                {
                    Chain newChain = new Chain() { TokenChainState = state };
                    foreach (Token token in surroundingFriendlys)
                    {
                        _tokensList.Remove(token);
                        token.Chain = newChain;
                        _tokensList.Add(token);
                    }
                    _tokensList.Remove(playToken);
                    playToken.Chain = newChain;
                    _tokensList.Add(playToken);
                }
                else
                {
                    Chain mergedChain = chainsList.First();
                    foreach (Chain chain in chainsList)
                    {
                        foreach (Token token in _tokensList.Where(x => x.Chain == chain))
                        {
                            token.Chain = mergedChain;
                        }
                    }
                    foreach (Token friendlyToken in surroundingFriendlys)
                    {
                        if (friendlyToken.Chain != mergedChain)
                        {
                            _tokensList.Remove(friendlyToken);
                            friendlyToken.Chain = mergedChain;
                            _tokensList.Add(friendlyToken);
                        }
                    }
                    _tokensList.Remove(playToken);
                    playToken.Chain = mergedChain;
                    _tokensList.Add(playToken);
                }
            }
        }

        public List<Token> FindAndSetChains()
        {
          List<Token> updatedTokensList = new List<Token>();

          foreach (Token token in _tokensList)
          {
            if (token.TokenState == TokenState.NONE)
            {
              updatedTokensList.Add(token);
            }
            else
            {
              var surroundingFriendlys = token.SurroundingTokens.Where(x => x.TokenState == token.TokenState);
              if (surroundingFriendlys.Count() == 0)
              {
                if (updatedTokensList.Contains(token))
                {
                  updatedTokensList.Remove(token);
                }
                updatedTokensList.Add(token);
              }
              else if (surroundingFriendlys.Count() == 1)
              {
                var friendly = surroundingFriendlys.First();
                Chain newChain = new Chain();
                if (updatedTokensList.Contains(friendly))
                {
                  updatedTokensList.Remove(friendly);
                }
                if (updatedTokensList.Contains(token))
                {
                  updatedTokensList.Remove(token);
                }
                friendly.Chain = newChain;
                token.Chain = newChain;
                updatedTokensList.Add(friendly);
                updatedTokensList.Add(token);
              }
            }
          }

          return updatedTokensList;
        }

        private bool ChainGotLiberties(Chain chain)
        {
            int i = 0;
            foreach (Token token in _tokensList.Where(x=>x.Chain == chain))
            {
                i = i + token.SurroundingTokens.Where(x => x.TokenState == TokenState.NONE).Count();
            }
            return i > 1;
        }

        #endregion
    }
}
