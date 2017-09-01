using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;

namespace Faura.src
{
    public class Message
    {
        public string TextboxType { get; set; }
        public string CharacterName { get; set; }
        public string CharacterID { get; set; }
        public string PortraitPosition { get; set; }
        public string MessageData { get; set; }

        private TextBoxType mBoxType;
        private int mCharacterName;
        private int mCharacterID;
        private int mMessageIndex;
        private short mPortraitPosition;
        private short mUnknown1;
        private int mMessageLength;

        public string mMessageData;

        public Message()
        {

        }


        public Message(EndianBinaryReader reader)
        {
            mBoxType = (TextBoxType)reader.ReadInt32();
            mCharacterName = reader.ReadInt32();
            mCharacterID = reader.ReadInt32();
            mMessageIndex = reader.ReadInt32();
            mPortraitPosition = reader.ReadInt16();
            mUnknown1 = reader.ReadInt16();
            mMessageLength = reader.ReadInt32();

            byte[] rawMessageData = reader.ReadBytes(mMessageLength);
            mMessageData = MessageDataProcessor.DecodeBytes(rawMessageData);

            PadMessageReader(reader);
        }

        public void Write(EndianBinaryWriter writer, int index)
        {
            writer.Write((int)mBoxType);
            writer.Write(mCharacterName);
            writer.Write(mCharacterID);
            writer.Write(index);
            writer.Write(mPortraitPosition);
            writer.Write(mUnknown1);

            byte[] rawMessageData = MessageDataProcessor.EncodeString(mMessageData);

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

    public enum TextBoxType : int
    {
        NormalDialog = 0,
        RoundedBubbly = 1,
        FullPortrait = 2,
        Unknown1 = 3,
        TopCentered = 4,
        BottomCentered = 5,
        StretchedAcrossBottom = 6,
        Invalid1 = 7,
        BottomRight = 8,
        PortraitAcrossBottom = 9,
        BottomCentered2 = 10,
        RoundedBubblyBottomCenter = 11,
        CosmosphereCenterPortrait = 12
    }
}
