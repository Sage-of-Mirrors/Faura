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
using System.Text.RegularExpressions;

namespace Faura
{
    public class Event
    {
        private List<Message> mMessageList;
        private List<Command> mCommandList_1;
        private List<Command> mCommandList_2;
        private List<string[]> mChoices;

        private Command[] CommandTemplates;

        public Event()
        {

        }

        public Event(string filePath, string outPath, string cmdVersion)
        {
            mMessageList = new List<Message>();
            mCommandList_1 = new List<Command>();
            mCommandList_2 = new List<Command>();
            mChoices = new List<string[]>();

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
                    mMessageList.Add(new Message(reader, i));

                // Command block 1
                int commandBlockSize = reader.ReadInt32() * 4;
                long startPos = reader.BaseStream.Position;

                while(reader.BaseStream.Position - startPos < commandBlockSize)
                {
                    uint cmdID = reader.ReadUInt32();

                    if (cmdID == 2147479592)
                    {
                        int formatTest = reader.PeekReadInt32();

                        if (formatTest == 1000 || formatTest == 0)
                        {
                            Command cmd = new Command(CommandTemplates.First(x => x.Name == "func_F028_2"));
                            cmd.ReadBinary(reader);
                            mCommandList_1.Add(cmd);
                        }
                        else
                        {
                            Command cmd = new Command(CommandTemplates.First(x => x.Name == "func_F028"));
                            cmd.ReadBinary(reader);
                            mCommandList_1.Add(cmd);
                        }

                        continue;
                    }
                    else if (cmdID == 2147479612)
                    {
                        int formatTest = reader.PeekReadInt32();

                        if (formatTest == 1)
                        {
                            Command cmd = new Command(CommandTemplates.First(x => x.Name == "func_F03C_2"));
                            cmd.ReadBinary(reader);
                            mCommandList_1.Add(cmd);
                        }
                        else
                        {
                            Command cmd = new Command(CommandTemplates.First(x => x.Name == "func_F03C"));
                            cmd.ReadBinary(reader);
                            mCommandList_1.Add(cmd);
                        }

                        continue;
                    }
                    else
                    {
                        Command cmd = new Command(CommandTemplates.First(x => x.ID == cmdID));
                        cmd.ReadBinary(reader);
                        mCommandList_1.Add(cmd);
                    }
                }

                // Command block 2
                commandBlockSize = reader.ReadInt32() * 4;
                startPos = reader.BaseStream.Position;

                while (reader.BaseStream.Position - startPos < commandBlockSize)
                {
                    uint cmdID = reader.ReadUInt32();
                    Command cmd = new Command(CommandTemplates.First(x => x.ID == cmdID));
                    cmd.ReadBinary(reader);
                    mCommandList_2.Add(cmd);
                }

                // Player choices
                int choiceBlockCount = reader.ReadInt32();

                for (int i = 0; i < choiceBlockCount; i++)
                {
                    List<string> choices = new List<string>();

                    int numStrings = reader.ReadInt32();

                    for (int j = 0; j < numStrings; j++)
                    {
                        int stringLength = reader.ReadInt32();
                        byte[] strBytes = reader.ReadBytes(stringLength);
                        choices.Add(MessageDataProcessor.DecodeBytes(strBytes));

                        MessageDataProcessor.PadMessageReader(reader);
                    }

                    mChoices.Add(choices.ToArray());
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
                List<Command> activeList = mCommandList_1;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (line.ToLower() == "# stream 1")
                        activeList = mCommandList_1;
                    if (line.ToLower() == "# stream 2")
                        activeList = mCommandList_2;
                    if (line.ToLower().Contains("# choices"))
                    {
                        MatchCollection choices = Regex.Matches(line, "\"[^\"]*\"");
                        List<string> choiceStrs = new List<string>();
                        foreach (Match m in choices)
                        {
                            choiceStrs.Add(m.Value.TrimStart('\"').TrimEnd('\"'));
                        }

                        mChoices.Add(choiceStrs.ToArray());
                    }

                    if (line.Length == 0 || line[0] == '#' || line[0] == ' ')
                        continue;

                    string[] commandDisassembly = line.Split(' ');

                    Command cmd = new Command(CommandTemplates.First(x => x.Name == commandDisassembly[0]));

                    // We need to convert the message names to actual indices, if required
                    if (cmd.Name == "DisplayDialogSeries")
                    {
                        int msg1, msg2;

                        // We'll try to parse an int out of the string. Failing that, we'll search for the message name
                        // in the message list.
                        if (!int.TryParse(commandDisassembly[1], out msg1))
                            msg1 = mMessageList.FindIndex(x => x.Name == commandDisassembly[1]);
                        if (!int.TryParse(commandDisassembly[2], out msg2))
                            msg2 = mMessageList.FindIndex(x => x.Name == commandDisassembly[2]);

                        // Replace the original strings to make it easy for the processor to load
                        commandDisassembly[1] = msg1.ToString();
                        commandDisassembly[2] = msg2.ToString();
                    }
                    else if (cmd.Name == "DisplayTemporaryDialog")
                    {
                        int msg;

                        if (!int.TryParse(commandDisassembly[1], out msg))
                            msg = mMessageList.FindIndex(x => x.Name == commandDisassembly[1]);

                        commandDisassembly[1] = msg.ToString();
                    }

                    if (cmd.Name == "func_F1E5")
                    {

                    }

                    for (int i = 0; i < cmd.ParameterCount; i++)
                        cmd.Variables[i].SetValue(commandDisassembly[i + 1], enums);

                    activeList.Add(cmd);
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

                // Write command stream 1
                writer.Write((int)0);
                long curOffset = writer.BaseStream.Position;

                foreach (Command com in mCommandList_1)
                    com.WriteBinary(writer, null);

                long size = writer.BaseStream.Position - curOffset;
                writer.BaseStream.Seek(curOffset - 4, SeekOrigin.Begin);
                writer.Write((int)size / 4);

                writer.Seek(0, SeekOrigin.End);

                // Write command stream 2
                writer.Write((int)0);
                curOffset = writer.BaseStream.Position;

                foreach (Command com in mCommandList_2)
                    com.WriteBinary(writer, null);

                size = writer.BaseStream.Position - curOffset;
                writer.BaseStream.Seek(curOffset - 4, SeekOrigin.Begin);
                writer.Write((int)size / 4);

                writer.Seek(0, SeekOrigin.End);

                writer.Write((int)mChoices.Count);

                foreach (string[] strArray in mChoices)
                {
                    writer.Write((int)strArray.Length);

                    foreach (string str in strArray)
                    {
                        byte[] chars = MessageDataProcessor.EncodeString(str);
                        writer.Write((int)chars.Length + 1);
                        writer.Write(chars);
                        writer.Write((byte)0);
                        MessageDataProcessor.PadMessageWriter(writer);
                    }
                }
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
                strWriter.WriteLine("# stream 1");

                foreach (Command com in mCommandList_1)
                {
                    if (com.Name == "CreatePosition" || com.Name == "BeginBlock")
                        strWriter.WriteLine();
                    if (com.Name == "SetPlayerChoices" || com.Name == "func_F0F8")
                    {
                        com.WriteString(strWriter, enums);
                        strWriter.WriteLine();
                        strWriter.Write("# choices ");

                        int choicesIndex = com.Variables[1].Value;

                        for (int i = 0; i < mChoices[choicesIndex].Length; i++)
                            strWriter.Write($"\"{ mChoices[choicesIndex][i] }\" ");

                        strWriter.WriteLine();
                        continue;
                    }
                    if (com.Name == "DisplayDialogSeries")
                    {
                        strWriter.Write($"{ com.Name } ");
                        strWriter.Write($"{ mMessageList[com.Variables[0].Value].Name } ");
                        strWriter.Write($"{ mMessageList[com.Variables[1].Value].Name } \n");

                        for (int i = com.Variables[0].Value; i <= com.Variables[1].Value; i++)
                            mMessageList[i].IsUsed = "True";
                        continue;
                    }
                    if (com.Name == "DisplayTemporaryDialog")
                    {
                        strWriter.Write($"{ com.Name } ");
                        strWriter.Write($"{ mMessageList[com.Variables[0].Value].Name } \n");
                        mMessageList[com.Variables[0].Value].IsUsed = "True";
                        continue;
                    }
                    else
                        com.WriteString(strWriter, enums);
                }

                strWriter.WriteLine();
                strWriter.WriteLine("# stream 2");

                foreach (Command com in mCommandList_2)
                {
                    if (com.Name == "CreatePosition" || com.Name == "BeginBlock")
                        strWriter.WriteLine();
                    if (com.Name == "SetPlayerChoices")
                    {
                        com.WriteString(strWriter, enums);
                        strWriter.WriteLine();
                        strWriter.Write("# choices ");

                        int choicesIndex = com.Variables[1].Value;

                        for (int i = 0; i < mChoices[choicesIndex].Length; i++)
                            strWriter.Write($"\"{ mChoices[choicesIndex][i] }\" ");

                        strWriter.WriteLine();
                        continue;
                    }
                    if (com.Name == "DisplayDialogSeries")
                    {
                        strWriter.Write($"{ com.Name } ");
                        strWriter.Write($"{ mMessageList[com.Variables[0].Value].Name } ");
                        strWriter.Write($"{ mMessageList[com.Variables[1].Value].Name } \n");

                        for (int i = com.Variables[0].Value; i <= com.Variables[1].Value; i++)
                            mMessageList[i].IsUsed = "True";
                    }
                    if (com.Name == "DisplayTemporaryDialog")
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
