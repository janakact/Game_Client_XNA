using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClientXNA.Game.AI
{
    public class TaskManager
    {
        public static String getMoveNew(GameDetail game, TimeSpan time)
        {
            List<int[]> items = new List<int[]>();
            foreach(LifePack l in game.lifePacks)
            {
                items.Add(new int[] { l.x, l.y, 5000,  ((time - l.arrivedTime).Seconds)   });
            }
            foreach (Coin c in game.coins)
            {
                items.Add(new int[] { c.x, c.y, c.value, ((time - c.arrivedTime).Seconds) });
            }

            List<int[]> playerDistances = new List<int[]>();
            int[] moves = new int[items.Count];

            foreach (Player p in game.players)
            {
                int[,,] grid = new int[10, 10, 4];

                int[] start = new int[] { p.x, p.y, p.direction };

                Queue<int[]> queue = new Queue<int[]>();
                queue.Enqueue(start);
                grid[start[0], start[1], start[2]] = 1;

                while (queue.Count > 0)
                {
                    int[] current = queue.Dequeue();
                    int currentDistance = grid[current[0], current[1], current[2]];

                    for (int i = 0; i < 4; i++)
                    {
                        int[] loc;
                        if (i != current[2])
                        {
                            if (grid[current[0], current[1], i] != 0) continue;

                            loc = new int[] { current[0], current[1], i };
                            queue.Enqueue(loc);
                            grid[loc[0], loc[1], loc[2]] = currentDistance + 1;

                            if (currentDistance == 1) grid[loc[0], loc[1], loc[2]] += 10000 * i;
                        }
                        else {
                            loc = null;
                            //up
                            if (i == 0 && current[1] > 0)
                                loc = new int[] { current[0], current[1] - 1, i };
                            //Down
                            else if (i == 2 && current[1] < 9)
                                loc = new int[] { current[0], current[1] + 1, i };
                            //Right
                            else if (i == 1 && current[0] < 9)
                                loc = new int[] { current[0] + 1, current[1], i };
                            //Left
                            else if (i == 3 && current[0] > 0)
                                loc = new int[] { current[0] - 1, current[1], i };


                            if (loc != null && grid[loc[0], loc[1], loc[2]] == 0 && game.blocks[loc[0], loc[1]].GetType() == typeof(EmptyBlock))
                            {
                                queue.Enqueue(loc);
                                grid[loc[0], loc[1], loc[2]] = currentDistance + 1;
                                if (currentDistance == 1) grid[loc[0], loc[1], loc[2]] += 10000 * i;
                            }
                        }
                    }
                }


                int[] distances = new int[items.Count];

                for(int i=0; i<items.Count; i++)
                { 
                    int x = items[i][0], y = items[i][1];
                    // int x = 1, y = 1;
                    int min = 9000;
                    int move = 5;
                    for (int j = 0; j < 4; j++)
                    {
                        if (min > (grid[x, y, j] % 10000))
                        {
                            min = grid[x, y, j] % 10000;
                            move = grid[x, y, j] / 10000;
                        }
                    }
                    distances[i] = min;
                    if(p.name==game.thisPlayer.name)
                    {
                        moves[i] = move;
                    }

                    
                }
                playerDistances.Add(distances);
            }

            if(items.Count == 0)
                return Constant.SHOOT;


            int[] minByOthers = new int[items.Count];
            for (int i = 0; i < items.Count; i++) minByOthers[i] = 9000;

            int thisPlayerId = 0;
            for(int i=0; i<game.players.Count; i++)
            {
                Player p = game.players[i];
                if (p.name == game.thisPlayer.name)
                {
                    thisPlayerId = i;
                    continue;
                }

                for (int j = 0; j < items.Count; j++)
                    if(minByOthers[j]>playerDistances[i][j])
                        minByOthers[j] = playerDistances[i][j];
            }


            int maxItemValue = 0, itemIndex = 0, minSteps = 9000; ;
            
            for(int i=0; i< items.Count; i++)
            {
                //if (items[i][3] > playerDistances[thisPlayerId][i]) minByOthers[i] = 0;

               // if (playerDistances[thisPlayerId][i] <= minByOthers[i])
                //{
                    if(minSteps> playerDistances[thisPlayerId][i])
                    {
                        minSteps = playerDistances[thisPlayerId][i];
                        itemIndex = i;
                    }
                   
                //}
                //if(maxItemValue< items[i][2])
                //{
                //    itemIndex = i;
                //    maxItemValue = items[i][2];
                //}
            }
            


            int finalMove = moves[itemIndex];
            if (finalMove == 0) return Constant.UP;
            else if (finalMove == 1) return Constant.RIGHT;
            else if (finalMove == 2) return Constant.DOWN;
            else if (finalMove == 3) return Constant.LEFT;
            else return "LOL";

        }



        public static List<int> getPath(GameDetail game,int from, int to)
        {
            List<int> path = new List<int>();

            //Mark cells unvisited | when we visit them, mark with parent cell's id
            int[] parents = new int[100];
            for (int i = 0; i < 100; i++)
                parents[i] = -1; 

            //Queue to enque tmperol cells
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(from);

            while(queue.Count>0)
            {
                int block = queue.Dequeue();
                int i = block / 10, j = block % 10;

                if (game.blocks[i, j].GetType() != typeof(EmptyBlock))
                {
                    parents[block] = -2;
                    continue;//do no add childs if it is a blocked cell
                }

                if(i>0)
                {
                    int tmp = (i - 1) * 10 + j;
                    if (parents[tmp] == -1)
                    {
                        parents[tmp] = block;
                        queue.Enqueue(tmp);
                    }
                }
                if (j > 0)
                {
                    int tmp = (i) * 10 + j-1;
                    if (parents[tmp] == -1)
                    {
                        parents[tmp] = block;
                        queue.Enqueue(tmp);
                    }
                }
                if (i <9)
                {
                    int tmp = (i + 1) * 10 + j;
                    if (parents[tmp] == -1)
                    {
                        parents[tmp] = block;
                        queue.Enqueue(tmp);
                    }
                }
                if (j < 9)
                {
                    int tmp = (i) * 10 + j+1;
                    if (parents[tmp] == -1)
                    {
                        parents[tmp] = block;
                        queue.Enqueue(tmp);
                    }
                }
            }

            //Return null if we can't find a possible path
            if (parents[to] <0) return path;


            //Find the path
            int movingCell = to;
            while(movingCell!=from)
            {
                path.Add(movingCell);
                movingCell = parents[movingCell];
            }
            path.Reverse();
            return path;
        }


        public static String getMove(GameDetail game)
        {

            //Find the closest coin
            int currentLoc = game.thisPlayer.x * 10 + game.thisPlayer.y;
            int minLoc = currentLoc;
            int minsDist = 100;
            foreach (dynamic coin in game.coins)
            {
                int dist = getPath(game, currentLoc, coin.x * 10 + coin.y).Count;
                if (dist < minsDist)
                {
                    minLoc = coin.x * 10 + coin.y;
                    minsDist = dist;
                }
            }

            foreach (dynamic lifePack in game.lifePacks)
            {
                int dist = getPath(game, currentLoc, lifePack.x * 10 + lifePack.y).Count;
                if (dist < minsDist)
                {
                    minLoc = lifePack.x * 10 + lifePack.y;
                    minsDist = dist;
                }
            }

            if (minsDist == 100) return Constant.SHOOT;

            int from = game.thisPlayer.x * 10 + game.thisPlayer.y;
            int to = minLoc;

            List<int> path = getPath(game, from, to);

            if (path == null || path.Count == 0)
            {
                Console.WriteLine("Error:" + from + " " + to);
                return Constant.SHOOT;
            }

            Console.WriteLine(path[0]);

            if (path[0] - from == 10) return Constant.RIGHT;
            if (path[0] - from == -10) return Constant.LEFT;
            if (path[0] - from == 1) return Constant.DOWN;
            else  return Constant.UP;
        }
    }
}
