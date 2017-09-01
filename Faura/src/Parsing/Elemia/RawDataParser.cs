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

            return new Event();
        }

        private void ReadMessages(EndianBinaryReader reader, Event ev)
        {
            int messageCount = reader.ReadInt32();

            for (int i = 0; i < messageCount; i++)
            {
                if (i == 94)
                {

                }
                AccountForMessagePadding(reader); // Message data is padded irregularly. This will make sure we can read them all correctly

                Message mes = new Message();

                mes.TextboxType = TextboxTypes[reader.ReadUInt32()];
                mes.CharacterName = CharacterNameIDs[reader.ReadUInt32()];
                //mes.CharacterID = CharacterMessageIDs[reader.ReadUInt32()]; // This can be 0xFFFFFFFF, but I'll add <0xFFFFFFFF, None> to the dictionary later
                reader.SkipInt32();

                reader.SkipInt32();

                mes.PortraitPosition = PortraitPositionIDs[reader.ReadUInt16()];

                reader.SkipInt16();

                int messageLength = reader.ReadInt32();
                mes.MessageData = MessageDataProcessor.DecodeBytes(reader.ReadBytes(messageLength));

                ev.MessageList.Add(mes);

                MessageDataProcessor.PadMessageReader(reader);
            }

            Debug_WriteMessages(@"D:\Ar Tonelico\1_messages_out.txt", ev);
        }

        private void Debug_WriteMessages(string path, Event ev)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                StringWriter strWriter = new StringWriter();

                foreach (Message mes in ev.MessageList)
                {
                    strWriter.WriteLine($"Box Type: { mes.TextboxType }, Character Name: { mes.CharacterName }, Character ID: { mes.CharacterID }, Portrait Position: { mes.PortraitPosition }");
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
            while (test >= 1082220674)
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
