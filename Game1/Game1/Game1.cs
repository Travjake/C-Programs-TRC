using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Font;
        Texture2D PlayerTexture, Apple;
        Vector2 Position;
        Vector2 Position2;
        bool follow = false;
        int score = 0;
        Rectangle mariobox, Evil;
        Timer aTimer = new System.Timers.Timer();
        bool faceright = true;
        public Texture2D PlayerAnimation;
        int elapsedTime;
        int frameCount;
        int currentFrame;
        Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destinationRect = new Rectangle();
        public int FrameWidth;
        public int FrameHeight;
        int Rowcount;
        Random random = new Random();

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
            IsMouseVisible = true;
            base.Initialize();

            FrameWidth = 200;
            FrameHeight = 300;
            frameCount = 4;
            Rowcount = 0;
            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Apple = Content.Load<Texture2D>("Apple");
            Font = Content.Load<SpriteFont>("Regular");
            PlayerTexture = Content.Load<Texture2D>("Sprite Sheet");
            Position = new Vector2(200, 200);
            Position2 = new Vector2(400, 200);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState current = Keyboard.GetState();
            MouseState State = Mouse.GetState();
           
            
            Vector2 temp = Position;

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > 100)
            {
                // Move to the next frame
                currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }

            if (current.IsKeyDown(Keys.W) && (current.IsKeyDown(Keys.A)))
            {
                Position.X -= 1;
                Position.Y -= 1;
                Rowcount = 2;
            }
            else if (current.IsKeyDown(Keys.W) && (current.IsKeyDown(Keys.D)))
            {
                Position.X += 1;
                Position.Y -= 1;
                Rowcount = 3;
            }
            else if (current.IsKeyDown(Keys.S) && (current.IsKeyDown(Keys.A)))
            {
                Position.X -= 1;
                Position.Y += 1;
                Rowcount = 2;
            }
            else if (current.IsKeyDown(Keys.S) && (current.IsKeyDown(Keys.D)))
            {
                Position.X += 1;
                Position.Y += 1;
                Rowcount = 3;
            }

            else if (current.IsKeyDown(Keys.W))
            {
                Position.Y -= 1;
                Rowcount = 1;
            }
            else if (current.IsKeyDown(Keys.A))
            {
                Position.X -= 1;
                Rowcount = 2;

            }
            else if (current.IsKeyDown(Keys.S))
            {
                Position.Y += 1;
                Rowcount = 0;
            }
            else if (current.IsKeyDown(Keys.D))
            {
                Position.X += 1;
                Rowcount = 3;
            }
            else currentFrame = 0;

                if (State.RightButton == ButtonState.Pressed)
                {
                    if (!aTimer.Enabled)
                    {
                        {
                            follow = !follow;

                        }
                        if (follow)
                        {
                            Position.X = State.X;
                            Position.Y = State.Y;
                        }
                        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        aTimer.Interval = 100;
                        aTimer.Enabled = true;
                    }
                    
                }
            Vector2 mousepos = new Vector2(State.X, State.Y);
            

            mariobox = new Rectangle((int)Position.X, (int)Position.Y, 50, 100);
            

            if (Evil.Intersects(mariobox))
                    Position = temp;
                // TODO: Add your update logic here
                
                base.Update(gameTime);

            sourceRect = new Rectangle(currentFrame * FrameWidth, Rowcount * FrameHeight, FrameWidth, FrameHeight);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            spriteBatch.Draw(PlayerTexture, mariobox, sourceRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.DrawString(Font, "Score: " + score, new Vector2(0, 0), Color.Black);
            spriteBatch.Draw(Apple,Position2, null, Color.White, 0f, Vector2.Zero, 1f,
      SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }





        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;
        }
    }
}
