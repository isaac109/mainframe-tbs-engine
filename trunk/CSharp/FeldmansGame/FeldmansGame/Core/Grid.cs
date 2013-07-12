using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mainframe.Constants;
using Mainframe.Core.Units;
using Mainframe.Animations;
using Microsoft.Xna.Framework.Input;
using Mainframe.Animations.Interface;

namespace Mainframe.Core
{
    /// <summary>
    /// The main container for a single level's active data.
    /// </summary>
    public class Grid
    {
        public enum GridState
        {
            noSelection = 0,
            selectedMotionAvailable,
            selectedMoving,
            selectedCastAvailable,
            castAnimating,
        }

        GridState gridState = GridState.noSelection;

        protected GridSpace[,] spaceArray;  //Array of the grid spaces
        protected bool[, , ,] connections;  //Pathing adjacency matrix
        protected int[, , ,] movementCosts; //Pathing cost matrix
        bool[,] visited;                    //Dijkstra tracking array;
        protected int sizeX, sizeY, numSpaces;  //Simple sizing holders
        //x dimension of the grid, y dimension of the grid, total size of the grid
        protected Vector2 originScreenPos,  //Current top left corner position for the grid
            minimapOrigin,                  //top left corner of the minimap's drawing
            minimapSpaceDim;                //Size of the minimap
        protected Vector2 minGridOrigin = new Vector2(ConstantHolder.GAME_WIDTH / -10, ConstantHolder.GAME_HEIGHT / -10 ), maxGridOrigin;
        protected Rectangle selectionRect = new Rectangle();
        protected Vector2 selectionRectDefaultSize = new Vector2();
        protected float mGridspaceWidth;   //size of a minimap hexagon
        protected Sprite minimapBackground, minimapSelect;
        protected Person selectedPerson;
        protected List<Vector2> availableSpaces;        //Spaces to which the selected actor may move, or the selected spell may be cast.
        protected LinkedList<Vector2> currentPath;
        protected bool acceptInput = true;
        protected Actor animatingActor;
        protected Vector2 animatingActorScreenPos;
        protected GridSpace animationFinalGoal;
        protected Vector2 animationCurrentGoal;

#region Construction
        /// <summary>
        /// Grid constructor, X by Y
        /// </summary>
        /// <param name="xDim">Width</param>
        /// <param name="yDim">Height</param>
        public Grid(int xDim, int yDim)
        {
            originScreenPos = minGridOrigin;    //Defaults the view to the top left
            maxGridOrigin = new Vector2(3.0f / 4 * ConstantHolder.HexagonGrid_HexSizeX * (xDim) - ConstantHolder.GAME_WIDTH, 
                ConstantHolder.HexagonGrid_HexSizeY * (yDim + 0.5f) - ConstantHolder.GAME_HEIGHT);
                //Set the grid position limits to keep it on the screen.
            maxGridOrigin.X += ConstantHolder.HexagonGrid_HexSizeX / 4 + ConstantHolder.GAME_WIDTH / 10;
                //Allow more distance, to let players see the edges more easily
            maxGridOrigin.Y += ConstantHolder.GAME_HEIGHT / 4;
            spaceArray = new GridSpace[xDim, yDim];
            connections = new bool[xDim, yDim, xDim, yDim];
            movementCosts = new int[xDim, yDim, xDim, yDim];
            sizeX = xDim;
            sizeY = yDim;
            numSpaces = sizeX * sizeY;
            Vector2 minimapDimensions = new Vector2(ConstantHolder.GAME_WIDTH * ConstantHolder.relativeMinimapSizeX,
                ConstantHolder.GAME_HEIGHT * ConstantHolder.relativeMinimapSizeY);
            float emptySpaceWidth;
            if (minimapDimensions.X / ((sizeX * 3.0 + 1)/ 4) > minimapDimensions.Y / (sizeY + 0.5f))
            {
                mGridspaceWidth = minimapDimensions.Y / (sizeY + 0.5f);
                minimapOrigin.Y = ConstantHolder.GAME_HEIGHT * (1 - ConstantHolder.relativeMinimapSizeY);
                emptySpaceWidth = minimapDimensions.X - (( 3.0f / 4 * sizeX + 0.25f) * mGridspaceWidth);
                minimapOrigin.X = emptySpaceWidth / 2 + ConstantHolder.GAME_WIDTH * (1 - ConstantHolder.relativeMinimapSizeX);
            }
            else            //This choice will determine whether the minimap fills horizontally (else) or vertically (if)
            {
                mGridspaceWidth = minimapDimensions.X / ((sizeX * 3.0f  + 1)/ 4);
                minimapOrigin.X = ConstantHolder.GAME_WIDTH * (1 - ConstantHolder.relativeMinimapSizeX);
                emptySpaceWidth = minimapDimensions.Y - ((sizeY + 0.5f)  * mGridspaceWidth);
                minimapOrigin.Y = emptySpaceWidth / 2 + ConstantHolder.GAME_HEIGHT * (1 - ConstantHolder.relativeMinimapSizeY);
            }
            minimapSpaceDim = new Vector2(mGridspaceWidth, mGridspaceWidth);
            selectionRect.Width = (int)(mGridspaceWidth * (float)ConstantHolder.GAME_WIDTH / (ConstantHolder.HexagonGrid_HexSizeX));
            selectionRectDefaultSize.X = selectionRect.Width;
            selectionRect.Height = (int)(mGridspaceWidth * ((float)ConstantHolder.GAME_HEIGHT / (ConstantHolder.HexagonGrid_HexSizeY)));
            selectionRectDefaultSize.Y = selectionRect.Height;
            InterfaceTextureHolder holder = InterfaceTextureHolder.Holder;
            minimapBackground = holder.GUISprites[(int)ConstantHolder.GUIMemberImages.MinimapBackground].copySprite();
            minimapSelect = holder.GUISprites[(int)ConstantHolder.GUIMemberImages.MinimapSelection].copySprite();
            visited = new bool[sizeX, sizeY];
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
            if(withinGrid(xTarget, yTarget))
            {
                connections[xSource, ySource, xTarget, yTarget] = cost > 0;
                movementCosts[xSource, ySource, xTarget, yTarget] = connections[xSource, ySource, xTarget, yTarget] ? cost : 0;
            }
        }

