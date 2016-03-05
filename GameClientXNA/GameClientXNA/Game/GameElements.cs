using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClientXNA
{
    public class Player
    {
        public String name;
        public int x;
        public int y;
        public int health = 100;
        public int coins;
        public int isShot;
        public int direction;
        public int points;
        public Color colour;
    }

    public class LifePack
    {
        public int x;
        public int y;
        public int lifeTime;
        public TimeSpan arrivedTime;
    }

    public class Coin
    {
        public int value;
        public int lifeTime;
        public int x;
        public int y;
        public TimeSpan arrivedTime;

    }

    public class Block
    {
        public int x;
        public int y;
    }

    public class Brick : Block
    {

        public int health;
        public Brick(int x, int y, int health)
        {
            this.x = x;
            this.y = y;
            this.health = health;
        }

    }
    public class Stone : Block
    {
        public Stone(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class Water : Block
    {
        public Water(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class EmptyBlock : Block
    {
        public EmptyBlock(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


}
