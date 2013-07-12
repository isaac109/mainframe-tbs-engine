using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mainframe.Animations.Environment;
using Mainframe.Core.Units;
using Mainframe.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Constants;

namespace Mainframe.Core
{
    public class SimpleGridSpace
    {
        protected Actor currentActor;
        protected Sprite spaceSprite;
        protected bool isOccupied;
        protected int moveCost, elevation, spaceType;
        protected Vector2 gridPos;
        protected SimpleGrid parent;
        protected Wall[] walls;
        EnvironmentTextureHolder holder;

        /// <summary>
        /// Creates a grid space that considers itself at position (posX, posY) in the parent grid.
        /// </summary>
        /// <param name="spaceTypeSelector">Chooses the texture for this space. Does not affect the properties of it otherwise.</param>
        /// <param name="posX">Horizontal position in the grid</param>
        /// <param name="posY">Vertical position in the grid</param>
        /// <param name="elevation"></param>
        /// <param name="Parent"></param>
        public SimpleGridSpace(int spaceTypeSelector, int posX, int posY, int elevation, SimpleGrid Parent)
        {
            holder = EnvironmentTextureHolder.Holder;
            currentActor = null;
            isOccupied = false;
            moveCost = 1;
            parent = Parent;
            gridPos = new Vector2(posX, posY);
            spaceSprite = holder.MainHexagonSprites[spaceTypeSelector].copySprite();
            spaceType = spaceTypeSelector;
            walls = new Wall[3];
            for (int i = 0; i < 3; ++i)
            {
                walls[i] = new Wall();
            }
            this.elevation = elevation;
        }

        public void putActor(Actor toPut)
        {
            if (!isOccupied)
            {
                isOccupied = true;
                currentActor = toPut;
            }
        }

        public void setSprite(int newSpriteIndex)
        {
            spaceSprite = holder.MainHexagonSprites[newSpriteIndex];
        }


        #region Drawing
        /// <summary>
        /// Draws all of the space's sprites, then if it's occupied, its occupant's sprites.
        /// </summary>
        /// <param name="batch">Main game spritebatch.</param>
        /// <param name="drawPos">Top left corner from which to draw</param>
        public void draw(SpriteBatch batch, Vector2 drawPos)
        {
            spaceSprite.Draw(batch, new Rectangle((int)drawPos.X, (int)drawPos.Y,
                (int)ConstantHolder.HexagonGrid_HexSizeX, spaceSprite.SpriteSizeY), Color.White);
            for (int i = 0; i < 3; ++i)
            {
                walls[i].Draw(batch, drawPos);
            }
            if (isOccupied)
            {
                CurrentActor.Draw(batch, drawPos + new Vector2(ConstantHolder.HexagonGrid_HexSizeX / 2, ConstantHolder.HexagonGrid_HexSizeY));
            }
        }
        #endregion

        public bool IsOccupied
        {
            get { return isOccupied; }
        }

        public int SpaceType
        {
            get { return spaceType; }
            set { spaceType = value; }
        }

        public Actor CurrentActor
        {
            get { return currentActor; }
        }

        public Wall[] Walls
        {
            get { return walls; }
        }

        public int Elevation
        {
            get { return elevation; }
            set { elevation = value; }
        }

        public Vector2 GridPosition
        {
            get { return gridPos; }
        }
    }
}
