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
        SpriteBatch spriteBatch;
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

            foreach (dynamic lifePack in gameDetail.lifePacks)
            {
                DrawLifePack(lifePack);
            }

            foreach (dynamic coin in gameDetail.coins)
            {
                DrawCoin(coin);
            }

            foreach (dynamic player in gameDetail.players)
            {
                DrawPlayer(player);
            }

            


        }
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
        }

        public void DrawBlock(Block b)
        {
            spriteBatch.Draw(blockTexture, new Vector2(b.x * 35, b.y * 35), Color.White);
        }

        public void DrawBlock(Brick b)
        {
            spriteBatch.Draw(brickTexture, new Rectangle(b.x * 35, b.y * 35, 30, 30), null, Color.White);
        }

        public void DrawBlock(Stone b)
        {
            spriteBatch.Draw(stoneTexture, new Rectangle(b.x * 35, b.y * 35, 30, 30), null, Color.White);
        }

        public void DrawBlock(Water b)
        {
            spriteBatch.Draw(waterTexture, new Rectangle(b.x * 35, b.y * 35, 30, 30), null, Color.White);
        }

        public void DrawPlayer(Player p)
        {
            //(float)((Math.PI/2)*p.direction+ Math.PI / 2 )
            //spriteBatch.Draw(tankTexture, new Rectangle(p.x * 35, p.y * 35, 30, 30), null, Color.Gray,0, tankOrigin, SpriteEffects.None, 0);
            // spriteBatch.Draw(tankTexture, new Rectangle(p.x * 35, p.y * 35, 30, 30), null, Color.White);
            spriteBatch.Draw(tankTexture, new Vector2(p.x * 35+15, p.y*35 +15 ), null, Color.White, (float)((Math.PI / 2) * p.direction + Math.PI / 2), tankOrigin, float.Parse("0.2") , SpriteEffects.None, 1);
        }

        public void DrawLifePack(LifePack l)
        {
            spriteBatch.Draw(lifePackTexture, new Rectangle(l.x * 35, l.y * 35, 30, 30), null, Color.White);
        }

        public void DrawCoin(Coin c)
        {
            spriteBatch.Draw(coinTexture, new Rectangle(c.x * 35, c.y * 35, 30, 30), null, Color.White);
        }
    }
}
