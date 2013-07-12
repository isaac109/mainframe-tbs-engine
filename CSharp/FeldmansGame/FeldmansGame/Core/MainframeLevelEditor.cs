using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mainframe;
using Mainframe.Core;
using Mainframe.Constants;
using Mainframe.Animations;
using Mainframe.Animations.Character;
using Mainframe.Animations.Environment;
using Mainframe.Animations.Interface;
using Mainframe.Core.Units;
using Mainframe.Forms;
using Mainframe.GUI;
using Mainframe.Saving;
using System.IO;
using Mainframe.Constants.Editor;

namespace Mainframe.Core
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainframeLevelEditor : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Level currentLevel;
        MouseState mouse;
        KeyboardState kb;
        Mainframe_Level_Editor Owner;

        /* Level Editor Stuff*/
        private IntPtr drawSurface;     //Used for drawing to Windows Form, rather than a new game.


        public MainframeLevelEditor()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #region Level Editor setup

        public MainframeLevelEditor(IntPtr drawSurface, Mainframe_Level_Editor owner)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = ConstantHolder.GAME_HEIGHT;
            graphics.PreferredBackBufferWidth = ConstantHolder.GAME_WIDTH;
            this.drawSurface = drawSurface;
            graphics.PreparingDeviceSettings +=
                new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle(this.Window.Handle).VisibleChanged +=
                new EventHandler(mainframeVisibleChanged);
            Owner = owner;
        }


        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }

        private void mainframeVisibleChanged(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle(this.Window.Handle).Visible == true)
                System.Windows.Forms.Control.FromHandle(this.Window.Handle).Visible = false;
        }

        public void setMouseHandle(IntPtr window)
        {
            Mouse.WindowHandle = window;
        }

        #endregion

        #region Game Stuff Setup

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ConstantHolder.textureLoader = TextureListXML.loadTextureXMLs("Content\\XMLs\\Textures\\Textures.xml");
            ActorTextureHolder.makeHolder(GraphicsDevice);
            EnvironmentTextureHolder.makeHolder(GraphicsDevice);
            InterfaceTextureHolder.makeHolder(Content, GraphicsDevice);
            Controls.Initialize();
            // TODO: use this.Content to load your game content here
        }

        #endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            kb = Keyboard.GetState();
            Controls.updateControls(mouse, kb);
            if (currentLevel != null && currentLevel.SimpleLevelGrid != null)
            {
                Owner.setBarTicks(currentLevel.SimpleLevelGrid.scrollNotchesX, currentLevel.SimpleLevelGrid.scrollNotchesY);
            }

            if (!Controls.ButtonsDown[(int)Controls.ButtonNames.leftMouse] && Controls.ButtonsLastDown[(int)Controls.ButtonNames.leftMouse] && withinGame(Controls.MousePosition))
            {
                onLeftClick(Controls.MousePosition);
            }
            if (!Controls.ButtonsDown[(int)Controls.ButtonNames.rightMouse] && Controls.ButtonsLastDown[(int)Controls.ButtonNames.rightMouse] && withinGame(Controls.MousePosition))
            {
                onRightClick(Controls.MousePosition);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// Checks whether a given position (usually a mouse click) lies within the screen bounds of the game.
        /// </summary>
        /// <param name="screenPosition">Position to check.</param>
        /// <returns></returns>
        protected bool withinGame(Vector2 screenPosition)
        {
            return screenPosition.X > 0 && screenPosition.Y > 0 && screenPosition.X < ConstantHolder.GAME_WIDTH && screenPosition.Y < ConstantHolder.GAME_HEIGHT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MousePosition"></param>
        protected void onLeftClick(Vector2 MousePosition)
        {
            if (currentLevel != null)
            {
                switch (EditorConstantHolder.currentEditingMode)
                {
                    case EditorConstantHolder.EditingMode.Space:
                        {
                            SimpleGridSpace temp;
                            switch (EditorConstantHolder.spaceEditingMode)
                            {
                                case EditorConstantHolder.SpaceOptions.addRemove:
                                    temp = currentLevel.SimpleLevelGrid.spaceAtScreenpos(MousePosition);
                                    if (temp != null)
                                    {
                                        temp.SpaceType = EditorConstantHolder.spaceTypeComboBox;
                                        temp.setSprite(EditorConstantHolder.spaceTypeComboBox);
                                    }
                                    break;

                                case EditorConstantHolder.SpaceOptions.elevation:
                                    temp = currentLevel.SimpleLevelGrid.spaceAtScreenpos(MousePosition);
                                    if (temp != null)
                                    {
                                        temp.Elevation++;
                                    }
                                    break;
                            }
                            break;
                        }

                    case EditorConstantHolder.EditingMode.Character:
                        {
                            SimpleGridSpace temp;
                            temp = currentLevel.SimpleLevelGrid.spaceAtScreenpos(MousePosition);
                            if (temp != null)
                            {
                                if (EditorConstantHolder.addHeroes)
                                {
                                    int[] abilitySels = { EditorConstantHolder.heroAbility1Sel, EditorConstantHolder.heroAbility2Sel, EditorConstantHolder.heroAbility3Sel };
                                    temp.putActor(new Hero(EditorConstantHolder.heroClassSel, EditorConstantHolder.heroLevel,
                                        abilitySels, EditorConstantHolder.heroSkin, temp.GridPosition));
                                }
                                else
                                {
                                }
                            }
                        }
                        break;
                }
            }
        }

        protected void onRightClick(Vector2 MousePosition)
        {
            if (currentLevel != null)
            {
                switch (EditorConstantHolder.currentEditingMode)
                {
                    case EditorConstantHolder.EditingMode.Space:
                        {
                            SimpleGridSpace temp;
                            switch (EditorConstantHolder.spaceEditingMode)
                            {
                                case EditorConstantHolder.SpaceOptions.addRemove:
                                    temp = currentLevel.SimpleLevelGrid.spaceAtScreenpos(MousePosition);
                                    if (temp != null)
                                    {
                                        temp.SpaceType = 0;
                                        temp.setSprite(0);
                                    }
                                    break;

                                case EditorConstantHolder.SpaceOptions.elevation:
                                    temp = currentLevel.SimpleLevelGrid.spaceAtScreenpos(MousePosition);
                                    if (temp != null)
                                    {
                                        temp.Elevation--;
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (currentLevel != null && currentLevel.SimpleLevelGrid != null)
                currentLevel.SimpleLevelGrid.draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }




        #region Level editor data setup

        public Level makeNewLevel(int sizeX, int sizeY, String name)
        {
            currentLevel = Level.makeEmptyGrid(sizeX, sizeY, name);
            return currentLevel;
        }

        public Level loadLevel(String filepath)
        {
            currentLevel = Level.loadSimpleLevelXML(filepath);
            return currentLevel;
        }

        public void saveLevel(Stream saveStream)
        {
            currentLevel.saveLevelXML(saveStream);
        }

        #endregion
    }
}