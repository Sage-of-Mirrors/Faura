using System;
using System.IO;
using GameFormatReader.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faura.src.Parsing.Elemia
{
    public partial class ElemiaEventParser : GustEventParser
    {
        public override Event ParseRawData(string filePath)
        {
            EndianBinaryReader reader = GetReader(filePath);
            Event ev = new Event();

            ReadMessages(reader, ev);
            ReadCommands(reader, ev);

            return ev;
        }

        private void ReadMessages(EndianBinaryReader reader, Event ev)
        {
            int messageCount = reader.ReadInt32();

            for (int i = 0; i < messageCount; i++)
            {
                Message mes = new Message();

                mes.TextboxType = TextboxTypes[reader.ReadUInt32()];
                mes.CharacterName = CharacterNameIDs[reader.ReadUInt32()];

                // This field contains either the sprite ID to tell the game who to hover the textbox over, or what portrait to display.
                if (mes.TextboxType == "Follow Character")
                {
                    mes.SpriteOrPortraitID = SpriteIDs[reader.ReadUInt32()]; // Sprite ID
                }
                else if (mes.TextboxType == "Portrait")
                {
                    mes.SpriteOrPortraitID = PortraitIDs[reader.ReadUInt32()]; // Portrait ID
                }
                else
                {
                    mes.SpriteOrPortraitID = SpriteIDs[reader.ReadUInt32()];
                }

                reader.SkipInt32();

                mes.PortraitPosition = PortraitPositionIDs[reader.ReadUInt16()];

                reader.SkipInt16();

                int messageLength = reader.ReadInt32();
                mes.MessageData = MessageDataProcessor.DecodeBytes(reader.ReadBytes(messageLength));

                ev.MessageList.Add(mes);

                MessageDataProcessor.PadMessageReader(reader);
                AccountForMessagePadding(reader); // Message data is padded irregularly. This will make sure we can read them all correctly
            }

            Debug_WriteMessages(@"D:\Ar Tonelico\1_messages_out.txt", ev);
        }

        private void ReadCommands(EndianBinaryReader reader, Event ev)
        {
            // The command block stores an int32* to the end of the event, some commands to run after
            // the event finishes.
            uint endPtrSize = reader.ReadUInt32();
            // We can use this value to calculate when to stop reading the bulk of the commands.
            // We subtract 1 from endPtrSize because it includes the int32* itself.
            uint endOffset = ((endPtrSize - 1) * 4) + (uint)reader.BaseStream.Position;

            while (reader.BaseStream.Position > endOffset)
            {

            }
        }

        private void Debug_WriteMessages(string path, Event ev)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                StringWriter strWriter = new StringWriter();

                foreach (Message mes in ev.MessageList)
                {
                    strWriter.WriteLine($"Box Type: { mes.TextboxType }, Character Name: { mes.CharacterName }, SpriteOrPortraitID: { mes.SpriteOrPortraitID }, Portrait Position: { mes.PortraitPosition }");
                    strWriter.WriteLine($"Text: { mes.MessageData }");
                    strWriter.WriteLine();
                }

                EndianBinaryWriter writer = new EndianBinaryWriter(stream, Endian.Big);
                writer.Write(strWriter.ToString().ToCharArray());
            }
        }

        private void AccountForMessagePadding(EndianBinaryReader reader)
        {
            uint test = reader.PeekReadUInt32();
            while (test <= 1082220674)
            {
                if (reader.PeekReadUInt32() >= 1082220674)
                    test = reader.ReadUInt32();
                else
                    break;
            }
        }

        public override void WriteRawData(string filePath, Event ev)
        {
            throw new NotImplementedException();
        }
    }
}
