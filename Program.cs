namespace Parser
{
    class Program
    {
        private static void Main(string[] args)
        {
            var content = InputController.TryGetInputFileContent();
            var chars = content.ToCharArray();
            var mainNode = DataNode.CreateInstance(chars);
            mainNode.PrintValues();
            StructCreator.CreateStruct(mainNode);
        }
    }
}