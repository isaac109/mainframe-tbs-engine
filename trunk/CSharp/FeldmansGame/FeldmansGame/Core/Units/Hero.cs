using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Animations;
using Mainframe.Constants;
using Mainframe.Animations.Character;
using Mainframe.Core.Combat;

namespace Mainframe.Core.Units
{
    /// <summary>
    /// Base Class for a character with non-attack abilities.
    /// </summary>
    public class Hero : Person
    {
        /// <summary>
        /// Different sets of animation frames.
        /// </summary>
        private enum columnNamesMain
        {
            idle_up = 0,
            idle_down = 1,
            idle_down_right = 2,
            idle_down_left,
            idle_up_right,
            idle_up_left,
            walking_up,
            walking_up_right,
            walking_up_left,
            walking_down,
            walking_down_right,
            walking_down_left,
            dying,
            dead
        };

        private enum columnNamesMinimap
        {
            idle = 0,
            dying,
            dead
        }

        private enum columnNamesPortrait
        {
            idle = 0,
            dying,
            dead
        }

        int level, currExp, expToNextLevel, classIndex;

        /// <summary>
        /// Hero Constructor
        /// </summary>
        /// <param name="Health">Both starting and maximum HP</param>
        /// <param name="Energy">Both starting, and maximum EP</param>
        /// <param name="Level">Current Level</param>
        /// <param name="currExperience">Amount of experience the Hero has earned so far. Does not cumulate upon levelling, resets to 0 or remainder of experience earned.</param>
        /// <param name="GridPosition">Grid Position, not screen.</param>
        /// <param name="MovementSpeed">Amount of space cost this person can move in one turn.</param>
        public Hero(int Health, int Energy, int MovementSpeed, int Level, int currExperience, Vector2 GridPosition)
            : base(Health, Energy, MovementSpeed, GridPosition)
        {
            level = Level;
            currExp = currExperience;   
            //Levels start at 1, but array indices start at 0. Decrement level by 1 for array access
            expToNextLevel = ConstantHolder.experienceToNextLevel[level-1];
        }

        public Hero(int classID, int level, int[] abilityIndices, int skinIndex, Vector2 GridPosition)
        {
            effects = new List<AbilityEffect>();
            this.gridPosition = GridPosition;
            ActorTextureHolder holder = ActorTextureHolder.Holder;
            actorSprites.Add(holder.MainSprites[skinIndex].copySprite());
            minimapSprites.Add(holder.MinimapSprites[skinIndex].copySprite());
            portrait = holder.PortraitSprites[skinIndex].copySprite();
        }

        /// <summary>
        /// Hero constructor
        /// </summary>
        /// <param name="Health">Amount of health upon loading.</param>
        /// <param name="MaxHealth">Maximum available health.</param>
        /// <param name="Energy">Amount of energy upon loading.</param>
        /// <param name="MaxEnergy">Maximum available health.</param>
        /// <param name="MovementSpeed">Amount of spaces the Person may move per turn.</param>
        /// <param name="GridPosition">Point in the grid where this person is being put.</param>
        /// <param name="currExperience">Current amount of experience toward the next level</param>
        /// <param name="Level">Current level of the Hero.</param>
        public Hero(int Health, int MaxHealth, int Energy, int MaxEnergy, int MovementSpeed, int Level, int currExperience, String Name, Vector2 GridPosition)
            : base(Health, MaxHealth, Energy, MaxEnergy, MovementSpeed, Name, GridPosition)
        {
            level = Level;
            currExp = currExperience;
            //Levels start at 1, but array indices start at 0. Decrement level by 1 for array access
            expToNextLevel = ConstantHolder.experienceToNextLevel[level - 1];
        }

        /// <summary>
        /// Creates all the sprites for this Hero, based on the TextureHolder class.
        /// </summary>
        protected override void populateSpriteList()
        {   //Adapt the TextureHolder.ActorImageTypes enumerator to change the values here. Hard coded there, don't do it here.
            int index = (int)ConstantHolder.ActorImageTypes.BaseActor;
            ActorTextureHolder holder = ActorTextureHolder.Holder;
            actorSprites.Add(holder.MainSprites[index].copySprite());
            minimapSprites.Add(holder.MinimapSprites[index].copySprite());
            portrait = holder.PortraitSprites[index].copySprite();
        }

        /// <summary>
        /// The hero's current Hero Level.
        /// </summary>
        public int Level
        {
            get { return level; }
        }

        /// <summary>
        /// Amout of experience the Hero has earned so far this level.
        /// </summary>
        public int CurrentExperience
        {
            get { return currExp; }
        }

        /// <summary>
        /// Amount of experience the Hero must earn in order to earn their next level.
        /// </summary>
        public int ExperienceToLevel
        {
            get { return expToNextLevel; }
        }

        public int ClassIndex
        {
            get { return classIndex; }
        }
    }
}