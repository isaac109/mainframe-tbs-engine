using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mainframe.Animations;
using Mainframe.Core.Units;

namespace Mainframe.Core.Combat
{
    /// <summary>
    /// Container for all effects a person's ability has, as well as performer for all casting.
    /// </summary>
    public class Ability
    {
        public enum ProjectileTypes : short
        {
            none = 0,
            straight,
            space,
        }

        protected float lashbackDamage;       //Damage done to the caster, instantly upon hit.
        protected float energyCostPerTurn;    //Energy removed from the caster per turn this effect is channeled.
        protected int cooldown = 0;           //Number of turns until this ability can be cast again after being cast.
        protected int currentCD = 0;          //Number of turns until the abiility is next avaialable.
        protected float energyCost;           //Amount of energy required to cast, and taken when cast.
        protected int currentChannelLeft;   //Amount of time until this ability goes off.
        protected int channelTime;            //Length of time this spell requires the character to be inactive between cast start and effects.
        protected List<AbilityEffect> targetAbilityEffects;             //Any and all over-time effects this spell will have
        protected List<AbilityEffect> casterAbilityEffects;             //Any and all over-time effects this spell will have
        protected List<InstantEffect> targetInstantEffects;       //Any and all instant effects dealt to targets of the spell
        protected List<InstantEffect> casterInstantEffects;       //Any and all instant effects dealt to the caster
        protected bool bAggressive;           //Indicates whether this ability will target enemies(T) or allies(F).
        protected short projectileType;           //Indicates whether the ability flies between targets or immediately hits.
        protected String AbilityName;         //Name of the ability displayed on the buttons
        protected Sprite buttonSprite;        //Sprite displayed on the button
        protected List<Sprite> projectileSprites;    //Sprite shown to fly between the caster and the target.
        protected Sprite impactSprite;        //Sprite to be added 
        protected Person caster = null, target = null;      //originator and target of the current cast cycle.

        /// <summary>
        /// Ability Constructor
        /// </summary>
        /// <param name="EnergyCost">Amount of energy required to cast, and taken when cast.</param>
        /// <param name="Aggressive">Indicates whether this ability will target enemies(T) or allies(F).</param>
        /// <param name="ButtonSprite">Sprite displayed on the button</param>
        /// <param name="Name">Name of the ability displayed on the buttons</param>
        /// <param name="overtimeEffects">Any and all effects this spell will have</param>
        /// <param name="Cooldown">Number of turns until this ability can be cast again.</param>
        /// <param name="ChannelTime">Length of time this spell requires the character to be inactive between cast start and effects.</param>
        /// <param name="EnergyCostPerTurn">Energy removed from the caster per turn this effect is channeled.</param>
        /// <param name="SelfDamage">Damage done to the caster, instantly upon hit.</param>
        /// <param name="ImpactSprite">Sprite added to any victim's Spritelist.</param>
        /// <param name="ProjectileType">Indicates whether this Ability will fly between targets (T), or immediately hit its target (F).</param>
        /// <param name="Projectile">Sprites shown to fly between targets, if this will create a projectile</param>
        public Ability(float EnergyCost, bool Aggressive, Sprite ButtonSprite, String Name, 
            List<AbilityEffect> overtimeEffects, short ProjectileType = (short) ProjectileTypes.none, List<Sprite> ProjectileSprites = null, Sprite ImpactSprite = null,
            int Cooldown = 0, int ChannelTime = 0, float EnergyCostPerTurn = 0, float SelfDamage = 0)
        {
            this.energyCost = EnergyCost;
            this.bAggressive = Aggressive;
            this.buttonSprite = ButtonSprite;
            this.AbilityName = Name;
            this.abilityEffects = overtimeEffects;
            this.cooldown = Cooldown;
            this.channelTime = ChannelTime;
            this.energyCostPerTurn = EnergyCostPerTurn;
            this.lashbackDamage = SelfDamage;
            this.projectileType = ProjectileType;
            this.impactSprite = ImpactSprite;
            this.projectileSprites = ProjectileSprites;
            if (projectileSprites == null)
            {
                projectileSprites = new List<Sprite>();
            }
        }

        /// <summary>
        /// Runs the logic to reduce timers, etc. At the start of a new turn
        /// </summary>
        public void newTurn()
        {
            if (currentCD > 0) currentCD--;
            if (currentChannelLeft > 0)
            {
                if(--currentChannelLeft == 0)
                    finishCast();
                else
                {
                    if (caster != null)
                    {
                        caster.takeDamage(null, 0, energyCostPerTurn);
                    }
                }
            }
        }

        /// <summary>
        /// Starts a timer on the Caster based on the Ability's ChannelTime, and at the end of the timer, launches the ability and its effects at the target. 
        /// </summary>
        /// <param name="Caster">Originator of the ability, who spends the mana, takes any lashback damage, and takes the credit.</param>
        /// <param name="Target">Person towards which the </param>
        public bool StartCast(Person Caster, Person Target)
        {
            if (!Caster.CanAct || Caster.Energy < energyCost || Caster.Health < lashbackDamage) return false;   //Checks any conditions that would make the Caster unable to use this ability.
            caster = Caster;
            target = Target;
            caster.CanAct = false;
            currentCD = cooldown;

            return true;
        }

        /// <summary>
        /// Launches the projectile if applicable, finishing off the ability.
        /// </summary>
        /// <returns></returns>
        public bool finishCast()
        {
            if (caster.Energy < energyCost || caster.Health < lashbackDamage) return false;   //Checks any conditions that would make the Caster unable to use this ability.
            caster.CanAct = true;
            caster.takeDamage(null, lashbackDamage, energyCost);
            switch (projectileType)
            {
                case (short) ProjectileTypes.none:
                    break;

                case (short) ProjectileTypes.space:
                    
                    break;

                case (short) ProjectileTypes.straight:
                    break;
            }
            return true;
        }
    }
}
