using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Animations;
using Mainframe.Animations.Interface;
using Mainframe.Constants;

namespace Mainframe.GUI
{
    /// <summary>
    /// Container for the selected hero's ability command buttons.
    /// </summary>
    public class OptionsBox
    {
        private Rectangle position;

        private Button[] buttons;

        private Sprite backgroundSprite;

        private SpriteFont font;

        private string displayText;

        /// <summary>
        /// Creates the options box.
        /// </summary>
        /// <param name="positon">Top left corner of the box.</param>
        /// <param name="InfoText">Text displayed above the buttons contained</param>
        /// <param name="buttonStrings">Array of the strings to put on the buttons</param>
        public OptionsBox(Rectangle positon, String InfoText, String[] buttonStrings)
        {
            InterfaceTextureHolder holder = InterfaceTextureHolder.Holder;
            font = holder.Fonts[(int)ConstantHolder.Fonts.defaultFont];
            this.position = positon;
            GUIManager manager = GUIManager.getManager();
            displayText = InfoText;
            buttons = new Button[buttonStrings.Length];
            backgroundSprite = holder.GUISprites[(int)ConstantHolder.GUIMemberImages.AbilityBackground].copySprite();//TBI: actual background for an options box.
            for (int i = 0; i < buttonStrings.Length; ++i)
            {
                //buttons[i] = manager.MakeTextButton();
            }
        }

        /// <summary>
        /// Clears all the buttons in the box.
        /// </summary>
        public void clear()
        {
            foreach (Button B in buttons)
            {
                B.clear();
            }
        }

        /// <summary>
        /// Draws the background, then the buttons in the box.
        /// </summary>
        /// <param name="batch">Main drawing batch</param>
        public void draw(SpriteBatch batch)
        {
            backgroundSprite.Draw(batch, position);
            batch.DrawString(font, displayText, new Vector2((font.MeasureString(displayText).X / 2) + position.X, position.Y + 20), Color.Black);
            foreach (Button B in buttons)
            {
                B.Draw(batch);
            }
        }

        /// <summary>
        /// Runs this frame's inputs through the buttons.
        /// </summary>
        public void update()
        {
            foreach (Button B in buttons)
            {
                B.Update();
            }
        }
    }
}
