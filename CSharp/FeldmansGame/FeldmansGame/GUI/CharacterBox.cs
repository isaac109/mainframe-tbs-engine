using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Core;
using Mainframe.Core.Units;
using Mainframe.Animations;
using Mainframe.Constants;
using Mainframe.Animations.Combat;
using Mainframe.Animations.Interface;

namespace Mainframe.GUI
{
    /// <summary>
    /// A stat Box that appears in the corner and displays various information about selected characters in the game.
    /// </summary>
    public class CharacterBox
    {
        protected Rectangle HealthBox, ExpBarBox;
        Person selectedChar;
        Person MouseOverChar;
        Sprite healthPanelBackground, abilityPanelBackground, mouseoverBackground, 
            healthBar, healthBarM, energyBar, energyBarM, experienceBar,
            healthPanelFrame, abilityPanelFrame, mouseOverFrame;
        TextButton[] Options;
        CombatTextureHolder combatHolder;
        InterfaceTextureHolder interfaceHolder;

        private enum barColumnNames
        {
            healthy = 0,
            low,
            urgent,
            empty
        }

        /// <summary>
        /// Creates a GUI element to show information about the selected Person and their abilities.
        /// </summary>
        /// <param name="CharacterLifeRect">Game screen area to draw the portrait, life and energy bars, any AbilityEffect icons, and level information</param>
        /// <param name="abilityAreaRect">Area to display the ability buttons, as well as the experience bar if the selected person is a hero.</param>
        public CharacterBox(Rectangle CharacterLifeRect, Rectangle abilityAreaRect)
        {
            Options = new TextButton[4];
        
            HealthBox = CharacterLifeRect;
            ExpBarBox = abilityAreaRect;
            Rectangle buttonRect = new Rectangle(ExpBarBox.X + 10, ExpBarBox.Y + 10, (ExpBarBox.Width - 25) / 4, ExpBarBox.Height - 50);
            interfaceHolder = InterfaceTextureHolder.Holder;
            combatHolder = CombatTextureHolder.Holder;

            Options[0] = new TextButton(buttonRect, interfaceHolder.ButtonSprites[(int)ConstantHolder.ButtonBackgrounds.Exit].copySprite(),
                buttonZero, interfaceHolder.Fonts[(int)ConstantHolder.Fonts.defaultFont], 
                "Yo, dawg.");
            
            buttonRect.X += (ExpBarBox.Width - 25) / 4 + 5;

            Options[1] = new TextButton(buttonRect, interfaceHolder.ButtonSprites[(int)ConstantHolder.ButtonBackgrounds.Exit].copySprite(),
                buttonOne, interfaceHolder.Fonts[(int)ConstantHolder.Fonts.defaultFont], "Sup, son.");
            
            buttonRect.X += (ExpBarBox.Width - 25) / 4 + 5;
            Options[2] = new TextButton(buttonRect, interfaceHolder.ButtonSprites[(int)ConstantHolder.ButtonBackgrounds.Exit].copySprite(),
                buttonTwo, interfaceHolder.Fonts[(int)ConstantHolder.Fonts.defaultFont], "I heard you liek mudkips");
            
            buttonRect.X += (ExpBarBox.Width - 25) / 4 + 5;
            Options[3] = new TextButton(buttonRect, interfaceHolder.ButtonSprites[(int)ConstantHolder.ButtonBackgrounds.Exit].copySprite(),
                buttonThree, interfaceHolder.Fonts[(int)ConstantHolder.Fonts.defaultFont], "Bro do you even lift?");


            healthPanelBackground = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.PortraitBackground].copySprite();
            abilityPanelBackground = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.AbilityBackground].copySprite();
            mouseoverBackground = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.MouseOverBackground].copySprite();
            healthPanelFrame = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.PortraitFrame].copySprite();
            abilityPanelFrame = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.AbilityFrame].copySprite();
            mouseOverFrame = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.MouseOverFrame].copySprite();
            healthBar = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.HealthBar].copySprite();
            healthBarM = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.HealthBar].copySprite();
            energyBar = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.EnergyBar].copySprite();
            energyBarM = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.EnergyBar].copySprite();
            experienceBar = interfaceHolder.GUISprites[(int)ConstantHolder.GUIMemberImages.ExperienceBar].copySprite();
        }

        /// <summary>
        /// Update the statBox to use the specified person as the displayed top character
        /// </summary>
        /// <param name="newPerson">The new person</param>
        public void setSelected(Person newPerson)
        {
            if (newPerson != null)
            {
                selectedChar = newPerson;
                //Options[0].ChangeText(newPerson.Abilities[0].ToString());
            }
        }

#region ButtonClicks
        /// <summary>
        /// Function to perform when the 0th button is clicked.
        /// </summary>
        public void buttonZero()
        {
        }


        /// <summary>
        /// Function to perform when the 1st button is clicked.
        /// </summary>
        public void buttonOne()
        {
        }


        /// <summary>
        /// Function to perform when the 2nd button is clicked.
        /// </summary>
        public void buttonTwo()
        {
        }


        /// <summary>
        /// Function to perform when the 3rd button is clicked.
        /// </summary>
        public void buttonThree()
        {
        }

