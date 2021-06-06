﻿using System;
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

        public void PrintValues()
        {
            foreach (var val in value)
            {
                if (val.Type == ValueType.String)
                {
                    var strValue = val.valueData as string;
                    Console.WriteLine($"{val.Name} {val.Type} {strValue}");
                }
            }
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
                    value = Parser.DefineArray(_valueChars);
                    break;
                case ValueType.String:
                    (Type, valueData) = Parser.DefineString(_valueChars);
                    break;
            }
        }
    }
}