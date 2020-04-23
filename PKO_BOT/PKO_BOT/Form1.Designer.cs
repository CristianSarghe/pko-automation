namespace PKO_BOT
{
    partial class Form1
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
            this.initializeButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.registerMonstersButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.pickupCheckbox = new System.Windows.Forms.CheckBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // initializeButton
            // 
            this.initializeButton.Location = new System.Drawing.Point(709, 78);
            this.initializeButton.Name = "initializeButton";
            this.initializeButton.Size = new System.Drawing.Size(171, 31);
            this.initializeButton.TabIndex = 0;
            this.initializeButton.Text = "Initialize";
            this.initializeButton.UseVisualStyleBackColor = true;
            this.initializeButton.Click += new System.EventHandler(this.initializeButton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(13, 13);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(690, 345);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // registerMonstersButton
            // 
            this.registerMonstersButton.Enabled = false;
            this.registerMonstersButton.Location = new System.Drawing.Point(709, 127);
            this.registerMonstersButton.Name = "registerMonstersButton";
            this.registerMonstersButton.Size = new System.Drawing.Size(171, 31);
            this.registerMonstersButton.TabIndex = 6;
            this.registerMonstersButton.Text = "Register monsters";
            this.registerMonstersButton.UseVisualStyleBackColor = true;
            this.registerMonstersButton.Click += new System.EventHandler(this.registerMonstersButton_Click);
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Location = new System.Drawing.Point(709, 178);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(171, 31);
            this.startButton.TabIndex = 7;
            this.startButton.Text = "Start attacking";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // pickupCheckbox
            // 
            this.pickupCheckbox.AutoSize = true;
            this.pickupCheckbox.Enabled = false;
            this.pickupCheckbox.Location = new System.Drawing.Point(709, 231);
            this.pickupCheckbox.Name = "pickupCheckbox";
            this.pickupCheckbox.Size = new System.Drawing.Size(113, 17);
            this.pickupCheckbox.TabIndex = 8;
            this.pickupCheckbox.Text = "Auto pick up items";
            this.pickupCheckbox.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(709, 264);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(171, 31);
            this.stopButton.TabIndex = 9;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 386);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.pickupCheckbox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.registerMonstersButton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.initializeButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button initializeButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button registerMonstersButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.CheckBox pickupCheckbox;
        private System.Windows.Forms.Button stopButton;
    }
}

