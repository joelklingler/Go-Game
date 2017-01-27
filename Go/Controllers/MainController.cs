using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using Go.Models;
using Go.Views;

namespace Go.Controlers
{
    using System.IO;
    using System.Windows.Forms.VisualStyles;
    using System.Windows.Media;

    using Go.Controllers;

    public class MainController
    {
        private int _moveCount = 0;

        private int _deadStones = 0;

        private bool _IsGameActive = true;

        private int _skipCount = 0;

        private Player _playerOne;

        private Player _playerTwo;

        private MoveController _mvController = new MoveController();

        private List<Token> _tokensList = new List<Token>();

        public const int _PLAYFIELDSIZE = 19;

        public const int _GRIDLENGTH = 45;
        
        Stopwatch _watch = new Stopwatch();

        private GameBoard _gameBoardInstance;

        public MainController(string playerNameOne, string playerNameTwo, TokenState tokenStatePlayerOne, TokenState tokenStatePlayerTwo)
        {
            _playerOne = new Player() { IsActive = true, PlayerName=playerNameOne, PlayerState = tokenStatePlayerOne };
            _playerTwo = new Player() { IsActive = false, PlayerName = playerNameTwo, PlayerState = tokenStatePlayerTwo };
        }

        public MainController()
        {
            StartLoadedGameBoardMain();
        }
        
        public void StartGame()
        {
            StartNewGameBoardMain();
            _watch.Start();
        }

        private void StartNewGameBoardMain()
        {
            PlayFieldGeneratorController playFieldGenerator = new PlayFieldGeneratorController();
            ColumnDefinition firstCol = new ColumnDefinition
                                            {
                                                Width = new GridLength(_GRIDLENGTH)
                                            };
            RowDefinition firstRow = new RowDefinition
                                         {
                                             Height = new GridLength(_GRIDLENGTH)
                                         };
            _tokensList = playFieldGenerator.CreateButtons(_PLAYFIELDSIZE);
            GameBoard gameBoard = new GameBoard(this, firstCol, firstRow, playFieldGenerator.GetColumns(_PLAYFIELDSIZE), playFieldGenerator.GetRows(_PLAYFIELDSIZE), _tokensList);
            gameBoard.Show();
        }

        private void ShowLoadedTokensList(List<Token> tokensList)
        {
            foreach (Token token in tokensList.Where(x=>x.TokenState != TokenState.NONE))
            {
                MoveController mv = new MoveController();
                mv.ShowToken(token);
            }
        }

        private void StartLoadedGameBoardMain()
        {
            PlayFieldGeneratorController playFieldGenerator = new PlayFieldGeneratorController();
            ColumnDefinition firstCol = new ColumnDefinition
            {
                Width = new GridLength(_GRIDLENGTH)
            };
            RowDefinition firstRow = new RowDefinition
            {
                Height = new GridLength(_GRIDLENGTH)
            };
            _tokensList = LoadGame();
            ShowLoadedTokensList(_tokensList);
            SetSurroundingPlayStones();
            SetChains();
            GameBoard gameBoard = new GameBoard(this, firstCol, firstRow, playFieldGenerator.GetColumns(_PLAYFIELDSIZE), playFieldGenerator.GetRows(_PLAYFIELDSIZE), _tokensList);
            gameBoard.Show();
            UpdateGameStatusInfo();
        }

      private void SetSurroundingPlayStones()
      {
        PlayFieldGeneratorController pfgc = new PlayFieldGeneratorController();
        foreach (Token token in _tokensList)
        {
          token.SurroundingTokens = pfgc.SetSurroundingTokens(_tokensList, token);
        }
      }

      private void SetChains()
      {
        PlayStateController psc = new PlayStateController(_tokensList);
        var updatedTokensList = psc.FindAndSetChains();
        _tokensList = updatedTokensList;
      }

      private void SetChains(List<Token> tokensList)
      {
        
      }

        public void Initialize(GameBoard gameBoardInstance)
        {
            _gameBoardInstance = gameBoardInstance;
        }

        public void UpdateGameStatusInfo()
        {
            if (_IsGameActive)
            {
                _gameBoardInstance._txtPlayerColor.Content = GetActivePlayer().PlayerState.ToString();
                _gameBoardInstance._txtPlayerName.Content = GetActivePlayer().PlayerName;
                _gameBoardInstance._txtScoreOne.Content = _playerOne.Score;
                _gameBoardInstance._txtScoreTwo.Content = _playerTwo.Score;
                _gameBoardInstance._txtStones.Content = _playerOne.MoveCount + _playerTwo.MoveCount - _deadStones;
                TimeSpan ts = _watch.Elapsed;
                _gameBoardInstance._txtTime.Content = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                _gameBoardInstance._txtZug.Content = _playerOne.MoveCount + _playerTwo.MoveCount + 1;
                if (_skipCount > 0)
                {
                    _gameBoardInstance._cmdSkip.BorderBrush = Brushes.Red;
                }
                else
                {
                    _gameBoardInstance._cmdSkip.BorderBrush = Brushes.Gray;
                }
            }
            else
            {
                _gameBoardInstance._txtPlayerColor.Content = "BEENDET";
                _gameBoardInstance._txtPlayerName.Content = "BEENDET";
            }
        }

