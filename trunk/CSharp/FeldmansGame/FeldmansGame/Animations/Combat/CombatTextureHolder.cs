using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Mainframe.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Mainframe.Animations.Combat
{
    public class CombatTextureHolder
    {
        protected static CombatTextureHolder CTHolder;

        protected Sprite[] abilityButtonSprites, 
            abilityAnimationSprites,
            abilityImpactSprites,
            abilityEffectSprites, 
            effectIconSprites;

        protected const int numABS = 0,         //Size to make the AbilityButtonSprite array.
            numAAS = 0,                         //Size to make the AbilityAnimationSprite array.
            numAIS = 0,                         //Size to make the AbilityImpactSprite array.
            numAES = 0,                         //Size to make the AbilityEffectSprite array.
            numEIS = 0;                         //Size to make the EffectIconSprite array.

        /// <summary>
        /// Constructs the CombatTextureHolder which will be accessable in static by the public.
        /// Holds all Ability and AbilityEffect related sprites.
        /// Should be called from the game's main class, in the LoadContent() function.
        /// </summary>
        /// <param name="manager">Content loader from the main game class.</param>
        /// <returns>The CTH created by the constructor.</returns>
        public static CombatTextureHolder makeHolder(GraphicsDevice graphics)
        {
            return CTHolder = new CombatTextureHolder(graphics);
        }

        /// <summary>
        /// Creates the CombatTextureHolder's arrays, and all the members in them.
        /// </summary>
        /// <param name="manager">Main game class's Content Loader.</param>
        protected CombatTextureHolder(GraphicsDevice graphics)
        {
            int numAbilities = ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.Ability).Count;
            int numAbilityEffects = ConstantHolder.textureLoader.getTexturesByType((int)TextureTypes.AbilityEffect).Count;
            abilityButtonSprites = new Sprite[numAbilities];
            abilityAnimationSprites = new Sprite[numAbilities];
            abilityEffectSprites = new Sprite[numAbilityEffects];
            abilityImpactSprites = new Sprite[numAbilities];
            effectIconSprites = new Sprite[numAbilityEffects];

            int abilityIndex = 0;

            foreach (TextureXML tex in ConstantHolder.textureLoader.textureCategories[(int)TextureTypes.Ability])
            {
                ConstantHolder.AbilityButtonDict.Add(tex.fileName, abilityIndex);
                //For abilities, portrait = impact
                loadAbilityAnimation(graphics, abilityIndex++, tex.fileName, tex.mainSize.vec, tex.ColumnHeights,
                                     tex.portraitName, tex.portraitSize.vec, tex.PortraitColumnHeights,
                                     tex.buttonName, tex.buttonSize.vec, tex.ButtonColumnHeights);
            }

            int effectIndex = 0;

            foreach (TextureXML tex in ConstantHolder.textureLoader.textureCategories[(int)TextureTypes.AbilityEffect])
            {
                ConstantHolder.AbilityEffectSpriteDict.Add(tex.fileName, effectIndex);
                loadEffect(graphics, effectIndex++, tex.fileName, tex.mainSize.vec, tex.ColumnHeights,
                    tex.iconName, tex.iconSize.vec, tex.IconColumnHeights);
            }
        }

        /// <summary>
        /// Adds the sprite identified by the assetName parameter to the AbilityAnimationSprites array at the given index.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager</param>
        /// <param name="arrayIndex">point in the AAS array to put the created sprite.</param>
        /// <param name="assetName">Name of the asset name, no extension, in the Combat/Abilities/Animation folder.</param>
        /// <param name="spriteSize">Size of an individual frame.</param>
        /// <param name="columnSizes">Array which holds the number of frames in each column.</param>
        protected void loadAbilityAnimation(GraphicsDevice graphics, int arrayIndex,
            String assetName, Vector2 spriteSize, int[] columnSizes,
            String buttonAssetName, Vector2 buttonSpriteSize, int[] buttonColumnSizes,
            String impactAssetName, Vector2 impactSpriteSize, int[] impactColumnSizes)
        {
            abilityAnimationSprites[arrayIndex] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\Combat\\Abilities\\Animation\\" + assetName, FileMode.Open)),
                spriteSize, columnSizes);
            abilityButtonSprites[arrayIndex] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\Combat\\Abilities\\Button\\" + assetName, FileMode.Open)),
                spriteSize, columnSizes);
            abilityImpactSprites[arrayIndex] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\Combat\\Abilities\\Impact\\" + assetName, FileMode.Open)),
                spriteSize, columnSizes);
        }

        /// <summary>
        /// Adds the sprite identified by the assetName parameter to the AbilityEffectSprites array at the given index.
        /// </summary>
        /// <param name="manager">Main game class's ContentManager</param>
        /// <param name="arrayIndex">point in the AES array to put the created sprite.</param>
        /// <param name="assetName">Name of the asset name, no extension, in the Combat/Effects/Animation folder.</param>
        /// <param name="spriteSize">Size of an individual frame.</param>
        /// <param name="columnSizes">Array which holds the number of frames in each column.</param>
        protected void loadEffect(GraphicsDevice graphics, int arrayIndex,
            String assetName, Vector2 spriteSize, int[] columnSizes,
            String iconName, Vector2 iconSize, int[] iconCNA)
        {
            FileStream fs = new FileStream("Content\\Combat\\Effects\\Animation\\" + assetName + ".png", FileMode.Open);
            abilityEffectSprites[arrayIndex] = new Sprite(
                Texture2D.FromStream(graphics, fs),
                spriteSize, columnSizes);
            effectIconSprites[arrayIndex] = new Sprite(
                Texture2D.FromStream(graphics, new FileStream("Content\\Combat\\Effects\\Icon\\" + iconName + ".png", FileMode.Open)),
                iconSize, iconCNA);
        }

        /// <summary>
        /// Access point for all sprite lists regarding Combat animations.
        /// </summary>
        public static CombatTextureHolder Holder
        {
            get { return CTHolder; }
        }

        /// <summary>
        /// Animations which will show in the grid, on top of persons who have the related effect on them.
        /// </summary>
        public Sprite[] AbilityEffectAnimationSprites
        {
            get { return abilityEffectSprites; }
        }

        /// <summary>
        /// Icons which will show in the StatBox when the selected person has the related effect on him.
        /// </summary>
        public Sprite[] AbilityEffectIconSprites
        {
            get { return effectIconSprites; }
        }

        /// <summary>
        /// Animations which will play through the grid when an ability is launched.
        /// </summary>
        public Sprite[] AbilityAnimationSprites
        {
            get { return abilityAnimationSprites; }
        }

        /// <summary>
        /// Backgrounds for buttons which trigger abilities.
        /// </summary>
        public Sprite[] AbilityButtonSprites
        {
            get { return abilityButtonSprites; }
        }

        /// <summary>
        /// Animations which will play on a person when they are hit by an Ability.
        /// </summary>
        public Sprite[] AbilityImpactAnimationSprites
        {
            get { return abilityImpactSprites; }
        }
    }
}
