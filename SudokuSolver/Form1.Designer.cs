namespace SudokuSolver
{
    partial class Form1
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
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadBoardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solveGameButton = new System.Windows.Forms.Button();
            this.updateBoardButton = new System.Windows.Forms.Button();
            this.clearBoardButton = new System.Windows.Forms.Button();
            this.speedBar = new System.Windows.Forms.TrackBar();
            this.waitTime = new System.Windows.Forms.Label();
            this.menuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).BeginInit();
            this.SuspendLayout();
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(284, 24);
            this.menuBar.TabIndex = 0;
            this.menuBar.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadBoardToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadBoardToolStripMenuItem
            // 
            this.loadBoardToolStripMenuItem.Name = "loadBoardToolStripMenuItem";
            this.loadBoardToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadBoardToolStripMenuItem.Text = "Load Board";
            this.loadBoardToolStripMenuItem.Click += new System.EventHandler(this.loadBoardToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // solveGameButton
            // 
            this.solveGameButton.Location = new System.Drawing.Point(12, 227);
            this.solveGameButton.Name = "solveGameButton";
            this.solveGameButton.Size = new System.Drawing.Size(75, 23);
            this.solveGameButton.TabIndex = 1;
            this.solveGameButton.Text = "Solve Game";
            this.solveGameButton.UseVisualStyleBackColor = true;
            this.solveGameButton.Click += new System.EventHandler(this.solveGameButton_Click);
            // 
            // updateBoardButton
            // 
            this.updateBoardButton.Location = new System.Drawing.Point(93, 227);
            this.updateBoardButton.Name = "updateBoardButton";
            this.updateBoardButton.Size = new System.Drawing.Size(75, 23);
            this.updateBoardButton.TabIndex = 2;
            this.updateBoardButton.Text = "Update";
            this.updateBoardButton.UseVisualStyleBackColor = true;
            this.updateBoardButton.Click += new System.EventHandler(this.updateBoardButton_Click);
            // 
            // clearBoardButton
            // 
            this.clearBoardButton.Location = new System.Drawing.Point(174, 227);
            this.clearBoardButton.Name = "clearBoardButton";
            this.clearBoardButton.Size = new System.Drawing.Size(75, 23);
            this.clearBoardButton.TabIndex = 3;
            this.clearBoardButton.Text = "Clear Board";
            this.clearBoardButton.UseVisualStyleBackColor = true;
            this.clearBoardButton.Click += new System.EventHandler(this.clearBoardButton_Click);
            // 
            // speedBar
            // 
            this.speedBar.Location = new System.Drawing.Point(93, 256);
            this.speedBar.Name = "speedBar";
            this.speedBar.Size = new System.Drawing.Size(104, 45);
            this.speedBar.TabIndex = 4;
            this.speedBar.Scroll += new System.EventHandler(this.speedBar_Scroll);
            // 
            // waitTime
            // 
            this.waitTime.AutoSize = true;
            this.waitTime.Location = new System.Drawing.Point(23, 266);
            this.waitTime.Name = "waitTime";
            this.waitTime.Size = new System.Drawing.Size(35, 13);
            this.waitTime.TabIndex = 5;
            this.waitTime.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 310);
            this.Controls.Add(this.waitTime);
            this.Controls.Add(this.speedBar);
            this.Controls.Add(this.clearBoardButton);
            this.Controls.Add(this.updateBoardButton);
            this.Controls.Add(this.solveGameButton);
            this.Controls.Add(this.menuBar);
            this.MainMenuStrip = this.menuBar;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadBoardToolStripMenuItem;
        private System.Windows.Forms.Button solveGameButton;
        private System.Windows.Forms.Button updateBoardButton;
        private System.Windows.Forms.Button clearBoardButton;
        private System.Windows.Forms.TrackBar speedBar;
        private System.Windows.Forms.Label waitTime;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
    }
}