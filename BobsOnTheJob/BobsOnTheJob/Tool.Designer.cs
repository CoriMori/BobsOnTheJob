namespace BobsOnTheJob
{
    partial class Tool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.varLabel = new System.Windows.Forms.Label();
            this.varBox = new System.Windows.Forms.TextBox();
            this.doorLoc = new System.Windows.Forms.Label();
            this.roomSize = new System.Windows.Forms.Label();
            this.roomWidth = new System.Windows.Forms.Label();
            this.roomHeight = new System.Windows.Forms.Label();
            this.playerSpawn = new System.Windows.Forms.Label();
            this.playerX = new System.Windows.Forms.NumericUpDown();
            this.roomHeightUpDown = new System.Windows.Forms.NumericUpDown();
            this.roomWidthUpDown = new System.Windows.Forms.NumericUpDown();
            this.varNote = new System.Windows.Forms.Label();
            this.fillArraybut = new System.Windows.Forms.Button();
            this.playerY = new System.Windows.Forms.NumericUpDown();
            this.playerXLabel = new System.Windows.Forms.Label();
            this.playerYLabel = new System.Windows.Forms.Label();
            this.fileNameBox = new System.Windows.Forms.TextBox();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.trapLabel = new System.Windows.Forms.Label();
            this.trapsX = new System.Windows.Forms.NumericUpDown();
            this.trapsY = new System.Windows.Forms.NumericUpDown();
            this.trapXLabel = new System.Windows.Forms.Label();
            this.trapsYLabel = new System.Windows.Forms.Label();
            this.trapRoomButton = new System.Windows.Forms.Button();
            this.topBut = new System.Windows.Forms.RadioButton();
            this.botBut = new System.Windows.Forms.RadioButton();
            this.rightBut = new System.Windows.Forms.RadioButton();
            this.leftBut = new System.Windows.Forms.RadioButton();
            this.numControl = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.playerX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomHeightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomWidthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trapsX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trapsY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numControl)).BeginInit();
            this.SuspendLayout();
            // 
            // varLabel
            // 
            this.varLabel.AutoSize = true;
            this.varLabel.Location = new System.Drawing.Point(76, 104);
            this.varLabel.Name = "varLabel";
            this.varLabel.Size = new System.Drawing.Size(102, 25);
            this.varLabel.TabIndex = 0;
            this.varLabel.Text = "Variables";
            // 
            // varBox
            // 
            this.varBox.Location = new System.Drawing.Point(35, 143);
            this.varBox.Name = "varBox";
            this.varBox.Size = new System.Drawing.Size(187, 31);
            this.varBox.TabIndex = 1;
            this.varBox.TextChanged += new System.EventHandler(this.VarBox_TextChanged);
            // 
            // doorLoc
            // 
            this.doorLoc.AutoSize = true;
            this.doorLoc.Location = new System.Drawing.Point(61, 193);
            this.doorLoc.Name = "doorLoc";
            this.doorLoc.Size = new System.Drawing.Size(146, 25);
            this.doorLoc.TabIndex = 2;
            this.doorLoc.Text = "Door Location";
            // 
            // roomSize
            // 
            this.roomSize.AutoSize = true;
            this.roomSize.Location = new System.Drawing.Point(316, 89);
            this.roomSize.Name = "roomSize";
            this.roomSize.Size = new System.Drawing.Size(116, 25);
            this.roomSize.TabIndex = 3;
            this.roomSize.Text = "Room Size";
            // 
            // roomWidth
            // 
            this.roomWidth.AutoSize = true;
            this.roomWidth.Location = new System.Drawing.Point(283, 115);
            this.roomWidth.Name = "roomWidth";
            this.roomWidth.Size = new System.Drawing.Size(67, 25);
            this.roomWidth.TabIndex = 4;
            this.roomWidth.Text = "Width";
            // 
            // roomHeight
            // 
            this.roomHeight.AutoSize = true;
            this.roomHeight.Location = new System.Drawing.Point(397, 115);
            this.roomHeight.Name = "roomHeight";
            this.roomHeight.Size = new System.Drawing.Size(74, 25);
            this.roomHeight.TabIndex = 5;
            this.roomHeight.Text = "Height";
            // 
            // playerSpawn
            // 
            this.playerSpawn.AutoSize = true;
            this.playerSpawn.Location = new System.Drawing.Point(302, 226);
            this.playerSpawn.Name = "playerSpawn";
            this.playerSpawn.Size = new System.Drawing.Size(144, 25);
            this.playerSpawn.TabIndex = 9;
            this.playerSpawn.Text = "Player Spawn";
            // 
            // playerX
            // 
            this.playerX.Location = new System.Drawing.Point(270, 297);
            this.playerX.Name = "playerX";
            this.playerX.Size = new System.Drawing.Size(80, 31);
            this.playerX.TabIndex = 11;
            this.playerX.ValueChanged += new System.EventHandler(this.PlayerX_ValueChanged);
            // 
            // roomHeightUpDown
            // 
            this.roomHeightUpDown.Location = new System.Drawing.Point(393, 143);
            this.roomHeightUpDown.Name = "roomHeightUpDown";
            this.roomHeightUpDown.Size = new System.Drawing.Size(78, 31);
            this.roomHeightUpDown.TabIndex = 12;
            this.roomHeightUpDown.ValueChanged += new System.EventHandler(this.RoomHeightUpDown_ValueChanged);
            // 
            // roomWidthUpDown
            // 
            this.roomWidthUpDown.Location = new System.Drawing.Point(270, 143);
            this.roomWidthUpDown.Name = "roomWidthUpDown";
            this.roomWidthUpDown.Size = new System.Drawing.Size(83, 31);
            this.roomWidthUpDown.TabIndex = 13;
            this.roomWidthUpDown.ValueChanged += new System.EventHandler(this.RoomWidthUpDown_ValueChanged);
            // 
            // varNote
            // 
            this.varNote.AutoSize = true;
            this.varNote.Location = new System.Drawing.Point(3, 9);
            this.varNote.Name = "varNote";
            this.varNote.Size = new System.Drawing.Size(302, 75);
            this.varNote.TabIndex = 14;
            this.varNote.Text = "(Seperate with commas)\r\n(5 variables)\r\n(Wall, floor, door, player,traps)\r\n";
            // 
            // fillArraybut
            // 
            this.fillArraybut.Location = new System.Drawing.Point(12, 488);
            this.fillArraybut.Name = "fillArraybut";
            this.fillArraybut.Size = new System.Drawing.Size(219, 59);
            this.fillArraybut.TabIndex = 15;
            this.fillArraybut.Text = "Submit";
            this.fillArraybut.UseVisualStyleBackColor = true;
            this.fillArraybut.Click += new System.EventHandler(this.FillArraybut_Click);
            // 
            // playerY
            // 
            this.playerY.Location = new System.Drawing.Point(380, 297);
            this.playerY.Name = "playerY";
            this.playerY.Size = new System.Drawing.Size(81, 31);
            this.playerY.TabIndex = 19;
            this.playerY.ValueChanged += new System.EventHandler(this.PlayerY_ValueChanged);
            // 
            // playerXLabel
            // 
            this.playerXLabel.AutoSize = true;
            this.playerXLabel.Location = new System.Drawing.Point(302, 269);
            this.playerXLabel.Name = "playerXLabel";
            this.playerXLabel.Size = new System.Drawing.Size(26, 25);
            this.playerXLabel.TabIndex = 20;
            this.playerXLabel.Text = "X";
            // 
            // playerYLabel
            // 
            this.playerYLabel.AutoSize = true;
            this.playerYLabel.Location = new System.Drawing.Point(405, 269);
            this.playerYLabel.Name = "playerYLabel";
            this.playerYLabel.Size = new System.Drawing.Size(27, 25);
            this.playerYLabel.TabIndex = 21;
            this.playerYLabel.Text = "Y";
            // 
            // fileNameBox
            // 
            this.fileNameBox.Location = new System.Drawing.Point(35, 408);
            this.fileNameBox.Name = "fileNameBox";
            this.fileNameBox.Size = new System.Drawing.Size(187, 31);
            this.fileNameBox.TabIndex = 22;
            this.fileNameBox.TextChanged += new System.EventHandler(this.FileNameBox_TextChanged);
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(61, 380);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(109, 25);
            this.fileNameLabel.TabIndex = 23;
            this.fileNameLabel.Text = "File Name";
            // 
            // trapLabel
            // 
            this.trapLabel.AutoSize = true;
            this.trapLabel.Location = new System.Drawing.Point(335, 349);
            this.trapLabel.Name = "trapLabel";
            this.trapLabel.Size = new System.Drawing.Size(67, 25);
            this.trapLabel.TabIndex = 24;
            this.trapLabel.Text = "Traps";
            // 
            // trapsX
            // 
            this.trapsX.Location = new System.Drawing.Point(267, 408);
            this.trapsX.Name = "trapsX";
            this.trapsX.Size = new System.Drawing.Size(83, 31);
            this.trapsX.TabIndex = 25;
            this.trapsX.ValueChanged += new System.EventHandler(this.TrapsX_ValueChanged);
            // 
            // trapsY
            // 
            this.trapsY.Location = new System.Drawing.Point(380, 408);
            this.trapsY.Name = "trapsY";
            this.trapsY.Size = new System.Drawing.Size(81, 31);
            this.trapsY.TabIndex = 26;
            this.trapsY.ValueChanged += new System.EventHandler(this.TrapsY_ValueChanged);
            // 
            // trapXLabel
            // 
            this.trapXLabel.AutoSize = true;
            this.trapXLabel.Location = new System.Drawing.Point(288, 380);
            this.trapXLabel.Name = "trapXLabel";
            this.trapXLabel.Size = new System.Drawing.Size(26, 25);
            this.trapXLabel.TabIndex = 27;
            this.trapXLabel.Text = "X";
            // 
            // trapsYLabel
            // 
            this.trapsYLabel.AutoSize = true;
            this.trapsYLabel.Location = new System.Drawing.Point(405, 380);
            this.trapsYLabel.Name = "trapsYLabel";
            this.trapsYLabel.Size = new System.Drawing.Size(27, 25);
            this.trapsYLabel.TabIndex = 28;
            this.trapsYLabel.Text = "Y";
            // 
            // trapRoomButton
            // 
            this.trapRoomButton.Location = new System.Drawing.Point(267, 476);
            this.trapRoomButton.Name = "trapRoomButton";
            this.trapRoomButton.Size = new System.Drawing.Size(83, 71);
            this.trapRoomButton.TabIndex = 29;
            this.trapRoomButton.Text = "Trap Room";
            this.trapRoomButton.UseVisualStyleBackColor = true;
            this.trapRoomButton.Click += new System.EventHandler(this.TrapRoomButton_Click);
            // 
            // topBut
            // 
            this.topBut.AutoSize = true;
            this.topBut.Location = new System.Drawing.Point(5, 226);
            this.topBut.Name = "topBut";
            this.topBut.Size = new System.Drawing.Size(80, 29);
            this.topBut.TabIndex = 30;
            this.topBut.TabStop = true;
            this.topBut.Text = "Top";
            this.topBut.UseVisualStyleBackColor = true;
            this.topBut.CheckedChanged += new System.EventHandler(this.TopBut_CheckedChanged);
            // 
            // botBut
            // 
            this.botBut.AutoSize = true;
            this.botBut.Location = new System.Drawing.Point(5, 269);
            this.botBut.Name = "botBut";
            this.botBut.Size = new System.Drawing.Size(110, 29);
            this.botBut.TabIndex = 31;
            this.botBut.TabStop = true;
            this.botBut.Text = "Bottom";
            this.botBut.UseVisualStyleBackColor = true;
            this.botBut.CheckedChanged += new System.EventHandler(this.BotBut_CheckedChanged);
            // 
            // rightBut
            // 
            this.rightBut.AutoSize = true;
            this.rightBut.Location = new System.Drawing.Point(114, 221);
            this.rightBut.Name = "rightBut";
            this.rightBut.Size = new System.Drawing.Size(93, 29);
            this.rightBut.TabIndex = 32;
            this.rightBut.TabStop = true;
            this.rightBut.Text = "Right";
            this.rightBut.UseVisualStyleBackColor = true;
            this.rightBut.CheckedChanged += new System.EventHandler(this.RightBut_CheckedChanged);
            // 
            // leftBut
            // 
            this.leftBut.AutoSize = true;
            this.leftBut.Location = new System.Drawing.Point(114, 269);
            this.leftBut.Name = "leftBut";
            this.leftBut.Size = new System.Drawing.Size(79, 29);
            this.leftBut.TabIndex = 33;
            this.leftBut.TabStop = true;
            this.leftBut.Text = "Left";
            this.leftBut.UseVisualStyleBackColor = true;
            this.leftBut.CheckedChanged += new System.EventHandler(this.LeftBut_CheckedChanged);
            // 
            // numControl
            // 
            this.numControl.Location = new System.Drawing.Point(58, 322);
            this.numControl.Name = "numControl";
            this.numControl.Size = new System.Drawing.Size(120, 31);
            this.numControl.TabIndex = 34;
            this.numControl.ValueChanged += new System.EventHandler(this.NumControl_ValueChanged);
            // 
            // Tool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 559);
            this.Controls.Add(this.numControl);
            this.Controls.Add(this.leftBut);
            this.Controls.Add(this.rightBut);
            this.Controls.Add(this.botBut);
            this.Controls.Add(this.topBut);
            this.Controls.Add(this.trapRoomButton);
            this.Controls.Add(this.trapsYLabel);
            this.Controls.Add(this.trapXLabel);
            this.Controls.Add(this.trapsY);
            this.Controls.Add(this.trapsX);
            this.Controls.Add(this.trapLabel);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.fileNameBox);
            this.Controls.Add(this.playerYLabel);
            this.Controls.Add(this.playerXLabel);
            this.Controls.Add(this.playerY);
            this.Controls.Add(this.fillArraybut);
            this.Controls.Add(this.varNote);
            this.Controls.Add(this.roomWidthUpDown);
            this.Controls.Add(this.roomHeightUpDown);
            this.Controls.Add(this.playerX);
            this.Controls.Add(this.playerSpawn);
            this.Controls.Add(this.roomHeight);
            this.Controls.Add(this.roomWidth);
            this.Controls.Add(this.roomSize);
            this.Controls.Add(this.doorLoc);
            this.Controls.Add(this.varBox);
            this.Controls.Add(this.varLabel);
            this.Name = "Tool";
            this.Text = "Tool";
            ((System.ComponentModel.ISupportInitialize)(this.playerX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomHeightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roomWidthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trapsX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trapsY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label varLabel;
        private System.Windows.Forms.TextBox varBox;
        private System.Windows.Forms.Label doorLoc;
        private System.Windows.Forms.Label roomSize;
        private System.Windows.Forms.Label roomWidth;
        private System.Windows.Forms.Label roomHeight;
        private System.Windows.Forms.Label playerSpawn;
        private System.Windows.Forms.NumericUpDown playerX;
        private System.Windows.Forms.NumericUpDown roomHeightUpDown;
        private System.Windows.Forms.NumericUpDown roomWidthUpDown;
        private System.Windows.Forms.Label varNote;
        private System.Windows.Forms.Button fillArraybut;
        private System.Windows.Forms.NumericUpDown playerY;
        private System.Windows.Forms.Label playerXLabel;
        private System.Windows.Forms.Label playerYLabel;
        private System.Windows.Forms.TextBox fileNameBox;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Label trapLabel;
        private System.Windows.Forms.NumericUpDown trapsX;
        private System.Windows.Forms.NumericUpDown trapsY;
        private System.Windows.Forms.Label trapXLabel;
        private System.Windows.Forms.Label trapsYLabel;
        private System.Windows.Forms.Button trapRoomButton;
        private System.Windows.Forms.RadioButton topBut;
        private System.Windows.Forms.RadioButton botBut;
        private System.Windows.Forms.RadioButton rightBut;
        private System.Windows.Forms.RadioButton leftBut;
        private System.Windows.Forms.NumericUpDown numControl;
    }
}