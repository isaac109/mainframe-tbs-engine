using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mainframe.Animations;
using Mainframe.Core.Units;
using Microsoft.Xna.Framework.Graphics;

namespace Mainframe.Core.Combat
{
    /// <summary>
    /// Extension of a Projectile which travels in a straight line.
    /// </summary>
    public class StraightProjectile : Projectile
    {
#region Variables
        protected Vector2 originLoc,    //Point relative to the Grid origin from which the projectile came. 
            targetLoc,                  //Point relative to the Grid origin toward which the projectile moves.
            currentLoc,                 //Point relative to the Grid origin at which the projectile currently is.
            gridOrigin;                 //Location relative to the screen at which the Grid has its top left point.
        protected Sprite animProjToTarget,  //Animation to display stretched between the projectile and the target
                         animCasterToTarget,//Animation to display stretched between the caster and the target
                         animCasterToProj;  //Animation to display stretched between the caster and the projectile
                                //Note: These three animations should originate from top left to bottom right. They will be stretched and flipped as necessary
        protected float speed;       //Rate at which the projectile moves per frame.
        protected float distanceToTravel, distanceTravelled;    //Trackers for the delete flag, so that we can delete once it's hit its target.
#endregion


#region Setup
        /// <summary>
        /// Creates a projectile which moves in a straight line from caster to target
        /// </summary>
        /// <param name="target">Person at which the projectile is aimed.</param>
        /// <param name="Speed">Speed at which the projectile moves per frame</param>
        /// <param name="EffectList">All the effects to put on targets</param>
        /// <param name="ProjectileAnimations">The animations that will play as this projectile flies.</param>
        /// <param name="ImpactSprite">Sprite while plays once when the projectile hits a target.</param>
        /// <param name="Caster">Originator of the projectile.</param>
        /// <param name="DamageFalloff">Ratio by which to reduce each effect each time a target is hit. (multiplies by 1-damageFalloff)</param>
        /// <param name="DurationFalloff">Ratio by which to reduce each effect duration each time a target is hit. (multiplies by 1-durationFalloff</param>
        /// <param name="targetEnemies">Chooses whether this projectile will hit enemies</param>
        /// <param name="targetFriendlies">Chooses whether this projectile will hit people on the same team</param>
        /// <param name="CasterToProjectileAnim">Sprite that displays between the caster and the projectile.</param>
        /// <param name="CasterToTargetAnim">Sprite that displays between the caster and the target</param>
        /// <param name="ProjectileToTargetAnim">Sprite that displays between the projectile and the target</param>
        public StraightProjectile(Person target, float Speed, AbilityEffect[] EffectList, Sprite[] ProjectileAnimations, Sprite ImpactSprite = null, Person Caster = null,
                                  bool targetEnemies = true, bool targetFriendlies = false, float DamageFalloff = 0, float DurationFalloff = 0, 
            Sprite CasterToProjectileAnim = null, Sprite CasterToTargetAnim = null, Sprite ProjectileToTargetAnim = null)
            : base(EffectList, ProjectileAnimations, ImpactSprite, Caster, targetEnemies, targetFriendlies, DamageFalloff, DurationFalloff)
        {
            originLoc = Caster.ScreenPositionNoOffset;
            currentLoc = Caster.ScreenPositionNoOffset;
            targetLoc = target.ScreenPositionNoOffset;
            animCasterToProj = CasterToProjectileAnim;
            animCasterToTarget = CasterToTargetAnim;
            animProjToTarget = ProjectileToTargetAnim;
            speed = Speed;
            distanceTravelled = 0;
            distanceToTravel = Vector2.Distance(originLoc, targetLoc);
        }
#endregion

#region Updating
        /// <summary>
        /// Moves the projectile, sets a new target if applicable, then calls the parent, which applies effects to the new target, if applicable.
        /// </summary>
        public override void Update()
        {
            Vector2 temp = (targetLoc - currentLoc);
            temp.Normalize();
            temp *= speed;
            currentLoc += temp;
            tempTarget = parentGrid.personAtScreenPos(currentLoc);
            distanceTravelled += speed;
            if (distanceTravelled >= distanceToTravel)
                deleteFlag = true;
            base.Update();
        }

#endregion

#region Drawing
        /// <summary>
        /// Draws the projectile on its path, as well as any path-centered sprites it has.
        /// </summary>
        /// <param name="batch">Main drawing batch</param>
        public void Draw(SpriteBatch batch)
        {
            foreach (Sprite projSprite in projectileAnimations)
            {
                Vector2 tempVect = currentLoc - new Vector2(projSprite.SpriteSizeX / 2, projSprite.SpriteSizeY / 2) - gridOrigin;
                Rectangle drawingRect = new Rectangle((int)tempVect.X, (int)tempVect.Y, projSprite.SpriteSizeX, projSprite.SpriteSizeY);
                Vector2 temp2 = targetLoc - currentLoc;
                float angle = (float)(Math.Atan(temp2.Y / temp2.X) * 180 / Math.PI);
                angle += temp2.Y < 0 ? 0 : 180;
                projSprite.Draw(batch, drawingRect, angle);
            }
            if (animCasterToProj != null)
            {
                DrawRotatedSprite(animCasterToProj, originLoc, currentLoc, batch);
            }
            if (animCasterToTarget != null)
            {
                DrawRotatedSprite(animCasterToTarget, originLoc, targetLoc, batch);
            }
            if (animProjToTarget != null)
            {
                DrawRotatedSprite(animProjToTarget, currentLoc, targetLoc, batch);
            }
        }

        /// <summary>
        /// Draws the sprite, which starts off directed from top left to bottom right, in the correct rotation so that it draws from origin to endpoint.
        /// </summary>
        /// <param name="toDraw">Sprite to rotate and draw</param>
        /// <param name="startPoint">Origin point for orientation and sizing.</param>
        /// <param name="endPoint">Ending point for orientation and sizing.</param>
        /// <param name="batch">Main drawing spritebatch</param>
        private void DrawRotatedSprite(Sprite toDraw, Vector2 startPoint, Vector2 endPoint, SpriteBatch batch)
        {
            Rectangle drawingRect = new Rectangle();
            drawingRect.X = (int)Math.Min(endPoint.X, startPoint.X) - (int)gridOrigin.X;
            drawingRect.Y = (int)Math.Min(endPoint.Y, startPoint.Y) - (int)gridOrigin.Y;
            drawingRect.Width = (int)Math.Abs(endPoint.X - startPoint.X);
            drawingRect.Height = (int)Math.Abs(endPoint.Y - startPoint.Y);
            if (startPoint.X < endPoint.X)
            {
                if (startPoint.Y < endPoint.Y)
                {
                    toDraw.Draw(batch, drawingRect);
                }
                else
                {
                    animCasterToProj.Draw(batch, drawingRect, SpriteEffects.FlipHorizontally);
                }
            }
            else
            {
                if (startPoint.Y < endPoint.Y)
                {
                    animCasterToProj.Draw(batch, drawingRect, SpriteEffects.FlipVertically);
                }
                else
                {
                    animCasterToProj.Draw(batch, drawingRect, 180);
                }

            }
        }

#endregion
    }
}
