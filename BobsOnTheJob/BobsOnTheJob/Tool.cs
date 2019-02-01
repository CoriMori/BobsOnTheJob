using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

//-----List for Cori's own Sanity------
//[x] Figure out how to get this to show up so I can test it
//[x] need to write array to file
//[x] need to fill room array with variables
//[x] need to seperate out variables for wall, floor, door, player, and others
//[x] add in else statements to catch negative values
//[x] establish a set number of variables for level building
//[x] finish funtionality for door and player spawn location
//[x] Add in pop up if something goes wrong
//[x] Fix bug where door only place at [0,0]
//[x] Fix the door x and door y values so that one is min/max and the other moves freely

namespace BobsOnTheJob
{
    /// <summary>
    /// Form that will handle the creation and design of levels by reading and writing to a binary file
    /// </summary>
    public partial class Tool : Form
    {
        #region Fields
        //fields for tool
        /*private FileStream inStream;
        private BinaryReader reader;
        private FileStream outStream;
        private BinaryWriter writer;
        private string level="LevelInfo.data";
        */
        private string[,] room;
        private string[] variableArray;
        private int heightOfRoom;
        private int widthOfRoom;
        private string wall;
        private string floor;
        private string door;
        private string player;
        private string traps;
        private int trapsXVal;
        private int trapsYVal;
        private int doorXVal;
        private int doorYVal;
        private int door2XVal;
        private int door2YVal;
        private int playerXVal;
        private int playerYVal;
        //Temp fields
        private StreamWriter writer;
        private string fileName;
        #endregion

        //"constructor"
        public Tool()
        {
            InitializeComponent();
        }
        #region Variable Methods
        /// <summary>
        /// Changes the file name so there will be multiple levels that can be edited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNameBox_TextChanged(object sender, EventArgs e)
        {
            fileName = fileNameBox.Text;
        }
        /// <summary>
        /// Writes variables entered into the variable box into a binary text file
        /// Seperate variables with commas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VarBox_TextChanged(object sender, EventArgs e)
        {
            string variables = varBox.Text;
            variableArray = variables.Split(',');
            if (variableArray.Length == 5)
            {
                wall = variableArray[0];
                floor = variableArray[1];
                door = variableArray[2];
                player = variableArray[3];
                traps = variableArray[4];
            }
            else if (variableArray.Length < 5)
            {
                return;
            }
            else if (variableArray.Length > 5)
            {
                string message = "Too many variables";
                MessageBox.Show(message);
            }
        }

        /// <summary>
        /// Sets the height of the room
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoomHeightUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (roomHeightUpDown.Value > 0)
            {
                heightOfRoom = (int)roomHeightUpDown.Value;
            }
        }

