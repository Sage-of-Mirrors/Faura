using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace Faura.src.Commands.Elemia
{
    class SetSpriteID : Command
    {
        public SetSpriteID() : base()
        {
            CommandID = 2147479721;
            CommandName = "SetSpriteID";
        }

        public override void ReadRawCommand(EndianBinaryReader reader)
        {
            Arguments.Add(reader.ReadUInt32()); // Sprite ID
        }

        public override void ReadTextCommand(string line)
        {
            throw new NotImplementedException();
        }
    }
}
