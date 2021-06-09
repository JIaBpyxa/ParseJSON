using System;
using System.IO;
using System.Linq;

namespace Parser
{
    public class ClassCreator
    {
        public static void CreateClass(DataNode dataNode)
        {
            if (dataNode.Type != ValueType.Object)
            {
                return;
            }

            var filePathAndName = $"Output/{dataNode.Name}.cs";

            using var streamWriter = new StreamWriter(filePathAndName, false);
            WriteClassTop(streamWriter, "testNameSpace", dataNode.Name);
            WriteClassMembers(streamWriter, dataNode);
            WriteClassEnd(streamWriter);

            DefineChildObjects(dataNode);
        }

        private static void WriteClassTop(StreamWriter streamWriter, string namespaceName, string className)
        {
            streamWriter.WriteLine($"namespace {namespaceName}");
            streamWriter.WriteLine("{");
            streamWriter.WriteLine($"\tpublic class {className}");
            streamWriter.WriteLine("\t{");
        }

        private static void WriteClassMembers(StreamWriter streamWriter, DataNode dataNode)
        {
            var number = 0;
            foreach (var valueNode in dataNode.value)
            {
                streamWriter.WriteLine($"\t\tpublic {GetStringByDataNode(valueNode, number)};");
                number++;
            }


            string GetStringByDataNode(DataNode dataNode, int objectNumber)
            {
                if (dataNode.Type == ValueType.Object)
                {
                    return $"{GetObjectType(dataNode)} {dataNode.Name}{objectNumber}";
                }
                else
                {
                    return $"{GetObjectType(dataNode)} {dataNode.Name}";
                }
            }

            string GetObjectType(DataNode dataNode)
            {
                switch (dataNode.Type)
                {
                    case ValueType.Object:
                        return $"{dataNode.Name}";
                    case ValueType.Array:
                        return $"{GetObjectType(dataNode.value[0])}[]";
                    case ValueType.String:
                        return "string";
                    case ValueType.Number:
                        return $"{GetNumberType(dataNode)}";
                    case ValueType.True:
                        return "bool";
                    case ValueType.False:
                        return "bool";
                    default:
                        return "object";
                }
            }

            string GetNumberType(DataNode dataNode)
            {
                var value = (string) dataNode.valueData;
                if (value.Contains('.') || value.Contains('f'))
                {
                    return "float";
                }
                else
                {
                    return "int";
                }
            }
        }

        private static void WriteClassEnd(StreamWriter streamWriter)
        {
            streamWriter.WriteLine("\t}");
            streamWriter.WriteLine("}");
        }

        private static void DefineChildObjects(DataNode dataNode)
        {
            foreach (var valueNode in dataNode.value)
            {
                if (valueNode.Type == ValueType.Object)
                {
                    CreateClass(valueNode);
                }
                else if (valueNode.Type == ValueType.Array && valueNode.value.Count != 0)
                {
                    if (valueNode.value[0].Type == ValueType.Object)
                    {
                        CreateClass(valueNode.value[0]);
                    }
                }
            }
        }
    }
}