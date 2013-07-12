using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mainframe.Core;
using System.Threading;

namespace Mainframe.Forms
{
    public partial class MainDeveloperMenu : Form
    {
        public MainDeveloperMenu()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(GameThread, 0);
            newThread.Start();
        }

        private void GameThread()
        {
            MainframeGame game = new MainframeGame();
            game.Run();
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(levelEditorThread, 0);
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }

        private void levelEditorThread()
        {
            Mainframe_Level_Editor editor = new Mainframe_Level_Editor();
            editor.Show();
            MainframeLevelEditor game = new MainframeLevelEditor(editor.getDrawSurface(), editor);
            game.setMouseHandle(editor.getDrawSurface());
            editor.setGameFocus(game);
            game.Run();
        }

        private void AbilityEditorButton_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(AbilityEditorThread, 0);
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
        }

        private void AbilityEditorThread()
        {
            AbilityEditor abed = new AbilityEditor();
            abed.Show();
            abed.Focus();
            abed.Activate();
        }
    }
}
