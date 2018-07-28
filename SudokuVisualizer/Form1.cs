using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuVisualizer
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// List of cells in the gameboard display
        /// </summary>
        private List<TextBox> _cells = new List<TextBox>();

        /// <summary>
        /// The gamebaord to use
        /// </summary>
        private readonly Board _gameBoard;

        public delegate void PlaceNumberCallback(int num, int row, int col, Color color);
        public delegate void LightUpNumberCallback(int row, int col, Color color);


        /// <summary>
        /// Construtor: Sets up the gameboard and initializes the UI
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.Text = "Sudoku Solver Visualized";
            _gameBoard = new Board(9, 9, placeNumber, lightUpNumber);
            createGUI();
            _cells.TrimExcess();
        }

        /// <summary>
        /// Dynamically creates the UI.
        /// </summary>
        private void createGUI()
        {
            this.BackColor = Color.LightSlateGray;

            createCells();
            addButtons();
            addSlider();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }


        /// <summary>
        /// Dynamically generates a 9x9 grid on the display
        /// </summary>
        private void createCells()
        {
            int top = menuBar.Height + 10;
            int left = 5;
            int cellWidth = 50;
            int cellHeight = 50;
            int cellSpacing = 2;
            int extendedCellSpacing = 5;

            TextBox tb;

            for (int i = 1; i <= 9; i++)
            {
                // Builds Columns
                for (int j = 1; j <= 9; j++)
                {
                    tb = new TextBox();
                    tb.Multiline = true;
                    tb.Font = new Font("Arial", 24, FontStyle.Bold);
                    tb.Width = cellWidth;
                    tb.Height = cellHeight;
                    tb.Top = top;
                    tb.Left = left;

                    // Adds extra spacing every three rows down
                    if (j % 3 == 0)
                    {
                        left += cellWidth + extendedCellSpacing;
                    }
                    else
                    {
                        left += cellWidth + cellSpacing;
                    }

                    this.Controls.Add(tb);
                    _cells.Add(tb);

                }

                left = 5;

                // Adds extra spacing every three columns over
                if (i % 3 == 0)
                {
                    top += cellHeight + extendedCellSpacing;
                }
                else
                {
                    top += cellHeight + cellSpacing;
                }
            }

            int formWidth = _cells[80].Location.X + cellWidth + 20;
            int formHeight = _cells[80].Location.Y + cellHeight * 2;

            this.Size = new Size(formWidth, formHeight);
        }

        /// <summary>
        /// Places the buttons on the board
        /// </summary>
        private void addButtons()
        {
            solveGameButton.SetBounds(_cells[3].Location.X + 27, this.Height - 25, 100, 25);
            updateBoardButton.SetBounds(_cells[6].Location.X + 27, this.Height - 25, 100, 25);
            clearBoardButton.SetBounds(_cells[0].Location.X + 27, this.Height - 25, 100, 25);
            this.Height += 50;
        }

        private void addSlider()
        {
            // Speed bar properties
            speedBar.Maximum = 400; // Longest amount of delay between moves
            speedBar.TickStyle = TickStyle.None;
            speedBar.Value = speedBar.Maximum; // Set it to the maximum amount of delay by default
            speedBar.Width = clearBoardButton.Location.X + updateBoardButton.Location.X - waitTime.Width + (updateBoardButton.Width / 2);
            speedBar.Location = new Point(clearBoardButton.Location.X + waitTime.Width, clearBoardButton.Location.Y + 50);

            // Delay time label 
            waitTime.Text = "Wait Time\n" + speedBar.Value + " ms";
            waitTime.Location = new Point(clearBoardButton.Location.X - 15, clearBoardButton.Location.Y + 50);

            this.Height += 45; //  Add some extra space to the board so the slide will be visible
        }

        /// <summary>
        /// Fills the cells with the values given in a 2D array
        /// </summary>
        /// <param name="cellNumbers">A 2D array of strings conating the value for each cell in the grid</param>
        private void fillCells(string[,] cellNumbers)
        {
            int counter = 0;

            for (int i = 0; i < cellNumbers.GetLength(0); i++)
            {
                for (int j = 0; j < cellNumbers.GetLength(1); j++)
                {
                    if (cellNumbers[i, j] == "0")
                        _cells[counter++].Text = ""; // Zeros should not be displayed since they are just away to identify empty boxes
                    else
                        _cells[counter++].Text = cellNumbers[i, j];
                }
            }
        }

        /// <summary>
        /// Sets all display cells back to empty
        /// </summary>
        private void emptyCells()
        {
            for (int i = 0; i < _cells.Capacity; i++)
            {
                _cells[i].Text = "";
            }
        }




        /// <summary>
        /// Loads a properly formatted gameboard from a text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO:
            // Display error if gameboard is not formatted properly

            string filePath; // Path of the gameboard file
            string input; // Data from the selected file

            OpenFileDialog fileBrowser = new OpenFileDialog();
            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                setBoardToDefault();
                filePath = fileBrowser.FileName;
                input = FileHandler.ReadFile(filePath);
                _gameBoard.CreateBoard(input);
                fillCells(_gameBoard.Gameboard);
                this.Text = "Sudoku Solver Visualized - " + filePath;
            }

        }
        /// <summary>
        /// Sets the board to it's default state
        /// </summary>
        private void setBoardToDefault()
        {
            _gameBoard.ClearBoard();
            emptyCells();
            resetColors(Color.Black);
        }

        /// <summary>
        /// Sets all of the cells foreground colors
        /// </summary>
        /// <param name="color">Color to set the foreground of the cells</param>
        private void resetColors(Color color)
        {
            for (int i = 0; i < _cells.Capacity; i++)
            {
                _cells[i].ForeColor = color;
            }
        }

        /// <summary>
        /// Wrapper to run the algorithm that solves the puzzle in a seperate thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solveGameButton_Click(object sender, EventArgs e)
        {
            if (_gameBoard.Gameboard != null && !_gameBoard.IsRunning)
            {
                _gameBoard.KillThread = false;
                _gameBoard.IsRunning = true;
                Thread thread = new Thread(new ThreadStart(solveGame));
                thread.Start();
            }

            if (_gameBoard.KillThread)
                setBoardToDefault();
        }

        /// <summary>
        /// Runs the algorithm that solves the puzzle
        /// </summary>
        private void solveGame()
        {
            if (!_gameBoard.SolveGame())
            {
                if (_gameBoard.KillThread)
                {
                    MessageBox.Show("Puzzle was aborted!", "Puzzle aboarted!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Puzzle could not be solved!", "Invalid Puzzle!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                resetColors(Color.Black);

            _gameBoard.IsRunning = false;

        }

        /// <summary>
        /// Clears the gamebaord and sets it to default settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearBoardButton_Click(object sender, EventArgs e)
        {
            if (!_gameBoard.IsRunning)
            {
                setBoardToDefault();
                _cells.ForEach(x => x.BackColor = Color.White);
                this.Text = "Sudoku Solver Visualized";
            }
        }

        /// <summary>
        /// Saves the numbers entered in each cell to a 2D array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateBoardButton_Click(object sender, EventArgs e)
        {
            if (!_gameBoard.IsRunning)
            {
                string input = "";

                for (int i = 0; i < _cells.Capacity; i++)
                {
                    if (_cells[i].Text.Trim() == "")
                        input += "0 ";
                    else
                        input += _cells[i].Text + " ";
                }

                _gameBoard.CreateBoard(input);
                this.Text = "Sudoku Solver Visualized - Custom";
            }

        }

        /// <summary>
        /// PLaces a number in a cell on the display
        /// </summary>
        /// <param name="num">The number to display</param>
        /// <param name="row">Row of the cell to place the number in</param>
        /// <param name="col">Column of the cell to place the number in</param>
        /// <param name="color">Color of the cell</param>
        public void placeNumber(int num, int row, int col, Color color)
        {
            int oneDIndex = (row * 9) + col;

            if (_cells[oneDIndex].InvokeRequired)
            {
                if (num == 0)
                    _cells[oneDIndex].Invoke((MethodInvoker)(() => _cells[oneDIndex].Text = "" ));
                else
                    _cells[oneDIndex].Invoke((MethodInvoker)(() => _cells[oneDIndex].Text = num.ToString() ));

                _cells[oneDIndex].Invoke((MethodInvoker)(() => _cells[oneDIndex].BackColor = color ));
            }

            int sleepTime = -1;

            if (speedBar.InvokeRequired)
            {
                speedBar.Invoke((MethodInvoker)(() => sleepTime = speedBar.Value ));
            }

            Thread.Sleep(sleepTime);
        }

        /// <summary>
        /// Changes the text color of a certian cell
        /// </summary>
        /// <param name="row">Row of the cell to place the number in</param>
        /// <param name="col">Column of the cell to place the number in</param>
        /// <param name="color">Color of the text</param>
        private int testIndex = -1;
        public void lightUpNumber(int row, int col, Color color)
        {
            int oneDIndex = (row * 9) + col;

            if (_cells[oneDIndex].InvokeRequired)
                _cells[oneDIndex].Invoke((MethodInvoker)(() => _cells[oneDIndex].ForeColor = color ));

            if (testIndex != -1)
                _cells[testIndex].ForeColor = Color.Black;

            testIndex = oneDIndex;

            int sleepTime = -1;

            if (speedBar.InvokeRequired)
                speedBar.Invoke((MethodInvoker)(() => sleepTime = speedBar.Value ));
          
            Thread.Sleep(sleepTime);

        }

        /// <summary>
        /// Sets the gameboard's KillThread property to true indicating we should stop running the thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gameBoard.KillThread = true;
        }

        /// <summary>
        /// Sets the delay time to the scrollbar value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void speedBar_Scroll(object sender, EventArgs e)
        {
            waitTime.Text = "Wait Time\n" + speedBar.Value + " ms";
        }
    }
}
