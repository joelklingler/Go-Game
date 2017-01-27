using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Controllers
{
    using System.Diagnostics;
    using System.IO;
    using System.Windows;

    using Go.Controlers;
    using Go.Models;

    public class GameDataOperator
    {

        private string _savePath = string.Format(@"{0}\{1}-{2:yyyyddMMhhmm}.go", MainWindow._savePath, "GoSave", DateTime.Now);

        public void SaveGame(List<Token> tokensList, Player playerOne, Player playerTwo, bool isGameActive, int deadStones, int moveCount, int skipCount)
        {
            int i = 0;
            string[] dataStrings = new string[400];
            foreach (Token token in tokensList)
            {
                dataStrings[i] = token.TokenInformation;
                i++;
            }
            dataStrings[i++] = "EO Tokens";
            dataStrings[i++] = string.Format("PLAYER1,{0},{1},{2},{3},{4}", playerOne.IsActive, playerOne.MoveCount, playerOne.PlayerName, playerOne.PlayerState, playerOne.Score);
            dataStrings[i++] = string.Format("PLAYER2,{0},{1},{2},{3},{4}", playerTwo.IsActive, playerTwo.MoveCount, playerTwo.PlayerName, playerTwo.PlayerState, playerTwo.Score);
            dataStrings[i++] = "EO Players";
            dataStrings[i++] = string.Format("GAME,{0},{1},{2},{3}", isGameActive, deadStones, moveCount, skipCount);
            dataStrings[i++] = "EO Game";
            try
            {
                File.WriteAllLines(_savePath, dataStrings);
            }
            catch (Exception)
            {
                MessageBox.Show("Das Spiel konnte nicht gespeichert werden.\nÜberprüfen Sie den vorgegebenen Speicherort unter Hauptmenue>Einstellungen");
            }
        }

        public List<Token> LoadGameTokens(string savePath)
        {
            string line;
            List<Token> newTokensList = new List<Token>();
            PlayFieldGeneratorController playFieldGeneratorController = new PlayFieldGeneratorController();
            System.IO.StreamReader file = new StreamReader(savePath);
            while ((line = file.ReadLine()) != "EO Tokens")
            {
                newTokensList = playFieldGeneratorController.GetButtons(line.Split(','), newTokensList);
            }
            return newTokensList;
        }

        public Player LoadGamePlayers(int playerIndex, string savePath)
        {
            if (playerIndex > 2 || playerIndex < 1)
            {
                throw new Exception("Invalid Player");
            }
            string line;
            List<Player> newPlayersList = new List<Player>();
            System.IO.StreamReader file = new StreamReader(savePath);
            while ((line = file.ReadLine()) != "EO Players")
            {
                if (line.Contains(string.Format("PLAYER{0}", playerIndex)))
                {
                    TokenState playerState = TokenState.NONE;
                    string[] information = line.Split(',');
                    switch (information[4])
                    {
                        case "BLACK":
                            playerState = TokenState.BLACK;
                            break;
                        case "WHITE":
                            playerState = TokenState.WHITE;
                            break;
                    }
                    Player player = new Player();
                    player.IsActive = Convert.ToBoolean(information[1]);
                    player.MoveCount = Convert.ToInt32(information[2]);
                    player.PlayerName = information[3];
                    player.PlayerState = playerState;
                    player.Score = Convert.ToInt32(information[5]);
                    return player;
                }
            }
            return new Player();
        }

        public string[] LoadGame(string savePath)
        {
            string line;
            System.IO.StreamReader file = new StreamReader(savePath);
            while ((line = file.ReadLine()) != "EO Game")
            {
                if (line.Contains("GAME"))
                {
                    return line.Split(',');
                }
            }
            return null;
        }
    }
}
