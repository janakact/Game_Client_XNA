using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace GameClientXNA
{
    
    class Constant
    {

        public static string SERVER_IP = "127.0.0.1";
        public static int SEND_PORT = 6000;
        public static int LISTEN_PORT = 7000;


        //public static string SERVER_IP = configData[0];//"127.0.0.1";
        //public static int SEND_PORT = int.Parse(configData[1]);//6000;
        //public static int LISTEN_PORT = int.Parse(configData[2]);//7000;


        #region "Messages to Send"
        public const string INITIALREQUEST = "JOIN#";
        public const string UP = "UP#";
        public const string DOWN = "DOWN#";
        public const string LEFT = "LEFT#";
        public const string RIGHT = "RIGHT#";
        public const string SHOOT = "SHOOT#";
        #endregion

        #region "S2C - Server To Client"
        public const string S2C_DEL = "#";

        public const string S2C_GAMESTARTED = "GAME_ALREADY_STARTED#";
        public const string S2C_NOTSTARTED = "GAME_NOT_STARTED_YET#";
        public const string S2C_GAMEOVER = "GAME_HAS_FINISHED#";
        public const string S2C_GAMEJUSTFINISHED = "GAME_FINISHED#";

        public const string S2C_CONTESTANTSFULL = "PLAYERS_FULL#";
        public const string S2C_ALREADYADDED = "ALREADY_ADDED#";

        public const string S2C_INVALIDCELL = "INVALID_CELL#";
        public const string S2C_NOTACONTESTANT = "NOT_A_VALID_CONTESTANT#";
        public const string S2C_TOOEARLY = "TOO_QUICK#";
        public const string S2C_CELLOCCUPIED = "CELL_OCCUPIED#";
        public const string S2C_HITONOBSTACLE = "OBSTACLE#";//Penalty should be added.
        public const string S2C_FALLENTOPIT = "PITFALL#";

        public const string S2C_NOTALIVE = "DEAD#";

        public const string S2C_REQUESTERROR = "REQUEST_ERROR#";
        public const string S2C_SERVERERROR = "SERVER_ERROR#";
        #endregion

        #region  "Grid values"
        public const string WATER = "W";
        public const string BRICK = "B";
        public const string STONE = "S";

        public const string COIN = "C";
        #endregion

        public static void Load()
        {

            try
            {
                StreamReader reader = new StreamReader("config.txt");
                string configText = reader.ReadLine();
                reader.Close();
                LoadFromText(configText);
            }
            catch
            {
                return;
            }

            //StreamWriter writetext = new StreamWriter("write.txt");
            //writetext.WriteLine("writing in text file");
            //writetext.Close();

        }

        public static bool LoadFromText(String configText)
        {
            try
            {
                string[] configData = configText.Split(':');

                SERVER_IP = configData[0];
                SEND_PORT = int.Parse(configData[1]);
                LISTEN_PORT = int.Parse(configData[2]);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
