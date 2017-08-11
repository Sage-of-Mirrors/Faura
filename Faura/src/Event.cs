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
        private List<Message> mMessageList;

        public Event()
        {

        }

        public Event(string filePath)
        {
            mMessageList = new List<Message>();

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

                    mMessageList.Add(new Message(reader));
                }

                int unknownInt = reader.ReadInt32();
            }

            using (FileStream debug = new FileStream($"D:\\Ar Tonelico\\event dump\\{Path.GetFileName(filePath)}.txt", FileMode.Create, FileAccess.Write))
            {
                EndianBinaryWriter writer = new EndianBinaryWriter(debug, Endian.Big);
                StreamWriter strWriter = new StreamWriter(debug, Encoding.GetEncoding(932));

                foreach (Message mes in mMessageList)
                {
                    mes.WriteString(writer);
                }

                //string str = strWriter.ToString();
                //writer.Write(strWriter.ToString());
            }
        }
    }
}