#endregion

        /// <summary>
        /// Update the statBox to use the specified character as the displayed bottom character
        /// </summary>
        /// <param name="newCharacter">The new character</param>
        public void SetMouseOver(Person newCharacter)
        {
            MouseOverChar = newCharacter;
        }

        /// <summary>
        /// Checks for a moused-over or newly selected person.
        /// </summary>
        public void update()
        {
            MouseOverChar = MainframeGame.currentLevelGrid.personAtScreenPos(Controls.MousePosition);
            if (Controls.ButtonsLastDown[(int)Controls.ButtonNames.leftMouse] && !Controls.ButtonsDown[(int)Controls.ButtonNames.leftMouse])
            {
                setSelected(MouseOverChar);
            }
            foreach (Button b in Options)
            {
                b.Update();
            }
        }

        /// <summary>
        /// Draws the background, then the characters and their stats.
        /// </summary>
        /// <param name="spriteBatch">Main spritebatch</param>
        public void draw(SpriteBatch spriteBatch)
        {
            drawStatsPanels(spriteBatch);
            if (selectedChar != null)
            {
                drawStats(spriteBatch, selectedChar);
            }
            if (MouseOverChar != null && MouseOverChar != selectedChar)
            {
                drawMouseOverStats(spriteBatch, MouseOverChar, new Vector2(20, 20) + Controls.MousePosition);
            }
            healthPanelFrame.Draw(spriteBatch, HealthBox);
            abilityPanelFrame.Draw(spriteBatch, ExpBarBox);
        }

        /// <summary>
        /// Draws the backgrounds for the character info panels
        /// </summary>
        /// <param name="batch">Main drawing spritebatch.</param>
        public void drawStatsPanels(SpriteBatch batch)
        {
            healthPanelBackground.Draw(batch, HealthBox);
            abilityPanelBackground.Draw(batch, ExpBarBox);
        }

        /// <summary>
        /// Handles the details of drawing the necessary stats as well as pictures and backgrounds.
        /// </summary>
        /// <param name="spriteBatch">Main drawing batch</param>
        /// <param name="person">Character whose stats are being drawn.</param>
        /// <param name="position">Top left corner of the box</param>
        public void drawStats(SpriteBatch spriteBatch, Person person)
        {
            SpriteFont Font = InterfaceTextureHolder.Holder.Fonts[(int)ConstantHolder.Fonts.defaultFont];
            float textLineHeight = Font.MeasureString("QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm,./?><';:\"[]\\|}{!@#$%^&*()_+1234567890-=`~").Y;
            float healthpct = (float)(person.Health) / (float)person.MaximumHealth;
            float energypct = (float)(person.Energy) / (float)person.MaxEnergy;
            int expToNextLevel = person is Hero ? ((Hero)person).ExperienceToLevel : 0;
            float exppct = person is Hero ? ((Hero)person).CurrentExperience * 1.0f/ expToNextLevel : 0;

            //Picture and name/level
            Rectangle loc = new Rectangle(HealthBox.X + 5, HealthBox.Y + 5, HealthBox.Width - 10, HealthBox.Width - 10);
            person.Portrait.Draw(spriteBatch, loc);
            loc.Y += HealthBox.Width;
            loc.Height = 15;
            healthBar.Draw(spriteBatch, loc, Color.Blue);
            healthBar.Draw(spriteBatch, loc, new Vector2(healthpct, 1.0f));
            loc.Y += 25;
            energyBar.Draw(spriteBatch, loc, Color.Purple);
            energyBar.Draw(spriteBatch, loc, new Vector2(energypct, 1.0f));
            if(person is Hero)
            {
                Hero tempChar = (Hero)person;
                spriteBatch.DrawString(Font, tempChar.Name + "\nLevel " + tempChar.Level, new Vector2(loc.X + 15, loc.Y + 15), Color.Black, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                loc.X = ExpBarBox.X;
                loc.Y = ConstantHolder.GAME_HEIGHT - 20;
                loc.Width = ExpBarBox.Width;
                loc.Height = 20;
                experienceBar.Draw(spriteBatch, loc, Color.Black);
                experienceBar.Draw(spriteBatch, loc, new Vector2(exppct, 1.0f));
            }
            for (int i = 0; i < 4; ++i)
            {
                Options[i].Draw(spriteBatch);
            }
            loc.X = 0;
            loc.Y = ConstantHolder.GAME_HEIGHT - 20;
            loc.Width = 20;
            loc.Height = 20;
            for (int i = 0; i < Math.Min(ExpBarBox.X / 20, person.EffectsOnPerson.Count); ++i)
            {
                person.EffectsOnPerson[i].DebuffIcon.Draw(spriteBatch, loc);
                loc.X += 20;
            }
        }

        /// <summary>
        /// Draws the tooltip when mousing over a character
        /// </summary>
        /// <param name="batch">Main drawing batch</param>
        /// <param name="target">Character being moused over</param>
        /// <param name="position">Top left of the drawing position.</param>
        public void drawMouseOverStats(SpriteBatch batch, Person target, Vector2 position)
        {
            float healthpct = (float)(target.Health) / (float)target.MaximumHealth;
            float energypct = (float)(target.Energy) / (float)target.MaxEnergy;
            Rectangle drawingLoc = new Rectangle((int)position.X, (int)position.Y, 70, 35);
            mouseoverBackground.Draw(batch, drawingLoc);
            mouseOverFrame.Draw(batch, drawingLoc);
            drawingLoc.X += 5;
            drawingLoc.Y += 5;
            drawingLoc.Width -= 10;
            drawingLoc.Height = 10;
            healthBarM.Draw(batch, drawingLoc, Color.Blue);
            healthBarM.Draw(batch, drawingLoc, new Vector2(healthpct, 1.0f));
            drawingLoc.Y += 15;
            energyBarM.Draw(batch, drawingLoc, Color.Purple);
            energyBarM.Draw(batch, drawingLoc, new Vector2(energypct, 1.0f));
        }
    }
}