        /// <summary>
        /// Creates the pathing adjacency matrices procedurally. Not for use in game, only testing. Game will load in data.
        /// </summary>
        public void makePathing()
        {   //Has a bunch of if statements, all just for edge-checking.
            //NOTE: Right now, everything is procedurally generated. TBI: Load from file
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    connections[i, j, i, j] = true;
                    movementCosts[i, j, i, j] = 0;
                    int maxElevationDifference = 20;
                    if(withinGrid(i, j-1))
                    {
                        connections[i, j, i, j - 1] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i, j - 1].Elevation) < maxElevationDifference;
                        movementCosts[i, j, i, j - 1] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i, j - 1].Elevation) + 1;
                    }
                    if(withinGrid(i, j+1))
                    {
                        connections[i, j, i, j + 1] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i, j + 1].Elevation) < maxElevationDifference;
                        movementCosts[i, j, i, j + 1] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i, j + 1].Elevation) + 1;
                    }
                    if (withinGrid(i - 1, j))
                    {
                        connections[i, j, i - 1, j] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i - 1, j].Elevation) < maxElevationDifference;
                        movementCosts[i, j, i - 1, j] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i - 1, j].Elevation) + 1;
                    }
                    if (withinGrid(i + 1, j))
                    {
                        connections[i, j, i + 1, j] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i + 1, j].Elevation) < maxElevationDifference;
                        movementCosts[i, j, i + 1, j] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i + 1, j].Elevation) + 1;
                    }
                    int tempJ = j + (i % 2 == 0 ? -1 : 1);
                    if (withinGrid(i - 1, tempJ))
                    {
                        connections[i, j, i - 1, tempJ] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i - 1, tempJ].Elevation) < maxElevationDifference;
                        movementCosts[i, j, i - 1, tempJ] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i - 1, tempJ].Elevation) + 1;
                    } 
                    if (withinGrid(i + 1, tempJ))
                    {
                        connections[i, j, i + 1, tempJ] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i + 1, tempJ].Elevation) < maxElevationDifference;
                        movementCosts[i, j, i + 1, tempJ] = Math.Abs(spaceArray[i, j].Elevation - spaceArray[i + 1, tempJ].Elevation) + 1;
                    }
                }
            }
        }
 
#endregion