        /// <summary>
        /// Sets the width of the room
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RoomWidthUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (roomWidthUpDown.Value > 0)
            {
                widthOfRoom = (int)roomWidthUpDown.Value;
            }
        }

        /// <summary>
        /// Sets the X coordinate of the Player 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerX_ValueChanged(object sender, EventArgs e)
        {
            if ((int)playerX.Value - 1 < widthOfRoom && (int)playerX.Value - 1 < heightOfRoom)
            {
                playerXVal = (int)playerX.Value;
            }
            else
            {
                string message = "Cannot place player outside of room";
                MessageBox.Show(message);
            }
        }

        /// <summary>
        /// Sets the Y coordinate of the Player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerY_ValueChanged(object sender, EventArgs e)
        {
            if ((int)playerY.Value - 1 < widthOfRoom && (int)playerY.Value - 1 < heightOfRoom)
            {
                playerYVal = (int)playerY.Value;
            }
            else
            {
                string message = "Cannot place player outside of room";
                MessageBox.Show(message);
            }
        }

        private void TrapsX_ValueChanged(object sender, EventArgs e)
        {
            if ((int)trapsX.Value < widthOfRoom && (int)playerY.Value < heightOfRoom)
                trapsXVal = (int)trapsX.Value;
            else
                MessageBox.Show("Cannot place traps outside of the room");
        }

        private void TrapsY_ValueChanged(object sender, EventArgs e)
        {
            if ((int)trapsX.Value < widthOfRoom && (int)playerY.Value < heightOfRoom)
                trapsYVal = (int)trapsY.Value;
            else
                MessageBox.Show("Cannot place traps outside of the room");
        }

        /// <summary>
        /// Changes the Y value of the door to 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TopBut_CheckedChanged(object sender, EventArgs e)
        {
            doorYVal = 0;
        }

        /// <summary>
        /// Changes the door Y value of the door to the max -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotBut_CheckedChanged(object sender, EventArgs e)
        {
            doorYVal = heightOfRoom-1;
        }

        /// <summary>
        /// Changes the X value of the door to the max value -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightBut_CheckedChanged(object sender, EventArgs e)
        {
            doorXVal = widthOfRoom-1;
        }

        /// <summary>
        /// Changes the X value of the door to 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftBut_CheckedChanged(object sender, EventArgs e)
        {
            doorXVal = 0;
        }

        /// <summary>
        /// Controls the X or Y variable for the door
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumControl_ValueChanged(object sender, EventArgs e)
        {
            if (doorXVal == 0 || doorXVal == widthOfRoom - 1)
                doorYVal = (int)numControl.Value;

            if (doorYVal == 0 || doorYVal == heightOfRoom-1)
                doorXVal = (int)numControl.Value;
        }

        #endregion
        #region Array Methods
        private void FillArray()
        {
            room = new string[heightOfRoom, widthOfRoom];
            for (int i = 0; i < heightOfRoom; i++)
            {
                for (int j = 0; j < widthOfRoom; j++)
                {
                    if (i == 0 && j < widthOfRoom)//top
                    {
                        room[i, j] = wall;
                    }
                    else if (i == heightOfRoom - 1 && j < widthOfRoom)//bot
                    {
                        room[i, j] = wall;
                    }
                    else if (i < heightOfRoom && j == widthOfRoom - 1)//right
                    {
                        room[i, j] = wall;
                    }
                    else if (i < heightOfRoom && j == 0)//left
                    {
                        room[i, j] = wall;
                    }
                    else if (i > 0 && i < heightOfRoom && j > 0 && j < widthOfRoom)//interior
                    {
                        room[i, j] = floor;
                    }
                    if (i == doorYVal && j == doorXVal)//places single door
                    {
                        room[i, j] = door;
                    }
                    if (i == playerXVal && j == playerYVal)// places player spawn
                    {
                        room[i, j] = player;
                    }
                    if (i == trapsXVal && j == trapsYVal)//places single trap
                    {
                        room[i, j] = traps;
                    }
                }
            }
        }

        /// <summary>
        /// Fills the interior of the room with traps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrapRoomButton_Click(object sender, EventArgs e)
        {
            room = new string[heightOfRoom, widthOfRoom];
            for (int i = 0; i < heightOfRoom; i++)
            {
                for (int j = 0; j < widthOfRoom; j++)
                {
                    if (i == 0 && j < widthOfRoom)//top
                    {
                        room[i, j] = wall;
                    }
                    else if (i == heightOfRoom - 1 && j < widthOfRoom)//bot
                    {
                        room[i, j] = wall;
                    }
                    else if (i < heightOfRoom && j == widthOfRoom - 1)//right
                    {
                        room[i, j] = wall;
                    }
                    else if (i < heightOfRoom && j == 0)//left
                    {
                        room[i, j] = wall;
                    }
                    else if (i > 0 && i < heightOfRoom && j > 0 && j < widthOfRoom)//interior
                    {
                        room[i, j] = traps;
                    }
                    if (i == doorXVal && j == doorYVal - 1)//places single door
                    {
                        room[i, j] = door;
                    }
                    if (i == playerXVal && j == playerYVal)// places player spawn
                    {
                        room[i, j] = player;
                    }
                    if (i == trapsXVal && j == trapsYVal)//places single trap
                    {
                        room[i, j] = traps;
                    }
                }
            }
            WriteArray(room);
            this.Close();
        }

        /// <summary>
        /// Writes the room array to the binary data file
        /// </summary>
        /// <param name="array"></param>
        private void WriteArray(string[,] array)
        {
            try
            {
                //outStream = File.OpenWrite(level);
                //writer = new BinaryWriter(outStream);
                writer = new StreamWriter(fileName);
                foreach (string v in variableArray)
                {
                    writer.Write($"{v} ");
                }
                writer.Write($"{widthOfRoom} {heightOfRoom} {doorXVal} {doorYVal}");
                writer.WriteLine();
                for (int i = 0; i < heightOfRoom; i++)
                {
                    for (int j = 0; j < widthOfRoom; j++)
                    {
                        writer.Write(room[i, j]);
                    }
                    writer.WriteLine(room[i, 0]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            finally
            {
                writer.Close();
            }
        }
        #endregion


        /// <summary>
        /// Fills and writes the room array to the binary data file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FillArraybut_Click(object sender, EventArgs e)
        {
            FillArray();
            WriteArray(room);
            this.Close();
        }
    }
}
