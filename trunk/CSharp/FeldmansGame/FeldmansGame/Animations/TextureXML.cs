using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using System.Xml.Serialization;

namespace Mainframe.Animations
{
    public enum TextureTypes : int
    {
        gridSpace,
        Wall,
        ActorMain,
        CharacterAbilityCombo,
        AbilityEffect,
        Ability,
        GUIMember,
        ButtonBackgrounds
    }

    public class TextureListXML
    {
        public List<TextureXML> textures;
        public List<List<TextureXML>> textureCategories;

        public TextureListXML()
        {
            textures = new List<TextureXML>();
        }

        public static TextureListXML loadTextureXMLs(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(TextureListXML));
            return (TextureListXML)xml.Deserialize(fs);
        }

        public List<TextureXML> getTexturesByType(int type)
        {
            if (textureCategories != null)
            {
                textureCategories = new List<List<TextureXML>>();
                for (int i = 0; i <= (int)TextureTypes.ButtonBackgrounds; ++i)
                {
                    textureCategories.Add(new List<TextureXML>());
                }
                foreach (TextureXML tex in textures)
                {
                    textureCategories[tex.texCategory].Add(tex);
                }
            }
            return textureCategories[type];
        }
    }

    public class TextureXML
    {
        public List<intXML> columnHeights, minimapColumnHeights, portraitColumnHeights, buttonColumnHeights, iconColumnHeights;
        public string fileName, minimapName, portraitName, buttonName, iconName;
        public v2XML mainSize, minimapSize, portraitSize, buttonSize, iconSize;
        public int texCategory;

        public TextureXML()
        {
            columnHeights = new List<intXML>();
            minimapColumnHeights = new List<intXML>();
            portraitColumnHeights = new List<intXML>();
            buttonColumnHeights = new List<intXML>();
            iconColumnHeights = new List<intXML>();
        }

        public int[] ColumnHeights
        {
            get
            {
                int[] toRet = new int[columnHeights.Count];
                for (int i = 0; i < columnHeights.Count; ++i)
                {
                    toRet[i] = columnHeights[i].value;
                }
                return toRet;
            }
        }

        public int[] MinimapColumnHeights
        {
            get
            {
                int[] toRet = new int[minimapColumnHeights.Count];
                for (int i = 0; i < minimapColumnHeights.Count; ++i)
                {
                    toRet[i] = minimapColumnHeights[i].value;
                }
                return toRet;
            }
        }

        public int[] PortraitColumnHeights
        {
            get
            {
                int[] toRet = new int[portraitColumnHeights.Count];
                for (int i = 0; i < portraitColumnHeights.Count; ++i)
                {
                    toRet[i] = portraitColumnHeights[i].value;
                }
                return toRet;
            }
        }

        public int[] ButtonColumnHeights
        {
            get
            {
                int[] toRet = new int[portraitColumnHeights.Count];
                for (int i = 0; i < portraitColumnHeights.Count; ++i)
                {
                    toRet[i] = portraitColumnHeights[i].value;
                }
                return toRet;
            }
        }

        public int[] IconColumnHeights
        {
            get
            {
                int[] toRet = new int[portraitColumnHeights.Count];
                for (int i = 0; i < portraitColumnHeights.Count; ++i)
                {
                    toRet[i] = portraitColumnHeights[i].value;
                }
                return toRet;
            }
        }
    }

    public class intXML
    {
        public int value;
    }

    public class v2XML
    {
        public int x, y;

        public Vector2 vec
        {
            get { return new Vector2(x, y); }
        }
    }
}