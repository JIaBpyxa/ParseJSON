using System;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var content = IOController.TryGetInputFileContent();
            Console.WriteLine(content);
        }
    }
}