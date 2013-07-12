using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mainframe.Constants;

namespace Mainframe.Animations
{
    /// <summary>
    /// Maintains a full sprite sheet, including the size of each sprite, the number of columns, and the number of frames in each column.
    /// Allows controls on visibility and translucency (by percent).
    /// </summary>
    public class Sprite
    {
	
        protected int[] columnHeights;
        protected Texture2D spriteSheet;
        protected Vector2 spriteSize;
        protected int currColumn, currRow = 0;
        protected bool bVisible;
        protected float alpha = 1;

        /// <summary>
        /// Sprite constructor.
        /// </summary>
        /// <param name="spritesheet">Full texture containing animations</param>
        /// <param name="spritesize">Constant size for the sprite</param>
        /// <param name="columnHeights">Individual column heights</param>
        public Sprite(Texture2D spritesheet, Vector2 spritesize, int[] columnHeights)
        {
            currColumn = 0;
            bVisible = true;
            this.spriteSheet = spritesheet;
            this.spriteSize = spritesize;
            this.columnHeights = columnHeights;
        }

        /// <summary>
        /// Draws from the given Vector Origin. Automatically advances the animation.
        /// </summary>
        /// <param name="batch">Spritebatch currently drawing</param>
        /// <param name="origin">Middle bottom point of the sprite (Following project convention)</param>
        /// <returns>True if the sprite has just completed an animation column, false otherwise.</returns>
        public bool Draw(SpriteBatch batch, Vector2 origin)
        {

            Vector2 posVector = origin - new Vector2(spriteSize.X / 2, spriteSize.Y);
            if (bVisible)
            {
                Rectangle sourceArea = new Rectangle((int)(currColumn * spriteSize.X),
                                                    (int)((currRow++) / ConstantHolder.FrameLength * spriteSize.Y), 
                                                    (int)(spriteSize.X), 
                                                    (int)(spriteSize.Y));
                batch.Draw(spriteSheet, posVector, sourceArea, Color.White * alpha);
                if (currRow / ConstantHolder.FrameLength >= columnHeights[currColumn]) currRow = 0;
            }
            return currRow == 0;
        }

        /// <summary>
        /// Draws to the given Rectangle. Automatically advances the animation.
        /// </summary>
        /// <param name="batch">Spritebatch currently drawing</param>
        /// <param name="targetArea">Target rectangle, given from the top left corner</param>
        /// <returns>True if the sprite has just completed an animation column, false otherwise.</returns>
        public bool Draw(SpriteBatch batch, Rectangle targetArea)
        {
            if (bVisible)
            {
                Rectangle sourceArea = new Rectangle((int)(currColumn * spriteSize.X),
                                                    (int)((currRow++) / ConstantHolder.FrameLength * spriteSize.Y),
                                                    (int)(spriteSize.X),
                                                    (int)(spriteSize.Y));
                batch.Draw(spriteSheet, targetArea, sourceArea, Color.White * alpha);
                if (currRow / ConstantHolder.FrameLength >= columnHeights[currColumn]) currRow = 0;
            }
            return currRow == 0 && bVisible;
        }

        /// <summary>
        /// Draws the Sprite, with the effects given, to the area given.
        /// </summary>
        /// <param name="batch">Main drawing spritebatch</param>
        /// <param name="targetArea">Area to which the sprite will be drawn</param>
        /// <param name="effect">Choose whether to flip the sprite in some way.</param>
        /// <returns>True if the current columns' last frame has just been drawn</returns>
        public bool Draw(SpriteBatch batch, Rectangle targetArea, SpriteEffects effect)
        {
            if (bVisible)
            {
                Rectangle sourceArea = new Rectangle((int)(currColumn * spriteSize.X),
                                                    (int)((currRow++) / ConstantHolder.FrameLength * spriteSize.Y),
                                                    (int)(spriteSize.X),
                                                    (int)(spriteSize.Y));
                batch.Draw(spriteSheet, targetArea, sourceArea, Color.White * alpha, 0, spriteSize / 2, effect, 0);
                if (currRow / ConstantHolder.FrameLength >= columnHeights[currColumn]) currRow = 0;
            }
            return currRow == 0 && bVisible;
        }

