using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;
using Newtonsoft.Json;
using Faura.Enums.Metafalica;

namespace Faura.Messages
{
    public class Message
    {
        private TextBoxType mBoxType;
        private CharacterNameID mCharacterName;
        private int mCharacterIDOrPortrait;
        private int mMessageIndex;
        private PortraitPosition mPortraitPosition;
        private short mUnknown1;
        private int mMessageLength;
        private string mMessageData;

        public string Name { get; set; }

        public string IsUsed { get; set; }

        public string TextboxType
        {
            get { return mBoxType.ToString(); }
            set { mBoxType = (TextBoxType)MessageDataProcessor.EnumValueFromString(typeof(TextBoxType), value); }
        }

        public string CharacterName
        {
            get { return mCharacterName.ToString(); }
            set { mCharacterName = (CharacterNameID)MessageDataProcessor.EnumValueFromString(typeof(CharacterNameID), value); }
        }

        public string CharacterID
        {
            get
            {
                if (mBoxType == TextBoxType.FULL_PORTRAIT)
                    return ((PortraitID)mCharacterIDOrPortrait).ToString();
                else
                    return ((CharacterID)mCharacterIDOrPortrait).ToString();
            }
            set
            {
                if (mBoxType == TextBoxType.FULL_PORTRAIT)
                    mCharacterIDOrPortrait = Convert.ToInt32(MessageDataProcessor.EnumValueFromString(typeof(PortraitID), value));
                else
                    mCharacterIDOrPortrait = Convert.ToInt32(MessageDataProcessor.EnumValueFromString(typeof(CharacterID), value));
            }
        }

        public string PortraitPosition
        {
            get { return mPortraitPosition.ToString(); }
            set { mPortraitPosition = (PortraitPosition)MessageDataProcessor.EnumValueFromString(typeof(PortraitPosition), value); }
        }

        public string Text
        {
            get { return mMessageData; }
            set { mMessageData = value; }
        }

        public Message()
        {

        }

        public Message(EndianBinaryReader reader, int index)
        {
            Name = $"msg_{ index }";
            IsUsed = "False";

            int msgBeginTest = reader.PeekReadInt32();
            while (msgBeginTest > (int)TextBoxType.COSMOSPHERE_CENTER_PORTRAIT || msgBeginTest < 0)
            {
                msgBeginTest = reader.ReadInt32();
                if (msgBeginTest <= (int)TextBoxType.COSMOSPHERE_CENTER_PORTRAIT && msgBeginTest >= 0)
                    reader.BaseStream.Position -= 4;
            }

            mBoxType = (TextBoxType)reader.ReadInt32();
            mCharacterName = (CharacterNameID)reader.ReadInt32();
            mCharacterIDOrPortrait = reader.ReadInt32();
            mMessageIndex = reader.ReadInt32();
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
            writer.Write(mCharacterIDOrPortrait);
            writer.Write(index);
            writer.Write((short)mPortraitPosition);
            writer.Write((short)1);

            byte[] rawMessageData = MessageDataProcessor.EncodeString(Text);

            writer.Write(rawMessageData.Length);
            writer.Write(rawMessageData);

            PadMessageWriter(writer);
        }

        private void PadMessageReader(EndianBinaryReader reader)
        {
            // Pad up to a 4 byte alignment
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
            // Pad up to a 4 byte alignment
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
