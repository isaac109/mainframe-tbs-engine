using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mainframe.Animations;
using Mainframe.Animations.Interface;
using Mainframe.Constants;

namespace Mainframe.GUI
{
    /// <summary>
    /// Text display area in the GUI.
    /// </summary>
    public class TextBox
    {
        private String textBuffer;
        private int maxVerticalLines;

        private Rectangle position;

        private SpriteFont font;
        private Sprite backgroundSprite;

        private string[] textLines;
        private const int maxLines = 10;
        InterfaceTextureHolder holder;
        /// <summary>
        /// Constructs a box to display whatever text is needed.
        /// </summary>
        /// <param name="position">Top left of the box's position.</param>
        /// <param name="initalString">String to be displayed.</param>
        public TextBox(Rectangle position, string initalString = "")
        {
            this.position = position;
            textBuffer = initalString;
            holder = InterfaceTextureHolder.Holder;
            font = holder.Fonts[(int)ConstantHolder.Fonts.defaultFont];
            backgroundSprite = holder.GUISprites[(int)ConstantHolder.GUIMemberImages.ExperienceBar].copySprite();//TBI:An actual textbox background.
            maxVerticalLines = (int)(position.Height/ font.MeasureString("The quick brown fox jumps over the lazy dog 0123456789!@#$%^&*{}[]()\"'\\").Y) + 4;
            textLines = new string[maxVerticalLines];
            for (int i = 0; i < maxVerticalLines; i++)
            {
                textLines[i] = "\n";
            }
        }

        /// <summary>
        /// Formats text to fit properly into text box and remove excess text.
        /// </summary>
        private void formatText()
        {
            int currentLine = 0, i = 0;
            string[] lines = textBuffer.Split('\n');

            while (i < lines.Length)
            {
                if (textLines[currentLine] == "\n")
                {
                    textLines[currentLine] = lines[currentLine];
                    currentLine++;
                }
                else
                {
                    currentLine++;
                    if (currentLine == maxVerticalLines)
                    {
                        for (int j = 1; j < maxVerticalLines; j++)
                        {
                            textLines[j - 1] = textLines[j];
                        }
                        textLines[maxVerticalLines] = lines[i];
                    }
                }
                i++;
            }
            textBuffer = "";
        }

        /// <summary>
        /// Creates room for another line of text.
        /// </summary>
        /// <param name="line">String to be written.</param>
        private void addLine(String line)
        {
            for (int i = maxVerticalLines - 1; i > 0; i--)
            {
                textLines[i] = textLines[i - 1];
            }
            textLines[0] = line;
        }
        
        /// <summary>
        /// Changes the text displayed in the box
        /// </summary>
        /// <param name="newText">New string to display</param>
        public void setText(string newText)
        {
            for (int i = 0; i < maxVerticalLines + 1; i++)
            {
                textLines[i] = "\n";
            }
            addLine(newText);
        }

        /// <summary>
        /// Adds another line of text to the box.
        /// </summary>
        /// <param name="textToAdd">The new line's string.</param>
        public void addText(string textToAdd)
        {
            addLine(textToAdd);
        }

        /// <summary>
        /// Draws the background, then the contained strings.
        /// </summary>
        /// <param name="batch">Main drawing batch</param>
        public void draw(SpriteBatch batch)
        {
            backgroundSprite.Draw(batch, position);
            for (int i = 0; i < maxVerticalLines; i++)
            {
                batch.DrawString(font, textLines[i], new Vector2(position.X, position.Y + (position.Height / maxVerticalLines) * i), Color.Black);
            }
        }
    }
}
