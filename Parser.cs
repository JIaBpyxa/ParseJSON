using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Parser.StructuralChars;

namespace Parser
{
    public static class Parser
    {
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
            var objectOpened = 0;
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

            for (var index = GetObjectBeginIndex(valueChars) + 1; index < valueChars.Length; index++)
            {
                switch (valueChars[index])
                {
                    case BeginObject:
                        objectOpened++;
                        CheckBeginObject(index);
                        break;
                    case EndObject:
                        objectOpened--;
                        CheckEndObject(index);
                        break;
                    case BeginArray:
                        arraysOpened++;
                        CheckBeginArray(index);
                        break;
                    case EndArray:
                        arraysOpened--;
                        CheckEndArray(index);
                        break;
                    case QuotationMark:
                        CheckQuotationMark(index);
                        break;
                }
            }

            return nodes;


            void CheckBeginObject(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;
                if (objectOpened != 1 || arraysOpened != 0) return;
                
                StartValue(index);
            }

            void CheckEndObject(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;
                if (objectOpened != 0 || arraysOpened != 0) return;

                EndValue(index);
                AddNode();
            }

            void CheckBeginArray(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;
                if (arraysOpened != 1 || objectOpened != 0) return;
                
                StartValue(index);
            }

            void CheckEndArray(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;
                if (objectOpened != 0 || arraysOpened != 0) return;

                EndValue(index);
                AddNode();
            }

            void CheckQuotationMark(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;

                if (isNameStarted)
                {
                    EndName(index);
                }
                else
                {
                    if (!isValueStarted)
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
                    else
                    {
                        if (objectOpened != 0 || arraysOpened != 0) return;
                        EndValue(index);
                        AddNode();
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

        public static List<DataNode> DefineArray(string arrayName, char[] valueChars)
        {
            var objectOpened = 0;
            var arraysOpened = 0;
            var isValueStarted = false;

            var valueStartIndex = -1;
            var valueEndIndex = -1;

            var valuesFound = 0;

            var nodes = new List<DataNode>();

            for (var index = GetArrayBeginIndex(valueChars) + 1; index < valueChars.Length; index++)
            {
                switch (valueChars[index])
                {
                    case BeginObject:
                        objectOpened++;
                        CheckBeginObject(index);
                        break;
                    case EndObject:
                        objectOpened--;
                        CheckEndObject(index);
                        break;
                    case BeginArray:
                        arraysOpened++;
                        CheckBeginArray(index);
                        break;
                    case EndArray:
                        arraysOpened--;
                        CheckEndArray(index);
                        break;
                    case QuotationMark:
                        CheckQuotationMark(index);
                        break;
                }
            }

            return nodes;


            void CheckBeginObject(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;
                if (objectOpened != 1 || arraysOpened != 0) return;
                
                StartValue(index);
            }

            void CheckEndObject(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;
                if (objectOpened != 0 || arraysOpened != 0) return;

                EndValue(index);
                AddNode();
            }

            void CheckBeginArray(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;
                if (arraysOpened != 1 || objectOpened != 0) return;
                
                StartValue(index);
            }

            void CheckEndArray(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;
                if (objectOpened != 0 || arraysOpened != 0) return;

                EndValue(index);
                AddNode();
            }

            void CheckQuotationMark(int index)
            {
                if (index > 0 && valueChars[index - 1] == '\\') return;

                if (!isValueStarted)
                {
                    StartValue(index);
                }
                else
                {
                    if (objectOpened != 0 || arraysOpened != 0) return;
                    EndValue(index);
                    AddNode();
                }
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
            }

            void AddNode()
            {
                var name = arrayName.ToLower().Replace(" ", "");
                if (name.EndsWith('s'))
                {
                    name = name[..^1];
                }

                var value = string.Concat(valueChars).Substring(valueStartIndex, valueEndIndex - valueStartIndex);
                var node = DataNode.CreateInstance(name, value.ToCharArray());
                nodes.Add(node);
            }
        }

        private static int GetObjectBeginIndex(char[] chars)
        {
            for (var index = 0; index < chars.Length; index++)
            {
                if (!chars[index].Equals(BeginObject)) continue;
                return index;
            }

            throw new Exception("Can't find object's begin");
        }

        private static int GetArrayBeginIndex(char[] chars)
        {
            for (var index = 0; index < chars.Length; index++)
            {
                if (!chars[index].Equals(BeginArray)) continue;
                return index;
            }

            throw new Exception("Can't find array's begin");
        }
    }
}