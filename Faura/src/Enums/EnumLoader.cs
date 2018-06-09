using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faura.Enums
{
    public static class EnumLoader
    {
        public static Enum[] GetEnumsFromVersion(string version)
        {
            switch (version)
            {
                case "metafalica":
                    return GetMetafalicaEnums();
            }

            return null;
        }

        private static Enum[] GetMetafalicaEnums()
        {
            List<Enum> metaEnums = new List<Enum>();

            metaEnums.Add(Metafalica.CharacterNameID.LUCA);
            metaEnums.Add(Metafalica.PortraitPosition.CENTERED);
            metaEnums.Add(Metafalica.PortraitSlot.LEFT1);
            metaEnums.Add(Metafalica.TextBoxType.NORMALDIALOG);

            return metaEnums.ToArray();
        }
    }
}