#region Updating
        /// <summary>
        /// Move an object through the grid.
        /// </summary>
        /// <param name="sourceToEnd">X and Y indicate source position. Width and Height indicate target position</param>
        public void moveActor(Rectangle sourceToEnd)
        {
            Actor movingActor = spaceArray[sourceToEnd.X, sourceToEnd.Y].tryGetActor();
            if (spaceArray[sourceToEnd.Width, sourceToEnd.Height].tryPutActor(movingActor))
            {
                spaceArray[sourceToEnd.X, sourceToEnd.Y].removeActor();
            }
        }

        /// <summary>
        /// Main update function, based on pre-updated Control static class.
        /// </summary>
        public void updateGrid()
        {
            if (Controls.ButtonsLastDown[(int)Controls.ButtonNames.leftMouse])
            {
                if (Controls.ButtonsDown[(int)Controls.ButtonNames.leftMouse])
                {
                    moveGrid(Controls.MousePosition - Controls.MouseLastPosition);
                }
                else
                {
                    onClick(Controls.MousePosition);
                    
                }
            }
            foreach (GridSpace space in spaceArray)
            {
                space.Update();
            }
            switch (gridState)
            {
                case GridState.castAnimating:
                    break;

                case GridState.noSelection:
                    break;

                case GridState.selectedCastAvailable:
                    break;

                case GridState.selectedMotionAvailable:
                    break;

                case GridState.selectedMoving:
                    if (Vector2.Distance(animationCurrentGoal, animatingActorScreenPos) < 3)
                    {
                        animatingActorScreenPos = animationCurrentGoal;
                        if (currentPath.Count > 0)
                        {
                            animationCurrentGoal = currentPath.First.Value;
                            currentPath.RemoveFirst();
                            if ((int)animationCurrentGoal.X % 2 == 1)
                            {
                                animationCurrentGoal.Y += 0.5f;
                            }
                            animationCurrentGoal.X *= 3 * ConstantHolder.HexagonGrid_HexSizeX / 4;
                            animationCurrentGoal.Y *= ConstantHolder.HexagonGrid_HexSizeY;
                            animationCurrentGoal -= originScreenPos;
                            animationCurrentGoal += new Vector2(ConstantHolder.HexagonGrid_HexSizeX / 3, ConstantHolder.HexagonGrid_HexSizeY);
                        }
                        else
                        {
                            changeState(GridState.noSelection);
                            animationFinalGoal.tryPutActor(animatingActor);
                            animatingActor = null;
                            break;
                        }
                    }
                    Vector2 movement = Vector2.Normalize(animationCurrentGoal - animatingActorScreenPos);
                    movement *= 3;
                    animatingActorScreenPos += movement;
                    break;

            }
        }

        public void onClick(Vector2 screenPosition)
        {
            GridSpace tempGS = spaceAtScreenpos(Controls.MousePosition);
            Person temp = (Person)tempGS.CurrentActor;

            switch (gridState)
            {
                case GridState.noSelection:
                    if (temp != null)
                    {
                        selectedPerson = temp;
                        foreach (GridSpace space in spaceArray)
                        {
                            space.AvailableFlag = false;
                        }
                        availableSpaces = getAvailableMoveSpaces(selectedPerson.BaseMoveSpeed, tempGS.GridPosition, true, false);
                        foreach (Vector2 v in availableSpaces)
                        {
                            spaceArray[(int)v.X, (int)v.Y].AvailableFlag = true;
                        }
                        changeState(GridState.selectedMotionAvailable);
                    }
                    break;

                case GridState.castAnimating:
                    break;

                case GridState.selectedCastAvailable:
                    break;

                case GridState.selectedMotionAvailable:
                    foreach (GridSpace space in spaceArray)
                    {
                        space.AvailableFlag = false;
                    }
                    currentPath = Dijkstra(selectedPerson.GridPosition, tempGS.GridPosition, true, false);
                    int movesDone = 0;
                    if (currentPath.Count > 0)
                    {
                        LinkedListNode<Vector2> current = currentPath.First;
                        while (movesDone < selectedPerson.BaseMoveSpeed && current.Next != null)
                        {
                            movesDone += movementCosts[(int)current.Value.X, (int)current.Value.Y, (int)current.Next.Value.X, (int)current.Next.Value.Y];
                            current = current.Next;
                        }
                        while (current.Next != null)
                        {
                            currentPath.Remove(current.Next);
                        }
                        animationFinalGoal = spaceArray[(int)currentPath.Last.Value.X, (int)currentPath.Last.Value.Y];
                        animationCurrentGoal = currentPath.First.Value;
                        animatingActorScreenPos = selectedPerson.ScreenPosition;
                        currentPath.RemoveFirst();
                        if ((int)animationCurrentGoal.X % 2 == 1)
                        {
                            animationCurrentGoal.Y += 0.5f;
                        }
                        animationCurrentGoal.X *= 3 * ConstantHolder.HexagonGrid_HexSizeX / 4;
                        animationCurrentGoal.Y *= ConstantHolder.HexagonGrid_HexSizeY;
                        animationCurrentGoal -= originScreenPos;
                        animationCurrentGoal += new Vector2(ConstantHolder.HexagonGrid_HexSizeX / 3, ConstantHolder.HexagonGrid_HexSizeY);
                        animatingActor = selectedPerson;
                        spaceArray[(int)selectedPerson.GridPosition.X, (int)selectedPerson.GridPosition.Y].removeActor();
                        selectedPerson = null;
                        changeState(GridState.selectedMoving);
                    }
                    break;

                case GridState.selectedMoving:
                    break;
            }
        }

        /// <summary>
        /// Move the visual grid display
        /// </summary>
        /// <param name="offsetVector">Movement to make.</param>
        public void moveGrid(Vector2 offsetVector)
        {
            originScreenPos -= offsetVector;
            animatingActorScreenPos -= offsetVector;
            if (originScreenPos.X > maxGridOrigin.X)
            {
                animatingActorScreenPos.X -= originScreenPos.X - maxGridOrigin.X;
                originScreenPos.X = maxGridOrigin.X;
            }
            if (originScreenPos.Y > maxGridOrigin.Y)
            {
                animatingActorScreenPos.Y -= originScreenPos.Y - maxGridOrigin.Y;
                originScreenPos.Y = maxGridOrigin.Y;
            }
            if (originScreenPos.X < minGridOrigin.X)
            {
                animatingActorScreenPos.X -= originScreenPos.X - minGridOrigin.X;
                originScreenPos.X = minGridOrigin.X;
            }
            if (originScreenPos.Y < minGridOrigin.Y)
            {
                animatingActorScreenPos.X -= originScreenPos.X - minGridOrigin.X;
                originScreenPos.Y = minGridOrigin.Y;
            }
            animatingActorScreenPos -= offsetVector;
            foreach (GridSpace gs in spaceArray)
            {
                if (gs.IsOccupied)
                    gs.CurrentActor.setPosition(gs.GridPosition);
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

        private void changeState(GridState state)
        {
            gridState = state;
        }
#endregion

#region Drawing
        /// <summary>
        /// Draws the grid, as well as any actors in that grid, then draws the minimap. This is the root draw function for a level's information.
        /// Does not draw the GUI
        /// </summary>
        /// <param name="batch">Drawing spritebatch</param>
        public void draw(SpriteBatch batch)
        {
            float spaceOffsetX = 0, spaceOffsetY = 0, gridPosOffsetX = originScreenPos.X, gridPosOffsetY = originScreenPos.Y;
            Vector2 drawPos;
            float spaceOffsetXMinimap = 0, spaceOffsetYMinimap = 0;
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
            switch (gridState)
            {
                case GridState.noSelection:
                    break;

                case GridState.castAnimating:
                    break;

                case GridState.selectedCastAvailable:
                    break;

                case GridState.selectedMotionAvailable:
                    break;

                case GridState.selectedMoving:
                    animatingActor.ScreenPosition = animatingActorScreenPos;
                    animatingActor.Draw(batch);
                    break;
            }
            //prepare to draw the minimap
            minimapBackground.Draw(batch,
                new Rectangle((int)((1 - ConstantHolder.relativeMinimapSizeX) * ConstantHolder.GAME_WIDTH) - 20,
                    (int)((1 - ConstantHolder.relativeMinimapSizeY) * ConstantHolder.GAME_HEIGHT) - 20,
                    (int)(ConstantHolder.relativeMinimapSizeX * ConstantHolder.GAME_WIDTH) + 21,
                    (int)(ConstantHolder.relativeMinimapSizeY * ConstantHolder.GAME_HEIGHT) + 21));
            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j += 2)
                {
                    drawPos = new Vector2(spaceOffsetXMinimap, spaceOffsetYMinimap) + minimapOrigin;
                    spaceArray[j, i].drawMinimap(batch, drawPos, minimapSpaceDim);
                    spaceOffsetXMinimap += mGridspaceWidth * 3 / 2;
                }
                //Similar algorithm here to the one above, only for the minimap
                spaceOffsetXMinimap = 3 * mGridspaceWidth / 4;
                spaceOffsetYMinimap += mGridspaceWidth / 2;

                for (int k = 1; k < (SizeX - (SizeX % 2)); k += 2)
                {
                    drawPos = new Vector2(spaceOffsetXMinimap, spaceOffsetYMinimap) + minimapOrigin;
                    spaceArray[k, i].drawMinimap(batch, drawPos, minimapSpaceDim);
                    spaceOffsetXMinimap += mGridspaceWidth * 3 / 2;
                }

                spaceOffsetXMinimap = 0;
                spaceOffsetYMinimap += mGridspaceWidth / 2;
            }
            //Move the selection rectangle to the correct position and draw it
            selectionRect.X = Math.Max((int) (ConstantHolder.GAME_WIDTH * (1-ConstantHolder.relativeMinimapSizeX)), 
                (int)(minimapOrigin.X + (originScreenPos.X * selectionRectDefaultSize.X / ConstantHolder.GAME_WIDTH)));
            selectionRect.Y = Math.Max((int) (ConstantHolder.GAME_HEIGHT * (1-ConstantHolder.relativeMinimapSizeY)),
                (int)(minimapOrigin.Y + (originScreenPos.Y * selectionRectDefaultSize.Y / ConstantHolder.GAME_HEIGHT)));
            selectionRect.Width = Math.Min((int)selectionRectDefaultSize.X, ConstantHolder.GAME_WIDTH - selectionRect.X);
            selectionRect.Height = Math.Min((int)selectionRectDefaultSize.Y, ConstantHolder.GAME_HEIGHT - selectionRect.Y);
            minimapSelect.Draw(batch, selectionRect);
        }
