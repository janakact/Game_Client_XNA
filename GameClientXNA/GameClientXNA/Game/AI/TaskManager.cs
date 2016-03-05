using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClientXNA.Game.AI
{
    public class TaskManager
    {
        public static String getMoveNew(GameDetail game )
        {

            return Constant.SHOOT;
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
