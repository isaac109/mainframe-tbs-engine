namespace Mainframe.Forms
{
    partial class Mainframe_Level_Editor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pctSurface = new System.Windows.Forms.PictureBox();
            this.EditorTabControl = new System.Windows.Forms.TabControl();
            this.spaceTab = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_AddRemove = new System.Windows.Forms.TabPage();
            this.gridSpaceComboBox = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tab_SpaceElevation = new System.Windows.Forms.TabPage();
            this.wallTab = new System.Windows.Forms.TabPage();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.characterTab = new System.Windows.Forms.TabPage();
            this.characterControl = new System.Windows.Forms.TabControl();
            this.characterTab_Hero = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.abilityComboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.abilityComboBox2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.abilityComboBox3 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.heroSkinComboBox = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.classComboBox = new System.Windows.Forms.ComboBox();
            this.CharacterTab_Person = new System.Windows.Forms.TabPage();
            this.objectTab = new System.Windows.Forms.TabPage();
            this.connectionTab = new System.Windows.Forms.TabPage();
            this.triggerTab = new System.Windows.Forms.TabPage();
            this.Instructions = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalScrollBar_Grid = new System.Windows.Forms.HScrollBar();
            this.verticalScrollBar_Grid = new System.Windows.Forms.VScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).BeginInit();
            this.EditorTabControl.SuspendLayout();
            this.spaceTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_AddRemove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.wallTab.SuspendLayout();
            this.characterTab.SuspendLayout();
            this.characterControl.SuspendLayout();
            this.characterTab_Hero.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pctSurface
            // 
            this.pctSurface.Location = new System.Drawing.Point(8, 30);
            this.pctSurface.Name = "pctSurface";
            this.pctSurface.Size = new System.Drawing.Size(800, 600);
            this.pctSurface.TabIndex = 0;
            this.pctSurface.TabStop = false;
            // 
            // EditorTabControl
            // 
            this.EditorTabControl.Controls.Add(this.spaceTab);
            this.EditorTabControl.Controls.Add(this.wallTab);
            this.EditorTabControl.Controls.Add(this.characterTab);
            this.EditorTabControl.Controls.Add(this.objectTab);
            this.EditorTabControl.Controls.Add(this.connectionTab);
            this.EditorTabControl.Controls.Add(this.triggerTab);
            this.EditorTabControl.Location = new System.Drawing.Point(874, 8);
            this.EditorTabControl.Name = "EditorTabControl";
            this.EditorTabControl.SelectedIndex = 0;
            this.EditorTabControl.Size = new System.Drawing.Size(302, 946);
            this.EditorTabControl.TabIndex = 0;
            this.EditorTabControl.SelectedIndexChanged += editorTabControl_SelIndexChanged;
            // 
            // spaceTab
            // 
            this.spaceTab.Controls.Add(this.tabControl1);
            this.spaceTab.Location = new System.Drawing.Point(4, 22);
            this.spaceTab.Name = "spaceTab";
            this.spaceTab.Padding = new System.Windows.Forms.Padding(3);
            this.spaceTab.Size = new System.Drawing.Size(294, 920);
            this.spaceTab.TabIndex = 0;
            this.spaceTab.Text = "Spaces";
            this.spaceTab.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_AddRemove);
            this.tabControl1.Controls.Add(this.tab_SpaceElevation);
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(282, 908);
            this.tabControl1.TabIndex = 0;
            // 
            // tab_AddRemove
            // 
            this.tab_AddRemove.Controls.Add(this.gridSpaceComboBox);
            this.tab_AddRemove.Controls.Add(this.pictureBox1);
            this.tab_AddRemove.Location = new System.Drawing.Point(4, 22);
            this.tab_AddRemove.Name = "tab_AddRemove";
            this.tab_AddRemove.Padding = new System.Windows.Forms.Padding(3);
            this.tab_AddRemove.Size = new System.Drawing.Size(274, 882);
            this.tab_AddRemove.TabIndex = 0;
            this.tab_AddRemove.Text = "Add/Remove Spaces";
            this.tab_AddRemove.UseVisualStyleBackColor = true;
            // 
            // gridSpaceComboBox
            // 
            this.gridSpaceComboBox.FormattingEnabled = true;
            this.gridSpaceComboBox.Location = new System.Drawing.Point(7, 7);
            this.gridSpaceComboBox.Name = "gridSpaceComboBox";
            this.gridSpaceComboBox.Size = new System.Drawing.Size(121, 21);
            this.gridSpaceComboBox.TabIndex = 1;
            this.gridSpaceComboBox.SelectedIndexChanged += new System.EventHandler(this.gridSpaceComboBox_SelIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 336);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tab_SpaceElevation
            // 
            this.tab_SpaceElevation.Location = new System.Drawing.Point(4, 22);
            this.tab_SpaceElevation.Name = "tab_SpaceElevation";
            this.tab_SpaceElevation.Padding = new System.Windows.Forms.Padding(3);
            this.tab_SpaceElevation.Size = new System.Drawing.Size(274, 882);
            this.tab_SpaceElevation.TabIndex = 1;
            this.tab_SpaceElevation.Text = "Change Elevations";
            this.tab_SpaceElevation.UseVisualStyleBackColor = true;
            // 
            // wallTab
            // 
            this.wallTab.Controls.Add(this.radioButton5);
            this.wallTab.Controls.Add(this.radioButton4);
            this.wallTab.Location = new System.Drawing.Point(4, 22);
            this.wallTab.Name = "wallTab";
            this.wallTab.Padding = new System.Windows.Forms.Padding(3);
            this.wallTab.Size = new System.Drawing.Size(294, 920);
            this.wallTab.TabIndex = 1;
            this.wallTab.Text = "Walls";
            this.wallTab.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(7, 30);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(85, 17);
            this.radioButton5.TabIndex = 1;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "radioButton5";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(7, 7);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(85, 17);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "radioButton4";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // characterTab
            // 
            this.characterTab.Controls.Add(this.characterControl);
            this.characterTab.Location = new System.Drawing.Point(4, 22);
            this.characterTab.Name = "characterTab";
            this.characterTab.Padding = new System.Windows.Forms.Padding(3);
            this.characterTab.Size = new System.Drawing.Size(294, 920);
            this.characterTab.TabIndex = 2;
            this.characterTab.Text = "Characters";
            this.characterTab.UseVisualStyleBackColor = true;
            // 
            // characterControl
            // 
            this.characterControl.Controls.Add(this.characterTab_Hero);
            this.characterControl.Controls.Add(this.CharacterTab_Person);
            this.characterControl.Location = new System.Drawing.Point(7, 7);
            this.characterControl.Name = "characterControl";
            this.characterControl.SelectedIndex = 0;
            this.characterControl.Size = new System.Drawing.Size(281, 907);
            this.characterControl.TabIndex = 0;
            // 
            // characterTab_Hero
            // 
            this.characterTab_Hero.Controls.Add(this.groupBox1);
            this.characterTab_Hero.Controls.Add(this.label3);
            this.characterTab_Hero.Controls.Add(this.heroSkinComboBox);
            this.characterTab_Hero.Controls.Add(this.numericUpDown1);
            this.characterTab_Hero.Controls.Add(this.label2);
            this.characterTab_Hero.Controls.Add(this.label1);
            this.characterTab_Hero.Controls.Add(this.classComboBox);
            this.characterTab_Hero.Location = new System.Drawing.Point(4, 22);
            this.characterTab_Hero.Name = "characterTab_Hero";
            this.characterTab_Hero.Padding = new System.Windows.Forms.Padding(3);
            this.characterTab_Hero.Size = new System.Drawing.Size(273, 881);
            this.characterTab_Hero.TabIndex = 0;
            this.characterTab_Hero.Text = "Hero";
            this.characterTab_Hero.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.abilityComboBox1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.abilityComboBox2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.abilityComboBox3);
            this.groupBox1.Location = new System.Drawing.Point(6, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 278);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Special Ability 1";
            // 
            // abilityComboBox1
            // 
            this.abilityComboBox1.FormattingEnabled = true;
            this.abilityComboBox1.Location = new System.Drawing.Point(6, 32);
            this.abilityComboBox1.Name = "abilityComboBox1";
            this.abilityComboBox1.Size = new System.Drawing.Size(121, 21);
            this.abilityComboBox1.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Special Ability 2";
            // 
            // abilityComboBox2
            // 
            this.abilityComboBox2.FormattingEnabled = true;
            this.abilityComboBox2.Location = new System.Drawing.Point(6, 80);
            this.abilityComboBox2.Name = "abilityComboBox2";
            this.abilityComboBox2.Size = new System.Drawing.Size(121, 21);
            this.abilityComboBox2.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Special Ability 3";
            // 
            // abilityComboBox3
            // 
            this.abilityComboBox3.FormattingEnabled = true;
            this.abilityComboBox3.Location = new System.Drawing.Point(6, 128);
            this.abilityComboBox3.Name = "abilityComboBox3";
            this.abilityComboBox3.Size = new System.Drawing.Size(121, 21);
            this.abilityComboBox3.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Choose character skin";
            // 
            // heroSkinComboBox
            // 
            this.heroSkinComboBox.FormattingEnabled = true;
            this.heroSkinComboBox.Location = new System.Drawing.Point(6, 73);
            this.heroSkinComboBox.Name = "heroSkinComboBox";
            this.heroSkinComboBox.Size = new System.Drawing.Size(121, 21);
            this.heroSkinComboBox.TabIndex = 4;
            this.heroSkinComboBox.SelectedIndexChanged += new System.EventHandler(this.heroSkinComboBox_SelectedIndexChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(147, 20);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Character Level";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose character class";
            // 
            // classComboBox
            // 
            this.classComboBox.FormattingEnabled = true;
            this.classComboBox.Location = new System.Drawing.Point(6, 19);
            this.classComboBox.Name = "classComboBox";
            this.classComboBox.Size = new System.Drawing.Size(121, 21);
            this.classComboBox.TabIndex = 0;
            // 
            // CharacterTab_Person
            // 
            this.CharacterTab_Person.Location = new System.Drawing.Point(4, 22);
            this.CharacterTab_Person.Name = "CharacterTab_Person";
            this.CharacterTab_Person.Padding = new System.Windows.Forms.Padding(3);
            this.CharacterTab_Person.Size = new System.Drawing.Size(273, 881);
            this.CharacterTab_Person.TabIndex = 1;
            this.CharacterTab_Person.Text = "Person";
            this.CharacterTab_Person.UseVisualStyleBackColor = true;
            // 
            // objectTab
            // 
            this.objectTab.Location = new System.Drawing.Point(4, 22);
            this.objectTab.Name = "objectTab";
            this.objectTab.Padding = new System.Windows.Forms.Padding(3);
            this.objectTab.Size = new System.Drawing.Size(294, 920);
            this.objectTab.TabIndex = 3;
            this.objectTab.Text = "Objects";
            this.objectTab.UseVisualStyleBackColor = true;
            // 
            // connectionTab
            // 
            this.connectionTab.Location = new System.Drawing.Point(4, 22);
            this.connectionTab.Name = "connectionTab";
            this.connectionTab.Padding = new System.Windows.Forms.Padding(3);
            this.connectionTab.Size = new System.Drawing.Size(294, 920);
            this.connectionTab.TabIndex = 4;
            this.connectionTab.Text = "Connections";
            this.connectionTab.UseVisualStyleBackColor = true;
            // 
            // triggerTab
            // 
            this.triggerTab.Location = new System.Drawing.Point(4, 22);
            this.triggerTab.Name = "triggerTab";
            this.triggerTab.Padding = new System.Windows.Forms.Padding(3);
            this.triggerTab.Size = new System.Drawing.Size(294, 920);
            this.triggerTab.TabIndex = 5;
            this.triggerTab.Text = "Triggers";
            this.triggerTab.UseVisualStyleBackColor = true;
            // 
            // Instructions
            // 
            this.Instructions.AutoSize = true;
            this.Instructions.Location = new System.Drawing.Point(8, 670);
            this.Instructions.Name = "Instructions";
            this.Instructions.Size = new System.Drawing.Size(428, 39);
            this.Instructions.TabIndex = 3;
            this.Instructions.Text = "Controls: Left click to place/paint on, Right click to remove. Middle click to mo" +
    "ve the grid.\r\n\r\nTo pull properties from an object into the brush, shift click.";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(5, 5);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1174, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // horizontalScrollBar_Grid
            // 
            this.horizontalScrollBar_Grid.Location = new System.Drawing.Point(8, 630);
            this.horizontalScrollBar_Grid.Name = "horizontalScrollBar_Grid";
            this.horizontalScrollBar_Grid.Size = new System.Drawing.Size(800, 30);
            this.horizontalScrollBar_Grid.TabIndex = 5;
            this.horizontalScrollBar_Grid.Scroll += new System.Windows.Forms.ScrollEventHandler(this.horizontalScrollBar_Grid_Scroll);
            // 
            // verticalScrollBar_Grid
            // 
            this.verticalScrollBar_Grid.Location = new System.Drawing.Point(808, 30);
            this.verticalScrollBar_Grid.Name = "verticalScrollBar_Grid";
            this.verticalScrollBar_Grid.Size = new System.Drawing.Size(30, 600);
            this.verticalScrollBar_Grid.TabIndex = 6;
            this.verticalScrollBar_Grid.Scroll += new System.Windows.Forms.ScrollEventHandler(this.verticalScrollBar_Grid_Scroll);
            // 
            // Mainframe_Level_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 962);
            this.Controls.Add(this.verticalScrollBar_Grid);
            this.Controls.Add(this.horizontalScrollBar_Grid);
            this.Controls.Add(this.Instructions);
            this.Controls.Add(this.EditorTabControl);
            this.Controls.Add(this.pctSurface);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Mainframe_Level_Editor";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Mainframe_Level_Editor";
            ((System.ComponentModel.ISupportInitialize)(this.pctSurface)).EndInit();
            this.EditorTabControl.ResumeLayout(false);
            this.spaceTab.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tab_AddRemove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.wallTab.ResumeLayout(false);
            this.wallTab.PerformLayout();
            this.characterTab.ResumeLayout(false);
            this.characterControl.ResumeLayout(false);
            this.characterTab_Hero.ResumeLayout(false);
            this.characterTab_Hero.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pctSurface;
        private System.Windows.Forms.TabControl EditorTabControl;
        private System.Windows.Forms.TabPage spaceTab;
        private System.Windows.Forms.TabPage wallTab;
        private System.Windows.Forms.TabPage characterTab;
        private System.Windows.Forms.TabPage objectTab;
        private System.Windows.Forms.Label Instructions;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabPage connectionTab;
        private System.Windows.Forms.TabPage triggerTab;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.HScrollBar horizontalScrollBar_Grid;
        private System.Windows.Forms.VScrollBar verticalScrollBar_Grid;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_AddRemove;
        private System.Windows.Forms.ComboBox gridSpaceComboBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tab_SpaceElevation;
        private System.Windows.Forms.TabControl characterControl;
        private System.Windows.Forms.TabPage characterTab_Hero;
        private System.Windows.Forms.TabPage CharacterTab_Person;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox classComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox heroSkinComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox abilityComboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox abilityComboBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox abilityComboBox3;
    }
}