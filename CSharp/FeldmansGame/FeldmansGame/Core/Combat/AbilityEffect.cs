using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mainframe.Constants;
using Mainframe.Animations;
using Mainframe.Core.Units;

namespace Mainframe.Core.Combat
{
    /// <summary>
    /// Base class for all effects abilities perform, such as damage/healing, DoTs/HoTs, slows, etc. As well as their duration.
    /// </summary>
    public class AbilityEffect
    {
        private float damageOnHit;          //Damage done instantly upon hit. Applies to all targets hit, reducing per target hit.
        private float damagePerTurn;        //Damage done to the target each turn after the initial hit, until duration is up
        private float dotDuration;          //Duration of any DoTs
        private float energyDamage;         //Energy removed from target upon hit, applies to all targets hit, reducing with each target hit.
        private float energyDamagePerTurn;  //Energy removed from the target each turn after the hit, until the eotDuration is up
        private float eotDuration;          //Duration of the energy debuff on the target
        private Sprite debuffSprite;        //Sprite to display as a debuff icon.
        private Sprite effectSprite;        //Sprite to display on top of the person on the grid.
        private Person caster;              //Person from whom this effect originated.
        
        /// <summary>
        /// AbilityEffect main constructor for all values
        /// </summary>
        /// <param name="damage">Damage done immediately on hit.</param>
        /// <param name="damageATurn">Damage done per turn to all hit.</param>
        /// <param name="dotLength">Duration of the DoT effect, if there is one</param>
        /// <param name="energyHit">Energy removed from the target immediately on hit.</param>
        /// <param name="energyDoT">Energy removed per turn on all hit.</param>
        /// <param name="EoTLength">Duration of the EoT</param>
        /// <param name="Caster">Originator of this Effect.</param>
        /// <param name="iconSprite">Icon for the debuff to display in the character stat sheet.</param>
        /// <param name="overlaySprite">Sprite which draws over the character on the grid while this effect is on.</param>
        public AbilityEffect(float damage, float damageATurn, float dotLength, float energyHit, float energyDoT, float EoTLength, Sprite iconSprite, Sprite overlaySprite = null, Person Caster = null)
        {
            damageOnHit = damage;
            damagePerTurn = damageATurn;
            dotDuration = dotLength;
            energyDamage = energyHit;
            energyDamagePerTurn = energyDoT;
            eotDuration = EoTLength;
            debuffSprite = iconSprite;
            caster = Caster;
            effectSprite = overlaySprite;
        }

        /// <summary>
        /// Changes the effect's sprites for the current instance and all subsequently copied instances
        /// </summary>
        /// <param name="iconSprite"></param>
        /// <param name="overlaySprite"></param>
        public void setSprites(Sprite iconSprite, Sprite overlaySprite = null)
        {
            debuffSprite = iconSprite;
            effectSprite = overlaySprite;
        }

        /// <summary>
        /// Multiplies all damage-related fields, but not durations, by the ratio given.
        /// </summary>
        /// <param name="ratio">Amount by which to multiply the effects.</param>
        public void multiplyEffects(float ratio)
        {
            damageOnHit *= ratio;
            damagePerTurn *= ratio;
            energyDamage *= ratio;
            energyDamagePerTurn *= ratio;
        }

        /// <summary>
        /// Multiplies all duration-related fields by the given ration.
        /// </summary>
        /// <param name="ratio">Amount by which to multiply the durations.</param>
        public void multiplyDurations(float ratio)
        {
            dotDuration *= ratio;
            eotDuration *= ratio;
        }

        /// <summary>
        /// Originator of this effect.
        /// </summary>
        public Person Caster
        {
            get { return caster; }
        }

        /// <summary>
        /// Damage done immediately on hit.
        /// </summary>
        public float DamageOnHit
        {
            get { return damageOnHit; }
        }

        /// <summary>
        /// Damage done per turn to all hit.
        /// </summary>
        public float DamagePerTurn
        {
            get { return damagePerTurn; }
        }

        /// <summary>
        /// Duration of the DoT effect, if there is one
        /// </summary>
        public float DoTDuration
        {
            get { return dotDuration; }
            set { dotDuration = value; }
        }

        /// <summary>
        /// Energy removed from the target immediately on hit.
        /// </summary>
        public float EnergyDamage
        {
            get { return energyDamage; }
        }

        /// <summary>
        /// Energy removed per turn on all hit.
        /// </summary>
        public float EnergyDamagePerTurn
        {
            get { return energyDamagePerTurn; }
        }

        /// <summary>
        /// Duration of the EoT
        /// </summary>
        public float EoTDuration
        {
            get { return eotDuration; }
            set { eotDuration = value; }
        }

        /// <summary>
        /// Icon for the debuff
        /// </summary>
        public Sprite DebuffIcon
        {
            get { return debuffSprite; }
        }

        /// <summary>
        /// Icon for the debuff
        /// </summary>
        public Sprite DebuffSpriteEffect
        {
            get { return effectSprite; }
        }
    }
}