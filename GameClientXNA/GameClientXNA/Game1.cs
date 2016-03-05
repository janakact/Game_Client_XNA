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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Comunicator
        NetworkClient networkClient;

        //AI
        Game.AI.MessageSender msgSender;

        //Game Details
        GameDetail gameDetail;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        int screenWidth, screenHeight;

        //Font to draw
        SpriteFont font;

        //Drawing Manager
        DrawingManager drawingManager;

        //Scene details
        private bool isHome = true;

        //Manual Move
        string nextMove;
        int waitCount = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Graphic initialize
            device = graphics.GraphicsDevice;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;

            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Crash & Burn";

            //Game Details
            gameDetail = new GameDetail();
            drawingManager = new DrawingManager(graphics,this.Content,gameDetail);
            msgSender = new Game.AI.MessageSender(networkClient);

            //Keyboard Dispatcher
            keyboard_dispatcher = new KeyboardDispatcher(this.Window);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //Load from configurations
            Constant.Load();

            font = Content.Load<SpriteFont>("font");

            //For Home
            textboxAddress = new TextBox(new Texture2D(device, 100, 10), new Texture2D(device, 100, 10), font);
            textboxAddress.X = 0;
            textboxAddress.Y = 0;
            textboxAddress.Width = 500;
            textboxAddress.Text = Constant.SERVER_IP+":"+Constant.SEND_PORT+":"+Constant.LISTEN_PORT;
            keyboard_dispatcher.Subscriber = textboxAddress;

            loadingTexture = Content.Load<Texture2D>("loading-texture");

            

            //Loading for game
            drawingManager.LoadContent( spriteBatch);


            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if(networkClient!=null)
                networkClient.StopListening();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (isHome)
                UpdateHome(gameTime);
            else
                UpdateGame(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (isHome)
                DrawHome(gameTime);
            else
                DrawGame(gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //_____________________________________________________________________________________Home
        #region Home Scene
        //Text Box to take Input 
        private KeyboardDispatcher keyboard_dispatcher;
        private TextBox textboxAddress;
        private Texture2D loadingTexture;
        private Texture2D homeTexture;

        public void DrawHome(GameTime gameTime)
        {
            homeTexture = Content.Load<Texture2D>("home");
            spriteBatch.Draw(homeTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            textboxAddress.Draw(spriteBatch, gameTime);
            
            //spriteBatch.Draw(loadingTexture, new Vector2(0, 0), Color.White);
            //spriteBatch.DrawString(font, textboxAddress.Text, new Vector2(20, 45), Color.White);
        }

        private void UpdateHome(GameTime gameTime)
        {
            //Set the subscriber
            //textboxAddress.Text = Constant.SERVER_IP;

            ProcessKeyboardHome();
        }
        private void ProcessKeyboardHome()
        {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Enter))
            {
                Constant.LoadFromText(textboxAddress.Text);
                if(networkClient!=null)
                networkClient.StopListening();

                networkClient = NetworkClient.getInstance(Constant.SERVER_IP, Constant.SEND_PORT, Constant.LISTEN_PORT);

                //Join the game
                networkClient.Send("JOIN#");
       
                networkClient.StartListening();

                //To send messages
                msgSender = new Game.AI.MessageSender(networkClient);
                isHome = false;
            }
        }
        #endregion

        //_______________________________________________________________________________________Game
        #region Game Scene
        private void DrawGame(GameTime gameTime)
        {
            drawingManager.DrawGame( gameTime);
        }

        
        private void UpdateGame(GameTime gameTime)
        {
            ProcessKeyboardGame();//Process keyboard - Esc to exit

            msgSender.update(gameTime.TotalGameTime); //Send the message will be handled by the msgSender. it will handle too-early situations

            //Process recieved messages
            Queue<String> recievedData = networkClient.RecievedData;
            while (recievedData.Count > 0)
            {
                String msg = recievedData.Dequeue();
                gameDetail.processMsg(msg, gameTime.TotalGameTime);
                if(msg == Constant.S2C_TOOEARLY)
                {
                   // msgSender.markSendFailed();
                }
            }

            //Update gameDetails. | for Coin,LifePack - Timeout
            gameDetail.update(gameTime.TotalGameTime);

            waitCount = (waitCount+1)%59;
            //if (waitCount == 0) networkClient.Send(nextMove);

            //AI MOve

            msgSender.setMessage( Game.AI.TaskManager.getMoveNew(gameDetail, gameTime.TotalGameTime));
        }

        private void ProcessKeyboardGame()
        {
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Escape))
            {
                isHome = true;
            }
            if (keybState.IsKeyDown(Keys.Up))
            {
               // networkClient.Send("LEFT#");
                msgSender.setMessage("UP#");
            }
            if (keybState.IsKeyDown(Keys.Down))
            {
                msgSender.setMessage("DOWN#"); 
            }
            if (keybState.IsKeyDown(Keys.Left))
            {
                msgSender.setMessage("LEFT#");
            }
            if (keybState.IsKeyDown(Keys.Right))
            {
                msgSender.setMessage("RIGHT#");
            }
            if (keybState.IsKeyDown(Keys.Space))
            {
                msgSender.setMessage("SHOOT#"); 
            }
        }
        #endregion
    }
}
