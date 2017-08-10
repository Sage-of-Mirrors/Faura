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
            //byte[] convData = Encoding.Convert(ShiftJis, Ascii, messageData);
            string initialDecoding = ShiftJis.GetString(messageData).Normalize(NormalizationForm.FormKC);

            return initialDecoding;
        }

        public static byte[] EncodeString(string messageData)
        {
            return new byte[1];
        }
    }
}
