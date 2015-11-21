using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameClientXNA
{
    //Drawing manager draws tha game
    class DrawingManager
    {
        SpriteBatch spriteBatch;
        GameTime gameTime;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        int screenWidth, screenHeight;

        //Textures
        Texture2D blockTexture;
        public DrawingManager(GraphicsDeviceManager graphics)
        {
            device = graphics.GraphicsDevice;
            this.graphics = graphics;
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;
        }

        public void LoadContent()
        {
            //Generate a white texture
            blockTexture = new Texture2D(device, 30, 30);
            Color[] blockData = new Color[blockTexture.Width * blockTexture.Height];
            for (int i = 0; i < blockData.Length; i++)
                blockData[i] = Color.White;
            blockTexture.SetData<Color>(blockData);
        }

        public void Begin(SpriteBatch spriteBatch, GameTime gameTime)
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

        }
    }
}
