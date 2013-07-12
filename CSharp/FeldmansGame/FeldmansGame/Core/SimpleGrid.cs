using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mainframe.Constants;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Constants.Editor;
using Mainframe.Core.Units;

namespace Mainframe.Core
{
    /// <summary>
    /// A simplification of the Grid class to use in editing. Different updating, less footprint, and less setup.
    /// </summary>
    public class SimpleGrid
    {
        protected SimpleGridSpace[,] spaceArray;  //Array of the grid spaces
        protected bool[, , ,] connections;  //Pathing adjacency matrix
        protected int[, , ,] movementCosts; //Pathing cost matrix
        protected int sizeX, sizeY, numSpaces;  //Simple sizing holders
        //x dimension of the grid, y dimension of the grid, total size of the grid
        protected Vector2 maxGridOrigin;    //Farthest the view can move down and right.
        protected Vector2 originScreenPos;
        protected bool largerThanScreenX, largerThanScreenY;    //Tracks whether the grid is too large to fit in the normal view and requires scrolling.
        protected Vector2 numSpacesOffScreen;

        public SimpleGrid(int xDim, int yDim)
        {
            maxGridOrigin = new Vector2(3.0f / 4 * ConstantHolder.HexagonGrid_HexSizeX * (xDim) - ConstantHolder.GAME_WIDTH,
                ConstantHolder.HexagonGrid_HexSizeY * (yDim + 0.5f) - ConstantHolder.GAME_HEIGHT);
            largerThanScreenX = maxGridOrigin.X > 0;
            largerThanScreenY = maxGridOrigin.Y > 0;
            spaceArray = new SimpleGridSpace[xDim, yDim];
            connections = new bool[xDim, yDim, xDim, yDim];
            movementCosts = new int[xDim, yDim, xDim, yDim];
            numSpacesOffScreen = new Vector2();
            sizeX = xDim;
            sizeY = yDim;
            if (sizeX * ConstantHolder.HexagonGrid_HexSizeX * 3 / 4 > ConstantHolder.GAME_WIDTH)
            {
                numSpacesOffScreen.X = (1 + (sizeX * ConstantHolder.HexagonGrid_HexSizeX * 3 / 4 - ConstantHolder.GAME_WIDTH) / (4/3 * ConstantHolder.HexagonGrid_HexSizeX));
            }
            if ((sizeY + 0.5f) * ConstantHolder.HexagonGrid_HexSizeY > ConstantHolder.GAME_HEIGHT)
            {
                numSpacesOffScreen.Y = 10 + ((sizeY + 0.5f) * ConstantHolder.HexagonGrid_HexSizeY - ConstantHolder.GAME_HEIGHT) / (ConstantHolder.HexagonGrid_HexSizeY );
            }
        }

        /// <summary>
        /// Sets the movement cost between two points in the grid. Will set to 0 if the connection is not available (connections[xSource, ySource, xTarget, yTarget] is false)
        /// </summary>
        /// <param name="xSource">X position of the source location</param>
        /// <param name="ySource">Y position of the source location</param>
        /// <param name="xTarget">X position of the target location</param>
        /// <param name="yTarget">Y position of the target location</param>
        /// <param name="cost">Cost of the movement from the source to the target, if the two are connected.</param>
        public void setMoveCost(int xSource, int ySource, int xTarget, int yTarget, int cost = 0)
        {
            if (withinGrid(xTarget, yTarget))
            {
                connections[xSource, ySource, xTarget, yTarget] = cost > 0;
                movementCosts[xSource, ySource, xTarget, yTarget] = connections[xSource, ySource, xTarget, yTarget] ? cost : 0;
            }
        }

        /// <summary>
        /// Checks is an input position is within the grid, to avoid OBOEs.
        /// </summary>
        /// <param name="X">Horizontal position to check.</param>
        /// <param name="Y">Vertical position to check</param>
        /// <returns>True if the position input will be valid in the grid.</returns>
        public bool withinGrid(float X, float Y)
        {
            return X >= 0 && Y >= 0 && X + 1 <= sizeX && Y + 1 <= sizeY;
        }

        /// <summary>
        /// Checks whether an input position is within the grid.
        /// </summary>
        /// <param name="position">Position to be checked.</param>
        /// <returns>True if the position is valid within the array.</returns>
        private bool withinGrid(Vector2 position)
        {
            return position.X >= 0 && position.Y >= 0 && position.X + 1 <= sizeX && position.Y + 1 <= sizeY;
        }


