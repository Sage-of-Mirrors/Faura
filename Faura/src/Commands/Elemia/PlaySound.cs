using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace Faura.src.Commands.Elemia
{
    class PlaySound : Command
    {
        public PlaySound() : base()
        {
            CommandID = 2147479756;
            CommandName = "PlaySound";
        }

        public override void ReadRawCommand(EndianBinaryReader reader)
        {
            Arguments.Add(reader.ReadUInt32()); // Sound ID
        }

        public override void ReadTextCommand(string line)
        {
            throw new NotImplementedException();
        }
    }
}
