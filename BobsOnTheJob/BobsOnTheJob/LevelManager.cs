using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// TO DO:
/// Figure out how/when the game will tell me what the new filename is
///  finish the filename ifs w/ more rooms enums 
///     get a set floor layout
/// </summary>

namespace BobsOnTheJob
{
    /// <summary>
    /// class that will handle the spawning of walls, character, collectables, etc
    /// </summary>
    class LevelManager
    {
        // fields for the class
        private Wall[,] bounds;
        private int height;
        private int width;
        private string[] splitLine;
        private char[] lineChars;
        private string currentLine;
        private StreamReader reader;
        Rooms currentRoom;
        private SpriteBatch drawer;
        private string fileName;
        private Texture2D playerTexture;
        private Texture2D wallTextureHorizontal;
        private Texture2D wallTextureVertical;
        private Texture2D floorTexture;
        private Texture2D doorTexture;
        private Texture2D coinTexture;
        private Texture2D trapTexture;
        private char trapVar;
        private char wallVar;
        private char floorVar;
        private char doorVar;
        private char playerVar;
        private float playerX;
        private float playerY;
        private Game1 gameboy;
        private Player boi;
        private List<Wall> doors;
        private Rooms previousRoom;
        private List<Wall> trapsAreGay;

        // filename will be changed somehow - trying to figure it out
        // then the drawing and rooms will depend on it
        // need to set a room layout
        public string FileName { get { return fileName; } set { fileName = value; } }
        public float PlayerX { get { return playerX; } }
        public float PlayerY { get { return playerY; } }
        public int Height { get { return height; } }
        public int Width { get { return width; } }
        public Wall[,] Bounds { get { return bounds; } }
        public List<Wall> Doors { get { return doors; } }
        public Rooms CurrentRoom { get { return currentRoom; } }
        public Rooms PrevRoom { get { return previousRoom; } }
        public List<Wall> Traps { get { return trapsAreGay; } }

        /// <summary>
        /// Constructor for the class
        /// takes a spriteBatch as a param
        /// initializes the array and makes the current room start room
        /// </summary>
        public LevelManager(SpriteBatch sb, Texture2D pt, Texture2D wth, Texture2D wtv, Texture2D ft, Texture2D dt, Texture2D ct, Texture2D tt, Game1 game, Player p)
        {
            drawer = sb;
            playerTexture = pt;
            wallTextureHorizontal = wth;
            wallTextureVertical = wtv;
            floorTexture = ft;
            doorTexture = dt;
            coinTexture = ct;
            gameboy = game;
            boi = p;
            currentRoom = Rooms.Start;
            fileName = "startRoom.txt";
            trapTexture = tt;
        }

