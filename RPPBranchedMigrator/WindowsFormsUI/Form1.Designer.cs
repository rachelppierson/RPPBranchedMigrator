namespace WindowsFormsUI
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
            this.BtnTestCodeNow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnTestCodeNow
            // 
            this.BtnTestCodeNow.Location = new System.Drawing.Point(23, 23);
            this.BtnTestCodeNow.Name = "BtnTestCodeNow";
            this.BtnTestCodeNow.Size = new System.Drawing.Size(75, 23);
            this.BtnTestCodeNow.TabIndex = 0;
            this.BtnTestCodeNow.Text = "Test Code Now";
            this.BtnTestCodeNow.UseVisualStyleBackColor = true;
            this.BtnTestCodeNow.Click += new System.EventHandler(this.BtnTestCodeNow_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 70);
            this.Controls.Add(this.BtnTestCodeNow);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnTestCodeNow;
    }
}

