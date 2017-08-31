using System.Drawing;
using System.Threading;

namespace SudokuSolver
{
    public class Board
    {
        private static int ROWS;
        public int Rows
        {
            get { return ROWS; }
        }

        private static int COLUMNS;
        public int Columns
        {
            get { return COLUMNS; }
        }

        private static string[,] GAMEBOARD = null;
        public string[,] GameBoard
        {
            get { return GAMEBOARD;  }
        }


        public Board(int numRows, int numColumns)
        {
            ROWS = numRows;
            COLUMNS = numColumns;
        }

        private static Form1 GUI;
        public Form1 Gui
        {
            set { GUI = value; }
        }

        private static bool killThread = false;
        public bool KillThread
        {
            get { return killThread; }
            set { killThread = value; }
        }

        private static bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }


        /// <summary>
        /// Creates a new soduku game board. Initializes it with the numbers you pass to it.
        /// </summary>
        /// <param name="boardNumbers">A string of numbers used to initialize the game board. All blank
        ///                            board spaces should be represented by 0.
        /// </param>
        public void createBoard(string boardNumbers)
        {

            // Replace new line character with empty string so 
            // we don't have to use regex in split
            boardNumbers = boardNumbers.Replace("\r\n", " ");

            // Create an array of all board numbers
            string[] tokens = boardNumbers.Split(' ');

            GAMEBOARD = new string[ROWS, COLUMNS];

            // Populate the board with the tokens
            int counter = 0;
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    GAMEBOARD[i, j] = tokens[counter++];
                }
            }
        }

        public void clearBoard()
        {
            GAMEBOARD = null;
        }

        /// <summary>
        /// Recursively solves the sudoku board.
        /// </summary>
        /// <returns>True if the recursion succeeded, false otherwise.</returns>
        public bool solveGame()
        {
            // Initialize our row and columns so we can manipulate them through 
            // refrence later.
            int row = -1;
            int column = -1;

            // BASECASE: There are spaces still left to place a number
            if (!setToEmptySpace(ref row, ref column))
            {
                return true;
            }

            // Trys to place numbers 1-9 into the empty cell set by setToEmptySpace.
            for (int curNum = 1; curNum <= 9; curNum++)
            {
                if (killThread)
                {
                    break;
                }

                GUI.placeNumber(curNum, row, column, Color.Wheat);

                // Check if it is safe to place a number in row, cloumn
                if (isSafeSpace(curNum, row, column))
                {
                    // Set the cell
                    setCell(curNum, row, column);
                    GUI.placeNumber(curNum, row, column, Color.Green);

                    // Go one stack frame deeper until setEmptySpace returns false
                    if (solveGame()) return true;

                    // Undo a wrong decision
                    unassignCell(row, column);
                }

                GUI.placeNumber(curNum, row, column, Color.Red);
            }

            GUI.placeNumber(0, row, column, Color.White);
            return false;
        }

        /// <summary>
        /// Finds the next empty space (0) on the Soduku board
        /// and sets two refrence integers to that spot
        /// </summary>
        /// <param name="row">The integer to set to the empty row spot</param>
        /// <param name="column">The integer to set to the empty column spot</param>
        /// <returns>True if there is an empty space on the board, false otherwise.</returns>
        private static bool setToEmptySpace(ref int row, ref int column)
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    if (GAMEBOARD[i, j].Equals("0"))
                    {
                        row = i;
                        column = j;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the cell at row, column to the specified number.
        /// </summary>
        /// <param name="num">The number to place into the cell.</param>
        /// <param name="row">The row of the cell.</param>
        /// <param name="column">The column of the cell.</param>
        private static void setCell(int num, int row, int column)
        {
            GAMEBOARD[row, column] = num.ToString();
        }

        /// <summary>
        /// Changes a cell to empty (0).
        /// </summary>
        /// <param name="row">Row of the cell to empty.</param>
        /// <param name="column">Column of the cell to empty.</param>
        private static void unassignCell(int row, int column)
        {
            GAMEBOARD[row, column] = 0.ToString();
        }

        /// <summary>
        /// Checks to see if there is any number collisions in the row, column and box.
        /// </summary>
        /// <param name="numToCheck">Number to check for collisions.</param>
        /// <param name="row">Row of numbers to check.</param>
        /// <param name="column">Column of numbers to check.</param>
        /// <returns></returns>
        private static bool isSafeSpace(int numToCheck, int row, int column)
        {
            string num = numToCheck.ToString();

            // row - row % 3 will force us to the top left cell of a 3 by 3 block
            if (colIsSafe(num, column) && rowIsSafe(num, row) && boxIsSafe(num, row - row % 3, column - column % 3))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks for number collisions in a column.
        /// </summary>
        /// <param name="numToCheck">Number to check for collisions.</param>
        /// <param name="colNumber">Column of numbers to check.</param>
        /// <returns>True if there are no collisions, false otherwise.</returns>
        private static bool colIsSafe(string numToCheck, int colNumber)
        {
            for (int i = 0; i < COLUMNS; i++)
            {
                if (GAMEBOARD[i, colNumber].Equals(numToCheck))
                {
                    GUI.lightUpNumber(i, colNumber, Color.Red);
                    return false;
                }

                //Light up number tested on board
                if (!GAMEBOARD[i, colNumber].Equals("0"))
                    GUI.lightUpNumber(i, colNumber, Color.LimeGreen);
            }

            return true;
        }


        /// <summary>
        /// Checks for number collisions in a row.
        /// </summary>
        /// <param name="numToCheck">Number to check for collisions.</param>
        /// <param name="colNumber">Row of numbers to check.</param>
        /// <returns>True if there are no collisions, false otherwise.</returns>
        private static bool rowIsSafe(string numToCheck, int rowNumber)
        {
            for (int i = 0; i < ROWS; i++)
            {
                if (GAMEBOARD[rowNumber, i].Equals(numToCheck))
                {
                    GUI.lightUpNumber(rowNumber, i, Color.Red);
                    return false;
                }
                if (!GAMEBOARD[rowNumber, i].Equals("0"))
                    GUI.lightUpNumber(rowNumber, i, Color.LimeGreen);
            }

            return true;
        }

        /// <summary>
        /// Checks for number collisions in a box.
        /// </summary>
        /// <param name="numToCheck">Number to check for collisions.</param>
        /// <param name="xPos">X posistion of the cell.</param>
        /// <param name="yPos">Y posistion of the cell.</param>
        /// <returns>True if there are no collisions in the box, false otherwise.</returns>
        private static bool boxIsSafe(string numToCheck, int xPos, int yPos)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {                 
                    if (GAMEBOARD[xPos + i, yPos + j].Equals(numToCheck))
                    {
                        GUI.lightUpNumber(xPos + i, yPos + j, Color.Red);
                        return false;
                    }

                    if (!GAMEBOARD[xPos + i, yPos + j].Equals("0"))
                        GUI.lightUpNumber(xPos + i, yPos + j, Color.LimeGreen);
                }
            }

            return true;
        }
    }
}
