using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Core;
using Mainframe.Animations;

namespace Mainframe.GUI
{
    /// <summary>
    /// Interface item which performs when clicked.
    /// </summary>
    public class Button
    {
        public enum columnName
        {
            up = 0,
            over,
            down,
        }

        private bool bActive;
        private bool bClickable;

        public Rectangle position;            //Top left corner of Button, as well as dimensions
        protected Sprite buttonImage;

        public delegate void ButtonFunction();
        public ButtonFunction function;

        /// <summary>
        /// Creates a new button that will perform a given function when clicked.
        /// </summary>
        /// <param name="pos">Area to which the button will be drawn, and on which it will check for input</param>
        /// <param name="spriteSheet">Texture sheet of the button. Should contain an up-state, a down-state, and an over-state.</param>
        /// <param name="function">The executable function this button well perform when clicked.</param>
        /// <param name="bClickable">Sets whether this button will perform its function when clicked</param>
        /// <param name="bActive">Sets whether this button will appear</param>
        public Button(Rectangle pos, Sprite spriteSheet, ButtonFunction function, bool bClickable = true, bool bActive = true)
        {
            this.position = pos;
            this.function = function;
            this.buttonImage = spriteSheet;
            this.bActive = bActive;
            this.bClickable = bClickable;
        }

        /// <summary>
        /// If the button is active and clickable, runs the controls through and sees if it needs to perform anything.
        /// </summary>
        public void Update()
        {
            if (bActive && bClickable)
            {
                bool bOver = OverArea(Controls.MousePosition);
                if (bOver)
                {
                    if (!Controls.ButtonsDown[(int)Controls.ButtonNames.leftMouse] && Controls.ButtonsLastDown[(int)Controls.ButtonNames.leftMouse])
                    {
                        function();
                    }
                    if(buttonImage.CurrentColumn != (int)columnName.over && Controls.ButtonsDown[(int)Controls.ButtonNames.leftMouse])
                        buttonImage.changeColumn((int)columnName.over);
                    else if(buttonImage.CurrentColumn != (int)columnName.down && !Controls.ButtonsDown[(int)Controls.ButtonNames.leftMouse])
                        buttonImage.changeColumn((int)columnName.down);
                }
                else
                {
                    buttonImage.changeColumn((int)columnName.up);
                }
            }
        }

        /// <summary>
        /// Looks to see if the given vector lies within the area of the button.
        /// </summary>
        /// <param name="positionToCheck"></param>
        /// <returns></returns>
        public bool OverArea(Vector2 positionToCheck)
        {
            return (positionToCheck.X >= position.X && positionToCheck.X <= position.X + position.Width && 
                positionToCheck.Y >= position.Y && positionToCheck.Y <= position.Y + position.Height);
        }

        /// <summary>
        /// Make this button appear.
        /// </summary>
        public void Activate()
        {
            bActive = true;
        }

        /// <summary>
        /// Make this button disappear.
        /// </summary>
        public void Deactivate()
        {
            bActive = false;
        }

        /// <summary>
        /// Change the clickability of the button
        /// </summary>
        /// <param name="bClickable">Boolean to set whether this button should respond.</param>
        public void MakeClickable(bool bClickable)
        {
            this.bClickable = bClickable;
        }

        /// <summary>
        /// Draws the button in its internal position, 
        /// </summary>
        /// <param name="spriteBatch">Main spritebatch for the game</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (bActive)
            {
                if (bClickable)
                {
                    buttonImage.Draw(spriteBatch, position, Color.White);
                }
                else
                {
                    buttonImage.Draw(spriteBatch, position, Color.Gray);
                }
            }
        }

        /// <summary>
        /// Removes this button from the GUIManager
        /// </summary>
        public void clear()
        {
            GUIManager.getManager().ClearButton(this);
        }

        /// <summary>
        /// Contains the up, down, and over textures.
        /// </summary>
        public Sprite SpriteSheet
        {
            get
            {
                return buttonImage;
            }
        }
    }
}