        /// <summary>
        /// method to draw the current room 
        /// initializes the reader based on the file
        /// </summary>
        /// <param name="room"></param>
        public void Draw()
        {
            // if statements to draw rooms that hook up rooms and which file to read from

            reader = new StreamReader(fileName);
            try
            {
                doors = new List<Wall>();
                trapsAreGay = new List<Wall>();
                currentLine = reader.ReadLine();
                splitLine = currentLine.Split(' ');
                wallVar = char.Parse(splitLine[0]);
                floorVar = char.Parse(splitLine[1]);
                doorVar = char.Parse(splitLine[2]);
                playerVar = char.Parse(splitLine[3]);
                trapVar = char.Parse(splitLine[4]);
                height = int.Parse(splitLine[5]);
                width = int.Parse(splitLine[6]);
                bounds = new Wall[height, width]; // for when they get written into the text file 

                // for loops that cycle through the text file 
                for (int r = 0; r < height; r++) // rows
                {
                    lineChars = reader.ReadLine().ToCharArray();
                    for (int c = 0; c < lineChars.Length; c++)
                    {

                        // walls
                        if (lineChars[c] == wallVar) // cols
                        {
                            if (r == 0 && c == 0) // top left
                            {
                                bounds[r, c] = new Wall(wallTextureHorizontal, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(0, 0), Color.White, 0f, true);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (c == 0 && r != 0) // left most col
                            {
                                // sets the x to the same, but the y is the y of previous plsy the height of previous
                                bounds[r, c] = new Wall(wallTextureHorizontal, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(0, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.White, 0f, true);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (r == 0 && c != 0)
                            {
                                bounds[r, c] = new Wall(wallTextureHorizontal, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, 0), Color.White, 0f, true);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (c != 0 && r != 0) // the far wall top right corner
                            {
                                bounds[r, c] = new Wall(wallTextureHorizontal, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.White, 0f, true);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                        }


                        // floors
                        else if (lineChars[c] == floorVar)
                        {
                            if (r == 0 && c == 0) // top left
                            {
                                bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), Vector2.Zero, Color.DarkGray, 0f, false);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (c == 0 && r != 0) // left most col
                            {
                                bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(0, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.DarkGray, 0f, false);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (r == 0 && c != 0)
                            {
                                bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, 0), Color.DarkGray, 0f, false);
                            }
                            else if (c != 0 && r != 0) // the far wall top left corner
                            {
                                bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.DarkGray, 0f, false);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                        }

                        // door
                        else if (lineChars[c] == doorVar)
                        {
                            int boiSize = Math.Max(boi.Width, boi.Height);

                            if (r == 0 && c == 0) // top left
                            {
                                bounds[r, c] = new Wall(doorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), Vector2.Zero, Color.White, 0f, true);
                                bounds[r, c].WillCollide = false;
                                doors.Add(bounds[r, c]);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (c == 0 && r != 0) // left most col
                            {
                                bounds[r, c] = new Wall(doorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), Math.Max(boiSize, 100 * gameboy.R), new Vector2(0, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.White, 0f, true);
                                bounds[r, c].WillCollide = false;
                                doors.Add(bounds[r,c]);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (r == 0 && c != 0)
                            {
                                bounds[r, c] = new Wall(doorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, 0), Color.White, 0f, true);
                                bounds[r, c].WillCollide = false;
                                doors.Add(bounds[r, c]);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (c != 0 && r != 0) // the far wall top right corner
                            {
                                bounds[r, c] = new Wall(doorTexture, Math.Max(boiSize, 100 * gameboy.R), Math.Max(boiSize, 100 * gameboy.R), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.White, 0f, true);
                                bounds[r, c].WillCollide = false;
                                doors.Add(bounds[r, c]);
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                        }

                        // player
                        else if (lineChars[c] == playerVar)
                        {
                            bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.DarkGray, 0f, false);
                            // Console.WriteLine("{0},{1}", r, c);
                        }

                        else if(lineChars[c] == trapVar)
                        {
                            if (r == 0 && c == 0) // top left
                            {
                                bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), Vector2.Zero, Color.DarkGray, 0f, true);
                                bounds[r, c].WillCollide = false;
                                trapsAreGay.Add(new Wall(trapTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), Vector2.Zero, Color.White, 0f, false));
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (c == 0 && r != 0) // left most col
                            {
                                bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(0, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.DarkGray, 0f, true);
                                bounds[r, c].WillCollide = true;
                                trapsAreGay.Add(new Wall(trapTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(0, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.White, 0f, false));
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (r == 0 && c != 0)
                            {
                                bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, 0), Color.DarkGray, 0f, true);
                                bounds[r, c].WillCollide = false;
                                trapsAreGay.Add(new Wall(trapTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, 0), Color.White, 0f, false));
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                            else if (c != 0 && r != 0) // the far wall top right corner
                            {
                                bounds[r, c] = new Wall(floorTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.DarkGray, 0f, true);
                                bounds[r, c].WillCollide = false;
                                trapsAreGay.Add(new Wall(trapTexture, gameboy.GraphicsDevice.Viewport.Width / bounds.GetLength(1), gameboy.GraphicsDevice.Viewport.Height / bounds.GetLength(0), new Vector2(bounds[r, c - 1].Rectangle.X + bounds[r, c - 1].Width, bounds[r - 1, c].Rectangle.Y + bounds[r - 1, c].Height), Color.White, 0f, false));
                                // Console.WriteLine("{0},{1}", r, c);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
            }
            // Console.WriteLine(currentRoom.ToString());
        }

        // updates for rooms 
        public bool IsTransitioning()
        {
            // for the start room
            if (currentRoom == Rooms.Start)
            {
                // goes to the rock room
                if(boi.Rectangle.Intersects(doors[0].Rectangle))
                {
                    currentRoom = Rooms.Rock;
                    fileName = "rockRoom.txt";
                    previousRoom = Rooms.Start;
                    return true;
                }
                // goes t the scissor room
                else if(boi.Rectangle.Intersects(doors[1].Rectangle))
                {
                    currentRoom = Rooms.Scissor;
                    fileName = "scissorRoom.txt";
                    previousRoom = Rooms.Start;
                    return true;
                }
            }

            // for the rock room
            else if(currentRoom == Rooms.Rock)
            {
                // goes to the trap room
                if(boi.Rectangle.Intersects(doors[0].Rectangle))
                {
                    currentRoom = Rooms.Trap;
                    fileName = "trapRoom.txt";
                    previousRoom = Rooms.Rock;
                    return true;
                }
                // goes back to the start room
                else if(boi.Rectangle.Intersects(doors[1].Rectangle))
                {
                    currentRoom = Rooms.Start;
                    fileName = "startRoom.txt";
                    previousRoom = Rooms.Rock;
                    return true;
                }
            }

            // for the trap room
            else if(currentRoom == Rooms.Trap)
            {
                // goes to rock
                if(boi.Rectangle.Intersects(doors[0].Rectangle))
                {
                    currentRoom = Rooms.Rock;
                    fileName = "rockRoom.txt";
                    previousRoom = Rooms.Trap;
                    return true;
                }
            }

            // for the scissor room
            else if(currentRoom == Rooms.Scissor)
            {
                // goes to start
                if(boi.Rectangle.Intersects(doors[0].Rectangle))
                {
                    currentRoom = Rooms.Start;
                    fileName = "startRoom.txt";
                    previousRoom = Rooms.Scissor;
                    return true;
                }

                // goes to  randome room
                else if(boi.Rectangle.Intersects(doors[1].Rectangle))
                {
                    currentRoom = Rooms.Rand;
                    fileName = "randRoom.txt";
                    previousRoom = Rooms.Scissor;
                    return true;
                }
            }

            // for the random room
            else if(currentRoom == Rooms.Rand)
            {
                // goes to scissor
                if(boi.Rectangle.Intersects(doors[0].Rectangle))
                {
                    currentRoom = Rooms.Scissor;
                    fileName = "scissorRoom.txt";
                    previousRoom = Rooms.Rand;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Dies()
        /// returns a bool as per whether or not the player is ona trap
        /// </summary>
        /// <returns></returns>
        public bool Dies()
        {
            for (int i = 0; i < trapsAreGay.Count; i++)
            {
                if (boi.Rectangle.Intersects(trapsAreGay[i].Rectangle))
                {
                    boi.Alive = false;
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Win()
        /// returns a bool as per whether or not the player has beeten the floor
        /// </summary>
        /// <returns></returns>
        public bool Win()
        {
            if(gameboy.Wins >= 3)
            {
                currentRoom = Rooms.Win;
                fileName = "winRoom.txt";
                return true;
            }
            return false;
        }
    }
}
