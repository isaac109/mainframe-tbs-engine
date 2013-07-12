using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mainframe.Constants;
using System.IO;
using System.IO.IsolatedStorage;

namespace Mainframe.Animations.Character
{
    public class ActorTextureHolder
    {
        protected static ActorTextureHolder ATholder;

        protected Sprite[] mainSprites, minimapSprites, portraitSprites;
        protected Sprite[,] ability_ActorSprites;   //2D array, Ability X ActorSprite


        /// <summary>
        /// Sets up the ActorTextureHolder, creating all contained sprites.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager, for loading in the data.</param>
        /// <returns>The ATH created to store the sprites.</returns>
        public static ActorTextureHolder makeHolder(GraphicsDevice graphics)
        {
            return ATholder = new ActorTextureHolder(graphics);
        }

        /// <summary>
        /// Loads in all the textures contained in this Holder and sets up the sprites with them, containing them in the proper arrays and positions.
        /// </summary>
        /// <param name="manager"></param>
        protected ActorTextureHolder(GraphicsDevice graphics)
        {
            mainSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.ActorMain).Count];
            minimapSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.ActorMain).Count];
            portraitSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.ActorMain).Count];
            ability_ActorSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.Ability).Count, 
                ConstantHolder.textureLoader.textureCategories[(int)TextureTypes.ActorMain].Count];
            int numPeople = 0;

            foreach (TextureXML tex in ConstantHolder.textureLoader.textureCategories[(int)TextureTypes.ActorMain])
            {
                ConstantHolder.ActorImageTypeDict.Add(tex.fileName, numPeople);
                loadActorImageData(graphics, numPeople++, tex.fileName, tex.mainSize.vec, tex.ColumnHeights, tex.minimapName, tex.minimapSize.vec, tex.MinimapColumnHeights, tex.portraitName, tex.portraitSize.vec, tex.PortraitColumnHeights);
            }
        }


        /// <summary>
        /// Loads assets in, and puts them in the proper array indices, for an actor, his minimap representation, and his portrait.
        /// </summary>
        /// <param name="manager">Content loader.</param>
        /// <param name="arrayPosition">Enumerator-based index in the arrays at which to store the loaded content</param>
        /// <param name="assetNameMain">Name of the asset in the Characters/Main folder</param>
        /// <param name="spriteSizeMain">Size of an individual sprite in the main spritesheet.</param>
        /// <param name="cNAMain">int array of the sizes of each column in the Main spritesheet.</param>
        /// <param name="assetNameMinimap">Name of the asset in the Characters/Minimap folder</param>
        /// <param name="spriteSizeMinimap">Size of an individual sprite in the minimap spritesheet.</param>
        /// <param name="cNAMinimap">int array of the sizes of each column in the Minimap spritesheet.</param>
        /// <param name="assetNamePortrait">Name of the asset in the Characters/Portrait folder</param>
        /// <param name="spriteSizePortrait">Size of an individual sprite in the portrait spritesheet.</param>
        /// <param name="cNAPortrait">int array of the sizes of each column in the portrait spritesheet.</param>
        protected void loadActorImageData(GraphicsDevice graphics, int arrayPosition, String assetNameMain, Vector2 spriteSizeMain, int[] cNAMain, String assetNameMinimap, Vector2 spriteSizeMinimap, int[] cNAMinimap, String assetNamePortrait, Vector2 spriteSizePortrait, int[] cNAPortrait)
        {
            FileStream fs = new FileStream("Content\\Characters\\Main\\" + assetNameMain + ".png", FileMode.Open);
            Texture2D mainSprite = Texture2D.FromStream(graphics, fs);
            mainSprites[arrayPosition] = new Sprite(
                mainSprite,
                spriteSizeMain,
                cNAMain);

            fs = new FileStream("Content\\Characters\\Minimap\\" + assetNameMinimap + ".png", FileMode.Open);
            Texture2D minimapSprite = Texture2D.FromStream(graphics, fs);
            minimapSprites[arrayPosition] = new Sprite(
                minimapSprite,
                spriteSizeMinimap,
                cNAMinimap);

            fs = new FileStream("Content\\Characters\\Portrait\\" + assetNamePortrait + ".png", FileMode.Open);
            Texture2D portraitSprite = Texture2D.FromStream(graphics, fs);
            portraitSprites[arrayPosition] = new Sprite(
                portraitSprite,
                spriteSizePortrait,
                cNAPortrait);
        }

        /// <summary>
        /// Accessor for the pre-built Holder, which has all Sprites created and ready to copy with copySprite();
        /// </summary>
        public static ActorTextureHolder Holder
        {
            get { return ATholder; }
        }

        /// <summary>
        /// Sprites to show on the main grid. To access, call MainSprites[index].copySprite(), in order to avoid object collision.
        /// </summary>
        public Sprite[] MainSprites
        {
            get { return mainSprites; }
        }

        /// <summary>
        /// Sprites to show on the minimap grid. To access, call MinimapSprites[index].copySprite(), in order to avoid object collision.
        /// </summary>
        public Sprite[] MinimapSprites
        {
            get { return minimapSprites; }
        }

        /// <summary>
        /// Sprites to show in the statbox when the unit is selected, or in the Team panel.
        /// To access, call PortraitSprites[index].copySprite(), in order to avoid object collision.
        /// </summary>
        public Sprite[] PortraitSprites
        {
            get { return portraitSprites; }
        }
    }
}
