using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mainframe.Core.Units;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mainframe.Constants;
using Mainframe.Animations;
using Mainframe.Core.Combat;
using Mainframe.Animations.Environment;

namespace Mainframe.Core
{
    /// <summary>
    /// A single space on which cover/characters can reside.
    /// </summary>
    public class GridSpace
    {
        protected Actor currentActor;
        protected Sprite spaceSprite, minimapSprite;
        protected bool isOccupied, availableFlag;   //isOccupied stores whether currentActor currently points to a value. availableFlag indicates whether this space is marked by the grid to be highlighted.
        protected int moveCost, elevation, spaceType;
        protected Vector2 gridPos;
        protected Grid parent;
        protected List<SpaceProjectile> projectilesInSpace;
        protected Wall[] walls;

        /// <summary>
        /// Enumerator used for tracking different sprite columns for animation purposes.
        /// </summary>
        private enum AnimationColumns
        {
            normal = 0,
            puzzleActive,
        }

        /// <summary>
        /// Default constructor for an un-positioned gridspace
        /// </summary>
        /// <param name="parentGrid">Grid of which to be a technical, but undrawn and inaccessible, member.</param>
        public GridSpace(Grid parentGrid)
        {
            currentActor = null;
            isOccupied = false;
            moveCost = 1;
            parent = parentGrid;
            walls = new Wall[3];
        }

        /// <summary>
        /// Creates a grid space that considers itself at position (posX, posY) in the parent grid.
        /// </summary>
        /// <param name="spaceTypeSelector">Chooses the texture for this space. Does not affect the properties of it otherwise.</param>
        /// <param name="posX">Horizontal position in the grid</param>
        /// <param name="posY">Vertical position in the grid</param>
        /// <param name="elevation"></param>
        /// <param name="Parent"></param>
        public GridSpace(int spaceTypeSelector, int posX, int posY, int elevation, Grid Parent)
        {
            EnvironmentTextureHolder holder = EnvironmentTextureHolder.Holder;
            currentActor = null;
            isOccupied = false;
            moveCost = 1;
            parent = Parent;
            projectilesInSpace = new List<SpaceProjectile>();
            gridPos = new Vector2(posX, posY);
            spaceSprite = holder.MainHexagonSprites[spaceTypeSelector].copySprite();
            minimapSprite = holder.MinimapHexagonSprites[spaceTypeSelector].copySprite();
            spaceType = spaceTypeSelector;
            walls = new Wall[3];
            for (int i = 0; i < 3; ++i)
            {
                walls[i] = new Wall();
            }
            this.elevation = elevation;
        }

#region Updating

        /// <summary>
        /// Runs frame to frame management operations.
        /// </summary>
        public void Update()
        {
            foreach (SpaceProjectile proj in projectilesInSpace)
            {
                proj.Update();
                if (proj.DeleteFlag)
                {
                    projectilesInSpace.Remove(proj);
                }
            }
        }

#endregion
#region Interaction
        /// <summary>
        /// Look for an actor in this space.
        /// </summary>
        /// <returns>The actor in this space, or null.</returns>
        public Actor tryGetActor()
        {
            if (isOccupied)
                return currentActor;
            return null;
        }

        /// <summary>
        /// Attempts to put an actor in this space.
        /// </summary>
        /// <param name="newOccupant">Actor to be placed</param>
        /// <returns>True if the space was empty and now has the new occupant, false if it was already occupied.</returns>
        public bool tryPutActor(Actor newOccupant)
        {
            if (!isOccupied)
            {
                currentActor = newOccupant;
                currentActor.setParent(parent);
                currentActor.setPosition(gridPos);
            }
            else
                return false;
            isOccupied = true;
            return true;
        }

        /// <summary>
        /// Get rid of the actor in this space.
        /// </summary>
        /// <returns>The removed actor</returns>
        public Actor removeActor()
        {
            Actor toReturn = currentActor;
            isOccupied = false;
            currentActor = null;
            return toReturn;
        }

        /// <summary>
        /// Puts a projectile into this space to be drawn.
        /// </summary>
        /// <param name="newProj">The new Projectile to be inserted.</param>
        public void AddProjectile(SpaceProjectile newProj)
        {
            projectilesInSpace.Add(newProj);
        }

#endregion

#region Drawing
        /// <summary>
        /// Draws all of the space's sprites, then if it's occupied, its occupant's sprites.
        /// </summary>
        /// <param name="batch">Main game spritebatch.</param>
        /// <param name="drawPos">Top left corner from which to draw</param>
        public void draw(SpriteBatch batch, Vector2 drawPos)
        {
            spaceSprite.Draw(batch, new Rectangle((int)drawPos.X, (int)drawPos.Y, 
                (int)ConstantHolder.HexagonGrid_HexSizeX, spaceSprite.SpriteSizeY), availableFlag ? Color.YellowGreen : Color.White);
            for (int i = 0; i < 3; ++i)
            {
                walls[i].Draw(batch, drawPos);
            }
            if (isOccupied)
            {
                CurrentActor.Draw(batch);
            }
            foreach(SpaceProjectile proj in projectilesInSpace)
            {
                proj.Draw(batch, drawPos);
            }
        }

        public void drawMinimap(SpriteBatch batch, Vector2 drawPos, Vector2 dimensions)
        {
            minimapSprite.Draw(batch, new Rectangle((int) drawPos.X, (int) drawPos.Y, (int) dimensions.X, (int) dimensions.Y));
            if (isOccupied)
            {
                currentActor.DrawMinimap(batch, drawPos, dimensions);
            }
        }
#endregion


#region Accessors/Mutators
        public Actor CurrentActor
        {
            get { return currentActor; }
        }

        public int SpaceType
        {
            get { return spaceType; }
        }

        public bool IsOccupied
        {
            get { return isOccupied; }
        }

        public Vector2 GridPosition
        {
            get { return gridPos; }
        }

        public int Elevation
        {
            get { return elevation; }
        }

        public bool AvailableFlag
        {
            get { return availableFlag; }
            set { availableFlag = value; }
        }

        public Wall[] Walls
        {
            get { return walls; }
        }
#endregion
    }
}