#endregion

#region Pathing

        #region Available Spaces

        /// <summary>
        /// Finds all spaces within the given range of the given origin point.
        /// </summary>
        /// <param name="moveSpeed">Maximum range at which to find a space.</param>
        /// <param name="origin">Point from which the element is originating</param>
        /// <param name="UseWeights">Determines whether one space movement will cost 1 point, or the value stored in the movementCosts 4d array.</param>
        /// <returns></returns>
        public List<Vector2> getAvailableMoveSpaces(int moveSpeed, Vector2 origin, bool UseWeights, bool moveThroughActors)
        {
            for (int i = 0; i < sizeX; ++i)
            {
                for (int j = 0; j < sizeY; ++j)
                {
                    visited[i, j] = false;
                }
            }
            List<Vector2> toReturn = new List<Vector2>();
            if(withinGrid(origin.X, origin.Y))
            {
                visited[(int)origin.X, (int)origin.Y] = true;
                addAvailableMoveSpacesRecursive(toReturn, origin, moveSpeed, UseWeights, moveThroughActors);
                return toReturn;
            }
            return toReturn;
        }

        /// <summary>
        /// Creates recursive branches of a simulated path existence tree, branch maximum 6, which will give us all the positions to which the element can move. 
        /// </summary>
        /// <param name="listToBuild">List of vector2's currently being built.</param>
        /// <param name="currentOrigin">Point from which we are currently looking out.</param>
        /// <param name="movementLeft">Amount of movement speed left before this branch will terminate.</param>
        /// <param name="useWeights">Determines whether to use weighted movement based on the spaces, or 1 cost per space moved.</param>
        protected void addAvailableMoveSpacesRecursive(List<Vector2> listToBuild, Vector2 currentOrigin, int movementLeft, bool useWeights, bool moveThroughActors)
        {
            if (movementLeft > 0)
            {
                moveSpacesRecursionInnerLogic(currentOrigin.X, currentOrigin.Y, currentOrigin.X, currentOrigin.Y - 1, listToBuild, movementLeft, useWeights, moveThroughActors);
                moveSpacesRecursionInnerLogic(currentOrigin.X, currentOrigin.Y, currentOrigin.X + 1, currentOrigin.Y, listToBuild, movementLeft, useWeights, moveThroughActors);
                moveSpacesRecursionInnerLogic(currentOrigin.X, currentOrigin.Y, currentOrigin.X + 1, currentOrigin.Y + ((int)currentOrigin.X % 2 == 0 ? -1 : 1),
                    listToBuild, movementLeft, useWeights, moveThroughActors);
                moveSpacesRecursionInnerLogic(currentOrigin.X, currentOrigin.Y, currentOrigin.X, currentOrigin.Y + 1, listToBuild, movementLeft, useWeights, moveThroughActors);
                moveSpacesRecursionInnerLogic(currentOrigin.X, currentOrigin.Y, currentOrigin.X - 1, currentOrigin.Y, listToBuild, movementLeft, useWeights, moveThroughActors);
                moveSpacesRecursionInnerLogic(currentOrigin.X, currentOrigin.Y, currentOrigin.X - 1, currentOrigin.Y + ((int)currentOrigin.X % 2 == 0 ? -1 : 1),
                    listToBuild, movementLeft, useWeights, moveThroughActors);
            }
        }

        /// <summary>
        /// Checks whether the next position is within the grid, then whether there is enough movement speed left in order to get to it.
        /// </summary>
        /// <param name="xIni">X position from which we are looking.</param>
        /// <param name="yIni">Y position from which we are looking.</param>
        /// <param name="xNext">X position to which we are looking.</param>
        /// <param name="yNext">Y position to which we are looking.</param>
        /// <param name="toBuild">List of vector2's currently being built.</param>
        /// <param name="movesLeft">Amount of movement speed left before this branch will terminate.</param>
        /// <param name="useWeights">Determines whether to use weighted movement based on the spaces, or 1 cost per space moved.</param>
        protected void moveSpacesRecursionInnerLogic(float xIni, float yIni, float xNext, float yNext, List<Vector2> toBuild, int movesLeft, bool useWeights, bool moveThroughActors)
        {
            if (movesLeft > 0 && withinGrid(xNext, yNext))
            {
                int xIIni = (int)xIni;
                int yIIni = (int)yIni;
                int xINext = (int)xNext;
                int yINext = (int)yNext;
                if ((useWeights ? (movementCosts[xIIni, yIIni, xINext, yINext] <= movesLeft) : connections[xIIni, yIIni, xINext, yINext])
                    && (moveThroughActors ? true : !(spaceArray[xINext, yINext].IsOccupied)))
                {
                    Vector2 next = new Vector2(xNext, yNext);
                    visited[xINext, yINext] = true;
                    toBuild.Add(next);
                    int newMovesLeft;
                    if (useWeights)
                    {
                        newMovesLeft = movesLeft - movementCosts[xIIni, yIIni, xINext, yINext];
                    }
                    else
                    {
                        newMovesLeft = movesLeft - 1;
                    }
                    if(newMovesLeft > 0)
                        addAvailableMoveSpacesRecursive(toBuild, next, newMovesLeft, useWeights, moveThroughActors);
                }
            }
        }

        #endregion

        #region Dijkstra
        /// <summary>
        /// Runs Dijkstra's Two Points Shortest Path algorithm
        /// </summary>
        /// <param name="sourceNodePos">Position at which the Vector2Path must start.</param>
        /// <param name="targetNodePos">Position at which the Vector2Path must end.</param>
        /// <param name="useWeights">Determines whether to use simple connection, or weighted paths.</param>
        /// <returns>The shortest Vector2Path between the two points</returns>
        public LinkedList<Vector2> Dijkstra(Vector2 sourceNodePos, Vector2 targetNodePos, bool useWeights, bool moveThroughActors)
        {
            LinkedList<Vector2> result = new LinkedList<Vector2>(), startingPath = new LinkedList<Vector2>();
            if(sourceNodePos == targetNodePos || (moveThroughActors && spaceArray[(int)targetNodePos.X, (int)targetNodePos.Y].IsOccupied)) return result;
            for(int i = 0; i < sizeX; ++i)
            {
                for(int j = 0; j < sizeY; ++j)
                {
                    visited[i,j] = false;
                }
            }
            int targetX = (int) targetNodePos.X;
            int targetY = (int) targetNodePos.Y;
            visited[(int) sourceNodePos.X, (int) sourceNodePos.Y] = true;
            Vector2Path currentPath = new Vector2Path(startingPath, sourceNodePos, 0);
            DijkstraQueue dkQ = new DijkstraQueue(visited);
            dkQ.enqueue(currentPath, targetNodePos);

            while(!visited[targetX, targetY] && dkQ.count > 0)
            {
                currentPath = dkQ.dequeue();
                currentPath.pathSoFar.AddLast(currentPath.nextStep);
                visited[(int)currentPath.nextStep.X, (int)currentPath.nextStep.Y] = true;
                //Enqueue the 4 cardinal directional neighbors
                dijkstraEnqueue(currentPath.nextStep + Vector2.UnitY, dkQ, currentPath, useWeights, targetNodePos, moveThroughActors);
                dijkstraEnqueue(currentPath.nextStep - Vector2.UnitY, dkQ, currentPath, useWeights, targetNodePos, moveThroughActors);
                dijkstraEnqueue(currentPath.nextStep + Vector2.UnitX, dkQ, currentPath, useWeights, targetNodePos, moveThroughActors);
                dijkstraEnqueue(currentPath.nextStep - Vector2.UnitX, dkQ, currentPath, useWeights, targetNodePos, moveThroughActors);

                //Enqueue the two off-kilter neighbors, which will either be above or below the horizontal neighbors.
                dijkstraEnqueue(currentPath.nextStep + Vector2.UnitX + Vector2.UnitY * (currentPath.nextStep.X % 2 == 0 ? -1 : 1),
                    dkQ, currentPath, useWeights, targetNodePos, moveThroughActors);
                dijkstraEnqueue(currentPath.nextStep - Vector2.UnitX + Vector2.UnitY * (currentPath.nextStep.X % 2 == 0 ? -1 : 1),
                    dkQ, currentPath, useWeights, targetNodePos, moveThroughActors);
            }
            result = currentPath.pathSoFar;
            return result;
        }

        /// <summary>
        /// Takes in a queue and it's most recently popped Vector2Path, then if applicable, enqueues a new Vector2Path, which is the old Vector2Path and the goal vector added on.
        /// </summary>
        /// <param name="nextStep"></param>
        /// <param name="queue"></param>
        /// <param name="soFar"></param>
        /// <param name="useWeights"></param>
        private void dijkstraEnqueue(Vector2 nextStep, DijkstraQueue queue, Vector2Path soFar, bool useWeights, Vector2 goalNode, bool moveThroughActors)
        {
            LinkedListNode<Vector2> currentNode = soFar.pathSoFar.Last;
            Vector2 currentPos = currentNode.Value;
            if (withinGrid(nextStep.X, nextStep.Y) && (!visited[(int)nextStep.X, (int)nextStep.Y] &&
                    connections[(int)currentPos.X, (int)currentPos.Y, (int)nextStep.X, (int)nextStep.Y] 
                    && moveThroughActors ? true : !spaceArray[(int)nextStep.X, (int)nextStep.Y].IsOccupied))
            {
                Vector2Path newPath = new Vector2Path(soFar.pathSoFar, nextStep,
                    soFar.totalCost + (useWeights ? movementCosts[(int)currentPos.X, (int)currentPos.Y, (int)nextStep.X, (int)nextStep.Y] : 1));
                    //If we are using weights, add the weight, otherwise add 1 for constant weighting.
                queue.enqueue(newPath, goalNode);
            }
        }

        /// <summary>
        /// Tracker for a Vector2Path through the grid, used for Dijkstra's algorithm
        /// </summary>
        struct Vector2Path
        {
            public LinkedList<Vector2> pathSoFar;
            public Vector2 nextStep;
            public int totalCost;

            /// <summary>
            /// Creates a Vector2Path object for Dijkstra's algorithm in a 2D or hexagonal grid.
            /// </summary>
            /// <param name="pathSF">The Path taken so far</param>
            /// <param name="next">Next node to visit</param>
            /// <param name="cost">Cost of taking the Vector2Path so far plus the next step</param>
            public Vector2Path(LinkedList<Vector2> pathSF, Vector2 next, int cost)
            {
                nextStep = next;
                pathSoFar = new LinkedList<Vector2>();
                LinkedListNode<Vector2> currentNode = pathSF.First;
                while (currentNode != null)
                {
                    pathSoFar.AddLast(new Vector2(currentNode.Value.X, currentNode.Value.Y));
                    currentNode = currentNode.Next;
                }
                totalCost = cost;
            }
        }

        /// <summary>
        /// A queue which takes in a Vector2Path object, 
        /// which contains a vector list Vector2Path, a next point, and a total cost to traverse the Vector2Path and move to that next point, then sorts on that cost.
        /// </summary>
        private class DijkstraQueue
        {
            LinkedList<Vector2Path> queue;
            public int count = 0;
            bool[,] visited;

            public DijkstraQueue(bool[,] visited)
            {
                this.visited = visited;
                queue = new LinkedList<Vector2Path>();
            }

            /// <summary>
            /// Put the Vector2Path into the correct position of the list
            /// </summary>
            /// <param name="newPath">Vector2Path to insert</param>
            public void enqueue(Vector2Path newPath, Vector2 finalGoal)
            {
                if (queue.Count == 0)
                {
                    queue.AddFirst(newPath);
                }
                else
                {
                    LinkedListNode<Vector2Path> current = queue.First;
                    LinkedListNode<Vector2Path> last = queue.First;
                    while (current != null && (newPath.totalCost > current.Value.totalCost || (newPath.totalCost == current.Value.totalCost
                        && Vector2.DistanceSquared(newPath.nextStep, finalGoal) > Vector2.DistanceSquared(current.Value.nextStep, finalGoal))))
                    {
                        if (visited[(int)current.Value.nextStep.X, (int)current.Value.nextStep.Y])
                        {
                            if (current == queue.First)
                            {
                                queue.RemoveFirst();
                                current = queue.First;
                                last = queue.First;
                            }
                            else
                            {
                                current = last;
                                queue.Remove(current.Next);
                                current = current.Next;
                            }
                            --count;
                        }
                        else
                        {
                            last = current;
                            current = current.Next;
                        }
                    }
                    LinkedListNode<Vector2Path> newNode = new LinkedListNode<Vector2Path>(newPath);
                    queue.AddAfter(last, newNode);
                }
                ++count;
            }

            /// <summary>
            /// Remove the lowest cost Path from the queue
            /// </summary>
            /// <returns>The lowest cost Vector2Path</returns>
            public Vector2Path dequeue()
            {
                --count;
                Vector2Path temp = queue.First.Value;
                queue.RemoveFirst();
                return temp;
            }
        }
        #endregion

