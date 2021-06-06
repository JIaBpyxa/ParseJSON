using System;
using System.Collections.Generic;

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
            return CreateInstance("mainObject", chars);
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

        private void DefineValue()
        {
            switch (Type)
            {
                case ValueType.Object:
                    value = Parser.DefineObject(_valueChars);
                    break;
                case ValueType.Array:
                    DefineArray();
                    break;
                case ValueType.String:
                    (Type, valueData) = Parser.DefineString(_valueChars);
                    break;
            }

            foreach (var val in value)
            {
                var strValue = val.valueData as string;
                Console.WriteLine($"{val.Name} {val.Type} {strValue}");
            }


            void DefineArray()
            {
                
            }
        }
    }
}