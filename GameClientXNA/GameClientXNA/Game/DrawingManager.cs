using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameClientXNA
{
    //Drawing manager draws tha game
    class DrawingManager
    {
        //Game elements
        SpriteBatch spriteBatch;
        SpriteFont font;
        SpriteFont fontH1;
        SpriteFont fontH2;
        SpriteFont fontDetails;

        GameTime gameTime;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        ContentManager content;
        int screenWidth, screenHeight;
        GameDetail gameDetail;

        //Textures
        Texture2D backgroundTexture;
        Texture2D blockTexture;
        Texture2D waterTexture;
        Texture2D brickTexture;
        Texture2D stoneTexture;
        Texture2D tankTexture;
        Texture2D lifePackTexture;
        Texture2D coinTexture;

        //Tank Origin
        Vector2 tankOrigin;
        
        public DrawingManager(GraphicsDeviceManager graphics, ContentManager content,GameDetail gameDetail)
        {
            device = graphics.GraphicsDevice;
            this.content = content;
            this.graphics = graphics;
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;
            this.gameDetail = gameDetail;
        }

        public void LoadContent(SpriteBatch spriteBatch)
        {

            this.spriteBatch = spriteBatch;
            //Generate a white texture

            backgroundTexture = content.Load<Texture2D>("background");
            stoneTexture = content.Load<Texture2D>("stone");
            waterTexture = content.Load<Texture2D>("water");
            brickTexture = content.Load<Texture2D>("brick");
            font = content.Load<SpriteFont>("font");
            fontH1 = content.Load<SpriteFont>("fontH1");
            fontH2 = content.Load<SpriteFont>("fontH2");
            fontDetails = content.Load<SpriteFont>("fontDetails");
            //blockTexture = content.Load<Texture2D>("block");
            blockTexture = new Texture2D(device, 30, 30);
            Color[] blockData = new Color[blockTexture.Width * blockTexture.Height];
            for (int i = 0; i < blockData.Length; i++)
                blockData[i] = Color.White;
            blockTexture.SetData<Color>(blockData);

            tankTexture = content.Load<Texture2D>("tank");
            tankOrigin = new Vector2(75, 75);

            lifePackTexture = content.Load<Texture2D>("lifePack");

            coinTexture = content.Load<Texture2D>("coin");
        }

        public void DrawGame(GameTime gameTime)
        {
            DrawScenery();
            this.gameTime = gameTime;

            //Draw grid
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    DrawBlock(gameDetail.blocks[i, j] as dynamic);

            //Draw LifePacks
            foreach (dynamic lifePack in gameDetail.lifePacks)
            {
                DrawLifePack(lifePack);
                DrawLifePackDetails(lifePack);
            }

            //foreach (dynamic lifePack in gameDetail.lifePacks)
            //{
            //    DrawLifePackLifeTime(lifePack);
            //}

            //Draw Coins
            foreach (dynamic coin in gameDetail.coins)
            {
                DrawCoin(coin);
                DrawCoinDetails(coin);
            }

            //Draw Players
            foreach (dynamic player in gameDetail.players)
            {
                if(player!=null)
                    DrawPlayer(player);
            }

            int x = 20;

            DrawTableHeaders();

            foreach (dynamic player in gameDetail.players)
            {
                
                if (player.name != null)
                    DrawText(player,x);
                x = x + 20;
            }
        }

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
        }

        public void DrawBlock(Block b)
        {
            spriteBatch.Draw(blockTexture, new Rectangle(b.x * 50, b.y * 50, 45, 45), null, Color.White);
        }

        public void DrawBlock(Brick b)
        {
            if (b.health == 100)
                spriteBatch.Draw(brickTexture, new Rectangle(b.x * 50, b.y * 50, 45, 45), null, Color.White);
            else if (b.health == 75)
            {
                spriteBatch.Draw(blockTexture, new Rectangle(b.x * 50, b.y * 50, 45, 45), null, Color.White);
                spriteBatch.Draw(brickTexture, new Rectangle(b.x * 50, b.y * 50, 45, 33), null, Color.White);
            }
            else if (b.health == 50)
            {
                spriteBatch.Draw(blockTexture, new Rectangle(b.x * 50, b.y * 50, 45, 45), null, Color.White);
                spriteBatch.Draw(brickTexture, new Rectangle(b.x * 50, b.y * 50, 45, 22), null, Color.White);
            }
            else if (b.health == 25)
            {
                spriteBatch.Draw(blockTexture, new Rectangle(b.x * 50, b.y * 50, 45, 45), null, Color.White);
                spriteBatch.Draw(brickTexture, new Rectangle(b.x * 50, b.y * 50, 45, 11), null, Color.White);
            }
            else
                spriteBatch.Draw(blockTexture, new Rectangle(b.x * 50, b.y * 50, 45, 45), null, Color.White);
        }

        public void DrawBlock(Stone b)
        {
            spriteBatch.Draw(stoneTexture, new Rectangle(b.x * 50, b.y * 50, 45, 45), null, Color.White);
        }

        public void DrawBlock(Water b)
        {
            spriteBatch.Draw(waterTexture, new Rectangle(b.x * 50, b.y * 50, 45, 45), null, Color.White);
        }

        public void DrawPlayer(Player p)
        {
            spriteBatch.Draw(tankTexture, new Vector2(p.x * 50+23, p.y*50 +23 ), null, p.colour, (float)((Math.PI / 2) * p.direction + Math.PI / 2), tankOrigin, float.Parse("0.3") , SpriteEffects.None, 0);
        }

        public void DrawLifePack(LifePack l)
        {
            spriteBatch.Draw(lifePackTexture, new Rectangle(l.x * 50, l.y * 50, 45, 45), null, Color.White);
        }

        public void DrawCoin(Coin c)
        {
            spriteBatch.Draw(coinTexture, new Rectangle(c.x * 50, c.y * 50, 45, 45), null, Color.White);
        }

        private void DrawTableHeaders()
        {
            spriteBatch.DrawString(fontH2, "Scoreboard ", new Vector2(650, 40), Color.DarkGoldenrod);
            spriteBatch.DrawString(fontH1, "Player ", new Vector2(550, 80), Color.White);
            spriteBatch.DrawString(fontH1, "Points ", new Vector2(650, 80), Color.White);
            spriteBatch.DrawString(fontH1, "Coins ($)" , new Vector2(750, 80), Color.White);
            spriteBatch.DrawString(fontH1, "Health (%)", new Vector2(850, 80), Color.White);
            
        }

        private void DrawText(Player p, int x)
        {
            string name = p.name;
            if (p.name == gameDetail.thisPlayer.name) name = "Me (" + p.name + ")";

            spriteBatch.DrawString(font, name, new Vector2(550, 80 + x), p.colour);
            spriteBatch.DrawString(font, p.points.ToString(), new Vector2(650, 80 + x), p.colour);
            spriteBatch.DrawString(font, p.coins.ToString(), new Vector2(750, 80 + x), p.colour);
            spriteBatch.DrawString(font, p.health.ToString(), new Vector2(850, 80 + x), p.colour);
        }

        private void DrawLifePackDetails(LifePack l)
        {
            spriteBatch.DrawString(fontDetails, "Life", new Vector2(l.x * 50, l.y * 50), Color.Black);
            spriteBatch.DrawString(fontDetails, "  Pack", new Vector2(l.x * 50, l.y * 50 + 10), Color.Black);
            spriteBatch.DrawString(fontDetails, l.lifeTime.ToString(), new Vector2(l.x*50, l.y*50+30), Color.Black);
        }

        private void DrawCoinDetails(Coin c)
        {
            spriteBatch.DrawString(fontDetails, "Coins", new Vector2(c.x * 50, c.y * 50), Color.Black);
            spriteBatch.DrawString(fontDetails, c.value.ToString() + "$", new Vector2(c.x * 50, c.y * 50 + 20), Color.Black);
            spriteBatch.DrawString(fontDetails, c.lifeTime.ToString(), new Vector2(c.x * 50, c.y * 50 + 30), Color.Black);
        }

    }
}
