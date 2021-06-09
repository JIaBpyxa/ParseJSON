using System;
using System.IO;

namespace Parser
{
    public class InputController
    {
        public static string TryGetInputFileContent()
        {
            try
            {
                return GetInputFileContent();
            }
            catch
            {
                Console.WriteLine("Cannot read file");
                return "";
            }
        }

        private static string GetInputFileContent()
        {
            Console.WriteLine("Input file's path");
            var path = Console.ReadLine();
            return File.ReadAllText(path);
        }
    }
}