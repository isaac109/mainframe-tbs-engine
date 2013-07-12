using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Animations;
using Mainframe.Core.Combat;
using Mainframe.Animations.Character;
using Mainframe.Constants;

namespace Mainframe.Core.Units
{
    /// <summary>
    /// Base class for all characters on the board.
    /// </summary>
    public class Person : Actor
    {
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

#region Variables
        protected int visibleHealth, maxHealth,
            visibleEnergy, maxEnergy;
        protected float internalHealth, healthRegen = 5,
            internalEnergy, energyRegen = 5;
        protected int spriteID;                 //Tracks the enum position of this person's sprite type, so that their ability animations can be loaded correctly.
        protected Ability[] abilities;
        protected List<AbilityEffect> effects;
        protected Sprite portrait;
        protected bool canAct = true;   //Determines whether the Person is currently able to cast abilities or move.
        protected short TeamFlag = 0;       //Keeps quick track of which team this person is on
        protected Ability castingAbility;   //Ability the person is currently casting
        protected int castTimer;            //Number of turns left before the current castingAbility goes off
        protected int moveSpeed;            //Number of space cost this person can move in a turn
#endregion

#region Setup
        /// <summary>
        /// Person constructor
        /// </summary>
        /// <param name="Health">Maximum and Current Health Points</param>
        /// <param name="Energy">Maximum and Current Energy Points</param>
        /// <param name="GridPosition">Position </param>
        /// <param name="MovementSpeed">Amount of space cost this person can move in one turn.</param>
        public Person(int Health, int Energy, int MovementSpeed, Vector2 GridPosition) 
            : base()
        {
            this.visibleHealth = Health;
            this.internalHealth = Health;
            this.maxHealth = Health;

            this.visibleEnergy = Energy;
            this.internalEnergy = Energy;
            this.maxEnergy = Energy;

            this.moveSpeed = MovementSpeed;

            effects = new List<AbilityEffect>();
            this.gridPosition = GridPosition;
            populateAbilityList();
        }

        public Person()
            : base()
        {
        }

        /// <summary>
        /// Person constructor for non-full health/energy initialization
        /// </summary>
        /// <param name="Health">Amount of health upon loading.</param>
        /// <param name="MaxHealth">Maximum available health.</param>
        /// <param name="Energy">Amount of energy upon loading.</param>
        /// <param name="MaxEnergy">Maximum available health.</param>
        /// <param name="MovementSpeed">Amount of spaces the Person may move per turn.</param>
        /// <param name="GridPosition">Point in the grid where this person is being put.</param>
        public Person(int Health, int MaxHealth, int Energy, int MaxEnergy, int MovementSpeed, String Name, Vector2 GridPosition)
            : base()
        {

            this.visibleHealth = Health;
            this.internalHealth = Health;
            this.maxHealth = MaxHealth;

            this.visibleEnergy = Energy;
            this.internalEnergy = Energy;
            this.maxEnergy = MaxEnergy;

            this.moveSpeed = MovementSpeed;

            this.name = Name;

            effects = new List<AbilityEffect>();
            this.gridPosition = GridPosition;
            populateAbilityList();
        }

        /// <summary>
        /// Creates the abilities the character will have available.
        /// </summary>
        protected virtual void populateAbilityList()
        {
            abilities = new Ability[1];
            //abilities[0] = new Ability();
        }

        /// <summary>
        /// Creates all the sprites for this Hero, based on the TextureHolder class.
        /// </summary>
        protected override void populateSpriteList()
        {   //Adapt the TextureHolder.ActorImageTypes enumerator to change the values here. Hard coded there, don't do it here.
            ActorTextureHolder holder = ActorTextureHolder.Holder;
            int index = (int)ConstantHolder.ActorImageTypes.BaseActor2;
            actorSprites.Add(holder.MainSprites[index].copySprite());
            minimapSprites.Add(holder.MinimapSprites[index].copySprite());
            portrait = holder.PortraitSprites[index].copySprite();
        }
#endregion

#region Updating

        /// <summary>
        /// Runs all normal automatic updates.
        /// </summary>
        public override void Update()
        {
            internalHealth += healthRegen;
            internalEnergy += energyRegen;
            visibleEnergy = (int)internalEnergy;
            visibleHealth = (int)internalHealth;
            base.Update();
        }

        /// <summary>
        /// Iterates through the current AbilityEffects on the person, taking damage/energyDamage as needed and removing finished effects.
        /// </summary>
        private void takeCurrentEffects()
        {
            bool removeFlag;
            foreach (AbilityEffect effect in effects)
            {
                removeFlag = true;
                if (effect.DoTDuration-- > 0)
                {
                    takeDamage(effect.Caster, (int)effect.DamagePerTurn, 0);
                    removeFlag = false;
                }
                if (effect.EoTDuration-- > 0)
                {
                    removeFlag = false;
                    takeDamage(effect.Caster, 0, (int)effect.EnergyDamagePerTurn);
                }
                if (removeFlag)
                    effects.Remove(effect);
            }
        }
#endregion

#region Drawing

