namespace Mainframe.Forms
{
    partial class Create_New_Level
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
            this.sizeXLabel = new System.Windows.Forms.Label();
            this.sizeXSelector = new System.Windows.Forms.NumericUpDown();
            this.sizeYSelector = new System.Windows.Forms.NumericUpDown();
            this.sizeYLabel = new System.Windows.Forms.Label();
            this.levelName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sizeXSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeYSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(382, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(186, 238);
            this.button1.TabIndex = 0;
            this.button1.Text = "Create Level";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // sizeXLabel
            // 
            this.sizeXLabel.AutoSize = true;
            this.sizeXLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.sizeXLabel.Location = new System.Drawing.Point(12, 12);
            this.sizeXLabel.Name = "sizeXLabel";
            this.sizeXLabel.Size = new System.Drawing.Size(134, 26);
            this.sizeXLabel.TabIndex = 2;
            this.sizeXLabel.Text = "Width of grid";
            // 
            // sizeXSelector
            // 
            this.sizeXSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.sizeXSelector.Location = new System.Drawing.Point(237, 10);
            this.sizeXSelector.Name = "sizeXSelector";
            this.sizeXSelector.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.sizeXSelector.Size = new System.Drawing.Size(139, 32);
            this.sizeXSelector.TabIndex = 3;
            // 
            // sizeYSelector
            // 
            this.sizeYSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.sizeYSelector.Location = new System.Drawing.Point(237, 60);
            this.sizeYSelector.Name = "sizeYSelector";
            this.sizeYSelector.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.sizeYSelector.Size = new System.Drawing.Size(139, 32);
            this.sizeYSelector.TabIndex = 5;
            // 
            // sizeYLabel
            // 
            this.sizeYLabel.AutoSize = true;
            this.sizeYLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.sizeYLabel.Location = new System.Drawing.Point(12, 62);
            this.sizeYLabel.Name = "sizeYLabel";
            this.sizeYLabel.Size = new System.Drawing.Size(141, 26);
            this.sizeYLabel.TabIndex = 4;
            this.sizeYLabel.Text = "Height of grid";
            // 
            // levelName
            // 
            this.levelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.levelName.Location = new System.Drawing.Point(17, 218);
            this.levelName.Name = "levelName";
            this.levelName.Size = new System.Drawing.Size(359, 32);
            this.levelName.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label1.Location = new System.Drawing.Point(12, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Level Name";
            // 
            // Create_New_Level
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.levelName);
            this.Controls.Add(this.sizeYSelector);
            this.Controls.Add(this.sizeYLabel);
            this.Controls.Add(this.sizeXSelector);
            this.Controls.Add(this.sizeXLabel);
            this.Controls.Add(this.button1);
            this.Name = "Create_New_Level";
            this.Text = "Create_New_Level";
            ((System.ComponentModel.ISupportInitialize)(this.sizeXSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeYSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label sizeXLabel;
        private System.Windows.Forms.NumericUpDown sizeXSelector;
        private System.Windows.Forms.NumericUpDown sizeYSelector;
        private System.Windows.Forms.Label sizeYLabel;
        private System.Windows.Forms.TextBox levelName;
        private System.Windows.Forms.Label label1;
    }
}