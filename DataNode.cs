using System.Collections.Generic;
using System.Linq;
using static System.Single;

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
                    DefineObject();
                    break;
                case ValueType.Array:
                    DefineArray();
                    break;
                case ValueType.String:
                    (Type, valueData) = Parser.DefineString(_valueChars);
                    break;
            }


            void DefineObject()
            {
                
            }

            void DefineArray()
            {
                
            }
        }
    }

    public enum ValueType
    {
        Object,
        Array,
        String,
        Number,
        True,
        False,
        Null
    }
}