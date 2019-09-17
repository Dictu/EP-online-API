namespace DemoWindowsFormsApplication
{
    partial class MainApplicationForm
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
            this.getTokenButton = new System.Windows.Forms.Button();
            this.bearerTextBox = new System.Windows.Forms.TextBox();
            this.toevoegenButton = new System.Windows.Forms.Button();
            this.vervangenButton = new System.Windows.Forms.Button();
            this.uitbreidenButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.hostTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // getTokenButton
            // 
            this.getTokenButton.Location = new System.Drawing.Point(603, 6);
            this.getTokenButton.Name = "getTokenButton";
            this.getTokenButton.Size = new System.Drawing.Size(191, 23);
            this.getTokenButton.TabIndex = 0;
            this.getTokenButton.Text = "Inloggen bij eHerkenning";
            this.getTokenButton.UseVisualStyleBackColor = true;
            this.getTokenButton.Click += new System.EventHandler(this.GetTokenButtonClick);
            // 
            // bearerTextBox
            // 
            this.bearerTextBox.Location = new System.Drawing.Point(15, 58);
            this.bearerTextBox.Multiline = true;
            this.bearerTextBox.Name = "bearerTextBox";
            this.bearerTextBox.Size = new System.Drawing.Size(779, 135);
            this.bearerTextBox.TabIndex = 1;
            // 
            // toevoegenButton
            // 
            this.toevoegenButton.Location = new System.Drawing.Point(15, 211);
            this.toevoegenButton.Name = "toevoegenButton";
            this.toevoegenButton.Size = new System.Drawing.Size(191, 23);
            this.toevoegenButton.TabIndex = 2;
            this.toevoegenButton.Text = "Energielabel registreren";
            this.toevoegenButton.UseVisualStyleBackColor = true;
            this.toevoegenButton.Click += new System.EventHandler(this.ToevoegenButtonClick);
            // 
            // vervangenButton
            // 
            this.vervangenButton.Location = new System.Drawing.Point(603, 211);
            this.vervangenButton.Name = "vervangenButton";
            this.vervangenButton.Size = new System.Drawing.Size(191, 23);
            this.vervangenButton.TabIndex = 3;
            this.vervangenButton.Text = "Energielabel vervangen";
            this.vervangenButton.UseVisualStyleBackColor = true;
            this.vervangenButton.Click += new System.EventHandler(this.VervangenButtonClick);
            // 
            // uitbreidenButton
            // 
            this.uitbreidenButton.Location = new System.Drawing.Point(316, 211);
            this.uitbreidenButton.Name = "uitbreidenButton";
            this.uitbreidenButton.Size = new System.Drawing.Size(191, 23);
            this.uitbreidenButton.TabIndex = 4;
            this.uitbreidenButton.Text = "Energielabel uitbreiden";
            this.uitbreidenButton.UseVisualStyleBackColor = true;
            this.uitbreidenButton.Click += new System.EventHandler(this.UitbreidenButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "EP-online API Host";
            // 
            // hostTextBox
            // 
            this.hostTextBox.Location = new System.Drawing.Point(117, 9);
            this.hostTextBox.Name = "hostTextBox";
            this.hostTextBox.Size = new System.Drawing.Size(480, 20);
            this.hostTextBox.TabIndex = 6;
            this.hostTextBox.Text = "Vul hier de url naar het token endpoint in.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Bearer token";
            // 
            // MainApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 305);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hostTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uitbreidenButton);
            this.Controls.Add(this.vervangenButton);
            this.Controls.Add(this.toevoegenButton);
            this.Controls.Add(this.bearerTextBox);
            this.Controls.Add(this.getTokenButton);
            this.Name = "MainApplicationForm";
            this.Text = "EP-online Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getTokenButton;
        private System.Windows.Forms.TextBox bearerTextBox;
        private System.Windows.Forms.Button toevoegenButton;
        private System.Windows.Forms.Button vervangenButton;
        private System.Windows.Forms.Button uitbreidenButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hostTextBox;
        private System.Windows.Forms.Label label2;
    }
}

