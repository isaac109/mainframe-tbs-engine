/** The Panther Engine, written by Matt Loesby
 * With assistance from:
 *          Matthew C. Shaffer
 *          Cole Cunningham
 *          Matthew Olivier
 *          Nicholas Lee
 * In collaboration with
 *          Lee Feldman
 *          Dylan
 *          Arianne Advincula
 *          Garrett Isaacs
 * Copyright 2010, 2011, 2012 Panther Games.
 * For permission to use or modify this code, please e-mail Matt Loesby at Loesby.Dev@gmail.com
 */

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
using Mainframe.GUI;
using Mainframe.Animations;
using Mainframe.Constants;
using Mainframe.Core.Units;
using Mainframe.Saving;
using Mainframe.Animations.Character;
using Mainframe.Animations.Combat;
using Mainframe.Animations.Interface;
using Mainframe.Animations.Environment;

namespace Mainframe.Core
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainframeGame : Microsoft.Xna.Framework.Game
    {
#region Enumerators
        public enum GameState
        {
            MainMenu,
            LoadingScreen,
            Gameplay_Combat,
            Gameplay_Combat_Casting,
            Gameplay_Paused,
            Gameplay_LevelSelect,
            Gameplay_LevelPrep,
            Gameplay_LevelEnd
        };
#endregion

#region variables

        //Public Objects
        public static GUIManager GUIMan;
        public static Grid currentLevelGrid;

        //Public primitives

        //Private Objects
        MouseState mouse;
        KeyboardState kb;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //TextureHolder GameTexHolder;
        ActorTextureHolder ActorHolder;
        CombatTextureHolder CombatHolder;
        InterfaceTextureHolder InterfaceHolder;
        EnvironmentTextureHolder EnviroHolder;
        static GameState mainGameState = GameState.Gameplay_Combat;

        /* Level Editor Stuff*/
        private IntPtr drawSurface;     //Used for drawing to Windows Form, rather than a new game.


#endregion

#region Setup
        public MainframeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = ConstantHolder.GAME_HEIGHT;
            graphics.PreferredBackBufferWidth = ConstantHolder.GAME_WIDTH;
        }


        #region Level Editor setup

        public MainframeGame(IntPtr drawSurface)
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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Controls.Initialize();
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
            ActorHolder = ActorTextureHolder.makeHolder(GraphicsDevice);
            CombatHolder = CombatTextureHolder.makeHolder(GraphicsDevice);
            EnviroHolder = EnvironmentTextureHolder.makeHolder(GraphicsDevice);
            InterfaceHolder = InterfaceTextureHolder.makeHolder(Content, GraphicsDevice);
            currentLevelGrid = Level.loadLevelXML("Content\\XMLs\\level.xml").LevelGrid;
            GUIMan = GUIManager.MakeManager();
            changeState(GameState.Gameplay_Combat);
            //currentLevelGrid = thing.SimpleLevelGrid;
            IsMouseVisible = true;
            changeState(GameState.Gameplay_Combat);
            //HexagonWidth = hex.Width;
            // TODO: use this.Content to load your game content here
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
#endregion

#region GUISetup
        private static void makeCombatGUI()
        {
            GUIMan.ClearGUIs();
            int statBoxX, statBoxY, statBoxW, statBoxH,
                abilityBoxX, abilityBoxY, abilityBoxW, abilityBoxH;
            statBoxX = 0;
            statBoxY = (int)((1 - (1.35 * ConstantHolder.relativeMinimapSizeY)) * ConstantHolder.GAME_HEIGHT);
            statBoxW = 120;
            statBoxH = ConstantHolder.GAME_HEIGHT - statBoxY;
            abilityBoxX = statBoxW;
            abilityBoxY = (int)((1 - ConstantHolder.relativeMinimapSizeY) * ConstantHolder.GAME_HEIGHT);
            //20 offset at the end here handles the fact that the Minimap clears a slight larger space for itself. Reference Mainframe.Core.Grid, line 280, in the Draw function, minimap section.
            abilityBoxW = (int)((1 - ConstantHolder.relativeMinimapSizeX) * ConstantHolder.GAME_WIDTH) - statBoxW - 20;
            abilityBoxH = ConstantHolder.GAME_HEIGHT - abilityBoxY; 
            GUIMan.createStatBox(new Rectangle(statBoxX, statBoxY, statBoxW, statBoxH), new Rectangle(abilityBoxX, abilityBoxY, abilityBoxW, abilityBoxH));
        }
#endregion

#region state transitions
        public static void startCast(Person caster, int abilityIndex)
        {

        }

        public static void changeState(GameState newState)
        {
            mainGameState = newState;
            switch (newState)
            {
                case GameState.Gameplay_Combat:
                    makeCombatGUI();
                    break;

                case GameState.Gameplay_LevelEnd:
                    break;

                case GameState.Gameplay_LevelPrep:
                    break;

                case GameState.Gameplay_LevelSelect:
                    break;

                case GameState.Gameplay_Paused:
                    break;

                case GameState.LoadingScreen:
                    break;

                case GameState.MainMenu:
                    break;

                case GameState.Gameplay_Combat_Casting:
                    break;
            }
        }
#endregion


#region updatingLogic

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
            if (kb.IsKeyDown(Keys.Escape)) this.Exit();
            switch(mainGameState)
            {
            case GameState.MainMenu:
                break;

            case GameState.Gameplay_Combat:
                GUIMan.Update();
                currentLevelGrid.updateGrid();
                    //NOTE: updating grid last, so we can hold a marker whether we should be dragging with the click or not.
                    //This feature is NYI, but we don't want to click on a button, move the mouse slightly, and have the grid move.
                break;
            }
            base.Update(gameTime);
        }
#endregion

#region Drawing
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(255, 167, 26));  //Day 9 Yellow

            spriteBatch.Begin();

            switch (mainGameState)
            {
                case GameState.MainMenu:
                    break;

                case GameState.Gameplay_Combat:
                    currentLevelGrid.draw(spriteBatch);
                    GUIMan.draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
#endregion
    }
}