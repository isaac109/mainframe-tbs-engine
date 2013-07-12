namespace Mainframe.Forms
{
    partial class MainDeveloperMenu
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.AbilityEditorButton = new System.Windows.Forms.Button();
            this.HeroEditor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 111);
            this.button1.TabIndex = 0;
            this.button1.Text = "Launch Level Editor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(155, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 111);
            this.button2.TabIndex = 1;
            this.button2.Text = "Launch Content Importation Tool";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 279);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(259, 121);
            this.button3.TabIndex = 2;
            this.button3.Text = "Launch Game";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // AbilityEditorButton
            // 
            this.AbilityEditorButton.Location = new System.Drawing.Point(13, 130);
            this.AbilityEditorButton.Name = "AbilityEditorButton";
            this.AbilityEditorButton.Size = new System.Drawing.Size(136, 143);
            this.AbilityEditorButton.TabIndex = 3;
            this.AbilityEditorButton.Text = "Ability Statistic Editor";
            this.AbilityEditorButton.UseVisualStyleBackColor = true;
            this.AbilityEditorButton.Click += new System.EventHandler(this.AbilityEditorButton_Click);
            // 
            // HeroEditor
            // 
            this.HeroEditor.Location = new System.Drawing.Point(156, 130);
            this.HeroEditor.Name = "HeroEditor";
            this.HeroEditor.Size = new System.Drawing.Size(116, 143);
            this.HeroEditor.TabIndex = 4;
            this.HeroEditor.Text = "Hero class editor";
            this.HeroEditor.UseVisualStyleBackColor = true;
            // 
            // MainDeveloperMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 407);
            this.Controls.Add(this.HeroEditor);
            this.Controls.Add(this.AbilityEditorButton);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "MainDeveloperMenu";
            this.Text = "Mainframe Developer Tools";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button AbilityEditorButton;
        private System.Windows.Forms.Button HeroEditor;
    }
}