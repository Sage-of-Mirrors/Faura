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
        private static Encoding Ascii = Encoding.ASCII;

        private static ushort[] halftofull = new ushort[95] {
        0x8140, 0x8149, 0x8168, 0x8194, 0x8190, 0x8193, 0x8195, 0x8166,
        0x8169, 0x816A, 0x8196, 0x817B, 0x8143, 0x817C, 0x8144, 0x815E,
        0x824F, 0x8250, 0x8251, 0x8252, 0x8253, 0x8254, 0x8255, 0x8256,
        0x8257, 0x8258, 0x8146, 0x8147, 0x8183, 0x8181, 0x8184, 0x8148,
        0x8197, 0x8260, 0x8261, 0x8262, 0x8263, 0x8264, 0x8265, 0x8266,
        0x8267, 0x8268, 0x8269, 0x826A, 0x826B, 0x826C, 0x826D, 0x826E,
        0x826F, 0x8270, 0x8271, 0x8272, 0x8273, 0x8274, 0x8275, 0x8276,
        0x8277, 0x8278, 0x8279, 0x816D, 0x818F, 0x816E, 0x814F, 0x8151,
        0x8165, 0x8281, 0x8282, 0x8283, 0x8284, 0x8285, 0x8286, 0x8287,
        0x8288, 0x8289, 0x828A, 0x828B, 0x828C, 0x828D, 0x828E, 0x828F,
        0x8290, 0x8291, 0x8292, 0x8293, 0x8294, 0x8295, 0x8296, 0x8297,
        0x8298, 0x8299, 0x829A, 0x816F, 0x8162, 0x8170, 0x8160
        };

        public static string DecodeBytes(byte[] messageData)
        {
            //byte[] convData = Encoding.Convert(ShiftJis, Ascii, messageData);
            string initialDecoding = ShiftJis.GetString(messageData).Replace('’', '\'');
            
            if (initialDecoding.Contains("CLYL"))
            {
                initialDecoding = initialDecoding.Replace("CLYL", "<YLW>");
            }

            if (initialDecoding.Contains("CLEG"))
            {
                initialDecoding = initialDecoding.Replace("CLEG", "<GRN>");
            }

            if (initialDecoding.Contains("CLRE"))
            {
                initialDecoding = initialDecoding.Replace("CLRE", "<RED>");
            }

            if (initialDecoding.Contains("CLR1"))
            {
                initialDecoding = initialDecoding.Replace("CLR1", "<RD2>");
            }

            if (initialDecoding.Contains("CLBR"))
            {
                initialDecoding = initialDecoding.Replace("CLBR", "<BRN>");
            }

            if (initialDecoding.Contains("CLBL"))
            {
                initialDecoding = initialDecoding.Replace("CLBL", "<BLU>");
            }

            if (initialDecoding.Contains("CLNR"))
            {
                initialDecoding = initialDecoding.Replace("CLNR", "<WHT>");
            }

            if (initialDecoding.Contains("#0"))
            {
                initialDecoding = initialDecoding.Replace("#0", "<HYM>");
            }

            if (initialDecoding.Contains("#1"))
            {
                initialDecoding = initialDecoding.Replace("#1", "<FT2>");
            }

            if (initialDecoding.Contains("##"))
            {
                initialDecoding = initialDecoding.Replace("##", "<NRM>");
            }

            if (initialDecoding.Contains("CR"))
            {
                initialDecoding = initialDecoding.Replace("CR", "<BR>");
            }

            return initialDecoding.Trim('\0').Normalize(NormalizationForm.FormKC);
        }

        public static byte[] EncodeString(string messageData)
        {
            List<byte> stringBytes = new List<byte>();

            for (int i = 0; i < messageData.Length; i++)
            {
                // If this char isn't <, then we just convert it and put it into the buffer
                if (messageData[i] != '<')
                {
                    byte[] charBytes = BitConverter.GetBytes(halftofull[messageData[i] - 32]);
                    stringBytes.AddRange(new byte[] { charBytes[1], charBytes[0] });

                    continue;
                }

                // Get the control code's chars
                List<char> ctrlCode = new List<char>();
                int curPos = i + 1;
                while (messageData[curPos] != '>')
                {
                    ctrlCode.Add(messageData[curPos]);
                    curPos++;
                }

                // Make a string from the chars for easy comparison
                string code = new string(ctrlCode.ToArray());
                // Remove the code from the message data, we're done reading it
                messageData = messageData.Remove(i, code.Length + 2);

                switch (code)
                {
                    case "YLW":
                        stringBytes.AddRange(Ascii.GetBytes("CLYL"));
                        break;
                    case "GRN":
                        stringBytes.AddRange(Ascii.GetBytes("CLEG"));
                        break;
                    case "RED":
                        stringBytes.AddRange(Ascii.GetBytes("CLRE"));
                        break;
                    case "RD2":
                        stringBytes.AddRange(Ascii.GetBytes("CLR1"));
                        break;
                    case "WHT":
                        stringBytes.AddRange(Ascii.GetBytes("CLNR"));
                        break;
                    case "BRN":
                        stringBytes.AddRange(Ascii.GetBytes("CLBR"));
                        break;
                    case "BLU":
                        stringBytes.AddRange(Ascii.GetBytes("CLBL"));
                        break;
                    case "BR":
                        stringBytes.AddRange(Ascii.GetBytes("CR"));
                        break;
                    case "HYM":
                    case "FT2":
                        if (code == "HYM")
                        {
                            // Add Hymmnos code, #0
                            stringBytes.Add((byte)'#');
                            stringBytes.Add((byte)'0');
                        }
                        else if (code == "FT2")
                        {
                            // Add font 2 code, #1
                            stringBytes.Add((byte)'#');
                            stringBytes.Add((byte)'1');
                        }

                        // Copy phrase into the buffer until we hit <, the start of <NRM> which returns the text to normal font
                        curPos = i;
                        while (messageData[curPos] != '<')
                        {
                            stringBytes.Add((byte)messageData[curPos]);
                            curPos++;
                        }

                        // Add normal font code, ##
                        stringBytes.Add((byte)'#');
                        stringBytes.Add((byte)'#');

                        // Remove <NRM> tag, we don't need it
                        messageData = messageData.Remove(curPos, 5);
                        i = curPos;
                        break;
                    default:
                        break;
                }

                i--;
            }

            stringBytes.Add(0);
            return stringBytes.ToArray();

            /*
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

            return ShiftJis.GetBytes(wideMsg);*/
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
