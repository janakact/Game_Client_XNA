using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClientXNA
{
   public class GameDetail
    {
        public string[,] grid;
        public List<Player> players ;//[] players;// = new List<Player>();
        public Player thisPlayer;
        public List<Coin> coins;
        public List<LifePack> lifePacks;
        //public List<Block> blocks;
        public Block[,] blocks;
        public Color[] colours;

        public GameDetail()
        {
            // players = new Player[5];

            players = new List<Player>();
            for (int i = 0; i < 5; i++)
                players.Add(new Player());

            thisPlayer = new Player();
            coins = new List<Coin>();
            lifePacks = new List<LifePack>();
            blocks = new Block[10, 10];
            colours = new Color[]{Color.MistyRose, Color.LightCyan, Color.PaleGreen, Color.Khaki, Color.PaleVioletRed};


            grid = new string[10, 10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] = "N";
                    blocks[i, j] = new EmptyBlock(i, j);
                }

            }
        }


        public void update(TimeSpan time)
        {
            
            // Remove life packs
            for (int i = 0; i < lifePacks.Count; i++)
            {
                int timeGap = (time - lifePacks[i].arrivedTime).Milliseconds + 1000 * ((time - lifePacks[i].arrivedTime).Seconds);
                if (timeGap > lifePacks[i].lifeTime)
                {
                    lifePacks.RemoveAt(i);
                    i--;
                }
            }

            // remove coins
            for (int i = 0; i < coins.Count; i++)
            {
                //Console.WriteLine(coins[i].lifeTime);
                int timeGap = (time - coins[i].arrivedTime).Milliseconds + 1000 * ((time - coins[i].arrivedTime).Seconds);
                if (timeGap > coins[i].lifeTime)
                {
                    coins.RemoveAt(i);
                    i--;
                }
            }

            //// remove players
            //for (int i = 0; i < players.Count; i++)
            //{
            //    if (players[i].health <= 0)
            //    {
            //        players.RemoveAt(i);
            //        i--;
            //    }
            //}

        }

        public void processMsg(String data, TimeSpan time)

        {


            //To Pani - update the grid[] as required.
            //This is the parser. add if conditions to identify messages and do the required process

            if (data.Length < 2) //Pre test for invalid messages
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
                    //blocks.Add(new Brick(x, y, 100));
                    blocks[x, y] = new Brick(x, y, 100);
                    //(blocks[x, y] as Brick).health = 234;
                    grid[x, y] = Constant.BRICK;
                }

                String[] stoneCordinates = arr[3].Split(';');
                for (int i = 0; i < stoneCordinates.Length; i++)
                {
                    int x = int.Parse(stoneCordinates[i][0].ToString());
                    int y = int.Parse(stoneCordinates[i][2].ToString());
                    //blocks.Add(new Stone(x, y));
                    blocks[x, y] = new Stone(x, y);
                    grid[x, y] = Constant.STONE;
                }

                String[] waterCordinates = arr[4].Split(';');
                for (int i = 0; i < waterCordinates.Length; i++)
                {
                    int x = int.Parse(waterCordinates[i][0].ToString());
                    int y = int.Parse(waterCordinates[i][2].ToString());
                    //blocks.Add(new Water(x, y));
                    blocks[x, y] = new Water(x, y);
                    grid[x, y] = Constant.WATER;
                }

                for(int i=0; i<10; i++)
                {
                    for(int j=0; j<10; j++)
                    {
                        if (blocks[i, j] == null)
                            blocks[i, j] = new EmptyBlock(i, j);
                    }
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
                c.arrivedTime = time; //Save the arrive time
                coins.Add(c);
            }

            if (data.Substring(0, 2) == "L:")
            {
                String[] arr = data.Split(':', '#');
                LifePack l = new LifePack();
                l.x = int.Parse(arr[1][0].ToString());
                l.y = int.Parse(arr[1][2].ToString());
                l.lifeTime = int.Parse(arr[2]);
                l.arrivedTime = time; //Save the arrive time
                lifePacks.Add(l);
            }


            if (data.Substring(0, 2) == "G:")
            {
                String[] arr = data.Split(':', '#');

                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (arr[i].Length > 1 && arr[i][0] == 'P')
                    {
                        String[] details1 = arr[i].Split(';');

                        int j = int.Parse(details1[0][1].ToString());
                        if (j < players.Count && j >= 0)
                        {
                            if (players[j] == null)
                                players[j] = new Player();
                            players[j].name = details1[0];
                            players[j].x = int.Parse(details1[1][0].ToString());
                            players[j].y = int.Parse(details1[1][2].ToString());
                            players[j].direction = int.Parse(details1[2][0].ToString());
                            players[j].isShot = int.Parse(details1[3][0].ToString());
                            players[j].health = int.Parse(details1[4]);
                            players[j].coins = int.Parse(details1[5]);
                            players[j].points = int.Parse(details1[6]);
                            players[j].colour = colours[j];

                        }
                    }

                    else if (arr[i].Length > 1)
                    {
                        String[] details2 = arr[i].Split(';');
                        for (int k = 0; k < details2.Length; k++)
                        {
                            String[] xyHealth = details2[k].Split(',');
                            int x = int.Parse(xyHealth[0].ToString());
                            int y = int.Parse(xyHealth[1].ToString());
                            int damage = int.Parse(xyHealth[2].ToString());
                            int health;

                            if (damage == 0)
                            {
                                health = 100;
                                (blocks[x, y] as Brick).health = health;
                            }

                            if (damage == 1)
                            {
                                health = 75;
                                (blocks[x, y] as Brick).health = health;
                            }

                            if (damage == 2)
                            {
                                health = 50;
                                (blocks[x, y] as Brick).health = health;
                            }

                            if (damage == 3)
                            {
                                health = 25;
                                (blocks[x, y] as Brick).health = health;
                            }

                            if (damage == 4)
                            {
                                health = 0;
                                (blocks[x, y] as Brick).health = health;
                            }

                            //(blocks[x, y] as Brick).health = health;
                            (blocks[x, y] as Brick).x = x;
                            (blocks[x, y] as Brick).y = y;

                        }

                    }

                }


                //After global update, check if there is a pile on a player's location then delete it
                foreach (dynamic player in players)
                {
                    if (player == null) continue;
                    // Remove life packs
                    for (int i = 0; i < lifePacks.Count; i++)
                    {
                        if (lifePacks[i].x==player.x &&  lifePacks[i].y == player.y)
                        {
                            lifePacks.RemoveAt(i);
                            i--;
                        }
                    }
                    // remove coins
                    for (int i = 0; i < coins.Count; i++)
                    {
                        //Console.WriteLine(coins[i].lifeTime);
                        if (coins[i].x == player.x && coins[i].y == player.y)
                        {
                            coins.RemoveAt(i);
                            i--;
                        }
                    }

                }
            }

            //By Janaka---------------------------------------------------------
            else if (data == Constant.S2C_GAMESTARTED)
            {
                //Game started
            }
            //Add others
            else if (data == Constant.S2C_TOOEARLY)
            {

                //Messages which can't be recognized
            }
            //------------------------------------------------------------------

        }
    }
}
