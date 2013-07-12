using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Reflection.Emit;
using Mainframe.Constants;
using Mainframe.Animations;
using Mainframe.Animations.Character;

namespace Mainframe.Core.Units
{
    /// <summary>
    /// Base class for any object which sits within the grid.
    /// </summary>
    public class Actor
    {
#region Variables
        protected bool bVisible = true;
        protected String name;

        private enum columnNames : int
        {
            idle = 0,
        }

        protected List<Sprite> actorSprites, minimapSprites, temporarySprites;
        protected Vector2 screenPosition, gridPosition;
        protected Grid parent;

#endregion

#region Setup
        /// <summary>
        /// Creates the Actor and its sprites using the TextureHolder class. Calls populateSpriteList().
        /// After creating, be sure to call tryPutActor from the GridSpace it will be assigned to.
        /// </summary>
        public Actor()
        {
            actorSprites = new List<Sprite>();
            minimapSprites = new List<Sprite>();
            temporarySprites = new List<Sprite>();
            populateSpriteList();
        }

        public Actor(String Name)
        {
            this.name = Name;
            actorSprites = new List<Sprite>();
            minimapSprites = new List<Sprite>();
            temporarySprites = new List<Sprite>();
            populateSpriteList();
        }

        /// <summary>
        /// Puts all the necessary sprites into this actor's list, so that it will be drawn.
        /// </summary>
        protected virtual void populateSpriteList()
        {
            //TBI: Override this method to take in input, so that we can more easily choose sprites. Probably take in int, which will be converted from TextureHolder enums.
            actorSprites.Add(ActorTextureHolder.Holder.MainSprites[(int)ConstantHolder.ActorImageTypes.BaseActor].copySprite());
        }

        protected virtual void populateSpriteList(int textureSelectionMain, int textureSelectionMinimap)
        {
            actorSprites.Add(ActorTextureHolder.Holder.MainSprites[textureSelectionMain].copySprite());
            minimapSprites.Add(ActorTextureHolder.Holder.MinimapSprites[textureSelectionMinimap].copySprite());
        }
#endregion

#region Updating
        /// <summary>
        /// Runs frame-to-frame updates.
        /// </summary>
        public virtual void Update()
        {
        }

#endregion

#region Interactions
        /// <summary>
        /// Changes the back-reference so that this Actor knows to which grid it belongs.
        /// </summary>
        /// <param name="newParent"></param>
        public void setParent(Grid newParent)
        {
            parent = newParent;
        }

        /// <summary>
        /// Put this actor into the given grid, at the given position 
        /// </summary>
        /// <param name="gridPos">position of the hexagon into which this is being placed (grid, not screen position)</param>
        public void setPosition(Vector2 gridPos)
        {
            gridPosition = gridPos;
            screenPosition = new Vector2((gridPosition.X + 0.5f) * 3 / 4 * ConstantHolder.HexagonGrid_HexSizeX, 
                (gridPosition.Y + 1) * ConstantHolder.HexagonGrid_HexSizeY) 
                - parent.CurrentScreenOffset;
            if (gridPosition.X % 2 == 1)
            {
                screenPosition.Y += ConstantHolder.HexagonGrid_HexSizeY / 2;
            }
        }

#endregion

        #region Drawing
        /// <summary>
        /// Draws the Actor at its internal position on the screen
        /// </summary>
        /// <param name="batch">Spritebatch being used to draw. DOES NOT OPEN OR CLOSE THE BATCH</param>
        public virtual void Draw(SpriteBatch batch)
        {
            foreach (Sprite thisSprite in actorSprites)
            {
                if (thisSprite.IsVisible)
                    thisSprite.Draw(batch, screenPosition);
            }
            foreach (Sprite tempSprite in temporarySprites)
            {
                if (tempSprite.IsVisible)
                {
                    if (tempSprite.Draw(batch, screenPosition))
                    {
                        temporarySprites.Remove(tempSprite);
                    }
                }
            }
        }
        /// <summary>
        /// Draws the Actor at the given position on the screen
        /// </summary>
        /// <param name="batch">Spritebatch being used to draw. DOES NOT OPEN OR CLOSE THE BATCH</param>
        public virtual void Draw(SpriteBatch batch, Vector2 drawPosition)
        {
            foreach (Sprite thisSprite in actorSprites)
            {
                if (thisSprite.IsVisible)
                    thisSprite.Draw(batch, drawPosition);
            }
            foreach (Sprite tempSprite in temporarySprites)
            {
                if (tempSprite.IsVisible)
                {
                    if (tempSprite.Draw(batch, drawPosition))
                    {
                        temporarySprites.Remove(tempSprite);
                    }
                }
            }
        }

        /// <summary>
        /// Virtual draw method to put people on the minimap. Left empty so that only important things show on the map.
        /// </summary>
        /// <param name="batch">Main drawing spritebatch</param>
        /// <param name="position">Top left corner of the containing hexagon</param>
        /// <param name="dimensions">Dimensions of the containing hexagon's bounding box.</param>
        public virtual void DrawMinimap(SpriteBatch batch, Vector2 position, Vector2 dimensions) { }

#endregion

#region Accessors/Mutators

        public Vector2 ScreenPosition
        {
            get { return screenPosition; }
            set { screenPosition = value; }
        }

        public Vector2 ScreenPositionNoOffset
        {
            get { return screenPosition + parent.CurrentScreenOffset; }
        }

        public Vector2 GridPosition
        {
            get { return gridPosition; }
        }

        public List<Sprite> TemporarySprites
        {
            get { return temporarySprites; }
            set { temporarySprites = value; }
        }

        public String Name
        {
            get { return name; }
        }
#endregion
    }
}
