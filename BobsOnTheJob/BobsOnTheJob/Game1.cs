using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

// random comment so i can re-push
// Fixed your shit 
namespace BobsOnTheJob
{
    public enum Rooms
    {
        Start,
        Test,
        Rock,
        Trap,
        Scissor,
        Rand,
        Win,
    }

    /// <summary>
    /// Cori also hates Git already
    /// Ryan has made this game his own.
    /// This is Josh's game. There are many like it, but this one is mine.
    /// Ryan hates Git already
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rng;

        #region Fields/Properties
        // Player Textures
        public Texture2D playerTexture;
        public Rectangle playerLocation;
        public SpriteFont stats;
        public Texture2D cursor;

        // Environment/Item Textures
        public Texture2D wallTopTexture;
        public Texture2D wallSideTexture;
        public Texture2D floorTexture;
        public Texture2D doorTexture;
        public Texture2D coinTexture;
        public Texture2D noteTexture;
        public Texture2D rockTexture;
        public Texture2D scissorsTexture;
        public Texture2D trapTexture;

        // Room State
        private Rooms currentRoom;
        public Rooms CurrentRoom { get { return currentRoom; } set { currentRoom = value; } }

        // Keyboard
        private KeyboardState kb;
        private KeyboardState previousKb;

        // Screen Ratios
        public Rectangle screen;
        public int R;

        // Class Objects
        List<Sprite> sprites;
        List<Sprite> puzzleRoomSprites;
        GameStateManager stateManager;
        LevelManager lvlManager;
        Player bob;
        Sprite gamePlate;
        Sprite gameObject;
        Sprite gamePlacer;
        Wall winDoor;
        // public Wall WinDoor { get { return winDoor; } }
        const int WINSTOWIN = 3;
        int wins;
        public int Wins { get { return wins; } }

        // collectables
        private Collectable DwaneJohnson;
        private Collectable FreddyKrueger;
        private Collectable starterNote;
        private Collectable randNote;

        // int to do the randomization of the pressure plate thing
        private int rand;
        #endregion

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
            //graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
            //graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width / 2;
            //graphics.IsFullScreen = true;
           //graphics.ApplyChanges();
            IsMouseVisible = false;
            rng = new Random();

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

            sprites = new List<Sprite>();

            #region Data
            // Screen ratios
            screen = this.GraphicsDevice.Viewport.Bounds;
            R = screen.Width / 800;

            // Font
            stats = Content.Load<SpriteFont>("Fonts//barcodeFont");

            // Tile textures
            doorTexture = Content.Load<Texture2D>("Environment//wall_top1");
            floorTexture = Content.Load<Texture2D>("Environment//stones");
            wallTopTexture = Content.Load<Texture2D>("Environment//wall1");
            wallSideTexture = Content.Load<Texture2D>("Environment//wall_top1");
            coinTexture = Content.Load<Texture2D>("Items//coin");
            noteTexture = Content.Load<Texture2D>("Items//SinglePage");
            rockTexture = Content.Load<Texture2D>("Items//rock");
            scissorsTexture = Content.Load<Texture2D>("Items//scissors");
            trapTexture = Content.Load<Texture2D>("Environment//trap");

            // Puzzle
            wins = 0;
            winDoor = null;
            #endregion

            #region Sets up the player
            playerTexture = Content.Load<Texture2D>("Bob//Movement//Movement_New");
            cursor = Content.Load<Texture2D>("GUI//Cursor");

            bob = new Player(playerTexture, -1, -1, new Vector2(350, 250), Color.White, 2f, true, this, sprites) // Player becomes a sprite
            {
                Input = new Input()
                {
                    Left = Keys.A,
                    Right = Keys.D,
                    Up = Keys.W,
                    Down = Keys.S,
                    Pickup = Keys.E,
                },
            };
            playerLocation = new Rectangle(screen.Width / 2, screen.Height / 2, bob.Width, bob.Height); ;
            #endregion


            // lvl manager thing
            lvlManager = new LevelManager(spriteBatch, playerTexture, wallTopTexture, wallSideTexture, floorTexture, doorTexture, coinTexture, trapTexture, this, bob);

