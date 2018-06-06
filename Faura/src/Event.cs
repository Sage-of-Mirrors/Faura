using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GameFormatReader.Common;
using Newtonsoft.Json;
using Faura.Messages;

namespace Faura
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
                    mMessageList.Add(new Message(reader));

                int unknownInt = reader.ReadInt32();
            }

            using (FileStream debug = new FileStream(@"D:\Ar Tonelico\eventtext.txt", FileMode.Create, FileAccess.Write))
            {
                StreamWriter strWriter = new StreamWriter(debug, Encoding.GetEncoding(932));
                string test = JsonConvert.SerializeObject(mMessageList, Formatting.Indented);

                strWriter.AutoFlush = true;
                strWriter.Write(test);

                /*foreach (Message mes in mMessageList)
                {
                    strWriter.Write(mes.Text);
                    strWriter.Write('\n');
                    strWriter.Write('\n');
                }*/

                //string str = strWriter.ToString();
                //writer.Write(strWriter.ToString());
            }
        }
    }
}
