using System;
using System.IO;

namespace Parser
{
    public class IOController
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
            var path = Console.ReadLine();
            return File.ReadAllText(path);
        }
    }
}