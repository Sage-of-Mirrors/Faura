using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Faura.Commands
{
    public struct Variable
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public bool HasEnum { get; set; }
        public string EnumName { get; set; }

        public Variable(Variable src)
        {
            Name = src.Name;
            Value = src.Value;
            HasEnum = src.HasEnum;
            EnumName = src.EnumName;
        }

        public void SetValue(string value)
        {
            if (true)
            {
                Value = Convert.ToInt32(value);
                return;
            }
        }

        public void WriteString(StreamWriter writer, Enum[] enums)
        {
            if (true)
            {
                writer.Write($"{ Value } ");
                return;
            }

            string enumName = EnumName;
            Enum thisEnum = enums.First(x => x.GetType().Name == enumName);
            Array enumValues = Enum.GetValues(thisEnum.GetType());

            if (enumValues.GetLength(0) > Value)
            {
                writer.Write($"{ enumValues.GetValue(Value).ToString() } ");
            }
            else
            {
                writer.Write($"{ Value } ");
            }
        }
    }
}
