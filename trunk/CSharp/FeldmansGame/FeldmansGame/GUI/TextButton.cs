using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mainframe.Animations;

namespace Mainframe.GUI
{
    /// <summary>
    /// Button with text as well as a texture.
    /// </summary>
    public class TextButton : Button
    {
        String text;

        SpriteFont font;

        /// <summary>
        /// Creates a button with a background and text on top of it.
        /// </summary>
        /// <param name="position">Top left corner of the textbutton, and its size.</param>
        /// <param name="sprite">Sprite containing the up, over, and down animations.</param>
        /// <param name="function">Function performed when the button is clicked</param>
        /// <param name="font">Font used to draw the text</param>
        /// <param name="text">Text to draw on the button</param>
        /// <param name="bClickable">Sets whether the button will respond to input</param>
        /// <param name="bActive">Sets whether the button will appear</param>
        public TextButton(Rectangle position, Sprite sprite, ButtonFunction function, SpriteFont font, String text = "", bool bClickable = true, bool bActive = true)
            : base(position, sprite, function, bClickable, bActive)
        {
            this.font = font;
            this.text = text;
        }

        /// <summary>
        /// Draws the button based on its internal variables
        /// </summary>
        /// <param name="spriteBatch">Main drawing batch.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(font, text, new Vector2(position.X + position.Width / 2, position.Y + position.Height / 2) - font.MeasureString(text) / 4, Color.Black, 0.0f, Vector2.Zero, .5f, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Sets the text on the button to a new value.
        /// </summary>
        /// <param name="newText">New text to draw on the button</param>
        public void ChangeText(String newText)
        {
            text = newText;
        }
        /// <summary>
        /// Changes the text on the button, using a new font.
        /// </summary>
        /// <param name="newText">New text to display</param>
        /// <param name="newFont">New font to use</param>
        public void ChangeText(String newText, SpriteFont newFont)
        {
            text = newText;
            font = newFont;
        }
    }
}