#endregion

#region AccessFunctions
        /// <summary>
        /// Returns the hero that can be found at the screen position given
        /// </summary>
        /// <param name="screenPos">Selection position on screen</param>
        /// <returns>Hero or Null</returns>
        public Hero heroAtScreenPos(Vector2 screenPos)
        {
            GridSpace gs = spaceAtScreenpos(screenPos);
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
            GridSpace gs = spaceAtScreenpos(screenPos);
            if (gs != null && gs.IsOccupied && gs.CurrentActor is Person)
                return (Person)gs.CurrentActor;
            return null;
        }

        /// <summary>
        /// Gets the space currently being moused over
        /// </summary>
        /// <param name="screenPos">Selection position on screen</param>
        /// <returns>GridSpace or empty space</returns>
        public GridSpace spaceAtScreenpos(Vector2 screenPos)
        {
            //potential TBI: More accurate math for handling corners. Potential idea: circular distance to 4 nearest points, choose closest one. Still has inaccuracies, but much less than current 
            int xDim = (int)((screenPos.X + originScreenPos.X) / (ConstantHolder.HexagonGrid_HexSizeX  * 3 / 4));
            float yPosAdjusted = xDim % 2 == 0 ? screenPos.Y + originScreenPos.Y : screenPos.Y + originScreenPos.Y - (ConstantHolder.HexagonGrid_HexSizeY / 2);
            int yDim = (int)(yPosAdjusted / ConstantHolder.HexagonGrid_HexSizeY);
            if(xDim >= 0 && xDim < sizeX && yDim >= 0 && yDim < sizeY)
                return spaceArray[xDim, yDim];
            else
            {
                return new GridSpace(this);
            }
        }
#endregion

#region Accessors/Mutators
        /// <summary>
        /// Array containing all important information. Accessor, not mutator.
        /// </summary>
        public GridSpace[,] GridSpaces
        {
            get { return spaceArray; }
        }

        /// <summary>
        /// Number of total spaces, of any type, in the grid.
        /// </summary>
        public int NumSpaces
        {
            get { return numSpaces; }
        }

        /// <summary>
        /// Width in gridspaces of the grid.
        /// </summary>
        public int SizeX
        {
            get { return sizeX; }
        }

        /// <summary>
        /// Height in spaces of the grid.
        /// </summary>
        public int SizeY
        {
            get { return sizeY; }
        }

        /// <summary>
        /// Current offset of the grid's screen position.
        /// </summary>
        public Vector2 CurrentScreenOffset
        {
            get { return originScreenPos; }
        }

        /// <summary>
        /// The Person currently selected by the player.
        /// </summary>
        public Person SelectedPerson
        {
            get { return selectedPerson; }
        }

        public int[, , ,] MoveCosts
        {
            get { return movementCosts; }
        }
#endregion
    }
}