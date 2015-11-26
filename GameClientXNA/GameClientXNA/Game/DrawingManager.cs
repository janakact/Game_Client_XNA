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

        //Textures
        Texture2D blockTexture;
        Texture2D tankTexture;
        public DrawingManager(GraphicsDeviceManager graphics, ContentManager content,SpriteBatch spriteBatch)
        {
            device = graphics.GraphicsDevice;
            this.content = content;
            this.graphics = graphics;
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;
            this.spriteBatch = spriteBatch;
        }

        public void LoadContent()
        {
            //Generate a white texture
            blockTexture = new Texture2D(device, 30, 30);
            Color[] blockData = new Color[blockTexture.Width * blockTexture.Height];
            for (int i = 0; i < blockData.Length; i++)
                blockData[i] = Color.White;
            blockTexture.SetData<Color>(blockData);

            tankTexture = content.Load<Texture2D>("tank");
        }

        public void DrawGame(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.gameTime = gameTime;
            this.spriteBatch = spriteBatch;
        }
  

        public void DrawBlock(Block b)
        {
            spriteBatch.Draw(blockTexture, new Vector2(b.x * 35, b.y * 35), Color.White);
        }

        public void DrawBlock(Brick b)
        {
            spriteBatch.Draw(blockTexture, new Vector2(b.x * 35, b.y * 35), Color.Cyan);
        }

        public void DrawBlock(Stone b)
        {
            spriteBatch.Draw(blockTexture, new Vector2(b.x * 35, b.y * 35), Color.BlanchedAlmond);
        }

        public void DrawBlock(Water b)
        {
            spriteBatch.Draw(blockTexture, new Vector2(b.x * 35, b.y * 35), Color.Blue);
        }

        public void DrawPlayer(Player p)
        {
            spriteBatch.Draw(tankTexture, new Rectangle(p.x * 35, p.y * 35, 30, 20), null, Color.Gray,(float)((Math.PI/2)*p.direction+ Math.PI / 2 ), new Vector2(0,0), SpriteEffects.None, 0);
        }
    }
}
