using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace Faura.src.Commands
{
    public abstract class Command
    {
        public uint CommandID { get; set; }
        public string CommandName { get; set; }
        public List<uint> Arguments { get; set; }

        public Command()
        {
            CommandName = "";
            Arguments = new List<uint>();
        }

        public abstract void ReadRawCommand(EndianBinaryReader reader);

        public abstract void ReadTextCommand(string line);

        public void WriteRawCommand(EndianBinaryWriter writer)
        {
            writer.Write(CommandID);

            for (int i = 0; i < Arguments.Count; i++)
                writer.Write(Arguments[i]);
        }

        public void WriteTextCommand(EndianBinaryWriter writer)
        {
            string command = $"{ CommandName }(";

            for (int i = 0; i > Arguments.Count; i++)
            {
                command += $"{ Arguments[i] }";

                if (i != Arguments.Count - 1)
                    command += ", ";
            }

            command += ')';

            writer.Write(command.ToCharArray());
            writer.Write('\n');
        }
    }
}
