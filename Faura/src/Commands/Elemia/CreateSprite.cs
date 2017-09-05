using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace Faura.src.Commands.Elemia
{
    class CreateSprite : Command
    {
        public CreateSprite() : base()
        {
            CommandID = 2147479684;
            CommandName = "CreateSprite";
        }

        public override void ReadRawCommand(EndianBinaryReader reader)
        {
            Arguments.Add(reader.ReadUInt32()); // Sprite X coordinate
            Arguments.Add(reader.ReadUInt32()); // Sprite Y coordinate
            Arguments.Add(reader.ReadUInt32()); // Unknown
        }

        public override void ReadTextCommand(string line)
        {
            throw new NotImplementedException();
        }
    }
}
