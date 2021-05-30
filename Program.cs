using System;
using System.IO;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var path = Console.ReadLine();

            try
            {
                var fileContent = File.ReadAllText(path);
                Console.WriteLine(fileContent);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot read file");
                throw;
            }
        }
    }
}