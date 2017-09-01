using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Faura.src.Parsing.Elemia
{
    public partial class ElemiaEventParser : GustEventParser
    {
        public ElemiaEventParser()
        {
            string profilePath = $"{ AppDomain.CurrentDomain.BaseDirectory }resources\\profiles\\Elemia";

            if (!Directory.Exists(profilePath))
                throw new Exception($"Elemia profile directory not found! Should have been at { profilePath }");
            else if (Directory.GetFiles(profilePath).Length == 0)
                throw new Exception($"Elemia profile directory is empty! Path is { profilePath }");

            LoadDictionaries(profilePath);
        }
    }
}