        #region Drawing
        /// <summary>
        /// Draws the grid, as well as any actors in that grid, then draws the minimap. This is the root draw function for a level's information.
        /// Does not draw the GUI
        /// </summary>
        /// <param name="batch">Drawing spritebatch</param>
        public void draw(SpriteBatch batch)
        {
            originScreenPos.X = (3.0f / 4 * EditorConstantHolder.editorScrollPosX * ConstantHolder.HexagonGrid_HexSizeX) - 10;
            originScreenPos.Y = (EditorConstantHolder.editorScrollPosY * ConstantHolder.HexagonGrid_HexSizeY / 2) - 10;
            float spaceOffsetX = 0, spaceOffsetY = 0, gridPosOffsetX = (3.0f / 4 * EditorConstantHolder.editorScrollPosX * ConstantHolder.HexagonGrid_HexSizeX) - 10, 
                gridPosOffsetY = (EditorConstantHolder.editorScrollPosY * ConstantHolder.HexagonGrid_HexSizeY / 2) - 10;
            Vector2 drawPos;
            for (int i = 0; i < SizeY; i++)
            {
                //Draw the even rows
                for (int j = 0; j < SizeX; j += 2)
                {   //Draws across first, then down.
                    //Prevents overwriting on the left edges
                    drawPos = new Vector2(spaceOffsetX - gridPosOffsetX, spaceOffsetY - gridPosOffsetY);
                    spaceArray[j, i].draw(batch, drawPos - (spaceArray[j, i].Elevation * new Vector2(0, ConstantHolder.HeightOffsetY)));
                    spaceOffsetX += ConstantHolder.HexagonGrid_HexSizeX * 3 / 2;
                }
                //Offset to the odd row origin
                spaceOffsetX = 3 * ConstantHolder.HexagonGrid_HexSizeX / 4;
                spaceOffsetY += ConstantHolder.HexagonGrid_HexSizeY / 2;
                //Draw the odd rows
                for (int k = 1; k < (SizeX - (SizeX % 2)); k += 2)
                {
                    drawPos = new Vector2(spaceOffsetX - gridPosOffsetX, spaceOffsetY - gridPosOffsetY);
                    spaceArray[k, i].draw(batch, drawPos - (spaceArray[k, i].Elevation * new Vector2(0, ConstantHolder.HeightOffsetY)));
                    spaceOffsetX += ConstantHolder.HexagonGrid_HexSizeX * 3 / 2;
                }

                //offset for the next row
                spaceOffsetX = 0;
                spaceOffsetY += ConstantHolder.HexagonGrid_HexSizeY / 2;
            }
        }
        #endregion



        #region AccessFunctions
        /// <summary>
        /// Returns the hero that can be found at the screen position given
        /// </summary>
        /// <param name="screenPos">Selection position on screen</param>
        /// <returns>Hero or Null</returns>
        public Hero heroAtScreenPos(Vector2 screenPos)
        {
            SimpleGridSpace gs = spaceAtScreenpos(screenPos);
            if (gs.IsOccupied && gs.CurrentActor is Hero)
                return (Hero)gs.CurrentActor;
            return null;
        }

        /// <summary>
        /// Returns the person in the grid space currently being moused over.
        /// </summary>
        /// <param name="screenPos">Selection position on screen</param>
        /// <returns>Person or Null</returns>
        public Person personAtScreenPos(Vector2 screenPos)
        {
            SimpleGridSpace gs = spaceAtScreenpos(screenPos);
            if (gs != null && gs.IsOccupied && gs.CurrentActor is Person)
                return (Person)gs.CurrentActor;
            return null;
        }

        /// <summary>
        /// Gets the space currently being moused over
        /// </summary>
        /// <param name="screenPos">Selection position on screen</param>
        /// <returns>GridSpace or empty space</returns>
        public SimpleGridSpace spaceAtScreenpos(Vector2 screenPos)
        {
            //potential TBI: More accurate math for handling corners. Potential idea: circular distance to 4 nearest points, choose closest one. Still has inaccuracies, but much less than current 
            int xDim = (int)((screenPos.X + originScreenPos.X) / (ConstantHolder.HexagonGrid_HexSizeX * 3 / 4));
            float yPosAdjusted = xDim % 2 == 0 ? screenPos.Y + originScreenPos.Y : screenPos.Y + originScreenPos.Y - (ConstantHolder.HexagonGrid_HexSizeY / 2);
            int yDim = (int)(yPosAdjusted / ConstantHolder.HexagonGrid_HexSizeY);
            if (xDim >= 0 && xDim < sizeX && yDim >= 0 && yDim < sizeY)
                return spaceArray[xDim, yDim];
            else
            {
                return null;
            }
        }
        #endregion

        public int SizeX
        {
            get { return sizeX; }
        }

        public int SizeY
        {
            get { return sizeY; }
        }

        public int scrollNotchesX
        {
            get { return 2 * (int)numSpacesOffScreen.X; }
        }

        public int scrollNotchesY
        {
            get { return 2 * (int)numSpacesOffScreen.Y; }
        }

        public SimpleGridSpace[,] GridSpaces
        {
            get { return spaceArray; }
        }

        public int[, , ,] MoveCosts
        {
            get { return movementCosts; }
        }
    }
}
