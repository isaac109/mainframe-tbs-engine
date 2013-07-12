using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Mainframe.Constants;
using Mainframe.Core;
using Mainframe.Core.Units;
using Microsoft.Xna.Framework;

namespace Mainframe.Saving
{
    public class Level
    {
#region XML
        public class LevelXML
        {
            public String Name;
            public int sizeX;
            public int sizeY;
            public List<RowXML> rows;

            public LevelXML()
            {
                rows = new List<RowXML>();
            }
        }

        public class RowXML
        {
            public int index;
            public List<ColumnXML> columns;

            public RowXML()
            {
                columns = new List<ColumnXML>();
            }
        }

        public class ColumnXML
        {
            public int index;
            public int type;
            public int elevation;
            public List<WallXML> walls;
            public HeroXML hero;
            public PersonXML person;
            public ActorXML actor;
            public int costNorth,
                costNorthEast,
                costSouthEast,
                costSouth,
                costSouthWest,
                costNorthWest;

            public ColumnXML()
            {
                walls = new List<WallXML>();
            }
        }

        public class WallXML
        {
            public int direction;
            //0 for North, 1 for NorthEast, 2 for SouthEast, 3 for South, 4 for SouthWest, 5 for NorthWest
            public int type;
        }

        public class HeroXML
        {
            public String name;
            public int level;
            public int classNum;
            public int currentHP;
            public int maxHP;
            public int currentEnergy;
            public int maxEnergy;
            public int currentXP;
            public int moveSpeed;
            public int team;
        }

        public class PersonXML
        {
            public String name;
            public int currentHP;
            public int maxHP;
            public int currentEnergy;
            public int maxEnergy;
            public int moveSpeed;
        }

        public class ActorXML
        {
            public String name;
        }


        public static Level loadSimpleLevelXML(String fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(LevelXML));
            LevelXML currentXML = (LevelXML)xml.Deserialize(fileStream);
            Level newLevel = new Level(currentXML.sizeX, currentXML.sizeY, currentXML.Name);
            newLevel.simpleLevelGrid = new SimpleGrid(currentXML.sizeX, currentXML.sizeY);
            newLevel.LevelName = currentXML.Name;
            foreach (RowXML row in currentXML.rows)
            {
                foreach (ColumnXML column in row.columns)
                {
                    newLevel.simpleLevelGrid.GridSpaces[row.index, column.index] = new SimpleGridSpace(column.type, row.index, column.index, column.elevation, newLevel.simpleLevelGrid);
                    newLevel.simpleLevelGrid.setMoveCost(row.index, column.index, row.index, column.index - 1, column.costNorth);
                    newLevel.simpleLevelGrid.setMoveCost(row.index, column.index, row.index + 1, column.index + (row.index % 2 == 0 ? -1 : 0), column.costNorthEast);
                    newLevel.simpleLevelGrid.setMoveCost(row.index, column.index, row.index + 1, column.index + (row.index % 2 == 0 ? 0 : 1), column.costSouthEast);
                    newLevel.simpleLevelGrid.setMoveCost(row.index, column.index, row.index, column.index + 1, column.costSouth);
                    newLevel.simpleLevelGrid.setMoveCost(row.index, column.index, row.index - 1, column.index + (row.index % 2 == 0 ? 0 : 1), column.costSouthWest);
                    newLevel.simpleLevelGrid.setMoveCost(row.index, column.index, row.index - 1, column.index + (row.index % 2 == 0 ? -1 : 0), column.costNorthWest);
                    if (column.hero != null)
                    {
                        HeroXML h = column.hero;
                        newLevel.simpleLevelGrid.GridSpaces[row.index, column.index].putActor(new Hero(h.currentHP, h.maxHP, h.currentEnergy, h.maxEnergy, h.moveSpeed, h.level, h.currentXP, h.name, new Vector2(row.index, column.index)));
                    }
                    else if (column.person != null)
                    {
                        PersonXML p = column.person;
                        newLevel.simpleLevelGrid.GridSpaces[row.index, column.index].putActor(new Person(p.currentHP, p.maxHP, p.currentEnergy, p.maxEnergy, p.moveSpeed, p.name, new Vector2(row.index, column.index)));
                    }
                    else if (column.actor != null)
                    {
                        ActorXML a = column.actor;
                        newLevel.simpleLevelGrid.GridSpaces[row.index, column.index].putActor(new Actor());
                    }
                }
            }
            for (int i = 0; i < currentXML.sizeX; ++i)
            {
                for (int j = 0; j < currentXML.sizeY; ++j)
                {
                    if (newLevel.simpleLevelGrid.GridSpaces[i,j] == null)
                    {
                        newLevel.simpleLevelGrid.GridSpaces[i, j] = new SimpleGridSpace(0, i, j, 0, newLevel.simpleLevelGrid);
                    }
                }
            }
            fileStream.Close();
            return newLevel;
        }


