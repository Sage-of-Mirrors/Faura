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

        public void SetValue(string value, Enum[] enums)
        {
            int dummy = 0;
            if (int.TryParse(value, out dummy))
            {
                Value = dummy;
                return;
            }

            if (!HasEnum)
                throw new Exception($"Non-numerical string found for variable { Name } with no enum!");

            string enumName = EnumName;
            Enum thisEnum = enums.First(x => x.GetType().Name == enumName);
            Enum enumVal = (Enum)Enum.Parse(thisEnum.GetType(), value);

            Value = Convert.ToInt32(enumVal);
        }

        public void WriteString(StreamWriter writer, Enum[] enums)
        {
            if (!HasEnum)
            {
                writer.Write($"{ Value } ");
                return;
            }

            string enumName = EnumName;
            Enum thisEnum = enums.First(x => x.GetType().Name == enumName);
            Array enumValues = Enum.GetValues(thisEnum.GetType());

            if (enumValues.GetLength(0) > Value)
            {
                writer.Write($"{ Enum.ToObject(thisEnum.GetType(), Value) } ");
            }
            else
            {
                writer.Write($"{ Value } ");
            }
        }
    }
}
