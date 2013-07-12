using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mainframe.Animations;

namespace Mainframe.Constants
{
    /// <summary>
    /// Maintains all important values for game logic, such as graphics sizing, texture analysis, game pacing, and ability balance.
    /// </summary>
    public static class ConstantHolder
    {
#region enumerators

        public enum Fonts
        {
            defaultFont = 0,
        }

        public static Dictionary<string, int> GridSpaceTypeDict = new Dictionary<string, int>(),
                                              WallImageTypeDict = new Dictionary<string, int>(),
                                              ActorImageTypeDict = new Dictionary<string, int>(),
                                              HeroClassDict = new Dictionary<string, int>(),
                                              GUIMemberImageDict = new Dictionary<string, int>(),
                                              ButtonBackgroundDict = new Dictionary<string, int>(),
                                              AbilityEffectIconDict = new Dictionary<string, int>(),
                                              AbilityEffectSpriteDict = new Dictionary<string, int>(),
                                              AbilityImpactDict = new Dictionary<string, int>(),
                                              AbilityProjectileDict = new Dictionary<string, int>(),
                                              AbilityButtonDict = new Dictionary<string, int>(),
                                              AbilityGraphicsDict = new Dictionary<string,int>();

        public static TextureListXML textureLoader;

        public enum ActorImageTypes
        {
            BaseActor = 0,
            BaseActor2 = 1,
        };

        public enum HeroClasses
        {
            Cyber_Soldier,
            War_Tech,
            Hacker,
            Engineer,
            Combat_Tech,
            Programmer,
            AI_Shadow,
            AI_Destroyer,
            AI_Paladin
        }

        public enum GUIMemberImages
        {
            MinimapBackground = 0,
            MinimapSelection = 1,
            PortraitBackground,
            PortraitFrame,
            AbilityBackground,
            AbilityFrame,
            MouseOverBackground,
            MouseOverFrame,
            HealthBar,
            EnergyBar,
            ExperienceBar,
            TeamBackground,
            TeamFrame,
            Overlay,
        }

        public enum ButtonBackgrounds
        {
            Exit = 0,
        }

        public enum Abilities
        {
            Attack = 0,
        }

        public enum AbilityEffectIcons
        {
            Burning = 0,
        }

        public enum AbilityEffectGraphics
        {
            Burning = 0,
        }

        public enum ProjectileGraphics
        {
            Arrow = 0,
        }
#endregion

#region GraphicsSizing/Placement
        /// <summary>
        /// Height in pixels of the game screen.
        /// </summary>
        public static int GAME_HEIGHT = 800;
       
        /// <summary>
        /// Width in pixels of the game screen.
        /// </summary>
        public static int GAME_WIDTH = 1080;

        /// <summary>
        /// Number of columns in the default level grid. TBI: Level loading and saving
        /// </summary>
        public static int GRIDSPACES_X = 45;
        
        /// <summary>
        /// Number of rows in the default level grid. TBI: Level loading and saving.
        /// </summary>
        public static int GRIDSPACES_Y = 35;

        /// <summary>
        /// The standard width of all hexagon textures - Hexagon component only, in a single frame.
        /// </summary>
        public static float HexagonGrid_HexSizeX = 60;
        
        /// <summary>
        /// Standard height of all hexagon textures - Hexagon component only, in a single frame.
        /// </summary>
        public static float HexagonGrid_HexSizeY = 30;
        
        /// <summary>
        /// Number of pixels one elevation difference makes on the screen.
        /// </summary>
        public static float HeightOffsetY = 8;

        /// <summary>
        /// Ratio of minimap width to total game width.
        /// </summary>
        public static float relativeMinimapSizeX = 0.2f;
        
        /// <summary>
        /// Ratio of minimap height to total game height
        /// </summary>
        public static float relativeMinimapSizeY = 0.2f;
#endregion

#region Texture Tracking
        /// <summary>
        /// Count of the number of Person textures loaded into the game, as well as their corresponding portaits and minimap pictures.
        /// </summary>
        public static int numPersonTextures = 2;    //Counts both minimap and main

        /// <summary>
        /// Number of castable abilities in the game, used to count in the TextureHolder class
        /// </summary>
        public static int numAbilities = 1;
#endregion

#region Game Pacing
        /// <summary>
        /// number of frames between each animation advancement
        /// </summary>
        public static int FrameLength = 5; //Note: default is 60 FPS
#endregion

#region Health and Energy levels
#endregion

#region Levelling
        public static int maxLevel = 7;
        public static int[] experienceToNextLevel = { 10, 20, 30, 40, 50, 60, 70 };
#endregion

    }
}
