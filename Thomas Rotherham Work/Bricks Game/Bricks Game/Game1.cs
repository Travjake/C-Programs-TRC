using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Bricks_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Texture2D imgBrick;
        public Texture2D imgPaddle;
        public Texture2D imgBall;
        public Texture2D imgPixel;
        public SoundEffect startSound;
        public SoundEffect brickSound;
        public SoundEffect paddleBounceSound;
        public SoundEffect wallBounceSound;
        public SoundEffect missSound;
        public SpriteFont youwin;
        SpriteFont Font;

        public Vector2 paddlePosition;
        public Vector2 ballMovement;
        public Vector2 ballPosition;
        public bool ballInMotion = false;
        public int speed = 8;

        Rectangle paddleRectangle;
        Rectangle ballRectangle;

        const int rows = 5;
        const int cols = 8;
        bool[,] brickMap = new bool[rows, cols];
        byte check = 0;
        int BricksKo = 0;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 400;
            graphics.ApplyChanges();
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
            Rectangle paddleRectangle;
            Rectangle ballRectangle;

            // TODO: use this.Content to load your game content here
            //load images
            imgBall = Content.Load<Texture2D>("Ball");
            imgPixel = Content.Load<Texture2D>("Pixel");
            imgPaddle = Content.Load<Texture2D>("Paddle");
            imgBrick = Content.Load<Texture2D>("Brick");

            //load sounds
            startSound = Content.Load<SoundEffect>("StartSound");
            brickSound = Content.Load<SoundEffect>("BrickSound");
            paddleBounceSound = Content.Load<SoundEffect>("PaddleBounceSound");
            wallBounceSound = Content.Load<SoundEffect>("WallBounceSound");
            missSound = Content.Load<SoundEffect>("MissSound");

            //load text
            youwin = Content.Load<SpriteFont>("Bold");

            //load font
            Font = Content.Load<SpriteFont>("Regular");


            paddlePosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - imgPaddle.Width, 550);

           

            //// Stay at end 
            SetBricks();
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

            paddleRectangle = new Rectangle((int)paddlePosition.X, (int)paddlePosition.Y, imgPaddle.Width, imgPaddle.Height);
            ballRectangle = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, imgBall.Width, imgBall.Height);

            // TODO: Add your update logic here

            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                if (paddlePosition.X < 0)
                    paddlePosition.X = 0;
                else
                    paddlePosition.X -= 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                if (paddlePosition.X > (graphics.PreferredBackBufferWidth - imgPaddle.Width))
                    paddlePosition.X = graphics.PreferredBackBufferWidth - imgPaddle.Width;
                else
                    paddlePosition.X += 2;
            }

            if (!ballInMotion)
                ballPosition = new Vector2(paddlePosition.X + (imgPaddle.Width / 2) - (imgBall.Width / 2),
                    paddlePosition.Y - imgBall.Height);
            else
                ballPosition += ballMovement;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                ballInMotion = true;
                ballMovement = new Vector2(-2, -(speed));
                check = 1;
            }

            if (paddleRectangle.Intersects(ballRectangle))
            {
                ballPosition.Y = paddlePosition.Y - imgBall.Height;
                ballMovement = Bounce(ballMovement, true, paddleBounceSound);
            }

            //check for bricks
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (brickMap[r, c] && ballRectangle.Intersects(new Rectangle(c * imgBrick.Width, r * imgBrick.Height, imgBrick.Width, imgBrick.Height)))
                    {
                        brickMap[r, c] = false;
                        ballMovement = Bounce(ballMovement, true, brickSound);
                        BricksKo++;
                    }
                }
            }
            if(ballPosition.X <= 0 || ballPosition.X >= graphics.PreferredBackBufferWidth - imgBall.Width)
            {
                ballMovement = Bounce(ballMovement, false, brickSound);
            }

            if (ballPosition.Y <= 0)
            {
                ballMovement = Bounce(ballMovement, true, brickSound);
            }

            if (ballPosition.Y >= graphics.PreferredBackBufferHeight + imgBall.Height)
            {
                ballPosition.X = paddlePosition.X + imgPaddle.Width / 2;
                ballPosition.Y = paddlePosition.Y - imgBall.Height;
                ballMovement = new Vector2(0,0);
                ballInMotion = false;
            }

            if (BricksKo == 39)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Font, "You Win!!!!", new Vector2(0, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.End();
            }

            

        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "Bricks Hit: " + BricksKo, new Vector2(0, graphics.PreferredBackBufferHeight - 100), Color.White);
            spriteBatch.End();

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            spriteBatch.Begin();
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (brickMap[r, c])
                        spriteBatch.Draw(imgBrick, new Rectangle(c * imgBrick.Width, r * imgBrick.Height, imgBrick.Width, imgBrick.Height), Color.Blue);
                }
            }
            spriteBatch.Draw(imgPaddle, paddlePosition);
            spriteBatch.Draw(imgBall, ballPosition);
            spriteBatch.End();


        }

        /////////For Bouncy Ball

        public Vector2 Bounce(Vector2 current, bool Vertical, SoundEffect sound)
        {
            if (Vertical)
                current = new Vector2(current.X, -(current.Y));
            else
                current = new Vector2(-(current.X), current.Y);

            sound.Play();

            return current;
        }

        //////// Bricks 
        public void SetBricks()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    brickMap[r, c] = true;
                }
            }
        }

    }
}
