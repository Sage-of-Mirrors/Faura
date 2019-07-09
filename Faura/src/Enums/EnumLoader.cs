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
            metaEnums.Add(Metafalica.CharacterID.ADVENTURER_A);
            metaEnums.Add(Metafalica.PortraitPosition.CENTERED);
            metaEnums.Add(Metafalica.PortraitSlot.LEFT1);
            metaEnums.Add(Metafalica.TextBoxType.NORMAL_DIALOG);
            metaEnums.Add(Metafalica.MusicID.dummy);
            metaEnums.Add(Metafalica.SoundID.dummy);
            metaEnums.Add(Metafalica.PortraitID.NONE);
            metaEnums.Add(Metafalica.EnemyID.dummy);
            metaEnums.Add(Metafalica.TextboxMode.SINGLE_TEXTBOX);
            metaEnums.Add(Metafalica.VisibilityEnum.INVISIBLE);
            metaEnums.Add(Metafalica.GraphicsMode.COSMOSPHERE);

            return metaEnums.ToArray();
        }
    }
}
