using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mainframe.Animations;
using Mainframe.Core;
using Mainframe.Animations.Interface;
using Mainframe.Constants;

namespace Mainframe.GUI
{
    /// <summary>
    /// Main container class for a GUI's drawn and interactable elements.
    /// </summary>uct
    public class GUIManager
    {
        private static InterfaceTextureHolder textureHolder;

        private static GUIManager manager;

        private List<Button> buttons;

        private CharacterBox statBox;

        private TextBox textBox;
        private OptionsBox optionsBox;

        /// <summary>
        /// Constructs the initial Manager, preparing it for use.
        /// </summary>
        /// <returns>The Manager</returns>
        public static GUIManager MakeManager()
        {
            return manager = new GUIManager();
        }

        /// <summary>
        /// Gets the already-constructed manager.
        /// </summary>
        /// <returns>The main Manager</returns>
        public static GUIManager getManager()
        {
            return manager;
        }

        /// <summary>
        /// Gets the TextureHolder, and sets up the list of views.
        /// </summary>
        private GUIManager()
        {
            buttons = new List<Button>();
            textureHolder = InterfaceTextureHolder.Holder;
        }

        /// <summary>
        /// Removes all buttons, etc. from the GUI.
        /// </summary>
        public void ClearGUIs()
        {
            ClearButtons();
            statBox = null;
            textBox = null;
            optionsBox = null;
        }

        /// <summary>
        /// Creates a button with the given properties.
        /// </summary>
        /// <param name="position">Screen position of top-left corner, as well as the size of the button</param>
        /// <param name="spriteSelect">Index in the TextureHolder.ButtonTextures enum to use to create the sprite</param>
        /// <param name="function">Function which the button will trigger when clicked</param>
        /// <param name="bActive">Whether to start the button off as drawn.</param>
        /// <returns>The created button</returns>
        public Button makeButton(Rectangle position, int spriteSelect, Button.ButtonFunction function, bool bActive = true)
        {
            Sprite buttonSprite = textureHolder.ButtonSprites[(int)ConstantHolder.ButtonBackgrounds.Exit].copySprite();
            Button newButton = new Button(position, buttonSprite, function, bActive);
            buttons.Add(newButton);
            return newButton;
        }

        /// <summary>
        /// Creates a button with text on it.
        /// </summary>
        /// <param name="position">Top left corner of button screen position, as well as the dimensions of the button.</param>
        /// <param name="spriteSheet">Background sprites for up, down, over.</param>
        /// <param name="function">Function performed when clicked</param>
        /// <param name="font">Font of text on button</param>
        /// <param name="text">Text on button</param>
        /// <param name="bClickable">Sets whether the button will accept input at the start</param>
        /// <param name="bActive">Sets whether the button will appear.</param>
        /// <returns>The button created</returns>
        public TextButton MakeTextButton(Rectangle position, Sprite spriteSheet, Button.ButtonFunction function, SpriteFont font, String text = "", bool bClickable = true, bool bActive = true)
        {
            TextButton newButton = new TextButton(position, spriteSheet, function, font, text, bClickable, bActive);
            buttons.Add(newButton);
            return newButton;
        }

        /// <summary>
        /// Creates an options box, which contains the selected hero's abilities.
        /// </summary>
        /// <param name="position">Top left corner of the options area.</param>
        /// <param name="InfoText">Info text for the box itself.</param>
        /// <param name="buttonStrings">Array of info texts for each button.</param>
        /// <returns></returns>
        public OptionsBox MakeOptionsBox(Rectangle position, String InfoText, String[] buttonStrings)
        {
            if (optionsBox != null)
            {
                optionsBox.clear();
            }
            optionsBox = new OptionsBox(position, InfoText, buttonStrings);
            return optionsBox;
        }

        /// <summary>
        /// Clears all the info text from the main options box.
        /// </summary>
        public void clearOptionsBox()
        {
            if (optionsBox != null)
            {
                optionsBox.clear();
                optionsBox = null;
            }
        }

        /// <summary>
        /// Removes a button from the GUI
        /// </summary>
        /// <param name="button">The button to be removed.</param>
        public void ClearButton(Button button)
        {
            buttons.Remove(button);
        }

        /// <summary>
        /// Removes all buttons from the GUI. Does not clear the Options box.
        /// </summary>
        public void ClearButtons()
        {
            buttons.Clear();
        }

        /// <summary>
        /// Creates the view for the selecter hero/character's statistics
        /// </summary>
        /// <param name="HealthAndEnergy">Rectangle to which to draw the portrait and the Health and Energy bars.</param>
        /// <param name="Experience">Rectangle to which to draw the experience bar, if it exists.</param>
        /// <returns>The created CharacterBox.</returns>
        public CharacterBox createStatBox(Rectangle HealthAndEnergy, Rectangle Experience)
        {
            statBox = new CharacterBox(HealthAndEnergy, Experience);
            return statBox;
        }

        /// <summary>
        /// Creates a text box at the given position.
        /// </summary>
        /// <param name="position">Top left corner of the box.</param>
        /// <returns>The created TextBox</returns>
        public TextBox createTextBox(Rectangle position)
        {
            textBox = new TextBox(position);
            return textBox;
        }

        /// <summary>
        /// Draws all the elements of the GUI.
        /// </summary>
        /// <param name="spriteBatch">Main batch</param>
        public void draw(SpriteBatch spriteBatch)
        {
            if (statBox != null)
            {
                statBox.draw(spriteBatch);
            }
            if (optionsBox != null)
            {
                optionsBox.draw(spriteBatch);
            }
            if (textBox != null)
            {
                textBox.draw(spriteBatch);
            } 
            for (int i = 0; i < buttons.Count; i++)
                buttons.ElementAt(i).Draw(spriteBatch);
            
        }

        /// <summary>
        /// Runs the controls through all the buttons.
        /// </summary>
        public void Update()//int mouseX, int mouseY, bool bLeftButtonDown, bool bRigtButtonDown, bool bLeftClick, bool bRightClick)
        {
            for (int i = 0; i < buttons.Count; i++)
                buttons.ElementAt(i).Update();
            if (statBox != null)
            {
                statBox.update();
            }
            if (optionsBox != null)
            {
                optionsBox.update();
            }
        }

        /// <summary>
        /// Main accessor for the current GUI.
        /// </summary>
        public GUIManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }
    }
}
