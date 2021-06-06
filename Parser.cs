using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public class Parser
    {
        public static int GetObjectBeginIndex(char[] chars)
        {
            for (var index = 0; index < chars.Length; index++)
            {
                if (!chars[index].Equals(StructuralChars.BeginObject)) continue;
                return index;
            }

            throw new Exception("Can't find object's begin");
        }

        public static int GetObjectEndIndex(char[] chars)
        {
            for (var index = chars.Length - 1; index >= 0; index--)
            {
                if (!chars[index].Equals(StructuralChars.EndObject)) continue;
                return index;
            }

            throw new Exception("Can't find object's end");
        }

        public static ValueType DefineValueType(IEnumerable<char> valueChars)
        {
            var firstStructuralChar = valueChars.First(c => c != StructuralChars.Space);

            return firstStructuralChar switch
            {
                StructuralChars.BeginObject => ValueType.Object,
                StructuralChars.BeginArray => ValueType.Array,
                StructuralChars.QuotationMark => ValueType.String
            };
        }

        public static (ValueType, object) DefineString(char[] valueChars)
        {
            var value = valueChars.ToString().Replace('"', ' ').ToLower();

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