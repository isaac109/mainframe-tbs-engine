using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mainframe.Animations;
using Mainframe.Core.Units;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Mainframe.Core.Combat
{
    /// <summary>
    /// Item which carries Ability Effects from a caster to a target.
    /// </summary>
    public class Projectile
    {
#region Variables
        protected Sprite[] projectileAnimations;    //The animations that will play as this projectile flies.
                    //For StraightProjectiles, the default angle will be directed along the X axis to the right
        protected AbilityEffect[] effects;          //Effects to take on each target hit.
        protected float damageFalloff;              //Ratio by which to reduce each effect each time a target is hit. (multiplies by 1-damageFalloff)
        protected float durationFalloff;            //Ratio by which to reduce each effect duration each time a target is hit. (multiplies by 1-durationFalloff)
        protected List<Person> targetsHit;          //Keeps track of all targets hit by the projectile so far, to avoid double-counting.
        protected Person caster;                    //Originator of the projectile.
        protected Sprite impactSprite;              //Sprite while plays once when the projectile hits a target.
        protected Grid parentGrid;                  //Grid on which this projectile is based.
        protected Person tempTarget;                //Holder for currently colliding actor.
        protected bool hitEnemies;                  //Chooses whether the projectile will affect enemies.
        protected bool hitFriendlies;               //Chooses whether the projectile will affect people on the same team.
        protected bool deleteFlag = false;             //Indicates to the owning space whether this projectile is ready to be deleted.
#endregion
        
#region Setup
        /// <summary>
        /// Constructs a projectile to carry Ability Effects across the screen.
        /// </summary>
        /// <param name="EffectList">Effects to take on each target hit.</param>
        /// <param name="ProjectileAnimations">The animations that will play as this projectile flies.</param>
        /// <param name="ImpactSprite">Sprite while plays once when the projectile hits a target.</param>
        /// <param name="Caster">Originator of the projectile.</param>
        /// <param name="DamageFalloff">Ratio by which to reduce each effect each time a target is hit. (multiplies by 1-damageFalloff)</param>
        /// <param name="DurationFalloff">Ratio by which to reduce each effect duration each time a target is hit. (multiplies by 1-durationFalloff)</param>
        /// <param name="targetEnemies">Chooses whether this projectile will hit enemies.</param>
        /// <param name="targetFriendlies">Chooses whether this projectile will hit people on the same team</param>
        public Projectile(AbilityEffect[] EffectList, Sprite[] ProjectileAnimations, Sprite ImpactSprite, Person Caster = null, bool targetEnemies = true, bool targetFriendlies = false, float DamageFalloff = 0, float DurationFalloff = 0)
        {
            effects = EffectList;
            projectileAnimations = ProjectileAnimations;
            impactSprite = ImpactSprite;
            caster = Caster;
            damageFalloff = DamageFalloff;
            durationFalloff = DurationFalloff;
            targetsHit = new List<Person>();
            hitEnemies = targetEnemies;
            hitFriendlies = targetFriendlies;
        }
#endregion

#region Updating
        /// <summary>
        /// Runs any collision checking for the Projectile. Called after subclasses, which handle the movements.
        /// </summary>
        public virtual void Update() 
        {
            if (tempTarget != null && 
                (tempTarget.TeamIndex != caster.TeamIndex) == hitEnemies ||
                (tempTarget.TeamIndex == caster.TeamIndex) == hitFriendlies)
            {
                CauseEffect(tempTarget);
            }
        }
#endregion

#region Interaction

        /// <summary>
        /// Puts the Projectile's effects on the target, then reduces values as necesasary.
        /// </summary>
        /// <param name="target">Person hit by the projectile.</param>
        public void CauseEffect(Person target)
        {
            foreach (AbilityEffect effect in effects)
            {
                target.takeEffect(effect);
                effect.multiplyDurations(1 - durationFalloff);
                effect.multiplyEffects(1 - damageFalloff);
            }
            targetsHit.Add(target);
        }

        /// <summary>
        /// Tells the projectile to which Grid it belongs.
        /// </summary>
        /// <param name="parentGrid">The grid in which this is interacting</param>
        public void setParent(Grid parentGrid)
        {
            this.parentGrid = parentGrid;
        }
#endregion

#region Drawing

        /// <summary>
        /// Draws the Projectile's motion sprites onto the target location.
        /// </summary>
        /// <param name="batch">Main drawing SpriteBatch</param>
        /// <param name="targetRect">Target area to which to draw.</param>
        public virtual void Draw(SpriteBatch batch, Rectangle targetRect)
        {
            foreach (Sprite thisSprite in projectileAnimations)
            {
                thisSprite.Draw(batch, targetRect);
            }
        }

#endregion

        
#region Accessors/Mutators

        public bool DeleteFlag
        {
            get { return deleteFlag; }
        }

#endregion
    }
}