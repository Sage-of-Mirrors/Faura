using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Faura.Messages
{
    public static class MessageDataProcessor
    {
        private static Encoding ShiftJis = Encoding.GetEncoding(932);
        private static Encoding Unicode = Encoding.Unicode;

        public static string DecodeBytes(byte[] messageData)
        {
            //byte[] convData = Encoding.Convert(ShiftJis, Ascii, messageData);
            string initialDecoding = ShiftJis.GetString(messageData).Replace('’', '\'');
            
            if (initialDecoding.Contains("CLYL"))
            {
                initialDecoding = initialDecoding.Replace("CLYL", "<CLY>");
            }

            if (initialDecoding.Contains("CLEG"))
            {
                initialDecoding = initialDecoding.Replace("CLEG", "<CLG>");
            }

            if (initialDecoding.Contains("CLRE"))
            {
                initialDecoding = initialDecoding.Replace("CLRE", "<CLR>");
            }

            if (initialDecoding.Contains("CLNR"))
            {
                initialDecoding = initialDecoding.Replace("CLNR", "<CLN>");
            }

            if (initialDecoding.Contains("#0"))
            {
                initialDecoding = initialDecoding.Replace("#0", "<HYM>");
                initialDecoding = initialDecoding.Replace("##", "<NRM>");
            }

            return initialDecoding.Trim('\0').Normalize(NormalizationForm.FormKC);
        }

        public static byte[] EncodeString(string messageData)
        {
            string msgCopy = messageData;
            List<KeyValuePair<int, string>> excisedControlCodes = new List<KeyValuePair<int, string>>();

            excisedControlCodes.AddRange(CheckControlCode(msgCopy, "<CLY>", out msgCopy));
            excisedControlCodes.AddRange(CheckControlCode(msgCopy, "<CLG>", out msgCopy));
            excisedControlCodes.AddRange(CheckControlCode(msgCopy, "<CLR>", out msgCopy));
            excisedControlCodes.AddRange(CheckControlCode(msgCopy, "<CLN>", out msgCopy));
            excisedControlCodes.AddRange(CheckControlCode(msgCopy, "<HYM>", out msgCopy));
            excisedControlCodes.AddRange(CheckControlCode(msgCopy, "<NRM>", out msgCopy));

            msgCopy.Replace('\'', '’');
            string wideMsg = Microsoft.VisualBasic.Strings.StrConv(msgCopy, VbStrConv.Wide, 17);

            for (int i =  excisedControlCodes.Count - 1; i > -1; i--)
            {
                KeyValuePair<int, string> keyVal = excisedControlCodes[i];
                wideMsg = wideMsg.Insert(keyVal.Key, keyVal.Value);
            }

            if (!wideMsg.EndsWith("\0"))
                wideMsg += '\0';

            return ShiftJis.GetBytes(wideMsg);
        }

        private static KeyValuePair<int, string>[] CheckControlCode(string message, string code, out string messageWithoutCodes)
        {
            List<KeyValuePair<int, string>> resultList = new List<KeyValuePair<int, string>>();

            messageWithoutCodes = message;

            if (!message.Contains(code))
            {
                return resultList.ToArray();
            }

            int instanceCount = message.Split(new string[] { code }, StringSplitOptions.None).Length - 1;

            for (int i = 0; i < instanceCount; i++)
            {
                int indexOfCode = message.IndexOf(code);

                KeyValuePair<int, string> newKey = new KeyValuePair<int, string>(indexOfCode, ControlTagToCode(code));
                resultList.Add(newKey);

                messageWithoutCodes = message.Remove(indexOfCode, code.Length);
            }

            return resultList.ToArray();
        }

        private static string ControlTagToCode(string code)
        {
            switch (code)
            {
                case "<CLY>":
                    return "CLYL";
                case "<CLG>":
                    return "CLEG";
                case "<CLR>":
                    return "CLRE";
                case "<CLN>":
                    return "CLNR";
                case "<HYM>":
                    return "#0";
                case "<NRM>":
                    return "##";
                default:
                    return "";
            }
        }

        public static Enum EnumValueFromString(Type source, string str)
        {
            // Some standardization, all uppercase + underscores
            string upperCase = str.ToUpper();
            upperCase.Replace(' ', '_');

            Enum value = (Enum)Enum.Parse(source, upperCase);

            return value;
        }
    }
}
