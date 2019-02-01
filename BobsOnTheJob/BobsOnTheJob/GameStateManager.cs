using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace BobsOnTheJob
{
    // Holds potential game states
    enum GameState
    {
        MainMenu,
        Instructions,
        Paused,
        Inventory,
        Journal, 
        Playing,
        Dead,
    }

    class GameStateManager
    {
        #region Fields
        // Will allow us to load content from the pipeline tool
        ContentManager Content;
        // Will allow us to get the graphics and such from the main class
        Game1 game;
        List<Sprite> gameSprites;
        MouseState previousMouse;
        MouseState currentMouse;
        Player bob;

        // Textures
        Texture2D title;
        Texture2D back;
        SpriteFont otherText;
        SpriteFont normalText;
        SpriteFont smallText;
        SpriteFont barcodeFont;

        // Stores game state
        private GameState gameState;
        private bool placing;

        // Will hold menu buttons
        private List<Component> gameComponents;
        private int inventoryPage;
        private List<string> noteDescriptions;
        List<Component> items;

        //Sound stuff
        Song bobTheme;
        #endregion

        #region Properties
        public GameState State { get { return gameState; } set { gameState = value; } }
        public bool Placing { get { return placing; } set { placing = value; } }
        #endregion

        #region Constructor
        public GameStateManager(Game1 game, List<Sprite> sprites, Player bob)
        {
            gameState = GameState.MainMenu;
            this.game = game;
            gameSprites = sprites;
            this.Content = game.Content;
            currentMouse = Mouse.GetState();
            previousMouse = currentMouse;
            this.bob = bob;
            inventoryPage = 0;
        }
        #endregion


        public void LoadContent()
        {
            gameComponents = null;
            barcodeFont = Content.Load<SpriteFont>("Fonts//barcodeFont");
            otherText = Content.Load<SpriteFont>("Fonts//arial15");
            smallText = Content.Load<SpriteFont>("Fonts//smallFont");
            Texture2D backTexture = Content.Load<Texture2D>("GUI//MenuOutline");
            Texture2D dragTexture = Content.Load<Texture2D>("GUI//DragButton");
            Texture2D listTexture = Content.Load<Texture2D>("GUI//list");

            if (bobTheme == null)
            {
                bobTheme = Content.Load<Song>("Sound//Bob'sTheme_mixdown");
                MediaPlayer.Play(bobTheme);
                MediaPlayer.IsRepeating = true;
            }

            // Loads content only for the game state it is needed
            switch (gameState)
            {
                #region Main Menu
                case GameState.MainMenu:
                    // Load textures and fonts
                    title = null;
                    title = Content.Load<Texture2D>("GUI//title"); 

                    // Create a Button and declare its events
                    Button play = new Button(Content.Load<Texture2D>("GUI//BlackButton"), Content.Load<Texture2D>("GUI//LightGrayButton"),
                        barcodeFont, "Play", 100 * game.R, 50 * game.R, new Vector2(game.screen.Width / 4,
                        game.screen.Height / 2), Color.White, Color.White, true, true);
                    play.Position -= new Vector2(play.Rectangle.Width / 2, 0);
                    play.Click += PlayButton_Click;
                    play.OnMouseDown += DefaultButton_OnMouseDown;
                    play.OnMouseUp += DefaultButton_OnMouseUp;

                    Button instructions = new Button(Content.Load<Texture2D>("GUI//BlackButton"), Content.Load<Texture2D>("GUI//LightGrayButton"),
                        barcodeFont, "Instr.", 100 * game.R, 50 * game.R, new Vector2(game.screen.Width / 2,
                        play.Position.Y), Color.White, Color.White, true, true);
                    instructions.Position -= new Vector2(instructions.Rectangle.Width / 2, 0);
                    instructions.Click += InstructionsButton_Click;
                    instructions.OnMouseDown += DefaultButton_OnMouseDown;
                    instructions.OnMouseUp += DefaultButton_OnMouseUp;

                    Button exit = new Button(Content.Load<Texture2D>("GUI//BlackButton"), Content.Load<Texture2D>("GUI//LightGrayButton"),
                        barcodeFont, "Exit", 100 * game.R, 50 * game.R, Vector2.Zero, Color.White, Color.White, true, true);
                    exit.Position = new Vector2(game.GraphicsDevice.Viewport.Bounds.Width * 3 / 4 -  exit.Rectangle.Width / 2, play.Position.Y);
                    exit.Click += QuitButton_Click;
                    exit.OnMouseDown += DefaultButton_OnMouseDown;
                    exit.OnMouseUp += DefaultButton_OnMouseUp;

                    Button tool = new Button(Content.Load<Texture2D>("GUI//BlackButton"), Content.Load<Texture2D>("GUI//DarkGrayButton"),
                        barcodeFont, "Tool", 100 * game.R, 50 * game.R, Vector2.Zero, Color.White, Color.White, false, true);
                    tool.Position = new Vector2(game.GraphicsDevice.Viewport.Bounds.Left + 25 / 2 * game.R,
                       game.GraphicsDevice.Viewport.Bounds.Bottom - tool.Height - 25 / 2 * game.R);
                    tool.OnMouseDown += DefaultButton_OnMouseDown;
                    tool.OnMouseUp += ToolButton_OnMouseUp;
                    tool.Click += ToolButton_Click;


                    // Adds buttons to the components
                    gameComponents = new List<Component>()
                    {
                        play,
                        exit,
                        instructions,
                        tool,
                    };
                    break;
#endregion

                #region Playing
                case GameState.Playing:
                    break;
#endregion

                #region Instructions
                case GameState.Instructions:

                    Button back_instr = new Button(Content.Load<Texture2D>("GUI//BlackButton"), Content.Load<Texture2D>("GUI//LightGrayButton"),
                        barcodeFont, "Back", 100 * game.R, 50 * game.R, new Vector2(game.GraphicsDevice.Viewport.Bounds.Left + 25 * game.R, game.GraphicsDevice.Viewport.Bounds.Top + 25 * game.R),
                        Color.White, Color.White, true, true);
                    back_instr.Click += BackButton_Click;
                    back_instr.OnMouseDown += DefaultButton_OnMouseDown;
                    back_instr.OnMouseUp += DefaultButton_OnMouseUp;

                    gameComponents = new List<Component>()
                    {
                        back_instr,
                    };
                    break;
#endregion

                #region Paused
                case GameState.Paused:

                    Button back_paused = new Button(backTexture, backTexture,
                        barcodeFont, "Paused", 375 * game.R, 375 * game.R, Vector2.Zero,
                        Color.White, Color.White, true, false);
                    back_paused.Position = new Vector2(game.screen.Center.X - back_paused.Width / 2,
                    game.screen.Bottom - back_paused.Height - 25 * game.R);
                    back_paused.TextPosition = new Vector2(back_paused.Rectangle.Left + back_paused.Width / 2 -
                        barcodeFont.MeasureString(back_paused.Text).X / 2, back_paused.Position.Y + back_paused.Height / 7
                        - barcodeFont.MeasureString(back_paused.Text).Y / 2);

                    Button home = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                        Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Home", back_paused.Width / 3, back_paused.Height / 6,
                        new Vector2(back_paused.Rectangle.Left + back_paused.Width / 20 + 25 * game.R, back_paused.Rectangle.Top + back_paused.Height / 3), Color.White, Color.White, true, true);
                    home.Click += MainMenuButton_Click;
                    home.OnMouseDown += DefaultButton_OnMouseDown;
                    home.OnMouseUp += DefaultButton_OnMouseUp;

                    Button resume = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                        Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Resume", home.Width, home.Height,
                        new Vector2(home.Position.X + home.Width + 25 * game.R, home.Position.Y), Color.White, Color.White, true, true);
                    resume.Click += ResumeButton_Click;
                    resume.OnMouseDown += DefaultButton_OnMouseDown;
                    resume.OnMouseUp += DefaultButton_OnMouseUp;

                    Button inventory = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                        Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Inventory", home.Width, home.Height,
                        Vector2.Zero, Color.White, Color.White, true, true);
                    inventory.Position = new Vector2(home.Position.X, home.Position.Y + inventory.Height + 25 * game.R / 2);
                    inventory.OnMouseDown += DefaultButton_OnMouseDown;
                    inventory.OnMouseUp += DefaultButton_OnMouseUp;
                    inventory.Click += Inv_Click;

                    Button journal = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                        Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Journal", home.Width, home.Height,
                        Vector2.Zero, Color.White, Color.White, true, true);
                    journal.Position = new Vector2(inventory.Position.X, inventory.Position.Y + journal.Height + 25 * game.R / 2);
                    journal.OnMouseDown += DefaultButton_OnMouseDown;
                    journal.OnMouseUp += DefaultButton_OnMouseUp;
                    journal.Click += Journal_Click;

                    Button drag_paused = new Button(Content.Load<Texture2D>("GUI//DragButton"),
                        Content.Load<Texture2D>("GUI//DragButton"), barcodeFont, "", 50 * game.R / 5 * 4, 50 * game.R / 5 * 4, Vector2.Zero, Color.White, Color.White, true, false);

                    drag_paused.Position = new Vector2(back_paused.Rectangle.Left + drag_paused.Width, back_paused.Rectangle.Top + (drag_paused.Height * 7/8));
                    drag_paused.OnMouseDown += DragButton_OnMouseDown;
                    drag_paused.OnMouseUp += DragButton_OnMouseUp;


                    gameComponents = new List<Component>()
                    {
                        back_paused,
                        resume,
                        home,
                        inventory,
                        journal,
                        drag_paused,
                    };
                    break;