            // Add to sprites
            #region Add to sprites
            Texture2D gamePlateTexture = coinTexture;


            gamePlateTexture = GiveRockPaperScissors(gamePlateTexture);

            gamePlate = new Sprite(Content.Load<Texture2D>("GUI//red_pressed"), 100 * R, 100 * R,
                new Vector2(screen.Right - 150 * R, screen.Height / 2 - 50 * R), Color.Red, 0f, false);
            gameObject = new Sprite(gamePlateTexture, 75 * R, 75 * R,
                new Vector2(gamePlate.Position.X + gamePlate.Width / 8, gamePlate.Position.Y + gamePlate.Height / 8),
                Color.White, 0f, false);
            gamePlacer = new Sprite(doorTexture, 50 * R, 50 * R, 
                new Vector2(gamePlate.Position.X - 100 * R, gamePlate.Position.Y + 25 * R), Color.PaleVioletRed, 0f, false);



            // use the add method w/ a nested for loop to add the 2d array from level manager
            sprites.Add(bob);
            sprites.Add(starterNote = new Note(noteTexture, 25 * R, 25 * R * 4 / 3, new Vector2(screen.Width / 2 - 25 / 2 * R, screen.Top + 100 * R), Color.White, 0f, true,
            "Once upon a midnight \nbasement, here \nstands Bob, \nchubby and complacent, \nover quaint but many \noffice supplies, a \nforgotten bore.\nWhile you waddle, \nnearly useless,\n" +
            "make yourself of \nworth and do this: \nFind the holy\n trinity of \nitemized hand mimicry. \nPlay along like \nchildren do, \nand the way will \nbe shown to you."));
            sprites.Add(new Light(Content.Load<Texture2D>("Items//LightMask"), 400 * R, 400 * R, new Vector2(gamePlate.Position.X - 100 * R, gamePlate.Position.Y - 150 * R), Color.Red, 0f, false, rng));
            sprites.Add(gamePlate);
            sprites.Add(gameObject);
            sprites.Insert(0, gamePlacer); 

            randNote = new Note(noteTexture, 25 * R, 25 * R * 4 / 3, new Vector2(screen.Width / 4 - 25 / 2 * R, screen.Top + 100 * R), Color.White, 0f, true,
            "You seriously haven't \nfigured it out yet? \nWhat on earth are \nyou doing?");
            #endregion

            #region Level
            // puts the level manager after so that it can actually do shit
            lvlManager = new LevelManager(spriteBatch, playerTexture, wallTopTexture, wallSideTexture, floorTexture, doorTexture, coinTexture, trapTexture, this, bob);
            lvlManager.Draw();
            for (int i = 0; i <= 10; i++)
            // {
            //     int blockWidth = this.GraphicsDevice.Viewport.Width / 11;
            //     int blockHeight = this.GraphicsDevice.Viewport.Height / 11;
            //     for (int j = 0; j <= 10; j++)
            //     {
            //         if (i == 0 || i == 10 || j == 0 || j == 10)
            //         {
            //             // One wall piece
            //             Sprite wall = new Sprite(wallPlaceholder, blockWidth, blockHeight, new Vector2(i * blockWidth + 5, j * blockHeight + 5), Color.White, 0f, true);
            //             sprites.Insert(0, wall);
            //         }
            //     }
            // }

            // used .Add() to put in what I got from lvlManager
            for (int r = 0; r < lvlManager.Height; r++)
            {
                for (int c = 0; c < lvlManager.Width; c++)
                {
                    sprites.Insert(0, lvlManager.Bounds[r, c]);
                }
            }
            #endregion

            // sets soem collectables so i can check the inventory for them
            DwaneJohnson = new Collectable(rockTexture, 25 * R, 25 * R, new Vector2(screen.Center.X - 25 / 2 * R, screen.Center.Y - 25 / 2 * R), Color.LightCyan, 0f, true);
            FreddyKrueger = new Collectable(scissorsTexture, 35 * R, 35 * R, new Vector2(screen.Center.X - 35 / 2 * R, screen.Center.Y - 35 / 2 * R), Color.LightCyan, 0f, true);

