using System.IO;
using System.Text;

namespace SudokuVisualizer
{
    class FileHandler
    { 
        public static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath, Encoding.UTF8);
        }
    }
}
