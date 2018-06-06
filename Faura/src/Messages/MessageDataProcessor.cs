using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faura
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
            return new byte[1];
        }
    }
}
