using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mainframe.Animations;
using Mainframe.Core.Units;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Constants;

namespace Mainframe.Core.Combat
{
    /// <summary>
    /// Extenstion of a Projectile while travels like an actor. 
    /// </summary>
    public class SpaceProjectile : Projectile
    {
        protected List<Vector2> Path;       //Grid positions of the gridSpaces through which the projectile will travel
        protected bool moveFlag = true,     //Indicates whether, during the next frame, the projectile should move.
            endFlag = false;                //Indicates whether the projectile has reached the end of its journey.
        protected GridSpace parentSpace;    //Gridspace in which the projectile is currently residing.
        protected bool[] drawFlagArray;     //indicates whether to draw each individual sprite;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectilePath">Points to move through in the grid</param>
        /// <param name="EffectList">Effects to take on each target hit.</param>
        /// <param name="ProjectileAnimations">The animations that will play as this projectile flies.</param>
        /// <param name="ImpactSprite">Sprite while plays once when the projectile hits a target.</param>
        /// <param name="Caster">Originator of the projectile.</param>
        /// <param name="targetEnemies">Chooses whether this projectile will hit enemies</param>
        /// <param name="targetFriendlies">Chooses whether this projectile will hit people on the same team</param>
        /// <param name="DamageFalloff">Ratio by which to reduce each effect each time a target is hit. (multiplies by 1-damageFalloff)</param>
        /// <param name="DurationFalloff">Ratio by which to reduce each effect duration each time a target is hit. (multiplies by 1-durationFalloff</param>
        public SpaceProjectile(List<Vector2> ProjectilePath, AbilityEffect[] EffectList, Sprite[] ProjectileAnimations,
                               Sprite ImpactSprite, Person Caster = null, bool targetEnemies = true, bool targetFriendlies = false, 
                               float DamageFalloff = 0, float DurationFalloff = 0)
            : base(EffectList, ProjectileAnimations, ImpactSprite, Caster, targetEnemies, targetFriendlies, DamageFalloff, DurationFalloff)
        {
            Path = ProjectilePath;
        }


        
#region Updating
        /// <summary>
        /// Moves the projectile, sets a new target if applicable, then calls the parent, which applies effects to the new target, if applicable.
        /// </summary>
        public override void Update()
        {
            if (endFlag)
            {
                if(!tempTarget.TemporarySprites.Contains(impactSprite))
                {
                    tempTarget.TemporarySprites.Add(impactSprite);
                    foreach (Sprite travelSprite in projectileAnimations)
                    {
                        tempTarget.TemporarySprites.Add(travelSprite);
                    }
                }
                deleteFlag = true;
            }
            else if (moveFlag)
            {
                moveFlag = false;
                Vector2 temp = Path[0];
                Path.RemoveAt(0);
                parentSpace = parentGrid.GridSpaces[(int)temp.X, (int)temp.Y];
                endFlag = Path.Count == 0; 
                tempTarget = (Person)parentSpace.tryGetActor();
                for (int i = 0; i < drawFlagArray.Length; ++i)
                {
                    drawFlagArray[i] = true;
                }
            }
            base.Update();
        }

#endregion

        

#region Interacting
        /// <summary>
        /// Sets the parent backPointers necessary for target tracking
        /// </summary>
        /// <param name="parentGrid">Grid to which this projectile belongs</param>
        /// <param name="parentSpace">Space in which this projectile should now reside</param>
        protected void setParents(Grid parentGrid, GridSpace parentSpace)
        {
            this.parentGrid = parentGrid;
            this.parentSpace = parentSpace;
        } 

        /// <summary>
        /// Sets the parent back pointer for the current space
        /// </summary>
        /// <param name="parentSpace">Space into which this projectile should move.</param>
        protected void setParentSpace(GridSpace parentSpace)
        {
            this.parentSpace = parentSpace;
        }

        /// <summary>
        /// Sets the movement flag to indicate whether the projectile should move next frame.
        /// </summary>
        /// <param name="newFlag">True to move next frame, false to stay in place.</param>
        public void setMoveFlag(bool newFlag)
        {
            moveFlag = newFlag;
        }
#endregion

#region Drawing

        /// <summary>
        /// Draws the Projectile at its current location, and checks for any flags it needs to set.
        /// </summary>
        /// <param name="batch">Spritebatch to draw with</param>
        /// <param name="targetLoc">Top left corner of the parent grid space.</param>
        public void Draw(SpriteBatch batch, Vector2 targetLoc)
        {
            bool temp;
            moveFlag = true;
            int i = 0;
            foreach (Sprite sprite in projectileAnimations)
            {
                if (drawFlagArray[i])
                {
                    temp = sprite.Draw(batch, new Rectangle((int)targetLoc.X, (int)(targetLoc.Y + ConstantHolder.HexagonGrid_HexSizeY - sprite.SpriteSizeY),
                        (int)ConstantHolder.HexagonGrid_HexSizeX, sprite.SpriteSizeY));
                    drawFlagArray[i++] = !temp;
                    moveFlag &= temp;
                }
            }
        }

#endregion

#region Accessors/Mutators


#endregion
    }
}
