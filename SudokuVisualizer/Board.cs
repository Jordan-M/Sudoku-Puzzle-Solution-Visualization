using System;
using System.Drawing;
using System.Threading;

namespace SudokuVisualizer
{
    public class Board
    {
        private readonly int _rows;
        private readonly int _columns;

        private readonly Form1.PlaceNumberCallback PlaceNumber;
        private readonly Form1.LightUpNumberCallback LightUpNumber;

        public string[,] Gameboard { get; private set; }
        public bool KillThread { get; set; } = false;
        public bool IsRunning { get; set; } = false;

        public Board(int numRows, int numColumns, Form1.PlaceNumberCallback placeNumber, Form1.LightUpNumberCallback lightUpNumber)
        {
            _rows = numRows;
            _columns = numColumns;
            PlaceNumber = placeNumber;
            LightUpNumber = lightUpNumber;
        }

        /// <summary>
        /// Creates a new soduku game board. Initializes it with the numbers you pass to it.
        /// </summary>
        /// <param name="boardNumbers">A string of numbers used to initialize the game board. All blank
        ///                            board spaces should be represented by 0.
        /// </param>
        public void CreateBoard(string boardNumbers)
        {

            // Replace new line character with empty string so 
            // we don't have to use regex in split
            boardNumbers = boardNumbers.Replace("\r\n", " ");

            // Create an array of all board numbers
            string[] tokens = boardNumbers.Split(' ');

            Gameboard = new string[_rows, _columns];

            // Populate the board with the tokens
            int counter = 0;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    Gameboard[i, j] = tokens[counter++];
                }
            }
        }

        public void ClearBoard()
        {
            Gameboard = null;
        }

        /// <summary>
        /// Recursively solves the sudoku board.
        /// </summary>
        /// <returns>True if the recursion succeeded, false otherwise.</returns>
        public bool SolveGame()
        {
            // Initialize our row and columns so we can manipulate them through 
            // refrence later.
            int row = -1;
            int column = -1;

            // BASECASE: There are spaces still left to place a number
            if (!SetToEmptySpace(ref row, ref column))
            {
                return true;
            }

            // Trys to place numbers 1-9 into the empty cell set by setToEmptySpace.
            for (int curNum = 1; curNum <= 9; curNum++)
            {
                if (KillThread)
                {
                    break;
                }

                PlaceNumber(curNum, row, column, Color.Wheat);

                // Check if it is safe to place a number in row, cloumn
                if (IsSafeSpace(curNum, row, column))
                {
                    // Set the cell
                    SetCell(curNum, row, column);
                   PlaceNumber(curNum, row, column, Color.Green);

                    // Go one stack frame deeper until setEmptySpace returns false
                    if (SolveGame()) return true;

                    // Undo a wrong decision
                    UnassignCell(row, column);
                }

                PlaceNumber(curNum, row, column, Color.Red);
            }

            PlaceNumber(0, row, column, Color.White);
            return false;
        }

        /// <summary>
        /// Finds the next empty space (0) on the Soduku board
        /// and sets two refrence integers to that spot
        /// </summary>
        /// <param name="row">The integer to set to the empty row spot</param>
        /// <param name="column">The integer to set to the empty column spot</param>
        /// <returns>True if there is an empty space on the board, false otherwise.</returns>
        private bool SetToEmptySpace(ref int row, ref int column)
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (Gameboard[i, j].Equals("0"))
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
        private void SetCell(int num, int row, int column)
        {
            Gameboard[row, column] = num.ToString();
        }

        /// <summary>
        /// Changes a cell to empty (0).
        /// </summary>
        /// <param name="row">Row of the cell to empty.</param>
        /// <param name="column">Column of the cell to empty.</param>
        private void UnassignCell(int row, int column)
        {
            Gameboard[row, column] = "0";
        }

        /// <summary>
        /// Checks to see if there is any number collisions in the row, column and box.
        /// </summary>
        /// <param name="numToCheck">Number to check for collisions.</param>
        /// <param name="row">Row of numbers to check.</param>
        /// <param name="column">Column of numbers to check.</param>
        /// <returns></returns>
        private bool IsSafeSpace(int numToCheck, int row, int column)
        {
            string num = numToCheck.ToString();

            // row - row % 3 will force us to the top left cell of a 3 by 3 block
            if (ColIsSafe(num, column) && RowIsSafe(num, row) && BoxIsSafe(num, row - row % 3, column - column % 3))
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
        private bool ColIsSafe(string numToCheck, int colNumber)
        {
            for (int i = 0; i < _columns; i++)
            {
                if (Gameboard[i, colNumber].Equals(numToCheck))
                {
                    LightUpNumber(i, colNumber, Color.Red);
                    return false;
                }

                //Light up number tested on board
                if (!Gameboard[i, colNumber].Equals("0"))
                    LightUpNumber(i, colNumber, Color.LimeGreen);
            }

            return true;
        }


        /// <summary>
        /// Checks for number collisions in a row.
        /// </summary>
        /// <param name="numToCheck">Number to check for collisions.</param>
        /// <param name="colNumber">Row of numbers to check.</param>
        /// <returns>True if there are no collisions, false otherwise.</returns>
        private bool RowIsSafe(string numToCheck, int rowNumber)
        {
            for (int i = 0; i < _rows; i++)
            {
                if (Gameboard[rowNumber, i].Equals(numToCheck))
                {
                    LightUpNumber(rowNumber, i, Color.Red);
                    return false;
                }
                if (!Gameboard[rowNumber, i].Equals("0"))
                    LightUpNumber(rowNumber, i, Color.LimeGreen);
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
        private bool BoxIsSafe(string numToCheck, int xPos, int yPos)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {                 
                    if (Gameboard[xPos + i, yPos + j].Equals(numToCheck))
                    {
                        LightUpNumber(xPos + i, yPos + j, Color.Red);
                        return false;
                    }

                    if (!Gameboard[xPos + i, yPos + j].Equals("0"))
                        LightUpNumber(xPos + i, yPos + j, Color.LimeGreen);
                }
            }

            return true;
        }
    }
}
