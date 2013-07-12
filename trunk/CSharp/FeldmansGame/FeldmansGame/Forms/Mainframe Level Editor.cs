using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mainframe.Core;
using Mainframe.Constants;
using System.Threading;
using System.IO;
using Mainframe.Saving;
using Mainframe.Constants.Editor;

namespace Mainframe.Forms
{
    public partial class Mainframe_Level_Editor : Form
    {
        MainframeLevelEditor Game;
        SaveFileDialog saveDialog;
        OpenFileDialog openDialog;

        public Mainframe_Level_Editor()
        {
            InitializeComponent();
            pctSurface.Width = ConstantHolder.GAME_WIDTH;
            pctSurface.Height = ConstantHolder.GAME_HEIGHT;
            Width = pctSurface.Width + 400;
            Height = pctSurface.Height + 200;
            EditorTabControl.SetBounds(ConstantHolder.GAME_WIDTH + 60, 30, Width - ConstantHolder.GAME_WIDTH - 60, Height - 100);
            Instructions.SetBounds(pctSurface.Bounds.X, pctSurface.Height + pctSurface.Bounds.Y + 45, Instructions.Width, Instructions.Height);
            verticalScrollBar_Grid.SetBounds(ConstantHolder.GAME_WIDTH + pctSurface.Bounds.X, pctSurface.Bounds.Y, 30, ConstantHolder.GAME_HEIGHT);
            horizontalScrollBar_Grid.SetBounds(pctSurface.Bounds.X, ConstantHolder.GAME_HEIGHT + pctSurface.Bounds.Y, ConstantHolder.GAME_WIDTH, 30);
            classComboBox.DataSource = System.Enum.GetValues(typeof(ConstantHolder.HeroClasses));
            abilityComboBox1.DataSource = System.Enum.GetValues(typeof(ConstantHolder.Abilities));
            abilityComboBox2.DataSource = System.Enum.GetValues(typeof(ConstantHolder.Abilities));
            abilityComboBox3.DataSource = System.Enum.GetValues(typeof(ConstantHolder.Abilities));
        }

        private void Mainframe_Level_Editor_Resize(object sender, System.EventArgs e)
        {
            EditorTabControl.SetBounds(ConstantHolder.GAME_WIDTH + 60, 30, Width - ConstantHolder.GAME_WIDTH - 60, Height - 100);
            Instructions.SetBounds(pctSurface.Bounds.X, pctSurface.Height + pctSurface.Bounds.Y + 45, Instructions.Width, Instructions.Height);
            verticalScrollBar_Grid.SetBounds(ConstantHolder.GAME_WIDTH + pctSurface.Bounds.X, pctSurface.Bounds.Y, 30, ConstantHolder.GAME_HEIGHT);
            horizontalScrollBar_Grid.SetBounds(pctSurface.Bounds.X, ConstantHolder.GAME_HEIGHT + pctSurface.Bounds.Y, ConstantHolder.GAME_WIDTH, 30);
        }

        public void setGameFocus(MainframeLevelEditor currentGame)
        {
            Game = currentGame;
        }

        public IntPtr getDrawSurface()
        {
            return pctSurface.Handle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridSpaceComboBox.Items.Clear();
            heroSkinComboBox.Items.Clear();
            foreach (KeyValuePair<string, int> kvp in ConstantHolder.GridSpaceTypeDict)
            {
                gridSpaceComboBox.Items.Add(kvp);
            }
            foreach (KeyValuePair<string, int> kvp in ConstantHolder.ActorImageTypeDict)
            {
                heroSkinComboBox.Items.Add(kvp);
            }
            Thread createNewLevelThread = new Thread(makeNewLevel);
            createNewLevelThread.SetApartmentState(ApartmentState.STA);
            createNewLevelThread.Start();
        }

        private void makeNewLevel()
        {
            Create_New_Level dialog = new Create_New_Level(Game, this);
            dialog.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (Game.currentLevel != null)
            {
                Game.currentLevel.saveLevelXML(saveDialog.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDialog = new SaveFileDialog();
            saveDialog.FileOk += saveFileDialog1_FileOk;
            saveDialog.ShowDialog();
        }

        public void setBarTicks(int width, int height)
        {
            horizontalScrollBar_Grid.Maximum = width;
            verticalScrollBar_Grid.Maximum = height;
        }

        private void verticalScrollBar_Grid_Scroll(object sender, ScrollEventArgs e)
        {
            EditorConstantHolder.editorScrollPosY = verticalScrollBar_Grid.Value;
        }

        private void horizontalScrollBar_Grid_Scroll(object sender, ScrollEventArgs e)
        {
            EditorConstantHolder.editorScrollPosX = horizontalScrollBar_Grid.Value;
        }

        private void openFileDialog1_FileOk(object sender, EventArgs e)
        {
            gridSpaceComboBox.Items.Clear();
            heroSkinComboBox.Items.Clear();
            foreach (KeyValuePair<string, int> kvp in ConstantHolder.GridSpaceTypeDict)
            {
                gridSpaceComboBox.Items.Add(kvp);
            }
            foreach (KeyValuePair<string, int> kvp in ConstantHolder.ActorImageTypeDict)
            {
                heroSkinComboBox.Items.Add(kvp);
            }
            Game.currentLevel = Level.loadSimpleLevelXML(openDialog.FileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDialog = new OpenFileDialog();
            openDialog.FileOk += openFileDialog1_FileOk;
            openDialog.ShowDialog();
        }

        private void gridSpaceComboBox_SelIndexChanged(object sender, EventArgs e)
        {
            EditorConstantHolder.spaceTypeComboBox = ConstantHolder.GridSpaceTypeDict[((KeyValuePair<string, int>)gridSpaceComboBox.SelectedItem).Key];
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditorConstantHolder.spaceEditingMode = EditorConstantHolder.SpaceOptions.elevation;
        }

        private void editorTabControl_SelIndexChanged(object sender, EventArgs e)
        {
            EditorConstantHolder.currentEditingMode = (EditorConstantHolder.EditingMode)EditorTabControl.SelectedIndex;
        }

        private void characterControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditorConstantHolder.addHeroes = characterControl.SelectedIndex == 0;
        }

        private void heroSkinComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditorConstantHolder.heroSkin = ConstantHolder.ActorImageTypeDict[((KeyValuePair<string, int>)heroSkinComboBox.SelectedItem).Key];
        }
    }
}
