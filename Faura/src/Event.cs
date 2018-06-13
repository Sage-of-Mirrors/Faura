using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GameFormatReader.Common;
using Newtonsoft.Json;
using Faura.Messages;
using Faura.Commands;
using Faura.Enums;

namespace Faura
{
    public class Event
    {
        private List<Message> mMessageList;
        private List<Command> mCommandList;

        private Command[] CommandTemplates;

        public Event()
        {

        }

        public Event(string filePath, string outPath, string cmdVersion)
        {
            mMessageList = new List<Message>();
            mCommandList = new List<Command>();
            string fileExt = Path.GetExtension(filePath);

            string actualVersion = cmdVersion;
            Enum[] versionEnums = LoadVersionEnums(filePath, fileExt == ".txt" ? true : false, cmdVersion, out actualVersion);
            CommandTemplates = LoadVersionTemplates(actualVersion);

            if (fileExt == ".evd")
            {
                LoadEvd(filePath);
                SaveTxt(filePath, actualVersion, versionEnums);
            }

            else if (fileExt == ".txt")
            {
                LoadTxt(filePath, versionEnums);
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

                int commandBlockSize = reader.ReadInt32() * 4;
                long startPos = reader.BaseStream.Position;

                while(reader.BaseStream.Position - startPos < commandBlockSize)
                {
                    uint cmdID = reader.ReadUInt32();
                    Command cmd = new Command(CommandTemplates.First(x => x.ID == cmdID));
                    cmd.ReadBinary(reader);
                    mCommandList.Add(cmd);
                }
            }
        }

        private void LoadTxt(string filePath, Enum[] enums)
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

            using (FileStream data = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(data);

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (line.Length == 0 || line[0] == '#' || line[0] == ' ')
                        continue;

                    string[] commandDisassembly = line.Split(' ');

                    Command cmd = new Command(CommandTemplates.First(x => x.Name == commandDisassembly[0]));
                    for (int i = 0; i < cmd.ParameterCount; i++)
                        cmd.Variables[i].SetValue(commandDisassembly[i + 1]);

                    mCommandList.Add(cmd);
                }
            }
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

                writer.Write((int)0);
                long curOffset = writer.BaseStream.Position;

                foreach (Command com in mCommandList)
                    com.WriteBinary(writer, null);

                long size = writer.BaseStream.Position - curOffset;
                writer.BaseStream.Seek(curOffset - 4, SeekOrigin.Begin);
                writer.Write((int)size / 4);
            }
        }

        private void SaveTxt(string filePath, string version, Enum[] enums)
        {
            string dir = Path.GetDirectoryName(filePath);
            string name = Path.GetFileNameWithoutExtension(filePath);

            using (FileStream cmdWriter = new FileStream(Path.Combine(dir, name + ".txt"), FileMode.Create, FileAccess.Write))
            {
                StreamWriter strWriter = new StreamWriter(cmdWriter, Encoding.UTF8);
                strWriter.AutoFlush = true;

                strWriter.WriteLine("# Event dumped by Faura. Visit https://github.com/Sage-of-Mirrors/Faura for issues or contact @SageOfMirrors.");
                strWriter.WriteLine($"# version { version }");

                strWriter.WriteLine();

                foreach (Command com in mCommandList)
                {
                    if (com.Name == "CreatePosition")
                        strWriter.WriteLine();
                    if (com.Name == "DisplayDialogSeries")
                    {
                        strWriter.Write($"{ com.Name } ");
                        strWriter.Write($"{ mMessageList[com.Variables[0].Value].Name } ");
                        strWriter.Write($"{ mMessageList[com.Variables[1].Value].Name } \n");

                        for (int i = com.Variables[0].Value; i <= com.Variables[1].Value; i++)
                            mMessageList[i].IsUsed = "True";
                    }
                    else if (com.Name == "DisplayTemporaryDialog")
                    {
                        strWriter.Write($"{ com.Name } ");
                        strWriter.Write($"{ mMessageList[com.Variables[0].Value].Name } \n");
                        mMessageList[com.Variables[0].Value].IsUsed = "True";
                    }
                    else
                        com.WriteString(strWriter, enums);
                }

                strWriter.WriteLine();
                strWriter.Write("# EOF");
            }

            using (FileStream msgWriter = new FileStream(Path.Combine(dir, name + "_msg.txt"), FileMode.Create, FileAccess.Write))
            {
                string jsonMsgs = JsonConvert.SerializeObject(mMessageList, Formatting.Indented);

                StreamWriter strWriter = new StreamWriter(msgWriter, Encoding.GetEncoding(932));
                strWriter.AutoFlush = true;
                strWriter.Write(jsonMsgs);
            }
        }

        private Enum[] LoadVersionEnums(string filePath, bool isTxt, string cmdVersion, out string version)
        {
            version = cmdVersion;

            // If the input is a txt file, we'll get the version from the file itself
            if (isTxt)
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader rdr = new StreamReader(file);

                    string versionComment = rdr.ReadLine();
                    while (!versionComment.ToLower().Contains("# version"))
                    {
                        if (rdr.EndOfStream)
                            throw new Exception($"File \"{ filePath }\" did not contain a version comment (#version)!");

                        versionComment = rdr.ReadLine();
                    }

                    version = versionComment.Split(' ')[2].ToLower();
                }
            }

            return EnumLoader.GetEnumsFromVersion(version);
        }

        private Command[] LoadVersionTemplates(string version)
        {
            string jsonString = "";

            switch(version)
            {
                case "metafalica":
                    jsonString = Properties.Resources.metafalica;
                    break;
                default:
                    throw new Exception($"Unknown version \"{ version }\"!");
            }

            return JsonConvert.DeserializeObject<Command[]>(jsonString);
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