#endregion

                #region Inventory
                case GameState.Inventory:
                    
                    gameComponents = new List<Component>();

                    Button back_inv = new Button(backTexture, backTexture,
                        barcodeFont, "Inventory", 375 * game.R, 375 * game.R,
                        Vector2.Zero, Color.White, Color.White, true, false);
                    if (placing) back_inv.Text += "\n(Placing)";
                    back_inv.TextPosition = new Vector2((back_inv.Rectangle.Left + (back_inv.Width)) + 25 * game.R -
                        (barcodeFont.MeasureString(back_inv.Text).X / 2), (back_inv.Rectangle.Y + back_inv.Height / 3 + 25 / 2 * game.R
                        - (barcodeFont.MeasureString(back_inv.Text).Y / 2)));
                    back_inv.Position = new Vector2(game.screen.Center.X - back_inv.Width / 2,
                        game.screen.Bottom - back_inv.Height - 25 * game.R);



                    Button list_inv = new Button(listTexture, listTexture, barcodeFont, "", back_inv.Width * 9 / 10, back_inv.Height * 2 / 3,
                        Vector2.Zero, Color.Gray, Color.White, true, false);
                    list_inv.Position = new Vector2(back_inv.Rectangle.Center.X - list_inv.Width / 2, back_inv.Rectangle.Center.Y - list_inv.Height / 3);

                    Button drag_inv = new Button(Content.Load<Texture2D>("GUI//DragButton"),
                        Content.Load<Texture2D>("GUI//DragButton"), barcodeFont, "", 50 * game.R / 5 * 4, 50 * game.R / 5 * 4, Vector2.Zero, Color.White, Color.White, true, false);
                    drag_inv.Position = new Vector2(back_inv.Rectangle.Left + drag_inv.Width, back_inv.Rectangle.Top + (drag_inv.Height * 7 / 8));
                    drag_inv.OnMouseDown += DragButton_OnMouseDown;
                    drag_inv.OnMouseUp += DragButton_OnMouseUp;


                    Button next_inv = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                        Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Next\nPage", 75 * game.R, 75 * game.R, Vector2.Zero, Color.White, Color.White, true, true);
                    next_inv.Position = new Vector2(back_inv.Rectangle.Right + next_inv.Width / 2, back_inv.Rectangle.Bottom - next_inv.Height * 3 / 2);
                    next_inv.OnMouseDown += DefaultButton_OnMouseDown;
                    next_inv.OnMouseUp += DefaultButton_OnMouseUp;
                    next_inv.Click += NextPageButton_Click;

                    Button previous = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                        Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Prev.\nPage", next_inv.Width, next_inv.Height, Vector2.Zero, Color.White, Color.White, true, true);
                    previous.Position = new Vector2(next_inv.Position.X, next_inv.Position.Y - previous.Height - 25 / 2 * game.R);
                    previous.OnMouseDown += DefaultButton_OnMouseDown;
                    previous.OnMouseUp += DefaultButton_OnMouseUp;
                    previous.Click += PrevPageButton_Click;

                    items = new List<Component>();

                    int index = 0;
                    int row = 3;
                    int column = 5;
                    int page = inventoryPage;

                    for(int i = 0; i < row; i++)
                    {
                        for(int j = 0; j < column; j++)
                        {
                            Collectable thisItem;
                            if(index + page * row * column < bob.inventory.Count && (thisItem = bob.inventory[index + page * row * column]) != null)
                            {
                                double width = 50 * game.R * thisItem.Texture.Width;
                                double height = 50 * game.R * thisItem.Texture.Width;
                                while (width >= list_inv.Width / 6 || height >= list_inv.Height / 6)
                                {
                                    width *= 0.95;
                                    height *= 0.95;
                                }
                                Button item = new Button(thisItem.Texture, thisItem.Texture, barcodeFont, "", (int)width, (int)height,
                                    new Vector2((j + 1) * list_inv.Width / 6 + list_inv.Position.X - (int)width / 2, (i * 2 + 1) * list_inv.Height / 6 + list_inv.Position.Y - (int)height / 2), 
                                    Color.White, Color.Black, true, true);

                                if (placing)
                                {
                                    item.Click += Item_Click;
                                }
                                items.Add(item);
                                index++;
                            }
                        }
                    }

                    gameComponents.Add(back_inv);
                    gameComponents.Add(list_inv);
                    gameComponents.Add(drag_inv);
                    if (bob.inventory.Count > index + page * row * column)
                        gameComponents.Add(next_inv);
                    if (page > 0)
                        gameComponents.Add(previous);
                    foreach (Component c in items) gameComponents.Add(c);
                    break;
                #endregion

                #region Journal
                case GameState.Journal:
                    gameComponents = new List<Component>();
                    noteDescriptions = new List<string>();

                    Button background_journal = new Button(backTexture, backTexture, barcodeFont, "Journal",
                        375 * game.R, 375 * game.R, Vector2.Zero, Color.White, Color.White, true, false);
                    background_journal.TextPosition = new Vector2((background_journal.Rectangle.Left + (background_journal.Rectangle.Width)) + 25 * game.R -
                        (barcodeFont.MeasureString(background_journal.Text).X / 2), (background_journal.Rectangle.Y + background_journal.Rectangle.Height / 3 + 25 / 2 * game.R
                        - (barcodeFont.MeasureString(background_journal.Text).Y / 2)));
                    background_journal.Position = new Vector2(game.screen.Center.X - background_journal.Width / 2,
                        game.screen.Bottom - background_journal.Height - 25 * game.R);

                    Button drag_journal = new Button(dragTexture, dragTexture, barcodeFont, "", 50 * game.R / 5 * 4, 50 * game.R / 5 * 4, Vector2.Zero, Color.White, Color.Black, true, false);
                    drag_journal.Position = new Vector2(background_journal.Rectangle.Left + drag_journal.Width, background_journal.Rectangle.Top + drag_journal.Height * 3 / 4);
                    drag_journal.OnMouseDown += DragButton_OnMouseDown;
                    drag_journal.OnMouseUp += DragButton_OnMouseUp;


                    Button next_journal = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                    Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Next\nPage", 75 * game.R, 75 * game.R, Vector2.Zero, Color.White, Color.White, true, true);
                    next_journal.Position = new Vector2(background_journal.Rectangle.Right + next_journal.Width / 2, background_journal.Rectangle.Bottom - next_journal.Height * 3 / 2);
                    next_journal.OnMouseDown += DefaultButton_OnMouseDown;
                    next_journal.OnMouseUp += DefaultButton_OnMouseUp;
                    next_journal.Click += NextPageButton_Click;


                    Button previous_journal = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                        Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Prev.\nPage", next_journal.Width, next_journal.Height, Vector2.Zero, Color.White, Color.White, true, true);
                    previous_journal.Position = new Vector2(next_journal.Position.X, next_journal.Position.Y - previous_journal.Height - 25 * game.R / 2);

                    previous_journal.OnMouseDown += DefaultButton_OnMouseDown;
                    previous_journal.OnMouseUp += DefaultButton_OnMouseUp;
                    previous_journal.Click += PrevPageButton_Click;

                    Button list_journal = new Button(dragTexture, dragTexture, barcodeFont, "", background_journal.Width * 9 / 10, background_journal.Height * 2 / 3,
                        Vector2.Zero, Color.Gray, Color.White, false, false);
                    list_journal.Position = new Vector2(background_journal.Rectangle.Center.X - background_journal.Width / 2, background_journal.Rectangle.Center.Y - list_journal.Height / 3);

                    List<Component> items_journal = new List<Component>();
                    int index_journal = 0;
                    int row_journal = 3;
                    int column_journal = 5;
                    int page_journal = inventoryPage;

                    List<Note> notes = new List<Note>();
                    int noteCount = 0;
                    foreach (Collectable c in bob.inventory)
                    {
                        if (c is Note)
                        {
                            Note n = (Note)c;
                            notes.Add(n);
                            noteCount++;
                        }
                    }

                    for (int i = 0; i < row_journal; i++)
                    {
                        for (int j = 0; j < column_journal; j++)
                        {
                            Collectable thisItem;
                            if (index_journal + page_journal * row_journal * column_journal < notes.Count && (thisItem = notes[index_journal + page_journal * row_journal * column_journal]) != null)
                            {
                                double width = 50 * thisItem.Texture.Width;
                                double height = 50 * thisItem.Texture.Width;
                                while (width >= list_journal.Width / 6 || height >= list_journal.Height / 6)
                                {
                                    width *= 0.95;
                                    height *= 0.95;
                                }

                                string itemText = "";
                                if(thisItem is Note)
                                {
                                    Note n = (Note)thisItem;
                                    itemText = n.Description.Substring(0, 5).Trim() + "...";      
                                    noteDescriptions.Add(n.Description);
                                }

                                Button item_journal = new Button(thisItem.Texture, thisItem.Texture, smallText, itemText, (int)width, (int)height,
                                    new Vector2((j + 1) * list_journal.Width / 6 + list_journal.Position.X - (int)width / 2, (i * 2 + 1) * list_journal.Height / 6 + list_journal.Position.Y - (int)height / 2),
                                    Color.White, Color.White, true, true);
                                if (thisItem is Note) item_journal.Height = item_journal.Width * 4 / 3;
                                item_journal.TextPosition = new Vector2((item_journal.Rectangle.Left + (item_journal.Rectangle.Width)) + 25 * game.R -
                                (smallText.MeasureString(item_journal.Text).X * 4 / 3), (item_journal.Rectangle.Bottom + (smallText.MeasureString(item_journal.Text).Y)));
                                item_journal.Click += Note_Click;
                                items_journal.Add(item_journal);
                                index_journal++;
                            }
                        }
                    }

                    gameComponents.Add(background_journal);
                    gameComponents.Add(list_journal);
                    gameComponents.Add(drag_journal);
                    if (notes.Count > index_journal + page_journal * row_journal * column_journal)
                        gameComponents.Add(next_journal);
                    if (page_journal > 0)
                        gameComponents.Add(previous_journal);
                    foreach (Component c in items_journal) gameComponents.Add(c);
                    break;
                #endregion

                #region Dead
                case GameState.Dead:
                    Button YoureDaed = new Button(backTexture, backTexture, barcodeFont, "YOU KILLED BOB!", game.screen.Width / 2, game.screen.Height / 2,
                        Vector2.Zero, Color.White, Color.White, true, false);
                    YoureDaed.Position = new Vector2(game.screen.Center.X - YoureDaed.Width / 2, game.screen.Center.Y - YoureDaed.Height / 2);
                    YoureDaed.TextPosition = new Vector2(YoureDaed.Rectangle.Center.X - (barcodeFont.MeasureString(YoureDaed.Text).X / 2), YoureDaed.Rectangle.Top + (barcodeFont.MeasureString(YoureDaed.Text).Y));

                    Button returnToMenu = new Button(Content.Load<Texture2D>("GUI//BlackButton"),
                        Content.Load<Texture2D>("GUI//LightGrayButton"), barcodeFont, "Restart\n Game", 150 * game.R, 100 * game.R, 
                        Vector2.Zero, Color.White, Color.White, true, true);
                    returnToMenu.Position = new Vector2(YoureDaed.Rectangle.Center.X - returnToMenu.Width / 2, YoureDaed.Rectangle.Center.Y - returnToMenu.Height / 4);

                    returnToMenu.OnMouseDown += DefaultButton_OnMouseDown;
                    returnToMenu.OnMouseUp += DefaultButton_OnMouseUp;
                    returnToMenu.Click += MainMenuButton_Click;

                    gameComponents = new List<Component>()
                    {
                        YoureDaed,
                        returnToMenu,
                    };
                    break;
                    #endregion
            }
        }


        #region Menu Events
        private void DefaultButton_OnMouseDown(object sender, EventArgs e)
        {
            if (sender is Button) // Button becomes gray
            {
                Button temp = (Button)sender;
                temp.CurrentTexture = temp.PressedTexture;
            }
        }

        private void DragButton_OnMouseDown(object sender, EventArgs e)
        {
            currentMouse = Mouse.GetState();

            Vector2 mouseChange = new Vector2(currentMouse.Position.X - previousMouse.Position.X,
                            currentMouse.Y - previousMouse.Y);

            foreach (Component c2 in gameComponents)
            {
                if (c2 is Button)
                {
                    Button component2 = (Button)c2;
                    Vector2 newPos = (component2.Position + mouseChange);
                    if (newPos.Y <= 0 || newPos.Y + component2.Height >= game.GraphicsDevice.Viewport.Bounds.Bottom)
                        mouseChange.Y = 0;
                    if (newPos.X <= 0 || newPos.X + component2.Width >= game.GraphicsDevice.Viewport.Bounds.Right)
                        mouseChange.X = 0;
                }
            }
            foreach (Component c in gameComponents)
            {
                if (c is Button)
                {
                    Button component = (Button)c;
                    component.Position += mouseChange;
                    component.TextPosition += mouseChange;
                }
            }

            previousMouse = currentMouse;
            }

        private void DefaultButton_OnMouseUp(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button temp = (Button)sender;
                temp.CurrentTexture = temp.UnpressedTexture;
            }
        }

        private void DragButton_OnMouseUp(object sender, EventArgs e)
        {
            currentMouse = Mouse.GetState();
            previousMouse = currentMouse;
        }

        private void ToolButton_OnMouseUp(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button temp = (Button)sender;
                if (temp.IsHovering)
                {
                    temp.Visible = true;
                    temp.CurrentTexture = temp.UnpressedTexture;
                }
                else
                {
                    temp.Visible = false;
                }
            }
        }

        private void ResumeButton_Click(object sender, EventArgs e)
        {
            gameState = GameState.Playing;
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            gameState = GameState.MainMenu;
            LoadContent();
            game.Reset();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            gameState = GameState.MainMenu;
            LoadContent();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            game.Exit(); // Closes game window
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            gameState = GameState.Playing; // plays the game
        }

        private void InstructionsButton_Click(object sender, EventArgs e)
        {
            gameState = GameState.Instructions;
            LoadContent();
        }

        private void ToolButton_Click(object sender, EventArgs e)
        {
            Tool tool = new Tool();
            tool.Show();
        }

        private void NextPageButton_Click(object sender, EventArgs e)
        {
            inventoryPage++;
            LoadContent();
        }

        private void PrevPageButton_Click(object sender, EventArgs e)
        {
            inventoryPage--;
            LoadContent();
        }
    
        private void Note_Click(object sender, EventArgs e)
        {
            Button note = null;
            if (sender is Button)
                note = (Button)sender;

            if (gameComponents[0] is Button)
            {
                Button main = (Button)gameComponents[0];
                Vector2 toPos = new Vector2(game.screen.Width / 4 - main.Width / 2,
                    game.screen.Height / 2 - main.Height / 2);
                Vector2 change = toPos - main.Position;

                foreach (Component c in gameComponents)
                {
                    if (c is Button)
                    {
                        Button b = (Button)c;
                        b.Position += change;
                        b.TextPosition += change;
                    }
                }
            }


            string paperWords = "";
            foreach(string description in noteDescriptions)
                if ((description.Substring(0, 5).Trim() + "...").Equals(note.Text))
                    paperWords = description;

            Button paper = new Button(Content.Load<Texture2D>("Items//SinglePage"), Content.Load<Texture2D>("Items//SinglePage"), 
                Content.Load<SpriteFont>("Fonts//barcodeFont"), paperWords, 300 * game.R, 400 * game.R,
                Vector2.Zero, Color.White, Color.Black, true, false);
            paper.Position = new Vector2(game.screen.Center.X + (game.screen.Width / 2 - paper.Width) / 2, game.screen.Top + (game.screen.Height - paper.Height) / 2);
            paper.TextPosition = new Vector2(paper.Position.X * 9 / 8, paper.Position.Y + paper.Width * 11 / 60);

            bool dontAdd = false;
            for(int i = 0; i < gameComponents.Count; i++)
                if (gameComponents[i] is Button)
                {
                    Button button = (Button)gameComponents[i];
                    if (button.Text == paper.Text) dontAdd = true;
                    if (button.Position == paper.Position)gameComponents.Remove(button);
                }
            if (!dontAdd) gameComponents.Add(paper);
        }

        private void Inv_Click(object sender, EventArgs e)
        {
            gameState = GameState.Inventory;
            LoadContent();
        }

        private void Journal_Click(object sender, EventArgs e)
        {
            gameState = GameState.Journal;
            LoadContent();
        }

        private void Item_Click(object sender, EventArgs e)
        {
            Button itemButton = null;
            if (sender is Button) itemButton = (Button)sender;

            // Set item to be placed
            Collectable item = null;
            Note note = null;
            int itemIndex = 15 * (inventoryPage) + items.IndexOf(itemButton);

            if (bob.inventory[itemIndex] is Note)
                note = (Note)bob.inventory[itemIndex];
            else item = bob.inventory[itemIndex];

            // Set position to be placed
            if (item == null)
            {
                note.Position = new Vector2(bob.Rectangle.Center.X - note.Width / 2, 
                    bob.Rectangle.Center.Y - note.Height / 2);
                bob.inventory.Remove(note);
                note.Collected = false;
                note.Placed = true;
                gameSprites.Add(note);
            }
            else
            {
                item.Position = new Vector2(bob.Rectangle.Center.X - item.Width / 2,
                    bob.Rectangle.Center.Y - item.Height / 2);
                bob.inventory.Remove(item);
                item.Collected = false;
                item.Placed = true;
                gameSprites.Add(item);
            }

            //bob.AnimState = AnimationState.Place;
            gameState = GameState.Playing;
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    UpdateDefault(gameTime);
                    break;
                case GameState.Instructions:
                    UpdateDefault(gameTime);
                    break;
                case GameState.Paused:
                    UpdateDefault(gameTime);
                    break;
                case GameState.Inventory:
                    UpdateDefault(gameTime);
                    break;
                case GameState.Journal:
                    UpdateDefault(gameTime);
                    break;
                case GameState.Playing:
                    // Do Nothing (This is delegated to the Game1 class)
                    break;
                case GameState.Dead:
                    UpdateDefault(gameTime);
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime, spriteBatch);
                    break;
                case GameState.Instructions:
                    DrawInstructions(gameTime, spriteBatch);
                    break;
                case GameState.Paused:
                    DrawDefault(gameTime, spriteBatch);
                    break;
                case GameState.Inventory:
                    DrawDefault(gameTime, spriteBatch);
                    break;
                case GameState.Journal:
                    DrawDefault(gameTime, spriteBatch);
                    break;
                case GameState.Playing:
                    // Do Nothing (This is delegated to the Game1 class)
                    break;
                case GameState.Dead:
                    DrawDefault(gameTime, spriteBatch);
                    break;
            }
        }

        #region Update Game States
        private void UpdateDefault(GameTime gameTime)
        {
            for(int i = 0; i < gameComponents.Count; i++) // updates buttons
            {
                gameComponents[i].Update(gameTime);
            }
        }
        #endregion

        #region Draw Game States
        private void DrawMainMenu(GameTime gameTime, SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.Clear(Color.FloralWhite); // background is white

            spriteBatch.Draw(title, // Draw title logo
                new Rectangle(game.GraphicsDevice.Viewport.Bounds.Width / 2 - title.Width / 5 / 2, 
                -50, title.Width / 5, title.Height / 5), Color.White);

            foreach(Component component in gameComponents) // draw buttons
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

        private void DrawInstructions(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle screen = game.GraphicsDevice.Viewport.Bounds;
            String titleText = "Instructions:";
            game.GraphicsDevice.Clear(Color.FloralWhite);
            // the string that will be printed as the instructions
            String instruct = String.Format("Search the depths of your office's basement\nfor clues on how to get back to up your boring\nlife in a cubicle.\n\nYour goal is to collect clues and use " 
                + "them to\nfind your way out of the basement.\n\nMovement: WASD\nPause: Esc\nInventory: I\nJournal: J\nCollect Item: E\nPlace Item: Q");

            // pints the title of the section the player is in: Instructions
            spriteBatch.DrawString(barcodeFont, titleText, new Vector2(screen.Width / 2 - otherText.MeasureString(titleText).X / 2, 20f), Color.Black);

            // actually printing out the instructions
            spriteBatch.DrawString(otherText, instruct, new Vector2(screen.Width / 2 - otherText.MeasureString(instruct).X / 2, screen.Height / 2 - otherText.MeasureString(instruct).Y / 2), Color.Black);
            foreach (Component component in gameComponents)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }


        private void DrawDefault(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int i = 0; i < gameComponents.Count; i++)
            {
                gameComponents[i].Draw(gameTime, spriteBatch);
            }
        }
#endregion

    }
}
