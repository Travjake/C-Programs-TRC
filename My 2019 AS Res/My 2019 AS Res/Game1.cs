﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Timers;

namespace My_2019_AS_Res
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont Label;
        //Textures
        public Texture2D BlackSquare;
        public Texture2D TopBorder;
        public Texture2D BottomBorder;
        public Texture2D LeftBorder;
        public Texture2D RightBorder;
        public Texture2D CoverSquare;
        Texture Marker;
        Texture2D WH, WL, BH, BL;
        Texture2D pixel;
        //Vectors
        Vector2 MousePos, CounterPos;
        //Form the Grid
        const int rows = 8;
        const int cols = 8;
        bool[,] background = new bool[rows, cols];
        //States
        //Timers
        Timer Halt = new System.Timers.Timer();
        KeyboardState current = Keyboard.GetState();

        Board Grid = new Board();

        public Game1()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 900;
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
            
            BlackSquare = Content.Load<Texture2D>("Black Square");
            WH = Content.Load<Texture2D>("White Higher");
            WL = Content.Load<Texture2D>("White Lower");
            BH = Content.Load<Texture2D>("Black Higher");
            BL = Content.Load<Texture2D>("Black Lower");
            CoverSquare = Content.Load<Texture2D>("Cover Square");
            Marker = Content.Load<Texture2D>("Marker");

            SetBackground();

            Label = Content.Load<SpriteFont>("Regular");

            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.Black }); // so that we can draw whatever color we want on top of it 

            Grid.Init();
            SetCounters(Grid);
            CounterPos = new Vector2(-1, -1);
            // TODO: use this.Content to load your game content here
        }

        public void SetCounters(Board B)
        {
            // foreach(Square S in B.grid)
            //{
            // if(S.SquareColour == Color.White && S.Y < 200)
            //  {
            //     S.counter = WL;
            // }
            //  if (S.SquareColour == Color.White && S.Y >= 600)
            // {
            //     S.counter = BL;
            // }
            // }

            

            for (int ycor = 0; ycor < 8; ycor++)
            {
                for (int xcor = 0; xcor < 8; xcor++)
                {
                    if(Grid.grid[xcor,ycor].SquareColour == Color.White && Grid.grid[xcor, ycor].Y * 100 < 200)
                    {
                        Grid.grid[xcor, ycor].counter = WL;
                        Grid.grid[xcor, ycor].active =true;
                        Console.WriteLine("Counter Placed: "+xcor+" "+ycor);
                    }
                    if (Grid.grid[xcor, ycor].SquareColour == Color.White && Grid.grid[xcor, ycor].Y * 100 >= 600)
                    {
                        Grid.grid[xcor, ycor].counter = BL;
                        Grid.grid[xcor, ycor].active = true;
                        Console.WriteLine("Counter Placed: " + xcor + " " + ycor);
                        Console.WriteLine("grid grid Y " + Grid.grid[xcor, ycor].Y);
                        Console.WriteLine("grid grid X " + Grid.grid[xcor, ycor].X);
                    }
                    

                    Console.WriteLine((Grid.grid[xcor, ycor].counter != null) + " Counter Created" + xcor + " " + ycor);
                    Console.WriteLine((Grid.grid[xcor, ycor].SquareColour) + " Counter Created" + xcor + " " + ycor);
                }
            }
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

            MouseState State = Mouse.GetState();
            

          
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           if (State.LeftButton == ButtonState.Pressed && !Halt.Enabled)
            {

                Vector2 MousePos = new Vector2(State.X, State.Y);
                MousePos.X = State.X;
                MousePos.Y = State.Y;
                int SelectedAreaXOverflow = State.X % 100;
                int SelectedAreaYOverflow = State.Y % 100;
                int SelectedX = ((State.X - SelectedAreaXOverflow) / 100);
                int SelectedY = ((State.Y - SelectedAreaYOverflow) / 100);
                Console.WriteLine("Clicked COOR " + (SelectedX) + " " + (SelectedY));
                Console.WriteLine("Clicked X,Y " + (State.X) + " " + (State.Y));

                try
                {
                    if (Grid.grid[SelectedX, SelectedY].active)
                        CounterPos = new Vector2(SelectedX, SelectedY);
                    else
                        CounterPos = new Vector2(-1, -1);
                }

                catch(Exception ex)
                { Console.WriteLine("Outside Selected Area: " + ex.Message); }

                Halt.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                Halt.Interval = 100;
                Halt.Enabled = true;
            }

            ////hereif (current.IsKeyDown(Keys.W))
            { Grid.grid[0, 0].active = false; }



            // TODO: Add your update logic here

            base.Update(gameTime);
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
            pixel.SetData(new[] { Color.Black });
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (background[r, c])
                        spriteBatch.Draw(BlackSquare, new Rectangle(c * BlackSquare.Width, r * BlackSquare.Height, BlackSquare.Width, BlackSquare.Height), Color.Blue);
                }
            }
            spriteBatch.End();


            spriteBatch.Begin();
            spriteBatch.Draw(pixel, new Rectangle(0, 0, 800, 2), Color.Black);//Top
            spriteBatch.Draw(pixel, new Rectangle(0, 800, 802, 2), Color.Black);//Bottom
            spriteBatch.Draw(pixel, new Rectangle(0, 0, 2, 800), Color.Black);//Left
            spriteBatch.Draw(pixel, new Rectangle(800, 0, 2, 800), Color.Black);//Right
           

            Vector2 start = new Vector2(820, 40);
            Vector2 increm = new Vector2(0, 100);
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.DrawString(Label, Convert.ToString(i+1), start, Color.Black);
                start += increm;
            }

            start = new Vector2(47, 820);
            increm = new Vector2(100, 0);
            for (int i = 0; i < 8; i++)
            {
                spriteBatch.DrawString(Label, Convert.ToString(i + 1), start, Color.Black);
                start += increm;
            }

            pixel.SetData(new[] { Color.White });

            if (CounterPos!=new Vector2(-1,-1) && Grid.grid[((int)CounterPos.X), (int)CounterPos.X].active)
            {
                Rectangle selected = new Rectangle((int)CounterPos.X * 100, (int)CounterPos.Y * 100, 100, 100);
                spriteBatch.Draw(pixel, selected, Color.LightGray);
            }
            Grid.Draw(spriteBatch);




            ///// herefor (int ycor = 0; ycor < 8; ycor++)
            //{
               // for (int xcor = 0; xcor < 8; xcor++)
               // {
                  //  if (Grid.grid[xcor, ycor].active == false && Grid.grid[xcor, ycor].SquareColour == Color.White)
                  //  {
                   //     int coverx = xcor * 100 + 4;
                   //     int covery = ycor * 100 + 4;
                   //     spriteBatch.Draw(CoverSquare, new Vector2(coverx, covery), null, Color.Red);
                   //     Console.WriteLine("Active? " + Grid.grid[xcor, ycor].active);
                   // }
               // }
          //  }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void SetBackground()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if((r + c) % 2 == 1)
                        background[r, c] = true;   
                }
                //else background[r, c] = false;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Halt.Enabled = false;
        }
    }
}
