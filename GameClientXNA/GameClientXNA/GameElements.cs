using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClientXNA
{
    class Player
    {
        public String name;
        public int x;
        public int y;
        public int health;
        public int coins;
        public int isShot;
        public int direction;
        public int points;
    }

    class LifePack
    {
        public int x;
        public int y;
        public int lifeTime;
    }

    class Coin
    {
        public int value;
        public int lifeTime;
        public int x;
        public int y;

    }

    class Block
    {
        public int x;
        public int y;
    }

    class Brick : Block
    {

        public int health;
        public Brick(int x, int y, int health)
        {
            this.x = x;
            this.y = y;
            this.health = health;
        }

    }
    class Stone : Block
    {
        public Stone(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class Water : Block
    {
        public Water(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


}
