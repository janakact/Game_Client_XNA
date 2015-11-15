using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClientXNA
{
    class GameDetail
    {
        public string[,] grid;
        public Player[] players;// = new List<Player>();
        public Player thisPlayer;
        public List<Coin> coins;
        public List<LifePack> lifePacks;
        public List<Block> blocks;
        
        public GameDetail()
        {
            players = new Player[5];
            thisPlayer = new Player();
            coins = new List<Coin>();
            lifePacks = new List<LifePack>();
            blocks = new List<Block>();

            //initializing the list
            for (int i = 0; i < 5; i++) { players[i] = new Player(); }


            grid = new string[10, 10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    grid[i, j] = "N";

            }
        }

        public void processMsg(String data)

        {
            //To Pani - update the grid[] as required.
            //This is the parser. add if conditions to identify messages and do the required process

            if (data.Length < 2) //Pre test for invalid messages :: Improve the condition
            {
                //Invalid message
                return;
            }

            if (data.Substring(0, 2) == "I:")
            {
                //Game init 
                String[] arr = data.Split(':', '#');

                //Player details---------------------
                thisPlayer.name = arr[1];
                players[int.Parse(arr[1][1].ToString())] = thisPlayer;

                //Console.WriteLine(thisPlayer.name);
                //Console.WriteLine(players[1].name);


                //Add bricks,stones,water------------
                String[] brickCordinates = arr[2].Split(';');
                for (int i = 0; i < brickCordinates.Length; i++)
                {
                    int x = int.Parse(brickCordinates[i][0].ToString());
                    int y = int.Parse(brickCordinates[i][2].ToString());
                    blocks.Add(new Brick(x, y, 100));
                    grid[x, y] = Constant.BRICK;
                }

                String[] stoneCordinates = arr[3].Split(';');
                for (int i = 0; i < stoneCordinates.Length; i++)
                {
                    int x = int.Parse(stoneCordinates[i][0].ToString());
                    int y = int.Parse(stoneCordinates[i][2].ToString());
                    blocks.Add(new Stone(x, y));
                    grid[x, y] = Constant.STONE;
                }

                String[] waterCordinates = arr[4].Split(';');
                for (int i = 0; i < waterCordinates.Length; i++)
                {
                    int x = int.Parse(waterCordinates[i][0].ToString());
                    int y = int.Parse(waterCordinates[i][2].ToString());
                    blocks.Add(new Water(x, y));
                    grid[x, y] = Constant.WATER;
                }

            }
            //Console.WriteLine(thisPlayer.name);
            if (data.Substring(0, 2) == "S:")
            {
                String[] arr = data.Split(':', ';', '#');

                thisPlayer.x = int.Parse(arr[2][0].ToString());
                thisPlayer.y = int.Parse(arr[2][2].ToString());
                thisPlayer.direction = int.Parse(arr[3][0].ToString());


            }

            if (data.Substring(0, 2) == "C:")
            {
                String[] arr = data.Split(':', '#');

                Coin c = new Coin();
                c.x = int.Parse(arr[1][0].ToString());
                c.y = int.Parse(arr[1][2].ToString());
                c.lifeTime = int.Parse(arr[2]);
                c.value = int.Parse(arr[3]);

                coins.Add(c);
            }

            if (data.Substring(0, 2) == "L:")
            {
                String[] arr = data.Split(':', '#');
                LifePack l = new LifePack();
                l.x = int.Parse(arr[1][0].ToString());
                l.y = int.Parse(arr[1][2].ToString());
                l.lifeTime = int.Parse(arr[2]);

                lifePacks.Add(l);
            }


            if (data.Substring(0, 2) == "G:" && false)
            {
                String[] arr = data.Split(':', '#');

                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i][0] == 'P')
                    {
                        String[] details = arr[i].Split(';');
                        int j = int.Parse(details[0][1].ToString());
                        if (j < 5 && j >= 0)
                        {
                            if (players[j] == null)
                                players[j] = new Player();
                            players[j].name = details[0];
                            players[j].x = int.Parse(details[1][0].ToString());
                            players[j].y = int.Parse(details[1][2].ToString());
                            players[j].direction = int.Parse(details[2][0].ToString());
                            players[j].isShot = int.Parse(details[3][0].ToString());
                            players[j].health = int.Parse(details[4]);
                            players[j].coins = int.Parse(details[5]);
                            players[j].points = int.Parse(details[6]);

                        }
                    }

                }


            }


            else if (data == Constant.S2C_GAMESTARTED)
            {
                //Game started
            }
            //Add others
            else
            {
                //Messages which can't be recognized
            }

        }
    }
}