        public static Level loadLevelXML(String fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(LevelXML));
            LevelXML currentXML = (LevelXML)xml.Deserialize(fileStream);
            Level newLevel = new Level(currentXML.sizeX, currentXML.sizeY, currentXML.Name);
            newLevel.levelGrid = new Grid(currentXML.sizeX, currentXML.sizeY);
            newLevel.LevelName = currentXML.Name;
            foreach (RowXML row in currentXML.rows)
            {
                foreach (ColumnXML column in row.columns)
                {
                    newLevel.levelGrid.GridSpaces[row.index, column.index] = new GridSpace(column.type, row.index, column.index, column.elevation, newLevel.levelGrid);
                    newLevel.levelGrid.setMoveCost(row.index, column.index, row.index, column.index - 1, column.costNorth);
                    newLevel.levelGrid.setMoveCost(row.index, column.index, row.index + 1, column.index + (row.index % 2 == 0 ? -1 : 0), column.costNorthEast);
                    newLevel.levelGrid.setMoveCost(row.index, column.index, row.index + 1, column.index + (row.index % 2 == 0 ? 0 : 1), column.costSouthEast);
                    newLevel.levelGrid.setMoveCost(row.index, column.index, row.index, column.index + 1, column.costSouth);
                    newLevel.levelGrid.setMoveCost(row.index, column.index, row.index - 1, column.index + (row.index % 2 == 0 ? 0 : 1), column.costSouthWest);
                    newLevel.levelGrid.setMoveCost(row.index, column.index, row.index - 1, column.index + (row.index % 2 == 0 ? -1 : 0), column.costNorthWest);
                    if (column.hero != null)
                    {
                        HeroXML h = column.hero;
                        newLevel.levelGrid.GridSpaces[row.index, column.index].tryPutActor(new Hero(h.currentHP, h.maxHP, h.currentEnergy, h.maxEnergy, h.moveSpeed, h.level, h.currentXP, h.name, new Vector2(row.index, column.index)));
                    }
                    else if (column.person != null)
                    {
                        PersonXML p = column.person;
                        newLevel.levelGrid.GridSpaces[row.index, column.index].tryPutActor(new Person(p.currentHP, p.maxHP, p.currentEnergy, p.maxEnergy, p.moveSpeed, p.name, new Vector2(row.index, column.index)));
                    }
                    else if (column.actor != null)
                    {
                        ActorXML a = column.actor;
                        newLevel.levelGrid.GridSpaces[row.index, column.index].tryPutActor(new Actor());
                    }
                }
            }
            for (int i = 0; i < currentXML.sizeX; ++i)
            {
                for (int j = 0; j < currentXML.sizeY; ++j)
                {
                    if (newLevel.levelGrid.GridSpaces[i, j] == null)
                    {
                        newLevel.levelGrid.GridSpaces[i, j] = new GridSpace(0, i, j, 0, newLevel.levelGrid);
                    }
                }
            }
            fileStream.Close();
            return newLevel;
        }

        /// <summary>
        /// Alias for saving a file, which goes through a File name rather than a stream.
        /// </summary>
        /// <param name="fileName">Path to the file save location and filename.</param>
        public void saveLevelXML(String fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate);
            saveLevelXML(stream);
        }

        /// <summary>
        /// Saves the file in XML format to the given Stream.
        /// </summary>
        /// <param name="saveFile">Stream to pass in, usually from a SaveFileDialog, to which to save the file.</param>
        public void saveLevelXML(Stream saveFile)
        {
            FileStream fileStream = (FileStream)saveFile;//new FileStream(filepath, FileMode.OpenOrCreate);
            XmlSerializer xml = new XmlSerializer(typeof(LevelXML));
            LevelXML levelXML = new LevelXML();
            levelXML.Name = LevelName;
            levelXML.sizeX = simpleLevelGrid.SizeX;
            levelXML.sizeY = simpleLevelGrid.SizeY;
            for (int i = 0; i < levelXML.sizeX; ++i)
            {
                RowXML newRow = new RowXML();
                newRow.index = i;
                for (int j = 0; j < levelXML.sizeY; ++j)
                {
                    ColumnXML newCol = new ColumnXML();
                    newCol.index = j;
                    newCol.type = simpleLevelGrid.GridSpaces[i, j].SpaceType;
                    newCol.elevation = simpleLevelGrid.GridSpaces[i,j].Elevation;
                    if (simpleLevelGrid.GridSpaces[i, j].IsOccupied)
                    {
                        Actor temp = simpleLevelGrid.GridSpaces[i,j].CurrentActor;
                        if (temp is Hero)
                        {
                            Hero tempH = (Hero) temp;
                            newCol.hero = new HeroXML();
                            newCol.hero.name = tempH.Name;
                            newCol.hero.moveSpeed = tempH.MoveSpeed;
                            newCol.hero.currentEnergy = tempH.Energy;
                            newCol.hero.currentHP = tempH.Health;
                            newCol.hero.maxEnergy = tempH.MaxEnergy;
                            newCol.hero.maxHP = tempH.MaximumHealth;
                            newCol.hero.team = tempH.TeamIndex;
                            newCol.hero.classNum = tempH.ClassIndex;
                            newCol.hero.level = tempH.Level;
                        }
                        else if (temp is Person)
                        {
                            Person tempP = (Person)temp;
                            newCol.person = new PersonXML();
                            newCol.person.name = tempP.Name;
                            newCol.person.moveSpeed = tempP.MoveSpeed;
                            newCol.person.currentEnergy = tempP.Energy;
                            newCol.person.currentHP = tempP.Health;
                            newCol.person.maxEnergy = tempP.MaxEnergy;
                            newCol.person.maxHP = tempP.MaximumHealth;
                        }
                        else
                        {
                            newCol.actor = new ActorXML();
                            newCol.actor.name = temp.Name;
                        }
                        newCol.costNorth = simpleLevelGrid.withinGrid(i, j - 1) ? simpleLevelGrid.MoveCosts[i, j, i, j - 1] : 0;
                        newCol.costNorthEast = simpleLevelGrid.withinGrid(i + 1, j + (i % 2 == 0 ? -1 : 0)) ? simpleLevelGrid.MoveCosts[i, j, i + 1, j + (i % 2 == 0 ? -1 : 0)] : 0;
                        newCol.costNorthWest = simpleLevelGrid.withinGrid(i - 1, j + (i % 2 == 0 ? -1 : 0)) ? simpleLevelGrid.MoveCosts[i, j, i - 1, j + (i % 2 == 0 ? -1 : 0)] : 0;
                        newCol.costSouth = simpleLevelGrid.withinGrid(i, j - 1) ? simpleLevelGrid.MoveCosts[i, j, i, j - 1] : 0;
                        newCol.costSouthEast = simpleLevelGrid.withinGrid(i + 1, j + (i % 2 == 0 ? 0 : 1)) ? simpleLevelGrid.MoveCosts[i, j, i + 1, j + (i % 2 == 0 ? 0 : 1)] : 0;
                        newCol.costSouthWest = simpleLevelGrid.withinGrid(i - 1, j + (i % 2 == 0 ? 0 : 1)) ? simpleLevelGrid.MoveCosts[i, j, i - 1, j + (i % 2 == 0 ? 0 : 1)] : 0;
                        for (int k = 0; k < 3; ++k)
                        {
                            WallXML newWall = new WallXML();
                            newWall.direction = k;
                            newWall.type = simpleLevelGrid.GridSpaces[i, j].Walls[k].WallType;
                            newCol.walls.Add(newWall);
                        }
                    }
                    newRow.columns.Add(newCol);
                }
                levelXML.rows.Add(newRow);
            }
            xml.Serialize(fileStream, levelXML);
            fileStream.Close();
        }

#endregion

        private SimpleGrid simpleLevelGrid;
        private Grid levelGrid;
        private List<Person> enemies;
        private List<Hero> playerTeam;
        private String LevelName;
        
        public Level(int SizeX, int SizeY, String name)
        {
            simpleLevelGrid = new SimpleGrid(SizeX, SizeY);
            for (int x = 0; x < SizeX; ++x)
            {
                for (int y = 0; y < SizeY; ++y)
                {
                    simpleLevelGrid.GridSpaces[x, y] = new SimpleGridSpace(0, x, y, 0, simpleLevelGrid);
                }
            }
            enemies = new List<Person>();
            playerTeam = new List<Hero>();
            LevelName = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="XDim"></param>
        /// <param name="YDim"></param>
        /// <returns></returns>
        public static Level makeEmptyGrid(int XDim, int YDim, String levelName)
        {
            return new Level(XDim, YDim, levelName);
        }

        public SimpleGrid SimpleLevelGrid
        {
            get { return simpleLevelGrid; }
        }

        public Grid LevelGrid
        {
            get { return levelGrid; }
        }
    }
}
