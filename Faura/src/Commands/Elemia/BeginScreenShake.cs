using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace Faura.src.Commands.Elemia
{
    class BeginScreenShake : Command
    {
        public BeginScreenShake() : base()
        {
            CommandID = 2147479732;
            CommandName = "BeginScreenShake";
        }

        public override void ReadRawCommand(EndianBinaryReader reader)
        {
            // It has no arguments
        }

        public override void ReadTextCommand(string line)
        {
            throw new NotImplementedException();
        }
    }
}