        /// <summary>
        /// Draws the sprite's next frame, to the target area, rotated as identified.
        /// </summary>
        /// <param name="batch">Main drawing spritebatch</param>
        /// <param name="targetArea">Area to which the sprite will aim before rotation around its center.</param>
        /// <param name="rotation">amount in degrees that the sprite will rotate</param>
        /// <returns></returns>
        public bool Draw(SpriteBatch batch, Rectangle targetArea, float rotation)
        {
            if (bVisible)
            {
                Rectangle sourceArea = new Rectangle((int)(currColumn * spriteSize.X),
                                                    (int)((currRow++) / ConstantHolder.FrameLength * spriteSize.Y),
                                                    (int)(spriteSize.X),
                                                    (int)(spriteSize.Y));
                batch.Draw(spriteSheet, targetArea, sourceArea, Color.White * alpha, (float)(Math.PI * rotation / 180), spriteSize / 2, SpriteEffects.None, 0);
                if (currRow / ConstantHolder.FrameLength >= columnHeights[currColumn]) currRow = 0;
            }
            return currRow == 0 && bVisible;
        }

        /// <summary>
        /// Draws to the given Rectangle, taking a subsection of the sprite given by the Vector. Automatically advances the animation.
        /// </summary>
        /// <param name="batch">Spritebatch currently drawing</param>
        /// <param name="targetArea">Target rectangle, given from the top left corner</param>
        /// <param name="subsectionRatio">Ratio of the area to take divided by the dimensions of a single sprite</param>
        /// <returns>True if the sprite has just completed an animation column, false otherwise.</returns>
        public bool Draw(SpriteBatch batch, Rectangle targetArea, Vector2 subsectionRatio)
        {
            if (bVisible)
            {
                targetArea.Width = (int)((float)targetArea.Width * subsectionRatio.X);
                targetArea.Height = (int)((float)targetArea.Height * subsectionRatio.Y);
                Rectangle sourceArea = new Rectangle((int)(currColumn * spriteSize.X),
                                                    (int)((currRow++) / ConstantHolder.FrameLength * spriteSize.Y),
                                                    (int)((float)spriteSize.X * subsectionRatio.X),
                                                    (int)((float)spriteSize.Y * subsectionRatio.Y));
                batch.Draw(spriteSheet, targetArea, sourceArea, Color.White * alpha);
                if (currRow / ConstantHolder.FrameLength >= columnHeights[currColumn]) currRow = 0;
            }
            return currRow == 0 && bVisible;
        }

        /// <summary>
        /// Draws to the given Rectangle. Automatically advances the animation.
        /// </summary>
        /// <param name="batch">Spritebatch currently drawing</param>
        /// <param name="color">Color filter to apply</param>
        /// <param name="targetArea">Target rectangle, given from the top left corner</param>
        /// <returns>True if the sprite has just completed an animation column, false otherwise.</returns>
        public bool Draw(SpriteBatch batch, Rectangle targetArea, Color color)
        {
            if (bVisible)
            {
                Rectangle sourceArea = new Rectangle((int)(currColumn * spriteSize.X),
                                                    (int)((currRow++) / ConstantHolder.FrameLength * spriteSize.Y),
                                                    (int)(spriteSize.X),
                                                    (int)(spriteSize.Y));
                batch.Draw(spriteSheet, targetArea, sourceArea, color * alpha);
                if (currRow / ConstantHolder.FrameLength >= columnHeights[currColumn]) currRow = 0;
            }
            return currRow == 0 && bVisible;
        }

        public Sprite copySprite()
        {
            return new Sprite(spriteSheet, spriteSize, columnHeights);
        }

        /// <summary>
        /// Change the sprite animation to a new column. Resets row count.
        /// </summary>
        /// <param name="newColumn">Position of the new column. Pass in an enum, cast as an int, to keep track.</param>
        public void changeColumn(int newColumn)
        {
            currColumn = newColumn;
            currRow = 0;
        }

        /// <summary>
        /// Will only draw when true.
        /// </summary>
        public bool IsVisible
        {
            get { return bVisible; }
            set { bVisible = value; }
        }

        /// <summary>
        /// Returns the width of the sprite. Unsettable.
        /// </summary>
        public int SpriteSizeX
        {
            get { return (int) spriteSize.X; }
        }

        /// <summary>
        /// Returns the height of the sprite. Unsettable.
        /// </summary>
        public int SpriteSizeY
        {
            get { return (int) spriteSize.Y; }
        }

        /// <summary>
        /// Sets the transparency level. 0 for completely transparent, 100 for opaque.
        /// </summary>
        public float percentVisible
        {
            get { return alpha * 100.0f; }
            set { alpha = value / 100.0f; }
        }

        /// <summary>
        /// The current column this sprite is animating through.
        /// </summary>
        public int CurrentColumn
        {
            get { return currColumn; }
        }
    }
}
