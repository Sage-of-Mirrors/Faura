using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFormatReader.Common;
using Newtonsoft.Json;

namespace Faura.src.Parsing
{
    public abstract class GustEventParser
    {
        protected Dictionary<uint, string> TextboxTypes;
        protected Dictionary<uint, string> SpriteIDs;
        protected Dictionary<uint, string> CharacterNameIDs;
        protected Dictionary<uint, string> CharacterEventIDs;
        protected Dictionary<uint, string> PortraitIDs;
        protected Dictionary<uint, string> PortraitPositionIDs;
        protected Dictionary<uint, string> MovieIDs;
        protected Dictionary<uint, string> BackgroundIDs;
        protected Dictionary<uint, string> MusicIDs;

        public abstract Event ParseRawData(string filePath);
        public abstract void WriteRawData(string filePath, Event ev);

        public abstract Event ParseTextData(string filePath);
        public abstract Event WriteTextData(string filePath, Event ev);

        protected EndianBinaryReader GetReader(string fileName)
        {
            MemoryStream memory = new MemoryStream();

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                stream.CopyTo(memory);
            }

            EndianBinaryReader reader = new EndianBinaryReader(memory, Endian.Little);
            reader.BaseStream.Position = 0;
            return reader;
        }

        protected void LoadDictionaries(string profileDirPath)
        {
            string[] profiles = Directory.GetFiles(profileDirPath);

            foreach (string str in profiles)
            {
                switch (Path.GetFileNameWithoutExtension(str).ToLowerInvariant())
                {
                    case "textboxtypes":
                        TextboxTypes = JsonConvert.DeserializeObject<Dictionary<uint, string>>(File.ReadAllText(str));
                        break;
                    case "characternames":
                        CharacterNameIDs = JsonConvert.DeserializeObject<Dictionary<uint, string>>(File.ReadAllText(str));
                        break;
                    case "spriteids":
                        SpriteIDs = JsonConvert.DeserializeObject<Dictionary<uint, string>>(File.ReadAllText(str));
                        break;
                    case "portraitpositions":
                        PortraitPositionIDs = JsonConvert.DeserializeObject<Dictionary<uint, string>>(File.ReadAllText(str));
                        break;
                    case "portraitids":
                        PortraitIDs = JsonConvert.DeserializeObject<Dictionary<uint, string>>(File.ReadAllText(str));
                        break;
                }
            }
        }
    }
}
