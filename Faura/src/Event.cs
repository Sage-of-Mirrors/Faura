using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GameFormatReader.Common;

namespace Faura.src
{
    public class Event
    {
        public List<Message> MessageList;

        public Event()
        {
            MessageList = new List<Message>();
        }

        public Event(string filePath)
        {
            MessageList = new List<Message>();

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Little);

                int messageCount = reader.ReadInt32();
                for (int i = 0; i < messageCount; i++)
                {
                    uint test = reader.PeekReadUInt32();
                    while (test >= 1082220674)
                    {
                        if (reader.PeekReadUInt32() >= 1082220674)
                            test = reader.ReadUInt32();
                        else
                            break;
                    }

                    MessageList.Add(new Message(reader));
                }

                int unknownInt = reader.ReadInt32();
            }

            using (FileStream debug = new FileStream($"D:\\Ar Tonelico\\event dump\\{Path.GetFileName(filePath)}.txt", FileMode.Create, FileAccess.Write))
            {
                //EndianBinaryWriter writer = new EndianBinaryWriter(debug, Endian.Big);
                StreamWriter strWriter = new StreamWriter(debug, Encoding.GetEncoding(932));

                foreach (Message mes in MessageList)
                {
                    strWriter.Write(mes.mMessageData);
                    strWriter.Write('\n');
                    strWriter.Write('\n');
                    strWriter.Flush();
                }

                //string str = strWriter.ToString();
                //writer.Write(strWriter.ToString());
            }
        }
    }
}