            // sets the random number for the pressure plate thing to rng.next(2)
            rand = rng.Next(2);

            // Loads content from other states (Don't worry, it'll only load what it's using)
            stateManager = new GameStateManager(this, sprites, bob);
            stateManager.LoadContent();
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
            // TODO: Add your update logic here

            // level manager stuff
            if(lvlManager.IsTransitioning() == true)
            {
                AddToSprites();
            }

            if(lvlManager.Win() == true && winDoor != null)
            {
                if(bob.Rectangle.Intersects(winDoor.Rectangle))
                {
                    AddToSprites();
                }
            }

            // Set keyboard data
            previousKb = kb;
            kb = Keyboard.GetState();
            Keys toInv = Keys.I;
            Keys toPlace = Keys.Q;
            Keys toPause = Keys.Escape;
            Keys toJourn = Keys.J;

            #region Conditional for Playing State (Same Conditional in Draw())

            // Conditional for Playing State (Same Conditional in Draw())
            if (stateManager.State == GameState.Playing)
            {
                // Update all sprites
                for(int i = 0; i < sprites.Count; i++)
                    sprites[i].Update(gameTime, sprites);

                // Esc to Pause Menu
                if (isPressedOnce(toPause))
                {
                    stateManager.State = GameState.Paused;
                    // Must recall LoadContent() in the GameStateManager to 
                    // load in data that is appropriate this time
                    stateManager.LoadContent();
                }

                // I to Inventory Menu
                if (isPressedOnce(toInv) || isPressedOnce(toPlace))
                {
                    if (isPressedOnce(toPlace)) stateManager.Placing = true;
                    stateManager.State = GameState.Inventory;
                    stateManager.LoadContent();
                }

                // J to Journal Menu
                if (isPressedOnce(toJourn))
                {
                    stateManager.State = GameState.Journal;
                    stateManager.LoadContent();
                }

                // checks if he dies - will restart/or something
                if (lvlManager.Dies() && !bob.Alive)
                {
                    // Console.WriteLine("true");
                    // restart the game or some shit
                    stateManager.State = GameState.Dead;
                    stateManager.LoadContent();
                }
            }
            else
            {
                // Update appropriate menu state
                stateManager.Update(gameTime);

                // Repress the activating key of a menu to exit that menu
                // and return to the playing state
                if ((stateManager.State == GameState.Inventory && isPressedOnce(toInv))
                    || (stateManager.State == GameState.Paused && isPressedOnce(toPause))
                    || (stateManager.State == GameState.Journal && isPressedOnce(toJourn))
                    || (stateManager.State == GameState.Inventory && stateManager.Placing && isPressedOnce(toPlace)))
                {
                    stateManager.State = GameState.Playing;
                    stateManager.Placing = false;
                }

                else if(stateManager.State != GameState.Paused && isPressedOnce(toPause))
                {
                    stateManager.State = GameState.Paused;
                    stateManager.LoadContent();
                }
            }
            #endregion

