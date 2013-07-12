using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Constants;
using System.IO;

namespace Mainframe.Animations.Interface
{
    public class InterfaceTextureHolder
    {
        protected static InterfaceTextureHolder ITHolder;

        protected Sprite[] buttonSprites, guiElementSprites;

        protected SpriteFont[] fonts;

        protected const int numBS = 1,          //Number of Button backgrounds in the array.
            numFonts = 1;                       //Number of fonts the game will use.

        /// <summary>
        /// Sets up the InterfaceTextureHolder with all the necessary Sprites.
        /// This should be called in the main game class's LoadContent function.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager.</param>
        /// <returns>The holder created by this class.</returns>
        public static InterfaceTextureHolder makeHolder(ContentManager manager, GraphicsDevice graphics)
        {
            return ITHolder = new InterfaceTextureHolder(manager, graphics);
        }

        /// <summary>
        /// Sets about creating the arrays to hold all Interface sprites and fonts, then creates the sprites and fonts and stores them in those arrays.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager.</param>
        protected InterfaceTextureHolder(ContentManager manager, GraphicsDevice graphics)
        {
            buttonSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.ButtonBackgrounds).Count];
            guiElementSprites = new Sprite[ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.GUIMember).Count];
            fonts = new SpriteFont[numFonts];

#region GUI Sprites
            int GUIIndex = 0;

            foreach (TextureXML tex in ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.GUIMember))
            {
                ConstantHolder.GUIMemberImageDict.Add(tex.fileName, GUIIndex);
                addGUISprite(graphics, GUIIndex++, tex.fileName, tex.mainSize.vec, tex.ColumnHeights);
            }
            /*
            int[] cna0 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.MinimapBackground, "minimapBackground", new Vector2(660, 368), cna0);

            int[] cna1 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.AbilityBackground, "StatBox\\Paddle", new Vector2(1, 1), cna1);

            int[] cna2 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.AbilityFrame, "MinimapSelection", new Vector2(157, 77), cna2);

            int[] cna3 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.EnergyBar, "StatBox\\EnergyBar", new Vector2(415, 37), cna3);

            int[] cna4 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.ExperienceBar, "StatBox\\ExpBar", new Vector2(1117, 40), cna4);

            int[] cna5 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.HealthBar, "StatBox\\healthbar", new Vector2(555, 39), cna5);

            int[] cna6 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.MinimapSelection, "MinimapSelection", new Vector2(157, 77), cna6);

            int[] cna7 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.MouseOverBackground, "StatBox\\Paddle", new Vector2(1, 1), cna7);

            int[] cna8 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.MouseOverFrame, "MinimapSelection", new Vector2(157, 77), cna8);

            int[] cna9 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.Overlay, "MinimapSelection", new Vector2(157, 77), cna9);

            int[] cna10 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.PortraitBackground, "StatBox\\Paddle", new Vector2(1, 1), cna10);

            int[] cna11 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.PortraitFrame, "MinimapSelection", new Vector2(157, 77), cna11);

            int[] cna12 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.TeamBackground, "StatBox\\Paddle", new Vector2(1, 1), cna12);

            int[] cna13 = { 1 };
            addGUISprite(graphics, (int)ConstantHolder.GUIMemberImages.TeamFrame, "MinimapSelection", new Vector2(157, 77), cna13);
#endregion

#region Button Sprites
            int[] cnab0 = { 1, 2, 1 };
            addButtonSprite(graphics, (int)ConstantHolder.ButtonBackgrounds.Exit, "DefaultButton", new Vector2(90, 90), cnab0);*/
#endregion
            
#region Fonts
            addFont(manager, (int)ConstantHolder.Fonts.defaultFont, "SpriteFont1");
#endregion
        }

        /// <summary>
        /// Creates a Button's background sprite, and places it at the corresponding position in the ButtonSprite array.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager</param>
        /// <param name="index">Point in the ButtonSprite array to place this.</param>
        /// <param name="assetName">Name of the asset in the GUI/Buttons folder.</param>
        /// <param name="spriteSize">Size of an individual frame.</param>
        /// <param name="columnNumberArray">Number of frames in each column.</param>
        protected void addButtonSprite(GraphicsDevice graphics, int index, String assetName, Vector2 spriteSize, int[] columnNumberArray)
        {
            buttonSprites[index] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\GUI\\Buttons\\" + assetName + ".png", FileMode.Open)),
                spriteSize, 
                columnNumberArray);
        }

        /// <summary>
        /// Creates a elements's sprite, and places it at the corresponding position in the GUIElement array.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager</param>
        /// <param name="index">Point in the GUIElementSprite array to place this.</param>
        /// <param name="assetName">Path to and name of the asset in the GUI folder.</param>
        /// <param name="spriteSize">Size of an individual frame.</param>
        /// <param name="columnNumberArray">Number of frames in each column.</param>
        protected void addGUISprite(GraphicsDevice graphics, int index, String assetPath, Vector2 spriteSize, int[] columnNumberArray)
        {
            guiElementSprites[index] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\GUI\\" + assetPath + ".png", FileMode.Open)),
                spriteSize,
                columnNumberArray);
        }

        /// <summary>
        /// Loads a font into the Fonts array.
        /// </summary>
        /// <param name="manager">Main game ContentManager</param>
        /// <param name="index">Point in the Fonts array to place this font.</param>
        /// <param name="assetName">Name of the asset file in GU/Fonts</param>
        protected void addFont(ContentManager manager, int index, String assetName)
        {
            fonts[index] = manager.Load<SpriteFont>("GUI\\Fonts\\" + assetName);
        }

        /// <summary>
        /// Access point for all arrays of sprites and fonts in the GUI.
        /// </summary>
        public static InterfaceTextureHolder Holder
        {
            get { return ITHolder; }
        }

        /// <summary>
        /// Holder for all the non-button elements of the GUI, like frames, overlays, and backgrounds.
        /// </summary>
        public Sprite[] GUISprites
        {
            get { return guiElementSprites; }
        }

        /// <summary>
        /// Holder for all the button backgrounds to be used in the game.
        /// </summary>
        public Sprite[] ButtonSprites
        {
            get { return buttonSprites; }
        }

        /// <summary>
        /// Holder for all the SpriteFonts to be used in the game.
        /// </summary>
        public SpriteFont[] Fonts
        {
            get { return fonts; }
        }
    }
}
