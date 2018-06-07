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
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"The file \"{ filePath }\" does not exist!");
                return;
            }

            mMessageList = new List<Message>();
            string fileExt = Path.GetExtension(filePath);

            if (fileExt == ".evd")
            {
                LoadEvd(filePath);
                SaveTxt(filePath);
            }

            else if (fileExt == ".txt")
            {
                LoadTxt(filePath);
                SaveEvd(filePath);
            }

            else
            {
                Console.WriteLine($"The file \"{ filePath }\" was not a recognized file type!");
                return;
            }
        }

        private void LoadEvd(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                EndianBinaryReader reader = new EndianBinaryReader(stream, Endian.Little);

                int messageCount = reader.ReadInt32();
                for (int i = 0; i < messageCount; i++)
                    mMessageList.Add(new Message(reader));

                int unknownInt = reader.ReadInt32();
            }
        }

        private void LoadTxt(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string messageFileName = fileName + "_msg.txt";

            string messageFilePath = Path.Combine(Path.GetDirectoryName(filePath), messageFileName);

            if (!File.Exists(messageFilePath))
            {
                Console.WriteLine($"Message file { messageFilePath } not found. Please make sure it exists and is in the same\ndirectory as the data file.");
                return;
            }

            string json = File.ReadAllText(messageFilePath);
            mMessageList.AddRange(JsonConvert.DeserializeObject<Message[]>(json));
        }

        private void SaveEvd(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            string name = Path.GetFileNameWithoutExtension(filePath);

            using (FileStream strm = new FileStream(Path.Combine(dir, name + ".evd"), FileMode.Create, FileAccess.Write))
            {
                EndianBinaryWriter writer = new EndianBinaryWriter(strm, Endian.Little);

                writer.Write(mMessageList.Count);

                for (int i = 0; i < mMessageList.Count; i++)
                    mMessageList[i].Write(writer, i);
            }
        }

        private void SaveTxt(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            string name = Path.GetFileNameWithoutExtension(filePath);

            using (FileStream msgWriter = new FileStream(Path.Combine(dir, name + "_msg.txt"), FileMode.Create, FileAccess.Write))
            {
                string jsonMsgs = JsonConvert.SerializeObject(mMessageList, Formatting.Indented);

                StreamWriter strWriter = new StreamWriter(msgWriter, Encoding.GetEncoding(932));
                strWriter.AutoFlush = true;
                strWriter.Write(jsonMsgs);
            }
        }

        /*private void Debug_DumpMessages()
        {
            using (FileStream debug = new FileStream(@"D:\Ar Tonelico\eventtext.txt", FileMode.Create, FileAccess.Write))
            {
                StreamWriter strWriter = new StreamWriter(debug, Encoding.GetEncoding(932));
                string test = JsonConvert.SerializeObject(mMessageList, Formatting.Indented);

                strWriter.AutoFlush = true;
                strWriter.Write(test);
            }
        }*/
    }
}
