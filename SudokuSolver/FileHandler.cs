using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class FileHandler
    { 
        public static string readFile(string filePath)
        {
            return File.ReadAllText(filePath, Encoding.UTF8);
        }
    }
}
