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
        /// <summary>
        /// // Your doing turns ////////////////////////////
        /// </summary>
        SpriteFont Label;
        //Textures
        public Texture2D BlackSquare;
        public Texture2D TopBorder;
        public Texture2D BottomBorder;
        public Texture2D LeftBorder;
        public Texture2D RightBorder;
        public Texture2D CoverSquare;
        
        Texture Marker;
        Texture2D WH, WL, BH, BL, CS;
        Texture2D pixel;
        //Vectors
        Vector2 MousePos, CounterPos,SelectedCounter;
        //Form the Grid
        const int rows = 8;
        const int cols = 8;
        bool[,] background = new bool[rows, cols];
        bool turn = false;
        bool WhiteWin;
        bool BlackWin;
        bool NewTurn = true;
        //States

        //Timers
        Timer Halt = new System.Timers.Timer();
        KeyboardState current = Keyboard.GetState();
        //Need ints
        int SelectedX = 0;
        int SelectedY = 0;
        int WhiteTaken = 0;
        int BlackTaken = 0;
        int PB = -1;
        int PW = -1;
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
            CS = Content.Load<Texture2D>("Counter Selected");
            CoverSquare = Content.Load<Texture2D>("Cover Square");
            Marker = Content.Load<Texture2D>("Marker");

            SetBackground();

            Label = Content.Load<SpriteFont>("Regular");

            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.Black }); // so that we can draw whatever color we want on top of it 

            Grid.Init();
            SetCounters(Grid);
            CounterPos = new Vector2(-1, -1);
            SelectedCounter = CounterPos;
            // TODO: use this.Content to load your game content here
        }

        public void SetCounters(Board B)
        {

            
            

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
            current = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           if (State.LeftButton == ButtonState.Pressed && !Halt.Enabled)
           {

                

                Vector2 MousePos = new Vector2(State.X, State.Y);
                //MousePos.X = State.X;
                //MousePos.Y = State.Y;
                int SelectedAreaXOverflow = State.X % 100;
                int SelectedAreaYOverflow = State.Y % 100;
                SelectedX = ((State.X - SelectedAreaXOverflow) / 100);
                SelectedY = ((State.Y - SelectedAreaYOverflow) / 100);
                Console.WriteLine("Clicked COOR " + (SelectedX) + " " + (SelectedY));
                Console.WriteLine("Clicked X,Y " + (State.X) + " " + (State.Y));


                if (current.IsKeyDown(Keys.F5))
                {
                    
                    
                }
               
               
                

                
                    

                try
                {
                    Console.WriteLine(Grid.grid[SelectedX, SelectedY].active);
                    if(Grid.grid[SelectedX, SelectedY].active)
                    {
                        if((Grid.grid[SelectedX, SelectedY].counter==WL || Grid.grid[SelectedX, SelectedY].counter == WH) && turn==false)
                        {
                            SelectedCounter = new Vector2(SelectedX, SelectedY);
                        }
                        else if((Grid.grid[SelectedX, SelectedY].counter == BL || Grid.grid[SelectedX, SelectedY].counter == BH ) && turn==true)
                        {
                            SelectedCounter = new Vector2(SelectedX, SelectedY);
                        }
                    }
                    else if(SelectedCounter!=new Vector2(-1,-1) && Grid.grid[SelectedX, SelectedY].SquareColour==Color.White)
                    {
                        //White Movements
                        if ((Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter == WL) && turn == false)
                        {
                            //Normal Movement ///////////////////////////////////
                            if ((SelectedX  == (int)SelectedCounter.X + 1 && SelectedY == (int)SelectedCounter.Y + 1) || (SelectedX == (int)SelectedCounter.X - 1 && SelectedY == (int)SelectedCounter.Y + 1 && Grid.grid[SelectedX, SelectedY].active == false))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1); 
                                turn = true;
                            }
                            //Different Taking ///////////////////////////////// 
                            else if(SelectedX == (int)SelectedCounter.X + 2 && SelectedY == (int)SelectedCounter.Y + 2 && Grid.grid[(int)SelectedCounter.X + 1,(int)SelectedCounter.Y +1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken++;
                                turn = true;
                            } //Taking single right
                            else if (SelectedX == (int)SelectedCounter.X - 2 && SelectedY == (int)SelectedCounter.Y + 2 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == BH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken++;
                                turn = true;
                            }//taking single left 
                            else if (SelectedX == (int)SelectedCounter.X - 4 && SelectedY == (int)SelectedCounter.Y + 4 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X - 2, (int)SelectedCounter.Y + 2].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == BH) && (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter == BL || (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter == BH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken = BlackTaken + 2;
                                turn = true;
                            }//taking double left 
                            else if (SelectedX == (int)SelectedCounter.X + 4 && SelectedY == (int)SelectedCounter.Y + 4 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X + 2, (int)SelectedCounter.Y + 2].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BH) && (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter == BL || (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter == BH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken = BlackTaken + 2;
                                turn = true;
                            }//taking double right

                            NewTurn = true;
                        }
                        
                        //Black Movements 
                        if ((Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter == BL) && turn == true)
                        {
                            //Normal Movement /////////////////////////////
                            if ((SelectedX == SelectedCounter.X + 1 && SelectedY == SelectedCounter.Y - 1) || (SelectedX == SelectedCounter.X - 1 && SelectedY== SelectedCounter.Y - 1 && Grid.grid[SelectedX,SelectedY].active == false))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1); 
                                turn = false;
                            }
                            //Diffrenent Taking /////////////////////////
                            else if (SelectedX == (int)SelectedCounter.X + 2 && SelectedY == (int)SelectedCounter.Y - 2 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == WL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == WH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken++;
                                turn = false;
                            } //Taking single right
                            else if (SelectedX == (int)SelectedCounter.X - 2 && SelectedY == (int)SelectedCounter.Y - 2 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == WL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == WH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken++;
                                turn = false;
                            }//taking single left 
                            else if (SelectedX == (int)SelectedCounter.X - 4 && SelectedY == (int)SelectedCounter.Y - 4 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X - 2, (int)SelectedCounter.Y - 2].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == WL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == WH) && (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter == WL || (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter == WH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken = WhiteTaken + 2;
                                turn = false;
                            }//taking double left 
                            else if (SelectedX == (int)SelectedCounter.X + 4 && SelectedY == (int)SelectedCounter.Y - 4 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X + 2, (int)SelectedCounter.Y - 2].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == WL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == WH) && (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter == WL || (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter == WH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken = WhiteTaken + 2;
                                turn = false;
                            }//taking double right
                            NewTurn = true;
                        }

                        ///Higher Movements White
                        if ((Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter == WH && turn == false))
                        {
                            ///Normal Movements///////////////////////
                            if ((SelectedX == (int)SelectedCounter.X + 1 && SelectedY == (int)SelectedCounter.Y + 1) || (SelectedX == (int)SelectedCounter.X - 1 && SelectedY == (int)SelectedCounter.Y + 1 && Grid.grid[SelectedX, SelectedY].active == false))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }
                            else if ((SelectedX == SelectedCounter.X + 1 && SelectedY == SelectedCounter.Y - 1) || (SelectedX == SelectedCounter.X - 1 && SelectedY == SelectedCounter.Y - 1 && Grid.grid[SelectedX, SelectedY].active == false))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }
                            ///Single Takes////////////////////////////////
                            else if(SelectedX == (int)SelectedCounter.X + 2 && SelectedY == (int)SelectedCounter.Y + 2 && Grid.grid[(int)SelectedCounter.X + 1,(int)SelectedCounter.Y +1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken++;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            } //Taking single right
                            else if (SelectedX == (int)SelectedCounter.X - 2 && SelectedY == (int)SelectedCounter.Y + 2 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == BH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken++;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking single left
                            else if (SelectedX == (int)SelectedCounter.X + 2 && SelectedY == (int)SelectedCounter.Y - 2 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == BL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == BH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken++;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            } //Taking single right
                            else if (SelectedX == (int)SelectedCounter.X - 2 && SelectedY == (int)SelectedCounter.Y - 2 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == BL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == BH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken++;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking single left 
                             ///Double Takes////////////////////////////////////////
                            else if (SelectedX == (int)SelectedCounter.X - 4 && SelectedY == (int)SelectedCounter.Y + 4 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X - 2, (int)SelectedCounter.Y + 2].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == BH) && (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter == BL || (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter == BH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken = BlackTaken + 2;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking double left 
                            else if (SelectedX == (int)SelectedCounter.X + 4 && SelectedY == (int)SelectedCounter.Y + 4 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X + 2, (int)SelectedCounter.Y + 2].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BH) && (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter == BL || (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter == BH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken = BlackTaken + 2;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking double right
                            else if (SelectedX == (int)SelectedCounter.X - 4 && SelectedY == (int)SelectedCounter.Y - 4 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X - 2, (int)SelectedCounter.Y - 2].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == BL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == BH) && (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter == BL || (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter == BH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken = BlackTaken + 2;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking double left 
                            else if (SelectedX == (int)SelectedCounter.X + 4 && SelectedY == (int)SelectedCounter.Y - 4 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X + 2, (int)SelectedCounter.Y - 2].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == BL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == BH) && (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter == BL || (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter == BH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                BlackTaken = BlackTaken + 2;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking double right
                            NewTurn = true;
                        }

                        /// Higher Movements Black
                        if ((Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter == BH && turn == true))
                        {
                            ///Normal Movements///////////////////////
                            if ((SelectedX == (int)SelectedCounter.X + 1 && SelectedY == (int)SelectedCounter.Y + 1) || (SelectedX == (int)SelectedCounter.X - 1 && SelectedY == (int)SelectedCounter.Y + 1 && Grid.grid[SelectedX, SelectedY].active == false))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }
                            else if ((SelectedX == SelectedCounter.X + 1 && SelectedY == SelectedCounter.Y - 1) || (SelectedX == SelectedCounter.X - 1 && SelectedY == SelectedCounter.Y - 1 && Grid.grid[SelectedX, SelectedY].active == false))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }
                            ///Single Takes////////////////////////////////
                            else if (SelectedX == (int)SelectedCounter.X + 2 && SelectedY == (int)SelectedCounter.Y + 2 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == WL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == WH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken++;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            } //Taking single right
                            else if (SelectedX == (int)SelectedCounter.X - 2 && SelectedY == (int)SelectedCounter.Y + 2 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == WL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == WH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken++;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking single left
                            else if (SelectedX == (int)SelectedCounter.X + 2 && SelectedY == (int)SelectedCounter.Y - 2 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == WL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == WH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken++;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            } //Taking single right
                            else if (SelectedX == (int)SelectedCounter.X - 2 && SelectedY == (int)SelectedCounter.Y - 2 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == WL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == WH)))
                            {
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken++;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking single left 
                             ///Double Takes////////////////////////////////////////
                            else if (SelectedX == (int)SelectedCounter.X - 4 && SelectedY == (int)SelectedCounter.Y + 4 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X - 2, (int)SelectedCounter.Y + 2].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == WL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter == WH) && (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter == WL || (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter == WH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y + 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken = WhiteTaken + 2;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking double left 
                            else if (SelectedX == (int)SelectedCounter.X + 4 && SelectedY == (int)SelectedCounter.Y + 4 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X + 2, (int)SelectedCounter.Y + 2].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == WL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == WH) && (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter == WL || (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter == WH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active = false;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y + 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken = WhiteTaken + 2;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking double right
                            else if (SelectedX == (int)SelectedCounter.X - 4 && SelectedY == (int)SelectedCounter.Y - 4 && Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X - 2, (int)SelectedCounter.Y - 2].active == false && (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == WL || (Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter == WH) && (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter == WL || (Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter == WH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X - 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X - 3, (int)SelectedCounter.Y - 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken = WhiteTaken + 2;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking double left 
                            else if (SelectedX == (int)SelectedCounter.X + 4 && SelectedY == (int)SelectedCounter.Y - 4 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active == true && Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].active == true && Grid.grid[SelectedX, SelectedY].active == false && Grid.grid[(int)SelectedCounter.X + 2, (int)SelectedCounter.Y - 2].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == WL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter == WH) && (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter == WL || (Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter == WH))))
                            {
                                //Setting Square States 
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                Grid.grid[SelectedX, SelectedY].active = true;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].active = false;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].active = false;
                                //Counter Status
                                Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y - 1].counter = null;
                                Grid.grid[(int)SelectedCounter.X + 3, (int)SelectedCounter.Y - 3].counter = null;
                                Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;
                                //Additional
                                SelectedCounter = new Vector2(-1, -1);
                                WhiteTaken = WhiteTaken + 2;
                                if (turn == true)
                                    turn = false;
                                else turn = true;
                            }//taking double right
                            NewTurn = true;
                        }

                        
                    }
                }

                catch(Exception ex)
                { Console.WriteLine("Outside Selected Area: " + ex.Message); }

                Halt.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                Halt.Interval = 100;
                Halt.Enabled = true;
           }

            try
            {
                if (SelectedY == 7 && Grid.grid[SelectedX, SelectedY].counter == WL)
                {
                    Grid.grid[SelectedX, SelectedY].counter = WH;
                }

                if (SelectedY == 0 && Grid.grid[SelectedX, SelectedY].counter == BL)
                {
                    Grid.grid[SelectedX, SelectedY].counter = BH;
                }

                if (PW == 0)
                {
                    BlackWin = true;
                }
                if (PB == 0)
                {
                    WhiteWin = true;
                }
            }
            catch(Exception ex2)
            {
                Console.WriteLine("Error: " + ex2);
            }



            // TODO: Add your update logic here
            PossibleMoves();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            MouseState State = Mouse.GetState();

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
                spriteBatch.DrawString(Label, Convert.ToString(i + 1), start, Color.Black);
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


            Grid.Draw(spriteBatch);

            

            
            ////////////////////////////////SpriteFonts
            spriteBatch.DrawString(Label, "White Moves: " + PW, new Vector2(10, 865), Color.Black);
            spriteBatch.DrawString(Label, "||", new Vector2(175, 864), Color.Black);
            spriteBatch.DrawString(Label, "Black Moves: " + PB, new Vector2(193, 865), Color.Black);
            spriteBatch.DrawString(Label, "||", new Vector2(350, 864), Color.Black);
            if (turn == false)
                spriteBatch.DrawString(Label, "Turn: White", new Vector2(365, 865), Color.Black);
            else
                spriteBatch.DrawString(Label, "Turn: Black", new Vector2(365, 865), Color.Black);

            if (BlackWin == true)
            {
                spriteBatch.DrawString(Label, "Black Wins!!!", new Vector2(700, 865), Color.Black);
                for (int i = 0; i < 8; i++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        Grid.grid[i, x].active = false;
                    }
                }
                
            }
            if (WhiteWin == true)
            {
                spriteBatch.DrawString(Label, "White Wins!!!", new Vector2(700, 865), Color.Black);
                for (int i = 0; i < 8; i++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        Grid.grid[i, x].active = false;
                    }
                }
            }

            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(CS, new Vector2(SelectedCounter.X * 100 + 15, SelectedCounter.Y * 100 + 15), Color.Yellow);
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

        private void PossibleMoves()
        {
            Vector2 Looking = new Vector2(-1, -1);
            Vector2 Looking2 = new Vector2(-1, -1);
            Vector2 Looking3 = new Vector2(-1, -1);
            Vector2 Looking4 = new Vector2(-1, -1);
            int RunThrough = 0;
            int CheckedPOptions = 0;
            
            Vector2 Num = new Vector2(0, 0);
            Vector2 Indent = new Vector2(1, 1);
            if (NewTurn == true)
            {
                PB = 0;
                PW = 0;
                while (RunThrough <= 3)
                {
                    for (Num.Y = 0; Num.Y < 8; Num.Y++)
                    {
                        for (Num.X = 0; Num.X < 8; Num.X++)
                        {
                            if (Grid.grid[(int)Num.X, (int)Num.Y].active == true)
                            {
                                Console.WriteLine("Checking: " + Num);
                                RunThrough = 0;
                                while (RunThrough < 4)
                                {
                                    
                                    if (RunThrough == 0)
                                        Indent = new Vector2(1, 1);

                                    Looking = Num + Indent;

                                    Console.WriteLine("Indent: " + Indent);
                                    if (Num.Y + Indent.Y >= 8 || Num.Y + Indent.Y <= -1 || Num.X + Indent.X >= 8 || Num.X + Indent.X <= -1)
                                    {
                                        Console.WriteLine("Breach of Index");
                                    }
                                    else
                                    {
                                        
                                        
                                        
                                        Console.WriteLine("Looking: " + Looking);
                                        if (Grid.grid[(int)Looking.X, (int)Looking.Y].active == false)
                                        {
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WL && Indent.Y > 0)
                                            {
                                                PW++;
                                                Console.WriteLine("Added PW");
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WH)
                                            {
                                                PW++;
                                                Console.WriteLine("Added PW");
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BL && Indent.Y < 0)
                                            {
                                                PB++;
                                                Console.WriteLine("Added PW");
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BH)
                                            {
                                                PB++;
                                                Console.WriteLine("Added PW");
                                            }
                                        }
                                        
                                    }

                                    Looking2 = Looking + Indent;

                                    if (Looking2.Y >= 8 || Looking2.Y <= -1 || Looking2.X >= 8 || Looking2.X <= -1)
                                    {
                                        Console.WriteLine("Breach of Index");
                                    }
                                    else
                                    {
                                        if (Grid.grid[(int)Num.X, (int)Num.Y].active == true && Grid.grid[(int)Looking.X, (int)Looking.Y].active == true && Grid.grid[(int)Looking2.X, (int)Looking2.Y].active == false)
                                        {
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WL && Indent.Y > 0 && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BH))
                                                PW++;
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WH && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BH))
                                                PW++;
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BL && Indent.Y < 0 && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WH))
                                                PB++;
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BH && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WH))
                                                PB++;
                                        }
                                    }

                                    Looking3 = Looking2 + Indent;
                                    Looking4 = Looking3 + Indent;

                                    if (Looking4.Y >= 8 || Looking4.Y <= -1 || Looking4.X >= 8 || Looking4.X <= -1)
                                    {
                                        Console.WriteLine("Breach of Index");
                                    }
                                    else
                                    {
                                        if (Grid.grid[(int)Num.X, (int)Num.Y].active == true && Grid.grid[(int)Looking.X, (int)Looking.Y].active == true && Grid.grid[(int)Looking2.X, (int)Looking2.Y].active == false && Grid.grid[(int)Looking3.X, (int)Looking3.Y].active == true && Grid.grid[(int)Looking4.X, (int)Looking4.Y].active == false)
                                        {
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WL && Indent.Y > 0 && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BH) && (Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == BL || Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == BH))
                                                PW++;
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WH && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BH) && (Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == BL || Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == BH))
                                                PW++;
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BL && Indent.Y < 0 && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WH) && (Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == WL || Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == WH))
                                                PB++;
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BH && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WH) && (Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == WL || Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == WH))
                                                PB++;
                                        }
                                    }


                                    CheckedPOptions++;
                                    RunThrough++;
                                    if (RunThrough == 1)
                                        Indent = new Vector2(1, -1);
                                    if (RunThrough == 2)
                                        Indent = new Vector2(-1, 1);
                                    if (RunThrough == 3)
                                        Indent = new Vector2(-1, -1);
                                }
                                Console.WriteLine("Out While 2");
                            }
                            
                        }
                    }
                    Console.WriteLine("PW: " + PW + " || PB: " + PB + " || Out of " + CheckedPOptions + " Checked Areas for Moves");

                    if (PB == 0 || PW == 0)
                        Winner();
                }

                


            }
            
            NewTurn = false;
        }

        private void Winner()
        {
            if (PW == 0)
            {
                BlackWin = true;
            }
            if (BlackTaken == 8)
            {
                WhiteWin = true;
            }
        }
    }
}
