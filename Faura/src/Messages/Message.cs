using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;
using Newtonsoft.Json;

namespace Faura.Messages
{
    public class Message
    {
        private TextBoxType mBoxType;
        private CharacterNameID mCharacterName;
        private int mCharacterID;
        private int mMessageIndex;
        private PortraitPosition mPortraitPosition;
        private short mUnknown1;
        private int mMessageLength;
        private string mMessageData;

        public string Name { get; set; }

        public string TextboxType
        {
            get { return mBoxType.ToString(); }
        }

        public string CharacterName
        {
            get { return mCharacterName.ToString(); }
        }

        public string CharacterID
        {
            get { return mCharacterID.ToString(); }
        }

        public string PortraitPosition
        {
            get { return mPortraitPosition.ToString(); }
        }

        public string Text
        {
            get { return mMessageData; }
        }

        public Message()
        {

        }

        public Message(EndianBinaryReader reader)
        {
            mBoxType = (TextBoxType)reader.ReadInt32();
            mCharacterName = (CharacterNameID)reader.ReadInt32();
            mCharacterID = reader.ReadInt32();
            mMessageIndex = reader.ReadInt32();
            Name = $"msg_{ mMessageIndex }";
            mPortraitPosition = (PortraitPosition)reader.ReadInt16();
            mUnknown1 = reader.ReadInt16();
            mMessageLength = reader.ReadInt32();

            byte[] rawMessageData = reader.ReadBytes(mMessageLength);
            mMessageData = MessageDataProcessor.DecodeBytes(rawMessageData).Trim();

            PadMessageReader(reader);
        }

        public void Write(EndianBinaryWriter writer, int index)
        {
            writer.Write((int)mBoxType);
            writer.Write((int)mCharacterName);
            writer.Write(mCharacterID);
            writer.Write(index);
            writer.Write((int)mPortraitPosition);
            writer.Write(mUnknown1);

            byte[] rawMessageData = MessageDataProcessor.EncodeString(Text);

            writer.Write(rawMessageData.Length);
            writer.Write(rawMessageData);

            PadMessageWriter(writer);
        }

        private void PadMessageReader(EndianBinaryReader reader)
        {
            // Pad up to a 32 byte alignment
            // Formula: (x + (n-1)) & ~(n-1)
            long nextAligned = (reader.BaseStream.Position + (4 - 1)) & ~(4 - 1);

            long delta = nextAligned - reader.BaseStream.Position;
            //reader.BaseStream.Position = reader.BaseStream.Position;

            for (int i = 0; i < delta; i++)
            {
                reader.SkipByte();
            }
        }

        private void PadMessageWriter(EndianBinaryWriter writer)
        {
            // Pad up to a 32 byte alignment
            // Formula: (x + (n-1)) & ~(n-1)
            long nextAligned = (writer.BaseStream.Length + (4 - 1)) & ~(4 - 1);

            long delta = nextAligned - writer.BaseStream.Length;
            writer.BaseStream.Position = writer.BaseStream.Length;
            for (int i = 0; i < delta; i++)
            {
                writer.Write((byte)0);
            }
        }
    }
}
