using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mainframe.Forms;
using Mainframe.Constants;
using Mainframe.Animations.Character;
using Mainframe.Animations.Environment;
using Mainframe.Animations.Interface;
using Mainframe.Core;

namespace Mainframe.Animations
{
    class ContentImportationGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouse;
        KeyboardState kb;
        ContentImportationMenu Owner;
        
        /* current editing stuff*/
        Texture2D currentSprite;


        /* Level Editor Stuff*/
        private IntPtr drawSurface;     //Used for drawing to Windows Form, rather than a new game.

        public ContentImportationGame(IntPtr drawSurface, ContentImportationMenu owner)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.drawSurface = drawSurface;
            graphics.PreparingDeviceSettings +=
                new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle(this.Window.Handle).VisibleChanged +=
                new EventHandler(mainframeVisibleChanged);
            Owner = owner;
        }

        private void mainframeVisibleChanged(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle(this.Window.Handle).Visible == true)
                System.Windows.Forms.Control.FromHandle(this.Window.Handle).Visible = false;
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }

        public void setMouseHandle(IntPtr window)
        {
            Mouse.WindowHandle = window;
        }


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
            Controls.Initialize();
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

        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            kb = Keyboard.GetState();
            Controls.updateControls(mouse, kb);
            base.Update(gameTime);
        }

        /*// <summary>
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
        }*/

    }
}
