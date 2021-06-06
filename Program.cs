using System;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var content = IOController.TryGetInputFileContent();
            var chars = content.ToCharArray();

            var objectBeginIndex = Parser.GetObjectBeginIndex(chars);
            var objectEndIndex = Parser.GetObjectEndIndex(chars);

            Console.WriteLine($"Begin {objectBeginIndex} End {objectEndIndex}");
            var mainNode = DataNode.CreateInstance(chars);
        }
    }
}