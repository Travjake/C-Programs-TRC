using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Timers;
using A1r.SimpleTextUI;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace My_2019_AS_Res
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SimpleTextUI menu;
        SimpleTextUI settings;
        SimpleTextUI credits;
        SimpleTextUI currentscreen;

        SpriteFont big;
        SpriteFont small;


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
        public Texture2D MenuPicture;

        Texture2D Marker, Marker2;
        Texture2D WH, WL, BH, BL, CS, WWC, BWC;
        Texture2D pixel;
        Texture2D OfflineMarker, ConnectingMarker, OnlineMarker;
        //Vectors
        Vector2 MousePos, CounterPos, SelectedCounter, SelectedCounter2;
        //Form the Grid

        const int Constwo = 2;
        static int rows = 8;
        static int cols = 8;
        bool[,] background = new bool[50, 50];

        //bool
        bool turn = false;
        bool WhiteWin;
        bool BlackWin;
        bool NewTurn = true;
        bool FirstTurn = true;
        bool ShowPossibleB = false;
        bool ShowPossibleW = false;
        bool Overload = false;
        bool First = true;
        bool ServerMove = false;
        bool ClientMove = false;
        bool IClient = false;
        bool NetworkedGame = false;
        bool PlayNetWorked = false;
        bool SinglePlayer = false;
        bool SetServer = true;
        bool JustConnected = false;
        bool AIMoved = false;
        bool Moved;
        //SaveGame

        //string
        string WhosTurn;
        //States

        //Arrays

        //Timers
        System.Timers.Timer Halt = new System.Timers.Timer();
        KeyboardState current = Keyboard.GetState();
        //Need ints
        int SelectedX = 0;
        int Room = 1;
        int SelectedY = 0;
        int WhiteTaken = 0;
        int BlackTaken = 0;
        double IncScale, IncScale2;
        int PB = -1;
        int PW = -1;
        int BlackSquareHeight;
        int BlackSquareWidth;
        Board Grid = new Board();
        int Height = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 8 ) * 6;
        int Width = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 8 ) * 6;
        int BlackCounterRows = 2;
        int WhiteCounterRows = 2;
        int Connecting = 0;
        int SinglePlayerDifficulty = 0;
        Single WCR, BCR;
        int XSave;
        int YSave;

        //Structs
        public struct Send
        {
            public int SelectedX;
            public int SelectedY;
            public Vector2 SelectedCounter;
        }

        public struct Move
        {
            public Vector2 Positon;
            public int MoveValue;
        }




        //Litenetlib
        bool server = false;
        bool client = false;
        int count;
        NetManager Server;
        NetManager Client;
        System.Timers.Timer atimer;

        enum GameState
        {
            MainMenu,
            Playing,
            Finished
        }

        GameState state = GameState.MainMenu;

        public Game1()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height  / 8 ) * 6;//Set screen Height
            graphics.PreferredBackBufferWidth = (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 8 ) * 6;//Set screen width
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

            //LiteNetLib////////////////////
            atimer = new System.Timers.Timer();
            atimer.Enabled = false;
            atimer.Interval = 4000;
            atimer.Elapsed += Atimer_Elapsed;



            //Load Textures
            BlackSquare = Content.Load<Texture2D>("Black Square");
            WH = Content.Load<Texture2D>("White Higher2");
            WL = Content.Load<Texture2D>("White Lower2");
            BH = Content.Load<Texture2D>("Black Higher2");
            BL = Content.Load<Texture2D>("Black Lower2");
            CS = Content.Load<Texture2D>("Counter Selected");
            WWC = Content.Load<Texture2D>("White Winning Counter");
            BWC = Content.Load<Texture2D>("Black Winning Counter");
            CoverSquare = Content.Load<Texture2D>("Cover Square");
            Marker = Content.Load<Texture2D>("Marker");
            Marker2 = Content.Load<Texture2D>("Marker2");
            OfflineMarker = Content.Load<Texture2D>("Offline");
            ConnectingMarker = Content.Load<Texture2D>("Connecting");
            OnlineMarker = Content.Load<Texture2D>("Online");
            MenuPicture = Content.Load<Texture2D>("Menu Background");
            SetBackground();


            Label = Content.Load<SpriteFont>("Regular"); //Loads Font type for writing
            big = Content.Load<SpriteFont>("Regular");
            small = Content.Load<SpriteFont>("Regular");

            // Set menus and screens
            menu = new SimpleTextUI(this, MenuPicture, big, new[] { "Play Single Player", "Play Two Player", "Host Multiplayer", "Join Multiplayer", "Settings", "Exit" })
            {
                TextColor = Color.Black,
                SelectedElement = new TextElement("> ", Color.LightGray),
                Align = Alignment.Left
            };

            settings = new SimpleTextUI(this, MenuPicture, big, new TextElement[]
            {
                //new SelectElement("Video", new[]{"Windowed","FullScreen"}),
                //new NumericElement("Music",1,3,0f,10f,1f),
                //new SelectElement("Difficulty\n", new[]{"Easy", "Medium", "Hard"}),
                new NumericElement("Rows \n  & Cols\n",rows,3,0f,50f,2f),
                new NumericElement("Black\n  Rows\n",BlackCounterRows,3,0f,24f,1f),
                new NumericElement("White\n  Rows\n",WhiteCounterRows,3,0f,24f,1f),
                new SelectElement("Show\n Possible White Moves\n", new[]{"False", "True"}),
                new SelectElement("Show\n Possible Black Moves\n", new[]{"False", "True"}),
                new NumericElement("Game\n Room:\n",Room,3,0f,99f,1f),
                new TextElement("Reset Setting to Default"),
                new TextElement("Back")
            });

            credits = new SimpleTextUI(this,MenuPicture, big, new TextElement[]
            {
                new TextElement("Travis Chapple"),
                new TextElement("   www."),
                new TextElement("       2019-2020"),
            });


            pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.Black }); // so that we can draw whatever color we want on top of it 



            if (state == GameState.Playing)
            {
                Grid.Init(rows, cols, BlackSquareWidth, BlackSquareHeight);//Runs grid method and draws grid
                SetCounters(Grid);// places counters passing ifo from the grid method
                CounterPos = new Vector2(-1, -1);
                SelectedCounter = CounterPos;
            }

            // TODO: use this.Content to load your game content here

            if (First == true)
            {
                First = false;
                currentscreen = menu;
            }
        }

        public void SetCounters(Board B)
        {


            int BBonus = (BlackCounterRows - 2) * 100;
            int WBonus = (WhiteCounterRows - 2) * 100;

            for (int ycor = 0; ycor < cols; ycor++) // Cycles through all squares on y axis
            {
                for (int xcor = 0; xcor < rows; xcor++)// same on x axis
                {
                    if (Grid.grid[xcor, ycor].SquareColour == Color.White && Grid.grid[xcor, ycor].Y * 100 < (200 + WBonus) && Grid.grid[xcor, ycor].X * 100 <= rows * 100)//declares where counters should be drawn and what colour
                    {
                        Grid.grid[xcor, ycor].counter = WL;//the drawn counter in this area
                        Grid.grid[xcor, ycor].active = true;

                    }
                    if (Grid.grid[xcor, ycor].SquareColour == Color.White && Grid.grid[xcor, ycor].X * 100 < rows * 100 && Grid.grid[xcor, ycor].Y * 100 < cols * 100 && Grid.grid[xcor, ycor].Y * 100 >= (cols * 100) - (200 + BBonus))//""
                    {
                        Grid.grid[xcor, ycor].counter = BL;//""
                        Grid.grid[xcor, ycor].active = true;

                    }



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
            BlackSquareHeight = (Height - (Height / 10)) / cols;
            BlackSquareWidth = (Width - (Width / 6)) / rows;

            if (BlackCounterRows + WhiteCounterRows <= cols)
                Overload = false;

            if (BlackCounterRows + WhiteCounterRows > cols)
                Overload = true;





            switch (state)
            {
                case GameState.MainMenu:
                    KeyboardState keys = Keyboard.GetState();
                    bool change = true;

                    if (JustConnected == true)
                    {
                        BlackSquareHeight = (Height - (Height / 10)) / cols;
                        BlackSquareWidth = (Width - (Width / 6)) / rows;
                        JustConnected = false;
                        state = GameState.Playing;
                        LoadContent();

                    }

                    if (!Halt.Enabled)
                    {
                        if (keys.IsKeyDown(Keys.Up))
                        {
                            currentscreen.Move(Direction.Up);
                        }

                        else if (keys.IsKeyDown(Keys.Down))
                        {
                            currentscreen.Move(Direction.Down);
                        }
                        else if (keys.IsKeyDown(Keys.Enter))
                        {
                            string test = currentscreen.GetCurrentCaption();

                            if (currentscreen == menu)
                            {
                                if (test == "Exit")
                                    Exit();
                                else if (test == "Settings")
                                {
                                    currentscreen = settings;
                                }
                                //else if (test == "Credits")
                                //{
                                    //currentscreen = credits;
                                //}
                                else if (test == "Join Multiplayer" && Overload == false) // && rows == 8 && cols == 8 && BlackCounterRows == 2 && WhiteCounterRows == 2)
                                {
                                    state = GameState.Playing;
                                    PlayNetWorked = true;
                                    SetServer = false;
                                    LoadContent();

                                }
                                else if (test == "Play Two Player" && Overload == false)
                                {
                                    state = GameState.Playing;

                                    LoadContent();



                                }
                                else if (test == "Play Single Player" && Overload == false)
                                {
                                    state = GameState.Playing;
                                    SinglePlayer = true;
                                    LoadContent();



                                }
                                else if (test == "Host Multiplayer" && Overload == false)// && rows == 8 && cols == 8 && BlackCounterRows == 2 && WhiteCounterRows == 2)
                                {
                                    state = GameState.Playing;
                                    PlayNetWorked = true;
                                    LoadContent();

                                }
                            }

                            else if (currentscreen == settings)
                            {
                                if (test == "Back")
                                {
                                    currentscreen = menu;
                                    if (cols < WhiteCounterRows + BlackCounterRows)
                                        Overload = true;
                                }
                                if (test ==  "Reset Setting to Default")
                                {
                                    cols = 8;
                                    rows = 8;
                                    BlackCounterRows = 2;
                                    WhiteCounterRows = 2;
                                    SinglePlayerDifficulty = 0;

                                    currentscreen = menu;
                                    LoadContent();
                                    currentscreen = settings;
                                }
                            }
                        }
                        else if (keys.IsKeyDown(Keys.Left))
                        {
                            currentscreen.Move(Direction.Left);
                            if (currentscreen.GetCurrentCaption() == "Video")
                            {
                                graphics.IsFullScreen = (currentscreen.GetCurrentValue() == "FullScreen");
                                graphics.ApplyChanges();
                            }
                            if (currentscreen.GetCurrentCaption() == "Difficulty\n")
                            {
                                SinglePlayerDifficulty--;
                            }
                            if (currentscreen.GetCurrentCaption() == "Rows \n  & Cols\n")
                            {
                                rows = rows - 2;
                                cols = cols - 2;
                            }
                            if (currentscreen.GetCurrentCaption() == "Black\n  Rows\n")
                            {
                                BlackCounterRows--;

                            }
                            if (currentscreen.GetCurrentCaption() == "White\n  Rows\n")
                            {
                                WhiteCounterRows--;
                            }
                            if (currentscreen.GetCurrentCaption() == "Show\n Possible Black Moves\n")
                            {
                                ShowPossibleB = false;
                            }
                            if (currentscreen.GetCurrentCaption() == "Show\n Possible White Moves\n")
                            {
                                ShowPossibleW = false;
                            }
                            if (currentscreen.GetCurrentCaption() == "Game\n Room:\n" && Room != 0)
                            {
                                Room--;
                            }
                        }

                        else if (keys.IsKeyDown(Keys.Right))
                        {
                            currentscreen.Move(Direction.Right);
                            if (currentscreen.GetCurrentCaption() == "Video")
                            {
                                graphics.IsFullScreen = (currentscreen.GetCurrentValue() == "FullScreen");
                                graphics.ApplyChanges();
                            }
                            if (currentscreen.GetCurrentCaption() == "Difficulty\n")
                            {
                                SinglePlayerDifficulty++;
                            }
                            if (currentscreen.GetCurrentCaption() == "Rows \n  & Cols\n")
                            {
                                rows = rows + 2;
                                cols = cols + 2;

                            }
                            if (currentscreen.GetCurrentCaption() == "Black\n  Rows\n")
                            {
                                BlackCounterRows++;
                            }
                            if (currentscreen.GetCurrentCaption() == "White\n  Rows\n")
                            {
                                WhiteCounterRows++;
                            }
                            if (currentscreen.GetCurrentCaption() == "Show\n Possible Black Moves\n")
                            {
                                ShowPossibleB = true;
                            }
                            if (currentscreen.GetCurrentCaption() == "Show\n Possible White Moves\n")
                            {
                                ShowPossibleW = true;
                            }
                            if (currentscreen.GetCurrentCaption() == "Game\n Room:\n" && Room != 99)
                            {
                                Room++;
                            }
                        }
                        else
                            change = false;

                        if (change)
                        {
                            Halt = new System.Timers.Timer();
                            Halt.Interval = 200;
                            Halt.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                            Halt.Enabled = true;
                        }
                    }

                    if (SinglePlayerDifficulty == 3)
                        SinglePlayerDifficulty = 0;
                    if (SinglePlayerDifficulty == -1)
                        SinglePlayerDifficulty = 2;

                    break;

                case GameState.Playing:
                    if (FirstTurn == true) // if one the first turn picks a random starter
                    {
                        Random rnd = new Random();
                        int LeadingTurn = rnd.Next(0, 100);
                        if (LeadingTurn <= 50)
                            turn = true;
                        if (LeadingTurn > 50)
                            turn = false;

                        FirstTurn = false;
                    }
                    //LiteNetLib//////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (PlayNetWorked == true && SetServer == true)// && rows == 8 && cols == 8 && BlackCounterRows == 2 && WhiteCounterRows == 2)
                    {
                        if (!server && !client)
                        {
                            IClient = false;
                            NetworkedGame = true;
                            turn = false;
                            Connecting = 1;

                            server = true;
                            Console.WriteLine("Server!!!");

                            EventBasedNetListener listener = new EventBasedNetListener();
                            Server = new NetManager(listener);
                            Server.Start(9050 /* port */);

                            listener.ConnectionRequestEvent += request =>
                            {
                                if (Server.PeersCount < 2 /* max connections */)
                                    request.AcceptIfKey(Convert.ToString(Room));
                                else
                                    request.Reject();
                            };

                            listener.PeerConnectedEvent += peer =>
                            {
                                Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
                                NetDataWriter writer = new NetDataWriter();                 // Create writer class
                                writer.Put("Hello client!");                                // Put some string
                                peer.Send(writer, DeliveryMethod.ReliableOrdered);             // Send with reliability
                            };

                            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
                            {
                                string temp = dataReader.GetString(100);
                                Console.WriteLine("We got: {0}", temp);
                                Connecting = 2;

                                if (!temp.Contains("Nothing") && !temp.Contains("Hello"))
                                {
                                    string[] values = temp.Split('~');

                                    SelectedCounter.X = (float)Convert.ToInt32(values[0]);
                                    SelectedCounter.Y = (float)Convert.ToInt32(values[1]);
                                    SelectedX = Convert.ToInt32(values[2]);
                                    SelectedY = Convert.ToInt32(values[3]);

                                    ClientMove = true;
                                }
                                dataReader.Recycle();
                            };
                        }
                    }

                    if (SetServer == false && PlayNetWorked == true)// && rows == 8 && cols == 8 && BlackCounterRows == 2 && WhiteCounterRows == 2)
                    {
                        if (!server && !client)
                        {
                            IClient = true;
                            NetworkedGame = true;
                            turn = false;


                            JustConnected = true;


                            client = true;
                            Console.WriteLine("Client!!!");
                            Connecting = 1;
                            EventBasedNetListener listener = new EventBasedNetListener();
                            Client = new NetManager(listener);

                            Client.Start();
                            Client.Connect("localhost" /* host ip or name */, 9050 /* port */, Convert.ToString(Room) /* text key or NetDataWriter */);

                            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
                            {
                                string temp = dataReader.GetString(100);
                                Console.WriteLine("We got: {0}", temp);
                                Connecting = 2;


                                if (!temp.Contains("Nothing") && !temp.Contains("Hello"))
                                {
                                    string[] values = temp.Split('~');

                                    SelectedCounter.X = (float)Convert.ToInt32(values[0]);
                                    SelectedCounter.Y = (float)Convert.ToInt32(values[1]);
                                    SelectedX = Convert.ToInt32(values[2]);
                                    SelectedY = Convert.ToInt32(values[3]);

                                    if (JustConnected == true)
                                    {
                                        cols = Convert.ToInt32(values[4]);
                                        rows = Convert.ToInt32(values[4]);
                                        BlackCounterRows = Convert.ToInt32(values[5]);
                                        WhiteCounterRows = Convert.ToInt32(values[6]);
                                        ShowPossibleB = Convert.ToBoolean(values[7]);
                                        ShowPossibleW = Convert.ToBoolean(values[8]);

                                        state = GameState.MainMenu;
                                        LoadContent();


                                    }

                                    ServerMove = true;

                                }
                                dataReader.Recycle();
                            };
                        }
                    }

                    if (server)
                    {
                        Server.PollEvents();
                        Thread.Sleep(15);
                        if (!atimer.Enabled)
                            atimer.Enabled = true;
                    }
                    else if (client)
                    {
                        Client.PollEvents();
                        Thread.Sleep(15);
                        if (!atimer.Enabled)
                            atimer.Enabled = true;
                    }


                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    IncScale = (((double)(BlackSquareWidth + BlackSquareHeight) / 2) / 100 * 30);
                    IncScale2 = (((double)(BlackSquareWidth + BlackSquareHeight) / 2) - IncScale) / 80;


                    MouseState State = Mouse.GetState(); // Gets state of mouse (x, y,pressed etc)
                    current = Keyboard.GetState(); // Gets Keyboard state (whats pressed etc)
                    Vector2 MousePos = new Vector2(State.X, State.Y); // Sets a vector that can be used for mouse position

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        if (client)
                            Client.Stop();
                        else if (server)
                            Server.Stop();

                        Exit();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        rows = 8;
                        cols = 8;
                        WhiteCounterRows = 2;
                        BlackCounterRows = 2;

                        LoadContent();

                        currentscreen = menu;
                        state = GameState.MainMenu;

                    }

                    if (State.LeftButton == ButtonState.Pressed && !Halt.Enabled && State.X > 5 && State.X < 265 && State.Y > Height - 45 && State.Y < Height - 15 && turn == false) // Turns on showing whites possible moves
                    {
                        if (ShowPossibleW == true)
                            ShowPossibleW = false;
                        else ShowPossibleW = true;

                        Halt.Elapsed += new ElapsedEventHandler(OnTimedEvent); // Stops one click on the area being able to aqctive the option mulitple times by running 'OnTimedEvent'
                        Halt.Interval = 100;
                        Halt.Enabled = true;
                    }

                    if (State.LeftButton == ButtonState.Pressed && !Halt.Enabled && State.X > 280 && State.X < 540 && State.Y > Height - 45 && State.Y < Height - 15 && turn == true)// "" But black
                    {
                        if (ShowPossibleB == true)
                            ShowPossibleB = false;
                        else ShowPossibleB = true;

                        Halt.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        Halt.Interval = 100;
                        Halt.Enabled = true;
                    }




                    if ((State.LeftButton == ButtonState.Pressed && !Halt.Enabled) || (ServerMove == true || ClientMove == true || (SinglePlayer == true && turn == true)))
                    {
                        if (ClientMove == false && ServerMove == false)
                        {
                            int SelectedAreaXOverflow = State.X % BlackSquareWidth;
                            int SelectedAreaYOverflow = State.Y % BlackSquareHeight;
                            SelectedX = ((State.X - SelectedAreaXOverflow) / BlackSquareWidth);
                            SelectedY = ((State.Y - SelectedAreaYOverflow) / BlackSquareHeight);
                            Console.WriteLine("Clicked COOR " + (SelectedX) + " " + (SelectedY));
                            Console.WriteLine("Clicked X,Y " + (State.X) + " " + (State.Y));
                            //Above calculates the grid coordinates of where is pressed
                        }



                        try
                        {

                            if ((Grid.grid[SelectedX, SelectedY].active == true && IClient == true && turn == true) || (Grid.grid[SelectedX, SelectedY].active == true && IClient == false && turn == false) || (NetworkedGame == false && Grid.grid[SelectedX, SelectedY].active == true) && SinglePlayer == false)
                            {
                                if ((Grid.grid[SelectedX, SelectedY].counter == WL || Grid.grid[SelectedX, SelectedY].counter == WH) && turn == false) // Allows you to only select white on whites turn and displays the yellow counter over the selected counter
                                {
                                    SelectedCounter = new Vector2(SelectedX, SelectedY);
                                }
                                else if ((Grid.grid[SelectedX, SelectedY].counter == BL || Grid.grid[SelectedX, SelectedY].counter == BH) && turn == true)// "" but for black counters
                                {
                                    SelectedCounter = new Vector2(SelectedX, SelectedY);
                                }
                            }
                            else if ((Grid.grid[SelectedX, SelectedY].SquareColour == Color.White)) // Below else if contains all possible movements are required criteria
                            {
                                //White Movements
                                if ((Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter == WL) && turn == false)
                                {
                                    //Normal Movement ///////////////////////////////////
                                    if ((SelectedX == (int)SelectedCounter.X + 1 && SelectedY == (int)SelectedCounter.Y + 1) || (SelectedX == (int)SelectedCounter.X - 1 && SelectedY == (int)SelectedCounter.Y + 1 && Grid.grid[SelectedX, SelectedY].active == false))
                                    {
                                        Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                        Grid.grid[SelectedX, SelectedY].active = true;
                                        Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                        Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;

                                        turn = true;
                                    }
                                    //Different Taking ///////////////////////////////// 
                                    else if (SelectedX == (int)SelectedCounter.X + 2 && SelectedY == (int)SelectedCounter.Y + 2 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BH)))
                                    {
                                        Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                        Grid.grid[SelectedX, SelectedY].active = true;
                                        Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active = false;
                                        Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                        Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter = null;
                                        Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;

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

                                        BlackTaken = BlackTaken + 2;
                                        turn = true;
                                    }//taking double right

                                    SelectedCounter2 = SelectedCounter;
                                    SelectedCounter = new Vector2(-1, -1);
                                    NewTurn = true;
                                }

                                //Black Movements 
                                if ((Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter == BL) && turn == true)
                                {
                                    //Normal Movement /////////////////////////////
                                    if ((SelectedX == SelectedCounter.X + 1 && SelectedY == SelectedCounter.Y - 1) || (SelectedX == SelectedCounter.X - 1 && SelectedY == SelectedCounter.Y - 1 && Grid.grid[SelectedX, SelectedY].active == false))
                                    {
                                        Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                        Grid.grid[SelectedX, SelectedY].active = true;
                                        Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                        Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;

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

                                        WhiteTaken = WhiteTaken + 2;
                                        turn = false;
                                    }//taking double right
                                    NewTurn = true;
                                    SelectedCounter2 = SelectedCounter;
                                    SelectedCounter = new Vector2(-1, -1);
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

                                        if (turn == true)
                                            turn = false;
                                        else turn = true;
                                    }
                                    ///Single Takes////////////////////////////////
                                    else if (SelectedX == (int)SelectedCounter.X + 2 && SelectedY == (int)SelectedCounter.Y + 2 && Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active == true && Grid.grid[SelectedX, SelectedY].active == false && (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BL || (Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter == BH)))
                                    {
                                        Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].active = false;
                                        Grid.grid[SelectedX, SelectedY].active = true;
                                        Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].active = false;
                                        Grid.grid[SelectedX, SelectedY].counter = Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter;
                                        Grid.grid[(int)SelectedCounter.X + 1, (int)SelectedCounter.Y + 1].counter = null;
                                        Grid.grid[(int)SelectedCounter.X, (int)SelectedCounter.Y].counter = null;

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

                                        BlackTaken = BlackTaken + 2;
                                        if (turn == true)
                                            turn = false;
                                        else turn = true;
                                    }//taking double right
                                    NewTurn = true;
                                    SelectedCounter2 = SelectedCounter;
                                    SelectedCounter = new Vector2(-1, -1);
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

                                        WhiteTaken = WhiteTaken + 2;
                                        if (turn == true)
                                            turn = false;
                                        else turn = true;
                                    }//taking double right
                                    NewTurn = true;
                                    SelectedCounter2 = SelectedCounter;
                                    SelectedCounter = new Vector2(-1, -1);
                                }


                            }

                            //Single Player Decisions
                            if (SinglePlayer == true && turn == true)
                            {
                                SinglePlayerMoves();

                                
                            }


                        }

                        catch //(Exception ex)
                        {
                            //Console.WriteLine("hello "); 
                        }




                        ServerMove = false;
                        ClientMove = false;

                        Halt.Elapsed += new ElapsedEventHandler(OnTimedEvent); //""
                        Halt.Interval = 100;
                        Halt.Enabled = true;
                    }

                    try
                    {
                        if (Grid.grid[SelectedX, SelectedY].active == false) // Allows you unhighlight a counter by pressing on an inactive square
                            SelectedCounter = new Vector2(-1, -1);

                        if (SelectedY == (cols - 1) && Grid.grid[SelectedX, SelectedY].counter == WL) // Criteria for when a white is crowned to a higher piece (furthest away square possible)
                        {
                            Grid.grid[SelectedX, SelectedY].counter = WH;
                        }

                        if (SelectedY == 0 && Grid.grid[SelectedX, SelectedY].counter == BL) // "" but for black counters
                        {
                            Grid.grid[SelectedX, SelectedY].counter = BH;
                        }

                        if (PW == 0) // Sets when who wins to true when they no longer have any possible moves
                        {
                            BlackWin = true;
                        }

                        if (PB == 0)// ""
                        {
                            WhiteWin = true;
                        }

                        if ((WhiteWin == true || BlackWin == true) && Keyboard.GetState().IsKeyDown(Keys.Enter))// Allows you to exit the game by pressing Esc when a teams won 
                        {
                            LoadContent();
                            currentscreen = menu;
                            state = GameState.MainMenu;
                        }
                    }
                    catch //(Exception ex2) // Catches Errors
                    {
                        //Console.WriteLine("Error: " + ex2);
                    }

                    for (int WCY = 0; WCY < 8; WCY++)
                    {
                        for (int WCX = 0; WCX < 8; WCX++)
                        {
                            if (WhiteWin == true)
                            {
                                if (Grid.grid[WCX, WCY].counter == WL || Grid.grid[WCX, WCY].counter == WH)
                                {
                                    Grid.grid[WCX, WCY].counter = WWC;
                                }
                            }
                            if (BlackWin == true)
                            {
                                if (Grid.grid[WCX, WCY].counter == BL || Grid.grid[WCX, WCY].counter == BH)
                                {
                                    Grid.grid[WCX, WCY].counter = BWC;
                                }

                            }
                        }
                    }


                    if (turn == false) // Swaps whos turn it is
                        WhosTurn = "White";
                    else WhosTurn = "Black";

                    // TODO: Add your update logic here
                    PossibleMoves(); // Checkes for how many moves are possible for each team 
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            switch (state)
            {
                case GameState.MainMenu:
                    currentscreen.Draw(gameTime);
                    break;

                case GameState.Playing:
                    MouseState State = Mouse.GetState();

                    // TODO: Add your drawing code here
                    spriteBatch.Begin();
                    pixel.SetData(new[] { Color.Black });
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < cols; c++)
                        {
                            if (background[r, c])
                                spriteBatch.Draw(BlackSquare, new Rectangle(c * BlackSquareWidth, r * BlackSquareHeight, BlackSquareWidth, BlackSquareHeight), Color.Blue); //Draws the physical squares

                            if (Grid.grid[r, c].PossibleMoveB == true && ShowPossibleB == true && turn == true) // Draws the placemarkers for possible moves if criteria is met
                                spriteBatch.Draw(Marker, new Vector2((float)(r * BlackSquareWidth + ((BlackSquareWidth - 100 * IncScale2) / 2)), (float)(c * BlackSquareHeight + ((BlackSquareHeight - 100 * IncScale2) / 2))), null, Color.White, 0f, Vector2.Zero, (float)IncScale2, SpriteEffects.None, 0f);

                            if (Grid.grid[r, c].PossibleMoveW == true && ShowPossibleW == true && turn == false) // "" but for white counters 
                                spriteBatch.Draw(Marker2, new Vector2((float)(r * BlackSquareWidth + ((BlackSquareWidth - 100 * IncScale2) / 2)), (float)(c * BlackSquareHeight + ((BlackSquareHeight - 100 * IncScale2) / 2))), null, Color.White, 0f, Vector2.Zero, (float)IncScale2, SpriteEffects.None, 0f);



                        }
                    }
                    spriteBatch.End();


                    spriteBatch.Begin(); // Specifies the beginning of drawing
                                         // draws the boards of the board




                    spriteBatch.Draw(pixel, new Rectangle(0, 0, rows * BlackSquareWidth, BlackSquareHeight / 50), Color.Black);//Top  x,y,xl,yl
                    spriteBatch.Draw(pixel, new Rectangle(0, cols * BlackSquareHeight, rows * BlackSquareWidth + 2, BlackSquareHeight / 50), Color.Black);//Bottom
                    spriteBatch.Draw(pixel, new Rectangle(0, 0, BlackSquareWidth / 50, cols * BlackSquareHeight), Color.Black);//Left
                    spriteBatch.Draw(pixel, new Rectangle(rows * BlackSquareWidth, 0, BlackSquareWidth / 50, cols * BlackSquareHeight), Color.Black);//Right

                    //spriteBatch.Draw(WL, new Vector2(Width - (Width / 100 * 11), Height - (Height / 100 * 59)), null,Color.White, 0f, Vector2.Zero, (float)IncScale2, SpriteEffects.None, 0f); // Draws counters under counters lost
                    //spriteBatch.Draw(BL, new Vector2(Width - (Width / 100 * 11), (Height - (Height / 100 * 53)) + WL.Height), null,Color.White, 0f, Vector2.Zero, (float)IncScale2, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(Label, "  Counters\n  Lost:\n  White: " + WhiteTaken + "\n   Black: " + BlackTaken, new Vector2(Width / 90 * 85, Height - (Height / 200 * 138)), Color.Black); // draws the text for counters lost
                                                                                                                                                                                                          //spriteBatch.DrawString(Label, "x " + WhiteTaken, new Vector2(Width - 90, Height - (Height / 200 * 124)), Color.Black);
                                                                                                                                                                                                          //spriteBatch.DrawString(Label, "x " + BlackTaken, new Vector2(Width - 90, Height - (Height / 200 * 111)), Color.Black);





                    Vector2 start = new Vector2(rows * BlackSquareWidth + (BlackSquareWidth / 5), BlackSquareHeight / 2 - (BlackSquareHeight / 20));
                    Vector2 increm = new Vector2(0, BlackSquareHeight);
                    for (int i = 0; i < cols; i++)
                    {
                        spriteBatch.DrawString(Label, Convert.ToString(i + 1), start, Color.Black); // draws nums on y of the board
                        start += increm;
                    }

                    start = new Vector2(BlackSquareWidth / 2 - (BlackSquareWidth / 20), cols * BlackSquareHeight + (BlackSquareHeight / 5));
                    increm = new Vector2(BlackSquareWidth, 0);
                    for (int i = 0; i < rows; i++)
                    {
                        spriteBatch.DrawString(Label, Convert.ToString(i + 1), start, Color.Black); // Draws nums on x of the board
                        start += increm;
                    }





                    pixel.SetData(new[] { Color.White });


                    Grid.Draw(spriteBatch, BlackSquareWidth, BlackSquareHeight);

                    ////////////////////////////////SpriteFonts

                    spriteBatch.DrawString(Label, "Possible Moves:  White: " + PW + "  Black: " + PB + "  ||  Counters Lost:  White: " + WhiteTaken + "  Black: " + BlackTaken + "  ||  Turn: " + WhosTurn, new Vector2(10, Height - 40), Color.Black); // HUD at the bottom of the screen

                    if (BlackWin == true)
                    {
                        spriteBatch.DrawString(Label, "Black Wins!!!", new Vector2(700, 865), Color.Black); // Displays when black how won
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
                        spriteBatch.DrawString(Label, "White Wins!!!", new Vector2(700, 865), Color.Black); // Displays when whites won
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
                    //spriteBatch.Draw(CS, new Vector2((float)(SelectedCounter.X * BlackSquareWidth + ((BlackSquareWidth - 75) / 2) / IncScale2), (float)(SelectedCounter.Y * BlackSquareHeight + ((BlackSquareHeight - 75) / 2) / IncScale2), Color.Yellow);
                    spriteBatch.Draw(CS, new Vector2((float)(SelectedCounter.X * BlackSquareWidth + ((BlackSquareWidth - 70 * IncScale2) / 2)), (float)(SelectedCounter.Y * BlackSquareHeight + ((BlackSquareHeight - 70 * IncScale2) / 2))), null, Color.White, 0f, Vector2.Zero, (float)IncScale2, SpriteEffects.None, 0f);
                    if (Connecting == 0)
                    {
                        spriteBatch.Draw(OfflineMarker, new Vector2(Width - OfflineMarker.Width, 0), Color.Red);
                    }
                    if (Connecting == 1)
                    {
                        spriteBatch.Draw(ConnectingMarker, new Vector2(Width - OfflineMarker.Width, 0), null);
                    }
                    if (Connecting == 2)
                    {
                        spriteBatch.Draw(OnlineMarker, new Vector2(Width - OfflineMarker.Width, 0),null);
                    }
                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);

        }

        public void HigherQM()
        {

        }
           

        public void SinglePlayerMoves()
        {
            int HighestCurrentMove = 0;
            bool SinglePlayerMoved = false;
            Vector2 HighestMoveLand = new Vector2 (-1,-1);
            Vector2 Offset = new Vector2(1, 1);
            Vector2 Remove = new Vector2(-1, -1);
            Vector2 Remove2 = new Vector2(-1, -1);

            

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    Grid.grid[x, y].SquareScoreBlack = 0;
                    int RunThrough = 0;

                    while (RunThrough <= 3)
                    {

                       

                        if ((Grid.grid[x, y].counter == BL || Grid.grid[x, y].counter == BH ) && (int)Offset.Y == -1)
                        {
                            if (x + Offset.X * 2 <= rows && x + Offset.X * 2 >= 0 && y + Offset.Y * 2 >= 0 && y + Offset.Y * 2 <= rows)
                            {
                                if ((Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL || Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH) && Grid.grid[x + (int)Offset.X * 2, y + (int)Offset.Y * 2].active == false)
                                {
                                    if (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL)
                                        Grid.grid[x, y].SquareScoreBlack = 1;
                                    else if (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH)
                                        Grid.grid[x, y].SquareScoreBlack = 2;

                                    SinglePlayerMoved = true;
                                    
                                }

                            }
                            if (x + Offset.X * 4 <= rows && x + Offset.X * 4 >= 0 && y + Offset.Y * 4 >= 0 && y + Offset.Y * 4 <= rows)
                            {
                                if ((Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL || Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH) && Grid.grid[x + (int)Offset.X * 2, y + (int)Offset.Y * 2].active == false && (Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WL || Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH) && Grid.grid[x + (int)Offset.X * 4, y + (int)Offset.Y * 4].active == false)
                                {
                                    if ((Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL && Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WH) || (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH && Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WL) || (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL && Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WL))
                                        Grid.grid[x, y].SquareScoreBlack = 3;
                                    if (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH && Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WH)
                                        Grid.grid[x, y].SquareScoreBlack = 4;

                                    SinglePlayerMoved = true;
                                    
                                }

                            }
                        }


                        if (Grid.grid[x, y].counter == BH)
                        {
                            if (x + Offset.X * 2 <= rows && x + Offset.X * 2 >= 0 && y + Offset.Y * 2 >= 0 && y + Offset.Y * 2 <= rows)
                            {
                                if ((Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL || Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH) && Grid.grid[x + (int)Offset.X * 2, y + (int)Offset.Y * 2].active == false)
                                {
                                    if (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL)
                                        Grid.grid[x, y].SquareScoreBlack = 1;
                                    else if (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH)
                                        Grid.grid[x, y].SquareScoreBlack = 2;

                                    SinglePlayerMoved = true;
                                }

                            }
                            if (x + Offset.X * 4 <= rows && x + Offset.X * 4 >= 0 && y + Offset.Y * 4 >= 0 && y + Offset.Y * 4 <= rows)
                            {
                                if ((Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL || Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH) && Grid.grid[x + (int)Offset.X * 2, y + (int)Offset.Y * 2].active == false && (Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WL || Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH) && Grid.grid[x + (int)Offset.X * 4, y + (int)Offset.Y * 4].active == false)
                                {
                                    if ((Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL && Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WH) || (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH && Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WL) || (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WL && Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WL))
                                        Grid.grid[x, y].SquareScoreBlack = 3;
                                    if (Grid.grid[x + (int)Offset.X, y + (int)Offset.Y].counter == WH && Grid.grid[x + (int)Offset.X * 3, y + (int)Offset.Y * 3].counter == WH)
                                        Grid.grid[x, y].SquareScoreBlack = 4;

                                    SinglePlayerMoved = true;
                                }

                            }
                        }

                        if(Grid.grid[x,y].SquareScoreBlack > HighestCurrentMove)
                        {
                            
                            
                            HighestCurrentMove = Grid.grid[x, y].SquareScoreBlack;

                            if(HighestCurrentMove<=2)
                                HighestMoveLand = new Vector2(x + (int)Offset.X * 2, y + (int)Offset.Y * 2);
                            else
                                HighestMoveLand = new Vector2(x + (int)Offset.X * 4, y + (int)Offset.Y * 4);

                            if (Grid.grid[x,y].SquareScoreBlack <= 2)
                            {
                                Remove = new Vector2(x + (int)Offset.X, y + (int)Offset.Y);
                                WhiteTaken++;
                            }
                            if (Grid.grid[x, y].SquareScoreBlack > 2)
                            {
                                Remove = new Vector2(x + (int)Offset.X, y + (int)Offset.Y);
                                Remove2 = new Vector2(x + (int)Offset.X * 3, y + (int)Offset.Y * 3);
                                WhiteTaken += 2;
                            }
                        }

                        if (RunThrough == 0)
                            Offset = new Vector2(-1, 1);
                        if (RunThrough == 1)
                            Offset = new Vector2(1, -1);
                        if (RunThrough == 2)
                            Offset = new Vector2(-1, -1);

                        RunThrough++;
                    }
                }
            }

            int Numberof0 = 0, Numberof1 = 0, Numberof2 = 0, Numberof3 = 0, Numberof4 = 0;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    Console.WriteLine("Location: " + x + ", " + y + " Square Score: " + Grid.grid[x, y].SquareScoreBlack);

                    if (Grid.grid[x, y].SquareScoreBlack == 0)
                        Numberof0++;
                    if (Grid.grid[x, y].SquareScoreBlack == 1)
                        Numberof1++;
                    if (Grid.grid[x, y].SquareScoreBlack == 2)
                        Numberof2++;
                    if (Grid.grid[x, y].SquareScoreBlack == 3)
                        Numberof3++;
                    if (Grid.grid[x, y].SquareScoreBlack == 4)
                        Numberof4++;
                }
            }

            int HighestScore = 0;
            Vector2 HighestMove = new Vector2(0, 0);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    if(Grid.grid[x,y].SquareScoreBlack > HighestScore)
                    {
                        HighestScore = Grid.grid[x, y].SquareScoreBlack;
                        HighestMove = new Vector2(x, y);
                    }
                }
            }

            Vector2 MoveMagnitude = new Vector2(HighestMoveLand.X - HighestMove.X, HighestMoveLand.Y - HighestMove.Y);

            if(Remove2 == new Vector2(-1,-1) && HighestScore > 0)
            {
                Grid.grid[(int)HighestMoveLand.X, (int)HighestMoveLand.Y].counter = Grid.grid[(int)HighestMove.X, (int)HighestMove.Y].counter;
                Grid.grid[(int)Remove.X, (int)Remove.Y].counter = null;
                Grid.grid[(int)Remove.X, (int)Remove.Y].active = false;
                Grid.grid[(int)HighestMoveLand.X, (int)HighestMoveLand.Y].active = true;
                Grid.grid[(int)HighestMove.X, (int)HighestMove.Y].active = false;
                Grid.grid[(int)HighestMove.X, (int)HighestMove.Y].counter = null;

                SinglePlayerMoved = true;
            }
            else if (Remove2 != new Vector2(-1, -1) && HighestScore > 0)
            {
                Grid.grid[(int)HighestMoveLand.X, (int)HighestMoveLand.Y].counter = Grid.grid[(int)HighestMove.X, (int)HighestMove.Y].counter;
                Grid.grid[(int)Remove.X, (int)Remove.Y].counter = null;
                Grid.grid[(int)Remove.X, (int)Remove.Y].active = false;
                Grid.grid[(int)Remove2.X, (int)Remove2.Y].counter = null;
                Grid.grid[(int)Remove2.X, (int)Remove2.Y].active = false;
                Grid.grid[(int)HighestMoveLand.X, (int)HighestMoveLand.Y].active = true;
                Grid.grid[(int)HighestMove.X, (int)HighestMove.Y].active = false;
                Grid.grid[(int)HighestMove.X, (int)HighestMove.Y].counter = null;

                SinglePlayerMoved = true;
            }

            if (HighestCurrentMove == 0 && SinglePlayerMoved == false)
            {
                Random RandomCo = new Random();

                while (SinglePlayerMoved == false)
                {
                    int x = RandomCo.Next(0, 8);
                    int y = RandomCo.Next(0, 8);

                    
                    //for (int y = 0; y < rows; y++)
                    // {
                    // for (int x = 0; x < rows; x++)
                    //{
                    if ((Grid.grid[x, y].counter == BL || Grid.grid[x, y].counter == BH))
                    {
                        Random Choice = new Random();


                        int choice = Choice.Next(1, 4);

                        if (x + 1 <= rows && y - 1 >= 0)
                            if (Grid.grid[x + 1, y - 1].active == false)
                            {
                                Grid.grid[x, y].active = false;
                                Grid.grid[x + 1, y - 1].active = true;
                                Grid.grid[x + 1, y - 1].counter = Grid.grid[x, y].counter;
                                Grid.grid[x, y].counter = null;

                                SinglePlayerMoved = true;
                            }
                        if (x - 1 >= 0 && y - 1 >= 0)
                            if (Grid.grid[x - 1, y - 1].active == false && SinglePlayerMoved == false)
                            {
                                Grid.grid[x, y].active = false;
                                Grid.grid[x - 1, y - 1].active = true;
                                Grid.grid[x - 1, y - 1].counter = Grid.grid[x, y].counter;
                                Grid.grid[x, y].counter = null;

                                SinglePlayerMoved = true;
                            }
                        if (x - 1 >= 0 && y + 1 <= rows)
                            if (Grid.grid[x, y].counter == BH && Grid.grid[x - 1, y + 1].active == false && SinglePlayerMoved == false)
                            {
                                Grid.grid[x, y].active = false;
                                Grid.grid[x - 1, y + 1].active = true;
                                Grid.grid[x - 1, y + 1].counter = Grid.grid[x, y].counter;
                                Grid.grid[x, y].counter = null;

                                SinglePlayerMoved = true;
                            }
                        if (x + 1 <= rows && y + 1 <= rows)
                            if (Grid.grid[x, y].counter == BH && Grid.grid[x + 1, y + 1].active == false && SinglePlayerMoved == false)
                            {
                                Grid.grid[x, y].active = false;
                                Grid.grid[x + 1, y + 1].active = true;
                                Grid.grid[x + 1, y + 1].counter = Grid.grid[x, y].counter;
                                Grid.grid[x, y].counter = null;

                                SinglePlayerMoved = true;

                            }

                    }
                }
                    //} 
               // }
            }
            if (SinglePlayerMoved == true)
                turn = false;

            for (int x = 0; x < rows; x++)
            {
                if(Grid.grid[x, 0].counter == BL)
                {
                    Grid.grid[x, 0].counter = BH;
                }
            }
            PossibleMoves();
        }
           
            
            
            
            
            
            
            
            
            
            
            ///Scoring Squares/////////////////////////////////////////////////////////////

            /*for (int xs = 0; xs < +cols; xs++)
            {
                for (int ys = 0; ys < cols; ys++)
                {
                    Grid.grid[xs, ys].SquareScoreBlack = 0;
                }
            }

            for (int x = 0; x <+ cols; x++)
            {
                for (int y = 0; y <+ cols; y++)
                {
                    if (Grid.grid[x, y].counter == BL || Grid.grid[x, y].counter == BH)
                    {
                        int RunThrough = 0;
                        while (RunThrough != 4)
                        {
                            Vector2 Indent = new Vector2(1, -1);

                            if (x + Indent.X * 3 + (2 * Indent.X) <= cols && x + Indent.X * 3 + (2 * Indent.X) >= 0 && y + Indent.Y * 3 <= cols && y + Indent.Y * 3 >= 0)///cover if statement
                            {
                                /// End Numbers Need to be square rooted to find average
                                ///Scores single jump over a Higher counter ///Needs to look for WH coming from black side
                                if ((Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].counter == WH || Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].counter == WL) && Grid.grid[x + ((int)Indent.X * 2), y + ((int)Indent.Y * 2)].active == false)
                                {
                                    if ((Indent.Y < 0 && Grid.grid[x, y].counter == BL) || Grid.grid[x, y].counter == BH)
                                    {
                                        Grid.grid[x + ((int)Indent.X * 2), y + ((int)Indent.Y * 2)].SquareScoreBlack = Grid.grid[x + ((int)Indent.X * 2), y + ((int)Indent.Y * 2)].SquareScoreBlack + 40;
                                        if (Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].counter == WH)
                                            Grid.grid[x + ((int)Indent.X * 2), y + ((int)Indent.Y * 2)].SquareScoreBlack += 5;
                                        if ((Grid.grid[x + (int)Indent.X * 3, y + (int)Indent.Y * 3].counter == WL || Grid.grid[x + (int)Indent.X * 3, y + (int)Indent.Y * 3].counter == WH) && Grid.grid[x + (int)Indent.X * 3 + (int)Indent.X * 2, y + (int)Indent.Y * 3].active == false)
                                            Grid.grid[x + ((int)Indent.X * 2), y + ((int)Indent.Y * 2)].SquareScoreBlack -= -45;
                                        if ((Grid.grid[x + (int)Indent.X * 3 + (int)Indent.X * 2, y + (int)Indent.Y * 3].counter == WH || Grid.grid[x + (int)Indent.X * 3 + (int)Indent.X * 2, y + (int)Indent.Y * 3].counter == WL) && Grid.grid[x + (int)Indent.X * 3, y + (int)Indent.Y * 3].active == false)
                                            Grid.grid[x + ((int)Indent.X * 2), y + ((int)Indent.Y * 2)].SquareScoreBlack -= -45;
                                        if ((Grid.grid[x + (int)Indent.X * 3 + (int)Indent.X * 2, y + (int)Indent.Y * 3].counter == WH || Grid.grid[x + (int)Indent.X * 3 + (int)Indent.X * 2, y + (int)Indent.Y * 3].counter == WL) && (Grid.grid[x + (int)Indent.X * 3, y + (int)Indent.Y * 3].counter == WL || Grid.grid[x + (int)Indent.X * 3, y + (int)Indent.Y * 3].counter == WH))
                                            Grid.grid[x + ((int)Indent.X * 2), y + ((int)Indent.Y * 2)].SquareScoreBlack -= -45;
                                    }
                                } 
                            }
                            if (x + Indent.X * 5 + (2 * Indent.X) <= cols && x + Indent.X * 5 + (2 * Indent.X) >= 0 && y + Indent.Y * 5 <= cols && y + Indent.Y * 5 >= 0)
                            {
                                if ((Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].counter == WH || Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].counter == WL) && Grid.grid[x + ((int)Indent.X * 2), y + ((int)Indent.Y * 2)].active == false && (Grid.grid[x + ((int)Indent.X * 3), y + ((int)Indent.Y * 3)].counter == WL || Grid.grid[x + ((int)Indent.X * 3), y + ((int)Indent.Y * 3)].counter == WH) && Grid.grid[x + ((int)Indent.X * 4), y + ((int)Indent.Y * 4)].active == false)
                                {
                                    if ((Indent.Y < 0 && Grid.grid[x, y].counter == BL) || Grid.grid[x, y].counter == BH)
                                    {
                                        Grid.grid[x + ((int)Indent.X * 4), y + ((int)Indent.Y * 4)].SquareScoreBlack = Grid.grid[x + ((int)Indent.X * 4), y + ((int)Indent.Y * 4)].SquareScoreBlack + 80;
                                        if (Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].counter == WH)
                                            Grid.grid[x + ((int)Indent.X * 4), y + ((int)Indent.Y * 4)].SquareScoreBlack += 5;
                                        if (Grid.grid[x + (int)Indent.X * 2, y + (int)Indent.Y * 2].counter == WH)
                                            Grid.grid[x + ((int)Indent.X * 4), y + ((int)Indent.Y * 4)].SquareScoreBlack += 5;
                                        if ((Grid.grid[x + (int)Indent.X * 5, y + (int)Indent.Y * 5].counter == WL || Grid.grid[x + (int)Indent.X * 5, y + (int)Indent.Y * 5].counter == WH) && Grid.grid[x + (int)Indent.X * 5 + (int)Indent.X * 2, y + (int)Indent.Y * 5].active == false)
                                            Grid.grid[x + ((int)Indent.X * 4), y + ((int)Indent.Y * 4)].SquareScoreBlack -= -45;
                                        if ((Grid.grid[x + (int)Indent.X * 5 + (int)Indent.X * 2, y + (int)Indent.Y * 5].counter == WH || Grid.grid[x + (int)Indent.X * 5 + (int)Indent.X * 2, y + (int)Indent.Y * 5].counter == WL) && Grid.grid[x + (int)Indent.X * 5, y + (int)Indent.Y * 5].active == false)
                                            Grid.grid[x + ((int)Indent.X * 4), y + ((int)Indent.Y * 4)].SquareScoreBlack -= -45;
                                        if ((Grid.grid[x + (int)Indent.X * 5 + (int)Indent.X * 2, y + (int)Indent.Y * 5].counter == WH || Grid.grid[x + (int)Indent.X * 5 + (int)Indent.X * 2, y + (int)Indent.Y * 5].counter == WL) && (Grid.grid[x + (int)Indent.X * 5, y + (int)Indent.Y * 5].counter == WL || Grid.grid[x + (int)Indent.X * 5, y + (int)Indent.Y * 5].counter == WH))
                                            Grid.grid[x + ((int)Indent.X * 4), y + ((int)Indent.Y * 4)].SquareScoreBlack -= -45;
                                    }
                                }
                            }
                            if (Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].active == false)
                            {
                                if ((Indent.Y < 0 && Grid.grid[x, y].counter == BL) || Grid.grid[x, y].counter == BH)
                                {
                                    if (Grid.grid[x + (int)Indent.X * 2, y + (int)Indent.Y * 2].counter == WL || Grid.grid[x + (int)Indent.X * 2, y + (int)Indent.Y * 2].counter == WH)
                                        Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].SquareScoreBlack -= 45;
                                    if (Grid.grid[(x + (int)Indent.X * 2) - (int)Indent.X * 2, y + (int)Indent.Y * 2].counter == WL || Grid.grid[x + (int)Indent.X * 2, y + (int)Indent.Y * 2].counter == WH)
                                        Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].SquareScoreBlack -= 45;
                                    if (Grid.grid[x + (int)Indent.X * 2, y + (int)Indent.Y * 2].counter == WH && Grid.grid[x, y - (int)Indent.Y * 2].active == false)
                                        Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].SquareScoreBlack -= 45;
                                    if (Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].active == false && Grid.grid[x + (int)Indent.X * 2, y + (int)Indent.Y * 2].active == false)
                                        Grid.grid[x + (int)Indent.X, y + (int)Indent.Y].SquareScoreBlack += 5;
                                }
                            }

                            if (RunThrough == 1)
                                Indent = new Vector2(-1, 1);
                            if (RunThrough == 2)
                                Indent = new Vector2(-1, -1);
                            if (RunThrough == 3)
                                Indent = new Vector2(1, 1);
                        }



                    }
                }
            }*/
        
    

        public void SetBackground()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if((r + c) % 2 == 1)
                        background[r, c] = true;   // Sets where the black squares should be drawn
                }
                
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Halt.Enabled = false;
        }

        private void PossibleMoves() // Finds the possible moves for each counter and the team as a whole 
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

                for (int xreset = 0; xreset < rows; xreset++)
                {
                    for (int yreset = 0; yreset < cols; yreset++)
                    {
                        Grid.grid[xreset, yreset].PossibleMoveW = false;
                        Grid.grid[xreset, yreset].PossibleMoveB = false;
                    }
                }

                while (RunThrough <= 3)
                {
                    for (Num.Y = 0; Num.Y < cols; Num.Y++)
                    {
                        for (Num.X = 0; Num.X < rows; Num.X++)
                        {
                            if (Grid.grid[(int)Num.X, (int)Num.Y].active == true)
                            {
                                
                                RunThrough = 0;
                                while (RunThrough < 4)
                                {
                                    
                                    if (RunThrough == 0)
                                        Indent = new Vector2(1, 1);

                                    Looking = Num + Indent;

                                    
                                    if (Num.Y + Indent.Y >= cols || Num.Y + Indent.Y <= -1 || Num.X + Indent.X >= rows || Num.X + Indent.X <= -1)
                                    {
                                        if (RunThrough == 0)
                                            Console.WriteLine("Breach of Index for Square: " + Num);
                                    }
                                    else
                                    {
                                        
                                        
                                        
                                        
                                        if (Grid.grid[(int)Looking.X, (int)Looking.Y].active == false)
                                        {
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WL && Indent.Y > 0)
                                            {
                                                PW++;
                                                Grid.grid[(int)Looking.X, (int)Looking.Y].PossibleMoveW = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WH)
                                            {
                                                PW++;
                                                Grid.grid[(int)Looking.X, (int)Looking.Y].PossibleMoveW = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BL && Indent.Y < 0)
                                            {
                                                PB++;
                                                Grid.grid[(int)Looking.X, (int)Looking.Y].PossibleMoveB = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BH)
                                            {
                                                PB++;
                                                Grid.grid[(int)Looking.X, (int)Looking.Y].PossibleMoveB = true;
                                            }
                                        }
                                        
                                    }

                                    Looking2 = Looking + Indent;

                                    if (Looking2.Y >= cols || Looking2.Y <= -1 || Looking2.X >= rows || Looking2.X <= -1)
                                    {
                                        
                                    }
                                    else
                                    {
                                        if (Grid.grid[(int)Num.X, (int)Num.Y].active == true && Grid.grid[(int)Looking.X, (int)Looking.Y].active == true && Grid.grid[(int)Looking2.X, (int)Looking2.Y].active == false)
                                        {
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WL && Indent.Y > 0 && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BH))
                                            {
                                                PW++;
                                                Grid.grid[(int)Looking2.X, (int)Looking2.Y].PossibleMoveW = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WH && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BH))
                                            {
                                                PW++;
                                                Grid.grid[(int)Looking2.X, (int)Looking2.Y].PossibleMoveW = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BL && Indent.Y < 0 && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WH))
                                            {
                                                PB++;
                                                Grid.grid[(int)Looking2.X, (int)Looking2.Y].PossibleMoveB = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BH && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WH))
                                            {
                                                PB++;
                                                Grid.grid[(int)Looking2.X, (int)Looking2.Y].PossibleMoveB = true;
                                            }
                                        }
                                    }

                                    Looking3 = Looking2 + Indent;
                                    Looking4 = Looking3 + Indent;

                                    if (Looking4.Y >= cols || Looking4.Y <= -1 || Looking4.X >= rows || Looking4.X <= -1)
                                    {
                                        
                                    }
                                    else
                                    {
                                        if (Grid.grid[(int)Num.X, (int)Num.Y].active == true && Grid.grid[(int)Looking.X, (int)Looking.Y].active == true && Grid.grid[(int)Looking2.X, (int)Looking2.Y].active == false && Grid.grid[(int)Looking3.X, (int)Looking3.Y].active == true && Grid.grid[(int)Looking4.X, (int)Looking4.Y].active == false)
                                        {
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WL && Indent.Y > 0 && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BH) && (Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == BL || Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == BH))
                                            {
                                                PW++;
                                                Grid.grid[(int)Looking4.X, (int)Looking4.Y].PossibleMoveW = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == WH && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == BH) && (Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == BL || Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == BH))
                                            {
                                                PW++;
                                                Grid.grid[(int)Looking4.X, (int)Looking4.Y].PossibleMoveW = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BL && Indent.Y < 0 && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WH) && (Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == WL || Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == WH))
                                            {
                                                PB++;
                                                Grid.grid[(int)Looking4.X, (int)Looking4.Y].PossibleMoveB = true;
                                            }
                                            if (Grid.grid[(int)Num.X, (int)Num.Y].counter == BH && (Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WL || Grid.grid[(int)Looking.X, (int)Looking.Y].counter == WH) && (Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == WL || Grid.grid[(int)Looking3.X, (int)Looking3.Y].counter == WH))
                                            {
                                                PB++;
                                                Grid.grid[(int)Looking4.X, (int)Looking4.Y].PossibleMoveB = true;
                                            }
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
            if (PB == 0)
            {
                WhiteWin = true;
            }
        }

        private void Atimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            atimer.Enabled = false;
            count++;
            NetDataWriter writer = new NetDataWriter();

            //Send S = new Send();
            //S.SelectedCounter = SelectedCounter;
            //S.SelectedX = SelectedX;
            //S.SelectedY = SelectedY;
            

            string test = SelectedCounter2.X + "~" + SelectedCounter2.Y + "~" + SelectedX + "~" + SelectedY + "~" + cols + "~" + BlackCounterRows + "~" + WhiteCounterRows + "~" + ShowPossibleB + "~" + ShowPossibleW;

            

            if (SelectedCounter2.X != -1 && SelectedCounter2.Y != -1)
                writer.Put(test);
            else
                writer.Put("Nothing");

             SelectedCounter2 = new Vector2(-1, -1);
            
            

            if (client)
                Client.SendToAll(writer, DeliveryMethod.ReliableOrdered);
            else if (server)
                Server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
        }


    }
}
