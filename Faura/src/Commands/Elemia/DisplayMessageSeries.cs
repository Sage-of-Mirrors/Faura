using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace Faura.src.Commands.Elemia
{
    class DisplayMessageSeries : Command
    {
        public DisplayMessageSeries() : base()
        {
            CommandID = 2147479646;
            CommandName = "DisplayMessageSeries";
        }

        public override void ReadRawCommand(EndianBinaryReader reader)
        {
            Arguments.Add(reader.ReadUInt32()); // Index of first message
            Arguments.Add(reader.ReadUInt32()); // Index of last message
        }

        public override void ReadTextCommand(string line)
        {
            throw new NotImplementedException();
        }
    }
}