        /// <summary>
        /// Draws the character and any effects it currently has.
        /// </summary>
        /// <param name="batch">Main drawing batch.</param>
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            foreach (AbilityEffect effect in effects)
            {
                effect.DebuffSpriteEffect.Draw(batch, screenPosition);
            }
        }

        /// <summary>
        /// Draws the minimap sprite for this character
        /// </summary>
        /// <param name="batch">Main spritebatch for drawing this game.</param>
        /// <param name="position">Screen position for the minimap</param>
        /// <param name="dimensions">Size of the sprite for the minimap</param>
        public override void DrawMinimap(SpriteBatch batch, Vector2 position, Vector2 dimensions)
        {
            foreach(Sprite m in minimapSprites)
                m.Draw(batch, new Rectangle((int) position.X, (int) position.Y, (int) dimensions.X, (int) dimensions.Y));
        }
#endregion

#region Interaction
        /// <summary>
        /// Changes the character's team, the damn traitor.
        /// </summary>
        /// <param name="newTeam">Character's new allegiance index.</param>
        public void setTeam(short newTeam)
        {
            TeamFlag = newTeam;
        }

        /// <summary>
        /// Deals health and energy damage to the person, who dies if their health reaches 0.
        /// </summary>
        /// <param name="Caster">Person to be given credit if this person dies.</param>
        /// <param name="healthDamage">Amount of health to lose. Negative amounts will heal, up to the person's max health.</param>
        /// <param name="energyDamage">Amount of energy to lose. Negative amounts will restore, up to the person's max energy.</param>
        public void takeDamage(Person Caster, float healthDamage, float energyDamage)
        {
            internalEnergy = Math.Min(maxEnergy, Math.Max(0, internalEnergy - energyDamage));
            visibleEnergy = (int) internalEnergy;
            internalHealth = Math.Min(maxHealth, Math.Max(0, internalHealth - healthDamage));
            visibleHealth = (int)internalHealth;
            if (internalHealth <= 0)
            {
                Die(Caster);
            }
        }

        /// <summary>
        /// Starts the dying animation playing, and once those are done, removes the player from the grid.
        /// </summary>
        /// <param name="killer"></param>
        public void Die(Person killer)
        {
            //TBI: Move all sprites into the temporary set, empty the main set. Set all temp columns to Dying column, where available.
            //After all sprites have finished, delete.
            //NOTE: In Hero override, they should only be set to the Dying column, not moved to temporary. After they go through dying, they should go to the Dead column.
        }

        /// <summary>
        /// Causes the effect to this person, and if it has any durations, adds the effect to the person's list.
        /// </summary>
        /// <param name="effectToTake">Effect to deal to this person.</param>
        public void takeEffect(AbilityEffect effectToTake)
        {
            takeDamage(effectToTake.Caster, effectToTake.DamageOnHit, effectToTake.EnergyDamage);
            if (effectToTake.DoTDuration + effectToTake.EoTDuration > 0)
            {
                effects.Add(effectToTake);
            }
        }
#endregion

#region Accessors and Mutators
        /// <summary>
        /// Current amount of health points the character has.
        /// </summary>
        public int Health
        {
            get { return visibleHealth; }
        }

        /// <summary>
        /// Maximum health points of the character.
        /// </summary>
        public int MaximumHealth
        {
            get { return maxHealth; }
        }

        /// <summary>
        /// Current amount of available energy points.
        /// </summary>
        public int Energy
        {
            get { return visibleEnergy; }
        }

        /// <summary>
        /// Maximum Energy level of the character.
        /// </summary>
        public int MaxEnergy
        {
            get { return maxEnergy; }
        }

        public int BaseMoveSpeed
        {
            get { return moveSpeed; }
        }

        /// <summary>
        /// This character's portrait for a CharacterBox.
        /// </summary>
        public Sprite Portrait
        {
            get { return portrait; }
        }

        /// <summary>
        /// The character's abilities available for use.
        /// </summary>
        public Ability[] Abilities
        {
            get { return abilities; }
        }

        /// <summary>
        /// Determines whether the Person is currently able to cast abilities or move.
        /// </summary>
        public bool CanAct
        {
            get { return canAct; }
            set { canAct = value; }
        }

        /// <summary>
        /// Simple indicator for team comparison.
        /// </summary>
        public short TeamIndex
        {
            get { return TeamFlag; }
        }

        public List<AbilityEffect> EffectsOnPerson
        {
            get { return effects; }
        }

        public int MoveSpeed
        {
            get { return moveSpeed; }
        }

#endregion
    }
}
