using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Parser.StructuralChars;

namespace Parser
{
    public static class Parser
    {
        public static int GetObjectBeginIndex(char[] chars)
        {
            for (var index = 0; index < chars.Length; index++)
            {
                if (!chars[index].Equals(BeginObject)) continue;
                return index;
            }

            throw new Exception("Can't find object's begin");
        }

        public static int GetObjectEndIndex(char[] chars)
        {
            for (var index = chars.Length - 1; index >= 0; index--)
            {
                if (!chars[index].Equals(EndObject)) continue;
                return index;
            }

            throw new Exception("Can't find object's end");
        }

        public static ValueType DefineValueType(IEnumerable<char> valueChars)
        {
            var firstStructuralChar = valueChars.First(c => c != Space);
            return firstStructuralChar switch
            {
                BeginObject => ValueType.Object,
                BeginArray => ValueType.Array,
                QuotationMark => ValueType.String
            };
        }

        public static List<DataNode> DefineObject(char[] valueChars)
        {
            var objectOpened = -1;
            var arraysOpened = 0;
            var isNameStarted = false;
            var isValueStarted = false;

            var nameStartIndex = -1;
            var nameEndIndex = -1;
            var valueStartIndex = -1;
            var valueEndIndex = -1;

            var namesFound = 0;
            var valuesFound = 0;

            var nodes = new List<DataNode>();

            for (var index = 0; index < valueChars.Length; index++)
            {
                switch (valueChars[index])
                {
                    case BeginObject:
                        objectOpened++;
                        break;
                    case EndObject:
                        objectOpened--;
                        break;
                    case BeginArray:
                        arraysOpened++;
                        break;
                    case EndArray:
                        arraysOpened--;
                        break;
                    case QuotationMark:
                        CheckQuotationMark(index);
                        break;
                }
            }

            return nodes;


            void CheckQuotationMark(int index)
            {
                if (isNameStarted)
                {
                    EndName(index);
                }
                else
                {
                    if (isValueStarted)
                    {
                        if (objectOpened == 0 && arraysOpened == 0)
                        {
                            EndValue(index);
                        }
                    }
                    else
                    {
                        if (namesFound == valuesFound)
                        {
                            StartName(index);
                        }
                        else
                        {
                            StartValue(index);
                        }
                    }
                }
            }

            void StartName(int index)
            {
                isNameStarted = true;
                nameStartIndex = index + 1;
            }

            void EndName(int index)
            {
                nameEndIndex = index - 1;
                isNameStarted = false;
                namesFound++;
            }

            void StartValue(int index)
            {
                isValueStarted = true;
                valueStartIndex = index;
            }

            void EndValue(int index)
            {
                valueEndIndex = index;
                isValueStarted = false;
                valuesFound++;
                AddNode();
            }

            void AddNode()
            {
                var name = string.Concat(valueChars).Substring(nameStartIndex, nameEndIndex - nameStartIndex + 1);
                var value = string.Concat(valueChars).Substring(valueStartIndex, valueEndIndex - valueStartIndex);
                var node = DataNode.CreateInstance(name, value.ToCharArray());
                nodes.Add(node);
            }
        }

        public static (ValueType, object) DefineString(char[] valueChars)
        {
            var value = string.Concat(valueChars).Replace("\"", "").ToLower();

            if (float.TryParse(value, out var number))
            {
                return (ValueType.Number, number);
            }

            return value switch
            {
                "true" => (ValueType.True, true),
                "false" => (ValueType.False, false),
                "null" => (ValueType.Null, null),
                _ => (ValueType.String, value)
            };
        }
    }
}