            #region Puzzle Logic
            if(stateManager.State == GameState.Playing)
            {
                for(int i = 0; i < sprites.Count; i++)
                {
                    if (sprites[i] is Collectable)
                    {
                        Collectable c = (Collectable)sprites[i];
                        // Has a collectable been placed in the block that has the appropriate texture
                        if (gamePlacer.Rectangle.Intersects(c.Rectangle)
                            && c.Placed)
                        {
                            if (CheckRockPaperScissors(c, gameObject))
                            {
                                wins++;

                                System.Threading.Thread.Sleep(500);

                                if (wins >= WINSTOWIN)
                                {
                                    gamePlate.Color = Color.Green;
                                    gamePlacer.Color = Color.Green;
                                    foreach (Sprite s_1 in sprites)
                                    {
                                        if (s_1 is Light)
                                        {
                                            Light l = (Light)s_1;
                                            l.Color = Color.Green;
                                        }
                                    }
                                }
                            }
                            Note n = null;
                            if (c is Note)
                            {
                                n = (Note)c;
                                sprites.Remove(n);
                                bob.inventory.Add(n);
                            }
                            else
                            {
                                sprites.Remove(c);
                                bob.inventory.Add(c);
                            }
                            gameObject.Texture = GiveRockPaperScissors(gameObject.Texture);
                        }
                    }
                }

                
                // Move puzzle box to indicate progress
                if(wins >= WINSTOWIN)
                {
                    if(gamePlate.Position.X < screen.Right)
                    {
                        gamePlate.Position += new Vector2(1, 0);
                        gameObject.Position += new Vector2(1, 0);
                    }
                    else
                    {
                        if (winDoor == null)
                        {
                            //System.Threading.Thread.Sleep(500);
                            sprites.Add(new Wall(doorTexture, 100, 100, new Vector2(screen.Right, gamePlate.Position.Y), Color.DarkGray, 0f, false));
                            winDoor = (Wall)sprites[sprites.Count - 1];
                        }
                        else
                        {
                            if(winDoor.Position.X > screen.Right - gamePlate.Width)
                            {
                                winDoor.Position += new Vector2(-1, 0);
                            }
                        }
                    }

                    
                }
            }
#endregion

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSteelBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            #region Drawing Game States
            // The game itself still shows on the screen if you are in any one 
            // of these game states
            if (stateManager.State == GameState.Playing
                 || stateManager.State == GameState.Paused
                 || stateManager.State == GameState.Inventory
                 || stateManager.State == GameState.Journal
                 || stateManager.State == GameState.Dead)
            { 
                foreach (Sprite sprite in sprites) 
                {
                    if (stateManager.State == GameState.Paused 
                        || stateManager.State == GameState.Inventory
                        || stateManager.State == GameState.Journal
                        || stateManager.State == GameState.Dead)
                    {
                        if (sprite.Color == Color.White)
                        {
                            sprite.Color = Color.LightGray;
                        }
                    }
                    else
                    {
                        if (sprite.Color == Color.LightGray)
                        {
                            sprite.Color = Color.White;
                        }
                    }
                    sprite.Draw(gameTime, spriteBatch);
                }
            }
            // Draw the appropriate menu state
            if (stateManager.State != GameState.Playing)
            {
                stateManager.Draw(gameTime, spriteBatch);
            }
            // Draws cursor last so it shows up above everything else
            spriteBatch.Draw(cursor, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, cursor.Width / 2, cursor.Height / 2), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
            #endregion
        }


        #region Extra Methods
        private bool isPressedOnce(Keys key)
        {
            return kb.IsKeyDown(key) && previousKb.IsKeyUp(key);
        }

        private void AddToSprites()
        {
            // lvl manager stuff
            lvlManager.Draw();

            sprites.Clear();

            // used .Add() to put in what I got from lvlManager
            for (int r = 0; r < lvlManager.Height; r++)
            {
                for (int c = 0; c < lvlManager.Width; c++)
                {
                    sprites.Insert(0, lvlManager.Bounds[r, c]);
                }
            }

            for(int x = 0; x < lvlManager.Traps.Count; x++)
            {
                sprites.Insert(sprites.Count - 1, lvlManager.Traps[x]);
            }

            if (lvlManager.CurrentRoom == Rooms.Start)
            {
                Texture2D gamePlateTexture = coinTexture;
                // int randOf3 = rng.Next(2);
                gamePlateTexture = GiveRockPaperScissors(gamePlateTexture);

                sprites.Add(gamePlate);
                sprites.Add(gameObject);
                sprites.Add(gamePlacer);
                sprites.Add(new Light(Content.Load<Texture2D>("Items//LightMask"), 400 * R, 400 * R, new Vector2(gamePlate.Position.X - 100 * R, gamePlate.Position.Y - 150 * R), Color.Red, 0f, false, rng));
                if (bob.inventory.Contains(starterNote) == false)
                {
                    sprites.Add(starterNote);
                }

                if (lvlManager.PrevRoom == Rooms.Rock)
                {
                    bob.Position = new Vector2(lvlManager.Doors[0].Rectangle.X, lvlManager.Doors[0].Rectangle.Y + 100);
                }

                else if (lvlManager.PrevRoom == Rooms.Scissor)
                {
                    bob.Position = new Vector2(lvlManager.Doors[1].Rectangle.X + 80, lvlManager.Doors[1].Rectangle.Y);
                }
            }

            else if (lvlManager.CurrentRoom == Rooms.Rock)
            {
                if (bob.inventory.Contains(DwaneJohnson) == false)
                {
                    sprites.Add(DwaneJohnson);
                }
                if (lvlManager.PrevRoom == Rooms.Start)
                {
                    bob.Position = new Vector2(lvlManager.Doors[1].Rectangle.X, lvlManager.Doors[1].Rectangle.Y - 75 * R);
                }
                else if (lvlManager.PrevRoom == Rooms.Trap)
                {
                    bob.Position = new Vector2(lvlManager.Doors[0].Rectangle.X + 80 * R, lvlManager.Doors[0].Rectangle.Y);
                }
            }

            else if (lvlManager.CurrentRoom == Rooms.Trap)
            {
                bob.Position = new Vector2(lvlManager.Doors[0].Rectangle.X - 50 * R, lvlManager.Doors[0].Rectangle.Y);
            }

            else if (lvlManager.CurrentRoom == Rooms.Scissor)
            {
                if (lvlManager.PrevRoom == Rooms.Start)
                {
                    bob.Position = new Vector2(lvlManager.Doors[0].Rectangle.X - 50 * R, lvlManager.Doors[0].Rectangle.Y);
                    if (bob.inventory.Contains(FreddyKrueger) == false)
                    {
                        sprites.Add(FreddyKrueger);
                    }
                }

                else if (lvlManager.PrevRoom == Rooms.Rand)
                {
                    bob.Position = new Vector2(lvlManager.Doors[1].Rectangle.X, lvlManager.Doors[1].Rectangle.Y - 75 * R);
                    if (bob.inventory.Contains(FreddyKrueger) == false)
                    {
                        sprites.Add(FreddyKrueger);
                    }
                }
            }

            else if (lvlManager.CurrentRoom == Rooms.Rand)
            {
                if(bob.inventory.Contains(randNote) == false)
                {
                    sprites.Add(randNote);
                }
                bob.Position = new Vector2(lvlManager.Doors[0].Rectangle.X, lvlManager.Doors[0].Rectangle.Y + 50 * R);
            }

            else if(lvlManager.CurrentRoom == Rooms.Win)
            {
                bob.Position = new Vector2(lvlManager.Doors[1].Rectangle.X + bob.Width * 2 * R, lvlManager.Doors[1].Rectangle.Y);
                foreach (Wall door in lvlManager.Doors)
                    door.WillCollide = true;
                sprites.Add(new Note(noteTexture, 25 * R, 25 * R * 4 / 3, new Vector2(screen.Width / 2, screen.Height / 2), Color.White, 0f, true,
            "Congratulations, my\nfine, fat, friend.\nYou have passed my\nfirst puzzle. But can\nyou beat the rest and\nmake it back to work?\nOnly time will tell."));
            }
            sprites.Insert(sprites.Count - 1, bob);
        }

        private bool CheckRockPaperScissors(Collectable c, Sprite s)
        {
            return (c.Texture == rockTexture && s.Texture == scissorsTexture)
                || (c.Texture == noteTexture && s.Texture == rockTexture)
                || (c.Texture == scissorsTexture && s.Texture == noteTexture);
        }

        private Texture2D GiveRockPaperScissors(Texture2D t)
        {
            Texture2D toReturn = null;
            do
            {
                int randOf3 = rng.Next(3);
                switch (randOf3)
                {
                    case 0: toReturn = rockTexture; break;
                    case 1: toReturn = scissorsTexture; break;
                    case 2: toReturn = noteTexture; break;
                    default: toReturn = null; break;
                }
            }
            while (t == toReturn);

            return toReturn;
        }

        public void Reset()
        {
            LoadContent();
        }
        #endregion



    }
}

