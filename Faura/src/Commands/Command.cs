using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;
using System.IO;

namespace Faura.Commands
{
    public struct Command
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public int ParameterCount { get; set; }

        public Variable[] Variables { get; set; }

        public Command(Command src)
        {
            Name = src.Name;
            ID = src.ID;
            ParameterCount = src.ParameterCount;

            Variables = new Variable[ParameterCount];
            for (int i = 0; i < ParameterCount; i++)
                Variables[i] = new Variable(src.Variables[i]);
        }

        public void ReadString(string commandStr)
        {

        }

        public void ReadBinary(EndianBinaryReader reader)
        {
            for (int i = 0; i < ParameterCount; i++)
                Variables[i].Value = reader.ReadInt32();
        }

        public void WriteString(StreamWriter writer, Enum[] enums)
        {
            writer.Write($"{ Name } ");

            foreach (Variable var in Variables)
                var.WriteString(writer, enums);

            writer.Write("\n");
        }

        public void WriteBinary(EndianBinaryWriter writer, Enum[] enums)
        {
            writer.Write(ID);

            foreach (Variable var in Variables)
                writer.Write(var.Value);
        }
    }
}
