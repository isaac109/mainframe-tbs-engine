using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mainframe.Core;

namespace Mainframe.Forms
{
    public partial class Create_New_Level : Form
    {
        MainframeLevelEditor game;
        Mainframe_Level_Editor editor;

        public Create_New_Level(MainframeLevelEditor Game, Mainframe_Level_Editor editor)
        {
            InitializeComponent();
            game = Game;
            this.editor = editor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.makeNewLevel((int) sizeXSelector.Value, (int)sizeYSelector.Value, levelName.Text);
            this.Close();
        }
    }
}
