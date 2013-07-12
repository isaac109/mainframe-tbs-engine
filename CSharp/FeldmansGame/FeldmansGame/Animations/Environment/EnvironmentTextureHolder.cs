using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Mainframe.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Mainframe.Animations.Environment
{
    public class EnvironmentTextureHolder
    {
        protected static EnvironmentTextureHolder ETholder;

        protected Sprite[] mainHexSprites, minimapHexSprites, wallSprites, minimapWallSprites;

        protected const int numHexSprites = 3,
            numWallSprites = 0;

        /// <summary>
        /// Constructs the initial TextureHolder for all environment sprites, such as grid spaces and walls.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager object.</param>
        /// <returns></returns>
        public static EnvironmentTextureHolder makeHolder(GraphicsDevice graphics)
        {
            return ETholder = new EnvironmentTextureHolder(graphics);
        }

        /// <summary>
        /// Creates the array to hold the Environment sprites, then the sprites themselves.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager object.</param>
        protected EnvironmentTextureHolder(GraphicsDevice graphics)
        {
            //Notes: Accessing these arrays and directly pulling the value will result in all objects with the same sprite animating together.
            //To avoid this, instead call array[position].copySprite(), which returns a new Sprite object separate from the stored one. This prevents overwriting.
            mainHexSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.gridSpace).Count];
            minimapHexSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.gridSpace).Count];
            wallSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.Wall).Count];
            minimapWallSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.Wall).Count];

            int hexIndex = 0, wallIndex = 0;
            ConstantHolder.GridSpaceTypeDict.Clear();
            ConstantHolder.WallImageTypeDict.Clear();

            foreach (TextureXML tex in ConstantHolder.textureLoader.textureCategories[(int)TextureTypes.gridSpace])
            {
                ConstantHolder.GridSpaceTypeDict.Add(tex.fileName, hexIndex);
                loadHexagonImageData(graphics, hexIndex++, tex.fileName, tex.mainSize.vec, tex.ColumnHeights, tex.minimapName, tex.minimapSize.vec, tex.MinimapColumnHeights);
            }
            foreach (TextureXML tex in ConstantHolder.textureLoader.textureCategories[(int)TextureTypes.Wall])
            {
                ConstantHolder.WallImageTypeDict.Add(tex.fileName, wallIndex);
                loadWallImageData(graphics, wallIndex++, tex.fileName, tex.mainSize.vec, tex.ColumnHeights, tex.minimapName, tex.minimapSize.vec, tex.MinimapColumnHeights);
            }
        }

        /// <summary>
        /// Loads in the data for a hexagon/gridSpace's sprites on both the main grid and the minimap.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager</param>
        /// <param name="arrayPosition">Point in the Hexagon arrays to place these sprites.</param>
        /// <param name="assetNameMain">Name of the asset to use on the grid, located in the Environment/Hexagons/Main folder.</param>
        /// <param name="spriteSizeMain">Size of a single frame in the main animation</param>
        /// <param name="columnNumberArrayMain">Number of frames in each column of the main spritesheet.</param>
        /// <param name="assetNameMinimap">Name of the asset to use on the minimap, located in the Environment/Hexagons/Minimap folder.</param>
        /// <param name="spriteSizeMinimap">Size of a single frame in the minimap animation.</param>
        /// <param name="columnNumberArrayMinimap">Number of frames in each column of the minimap spritesheet.</param>
        private void loadHexagonImageData(GraphicsDevice graphics, int arrayPosition, string assetNameMain, Vector2 spriteSizeMain, int[] columnNumberArrayMain, string assetNameMinimap, Vector2 spriteSizeMinimap, int[] columnNumberArrayMinimap)
        {
            mainHexSprites[arrayPosition] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\Environment\\Hexagons\\Main\\" + assetNameMain + ".png", FileMode.Open)) ,
                spriteSizeMain,
                columnNumberArrayMain);
            minimapHexSprites[arrayPosition] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\Environment\\Hexagons\\Minimap\\" + assetNameMinimap + ".png", FileMode.Open)),
                spriteSizeMinimap,
                columnNumberArrayMinimap);
        }

        /// <summary>
        /// Creates and adds to the proper arrays the sprites for a wall in both the main view and the minimap.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager</param>
        /// <param name="arrayPosition">Point in the Wall arrays to place the new sprites.</param>
        /// <param name="assetNameMain">Name of the asset for the main grid view, in the Environment/Walls/Main folder.</param>
        /// <param name="spriteSizeMain">Size of a single frame in the main sprite.</param>
        /// <param name="columnNumberArrayMain">Number of frames in each column of the main sprite.</param>
        /// <param name="assetNameMinimap">Name of the asset for the minimap view, in the Environment/Walls/Minimap folder.</param>
        /// <param name="spriteSizeMinimap">Size of a single frame in the minimap sprite.</param>
        /// <param name="columnNumberArrayMinimap">Number of frames in each column of the minimap sprite.</param>
        private void loadWallImageData(GraphicsDevice graphics, int arrayPosition, string assetNameMain, Vector2 spriteSizeMain, int[] columnNumberArrayMain, string assetNameMinimap, Vector2 spriteSizeMinimap, int[] columnNumberArrayMinimap)
        {
            wallSprites[arrayPosition] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\Environment\\Walls\\Main\\" + assetNameMain + ".png", FileMode.Open)),
                spriteSizeMain, columnNumberArrayMain);
            minimapWallSprites[arrayPosition] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\Environment\\Walls\\Minimap\\" + assetNameMinimap + ".png", FileMode.Open)),
                spriteSizeMinimap, columnNumberArrayMinimap);
        }

        /// <summary>
        /// Main access point for all arrays holding environment assets.
        /// </summary>
        public static EnvironmentTextureHolder Holder
        {
            get { return ETholder; }
        }

        /// <summary>
        /// Array of grid space sprites to display in the main view.
        /// </summary>
        public Sprite[] MainHexagonSprites
        {
            get { return mainHexSprites; }
        }

        /// <summary>
        /// Array of grid space sprites to display on the minimap
        /// </summary>
        public Sprite[] MinimapHexagonSprites
        {
            get { return minimapHexSprites; }
        }

        /// <summary>
        /// Array of Wall sprites to display in the main view
        /// </summary>
        public Sprite[] WallSprites
        {
            get { return wallSprites; }
        }

        /// <summary>
        /// Array of Wall sprites to display on the minimap.
        /// </summary>
        public Sprite[] MinimapWallSprites
        {
            get { return minimapWallSprites; }
        }
    }
}