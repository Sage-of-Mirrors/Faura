using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace Faura.src.Commands.Elemia
{
    public class SetShakeScreenProperties : Command
    {
        public SetShakeScreenProperties() : base()
        {
            CommandID = 2147479734;
            CommandName = "SetShakeScreenProperties";
        }

        public override void ReadRawCommand(EndianBinaryReader reader)
        {
            Arguments.Add(reader.ReadUInt32()); // Intensity
            Arguments.Add(reader.ReadUInt32()); // Unknown. Controller rumble pattern?
        }

        public override void ReadTextCommand(string line)
        {
            throw new NotImplementedException();
        }
    }
}
