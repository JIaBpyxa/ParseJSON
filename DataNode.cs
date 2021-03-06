using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public class DataNode
    {
        public string Name { get; }
        public ValueType Type { get; private set; }
        public List<DataNode> value;
        public object valueData;
        private readonly char[] _valueChars;

        public static DataNode CreateInstance(char[] chars)
        {
            return CreateInstance("mainClass", chars);
        }

        public static DataNode CreateInstance(string name, char[] chars)
        {
            return new DataNode(name, chars);
        }

        private DataNode(string name, char[] chars)
        {
            value = new List<DataNode>();
            Name = name.ToLower();
            _valueChars = chars;
            Type = Parser.DefineValueType(chars);
            DefineValue();
        }

        public void PrintValues(int floor = 0)
        {
            foreach (var node in value)
            {
                var tabs = string.Concat(Enumerable.Repeat("\t", floor));
                Console.WriteLine($"{tabs}{node.Name} {node.Type}");
                if (node.Type is ValueType.String or ValueType.Number or ValueType.True or ValueType.False)
                {
                    var strValue = node.valueData as string;
                    Console.WriteLine($"{tabs}{strValue}");
                }
                node.PrintValues(floor + 1);
            }
        }

        private void DefineValue()
        {
            switch (Type)
            {
                case ValueType.Object:
                    value = Parser.DefineObject(_valueChars);
                    break;
                case ValueType.Array:
                    value = Parser.DefineArray(Name, _valueChars);
                    break;
                case ValueType.String:
                    (Type, valueData) = Parser.DefineString(_valueChars);
                    break;
            }
        }
    }
}