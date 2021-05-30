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
                var path = Console.ReadLine();
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot read file");
                return "";
            }
        }
    }
}