using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mainframe.Constants.Editor
{
    public class EditorConstantHolder
    {
        public enum EditingMode
        {
            Space,
            Wall,
            Character,
            Object,
            Connection,
            Trigger
        }

        public static EditingMode currentEditingMode = EditingMode.Space;

        #region Viewport

        public static int editorScrollPosX = 0, editorScrollPosY = 0;

        #endregion

        #region Spaces options
        public enum SpaceOptions
        {
            addRemove,
            elevation
        }

        public static SpaceOptions spaceEditingMode = SpaceOptions.addRemove;

        public static int spaceTypeComboBox;
        #endregion

        #region Walls options

        #endregion

        #region Characters Options

        public static bool addHeroes = true;

        #region Heroes

        public static int heroClassSel = 0;
        public static int heroLevel = 0;
        public static int heroSkin = 0;
        public static int heroAbility1Sel = 0;
        public static int heroAbility2Sel = 0;
        public static int heroAbility3Sel = 0;

        #endregion

        #region Persons

        #endregion

        #endregion

        #region Objects options

        #endregion

        #region Connections options

        #endregion

        #region trigger options

        #endregion
    }
}
