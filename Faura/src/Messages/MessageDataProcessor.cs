using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faura.src
{
    public static class MessageDataProcessor
    {
        private static Encoding ShiftJis = Encoding.GetEncoding(932);
        private static Encoding Ascii = Encoding.ASCII;

        public static string DecodeBytes(byte[] messageData)
        {
            string initialDecoding = ShiftJis.GetString(messageData).Normalize(NormalizationForm.FormKC);

            return ControlCodesToEscapeChars(initialDecoding);
        }

        public static byte[] EncodeString(string messageData)
        {
            return new byte[1];
        }

        private static string ControlCodesToEscapeChars(string normalizedData)
        {
            List<char> chars = new List<char>();

            int advanceAmount = 1;
            for (int i = 0; i < normalizedData.Length; i += advanceAmount)
            {
                switch (normalizedData[i])
                {
                    case 'C':
                        char[] processedCode = CheckCControlCode(normalizedData, i, out advanceAmount);
                        chars.AddRange(processedCode);
                        break;
                    case '#':
                        if (normalizedData[i + 1] == '0')
                        {
                            chars.AddRange("\\hym".ToArray());
                            advanceAmount = 2;
                        }
                        else if (normalizedData[i + 1] == '#')
                        {
                            chars.AddRange("\\eng".ToArray());
                            advanceAmount = 2;
                        }
                        break;
                    case '’':
                        chars.Add('\'');
                        break;
                    default:
                        chars.Add(normalizedData[i]);
                        advanceAmount = 1;
                        break;
                }
            }

            return new string(chars.ToArray());
        }

        private static char[] CheckCControlCode(string mesData, int index, out int advanceAmount)
        {
            List<Char> tempList = new List<char>();

            // Carriage return (new line)
            if (mesData[index + 1] == 'R')
            {
                tempList.Add('\n');
                advanceAmount = 2;
            }
            // Color. We have to figure out which though
            else if (mesData[index + 1] == 'L')
            {
                string lastTwoChars = mesData.Substring(index + 2, 2);

                switch (lastTwoChars)
                {
                    case "NR":
                        tempList.AddRange("\\nrm".ToArray());
                        advanceAmount = 4;
                        break;
                    case "YL":
                        tempList.AddRange("\\ylw".ToArray());
                        advanceAmount = 4;
                        break;
                    case "EG":
                        tempList.AddRange("\\grn".ToArray());
                        advanceAmount = 4;
                        break;
                    case "RE":
                        tempList.AddRange("\\red".ToArray());
                        advanceAmount = 4;
                        break;
                    default:
                        throw new Exception($"Unknown color { lastTwoChars }!");
                }
            }
            else
            {
                tempList.Add(mesData[index]);
                advanceAmount = 1;
            }

            return tempList.ToArray();
        }

        private static byte[] EscapeCharsToControlCodes(string normalizedData)
        {
            return new byte[1];
        }
    }
}
