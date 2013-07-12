using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mainframe.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Constants;

namespace Mainframe.Core.Units
{
    public class Wall
    {
        public enum WallColumn
        {
            left = 0,
            middle,
            right
        }


        protected int wallType = 0;   //0 = empty wall, no barrier.
                            //All others determine texture.
        Sprite wallSprite;

        public Wall()
        {
        }

        public void Draw(SpriteBatch batch, Vector2 tilePosition)
        {
            /*switch (wallSprite.CurrentColumn)
            {
                case (int)WallColumn.left:
                    tilePosition.Y += ConstantHolder.HexagonGrid_HexSizeY / 2;
                    tilePosition.Y -= wallSprite.SpriteSizeY;
                    break;

                case (int)WallColumn.middle:
                    tilePosition.Y -= wallSprite.SpriteSizeY;
                    tilePosition.X += ConstantHolder.HexagonGrid_HexSizeX / 4;
                    break;

                case (int)WallColumn.right:
                    tilePosition.Y += ConstantHolder.HexagonGrid_HexSizeY / 2;
                    tilePosition.Y -= wallSprite.SpriteSizeY;
                    tilePosition.X += 3 * ConstantHolder.HexagonGrid_HexSizeX / 4;
                    break;
            }
            wallSprite.Draw(batch, tilePosition);*/
        }

        public int WallType
        {
            get { return wallType; }
        }
    }
}