        public bool OnPlayStoneClick(Token clickedStone)
        {
            Player player = GetActivePlayer();
            if (clickedStone != null)
            {
                MoveResult moveResult;
                if (player != null)
                {
                    moveResult = _mvController.Move(player, clickedStone, _tokensList);
                    if (moveResult.IsNormalMoveSuccess || moveResult.IsMoveKill)
                    {
                        clickedStone.TokenState = player.PlayerState;
                        if (_tokensList.Where(x => x.TokenState == TokenState.NONE).Count() == 0)
                        {
                            GameOver();
                        }
                        ChangePlayerState();
                    }
                    if (moveResult.IsMoveKill)
                    {
                        player.Score += moveResult.KillCount;
                        ChangePlayerState();
                    }
                }
                _skipCount = 0;
                return true;
            }
            else
            {
                _skipCount++;
                player.MoveCount ++;
                _deadStones ++;
                if (_skipCount >= 2)
                {
                    GameOver();
                    return false;
                }
                else
                {
                    ChangePlayerState();
                    return true;
                }
            }
        }

        private Player GetActivePlayer()
        {
            if (_playerOne.IsActive)
            {
                return _playerOne;
            }
            if (_playerTwo.IsActive)
            {
                return _playerTwo;
            }
            return null;
        }

        private void ChangePlayerState()
        {
            if (_playerOne.IsActive)
            {
                _playerOne.IsActive = false;
                _playerTwo.IsActive = true;
            }
            else
            {
                if (_playerTwo.IsActive)
                {
                    _playerTwo.IsActive = false;
                    _playerOne.IsActive = true;
                }
            }
        }

        public void GameOver()
        {
            _IsGameActive = false;
            _watch.Stop();
            MessageBox.Show(string.Format("Beide Spieler haben gepasst.\nDas Spiel ist beendet.\n\nGewinner:\t{0} ({1})", GetWinner().PlayerName, GetWinner().PlayerState), "Spiel beendet", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public Player GetWinner()
        {
            int whiteFields = _tokensList.Where(x => x.TokenState == TokenState.WHITE).Count();
            int blackFields = _tokensList.Where(x => x.TokenState == TokenState.BLACK).Count();
            if (whiteFields > blackFields)
            {
                if (_playerOne.PlayerState == TokenState.WHITE)
                {
                    return _playerOne;
                }
                else if (_playerTwo.PlayerState == TokenState.WHITE)
                {
                    return _playerTwo;
                }
            }
            else if (blackFields > whiteFields)
            {
                if (_playerOne.PlayerState == TokenState.BLACK)
                {
                    return _playerOne;
                }
                else if (_playerTwo.PlayerState == TokenState.BLACK)
                {
                    return _playerTwo;
                }
            }
            else
            {
                return new Player() { PlayerName = "Unentschieden", Score = 0 };
            }
            return null;
        }

        public void SaveGame()
        {
            GameDataOperator gameDataOperator = new GameDataOperator();
            gameDataOperator.SaveGame(_tokensList, _playerOne, _playerTwo, _IsGameActive, _deadStones, _moveCount, _skipCount);
        }

        private List<Token> LoadGame()
        {
          string savePath = @"C:\";
          // Create OpenFileDialog 
          Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



          // Set filter for file extension and default file extension 
          dlg.DefaultExt = ".go";
          dlg.Filter = "Go Files (*.go)|*.go";


          // Display OpenFileDialog by calling ShowDialog method 
          Nullable<bool> result = dlg.ShowDialog();


          // Get the selected file name and display in a TextBox 
          if (result == true)
          {
            // Open document 
            string filename = dlg.FileName;
            savePath = filename;
          }
            try
            {
              if (savePath == "")
              {
                MessageBox.Show("Ungültige Auswahl!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
              }
              else
              {
                GameDataOperator gdo = new GameDataOperator();
                _playerOne = gdo.LoadGamePlayers(1, savePath);
                _playerTwo = gdo.LoadGamePlayers(2, savePath);
                string[] information = gdo.LoadGame(savePath);
                if (information != null)
                {
                  _IsGameActive = Convert.ToBoolean(information[1]);
                  _deadStones = Convert.ToInt32(information[2]);
                  _moveCount = Convert.ToInt32(information[3]);
                  _skipCount = Convert.ToInt32(information[4]);
                }
                return gdo.LoadGameTokens(savePath);
              }
            }
            catch (Exception)
            {
                MessageBox.Show("Das Spiel konnte nicht geladen werden. Überprüfen Sie den Dateipfad zum gespeicherten Spiel.");
            }
            return null;
        }
    }
}
