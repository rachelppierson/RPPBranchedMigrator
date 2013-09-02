namespace WindowsFormsUI
{
    partial class WindowsFormsUI
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
            this.TxbConnectionString = new System.Windows.Forms.TextBox();
            this.LblConnectionString = new System.Windows.Forms.Label();
            this.SplMainUIArea = new System.Windows.Forms.SplitContainer();
            this.LblAvailableMigrations = new System.Windows.Forms.Label();
            this.DGVAvailableMigrations = new System.Windows.Forms.DataGridView();
            this.LblEquivCommand = new System.Windows.Forms.Label();
            this.TxbCommandLineEquivalent = new System.Windows.Forms.TextBox();
            this.BtnRunMigrations = new System.Windows.Forms.Button();
            this.LblMigFilesFolder = new System.Windows.Forms.Label();
            this.TxbMigFilesFolder = new System.Windows.Forms.TextBox();
            this.BtnMigFilesFolderEllipsis = new System.Windows.Forms.Button();
            this.BtnTestConnection = new System.Windows.Forms.Button();
            this.LblConnectionTestSucceeded = new System.Windows.Forms.Label();
            this.LblConnectionTestFailed = new System.Windows.Forms.Label();
            this.PnlMigDirection = new System.Windows.Forms.Panel();
            this.RbnMigDirectionDown = new System.Windows.Forms.RadioButton();
            this.RbnMigDirectionUp = new System.Windows.Forms.RadioButton();
            this.LblMigDirection = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SplMainUIArea)).BeginInit();
            this.SplMainUIArea.Panel1.SuspendLayout();
            this.SplMainUIArea.Panel2.SuspendLayout();
            this.SplMainUIArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVAvailableMigrations)).BeginInit();
            this.PnlMigDirection.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnTestCodeNow
            // 
            this.BtnTestCodeNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnTestCodeNow.Location = new System.Drawing.Point(433, 432);
            this.BtnTestCodeNow.Name = "BtnTestCodeNow";
            this.BtnTestCodeNow.Size = new System.Drawing.Size(75, 23);
            this.BtnTestCodeNow.TabIndex = 0;
            this.BtnTestCodeNow.Text = "Test Code Now";
            this.BtnTestCodeNow.UseVisualStyleBackColor = true;
            this.BtnTestCodeNow.Click += new System.EventHandler(this.BtnTestCodeNow_Click);
            // 
            // TxbConnectionString
            // 
            this.TxbConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TxbConnectionString.Location = new System.Drawing.Point(12, 33);
            this.TxbConnectionString.Multiline = true;
            this.TxbConnectionString.Name = "TxbConnectionString";
            this.TxbConnectionString.Size = new System.Drawing.Size(512, 54);
            this.TxbConnectionString.TabIndex = 3;
            this.TxbConnectionString.TextChanged += new System.EventHandler(this.TxbConnectionString_TextChanged);
            // 
            // LblConnectionString
            // 
            this.LblConnectionString.AutoSize = true;
            this.LblConnectionString.Location = new System.Drawing.Point(9, 15);
            this.LblConnectionString.Name = "LblConnectionString";
            this.LblConnectionString.Size = new System.Drawing.Size(94, 13);
            this.LblConnectionString.TabIndex = 4;
            this.LblConnectionString.Text = "Connection String:";
            // 
            // SplMainUIArea
            // 
            this.SplMainUIArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SplMainUIArea.Location = new System.Drawing.Point(12, 118);
            this.SplMainUIArea.Name = "SplMainUIArea";
            this.SplMainUIArea.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplMainUIArea.Panel1
            // 
            this.SplMainUIArea.Panel1.Controls.Add(this.LblAvailableMigrations);
            this.SplMainUIArea.Panel1.Controls.Add(this.DGVAvailableMigrations);
            // 
            // SplMainUIArea.Panel2
            // 
            this.SplMainUIArea.Panel2.Controls.Add(this.LblEquivCommand);
            this.SplMainUIArea.Panel2.Controls.Add(this.TxbCommandLineEquivalent);
            this.SplMainUIArea.Size = new System.Drawing.Size(596, 303);
            this.SplMainUIArea.SplitterDistance = 189;
            this.SplMainUIArea.TabIndex = 5;
            // 
            // LblAvailableMigrations
            // 
            this.LblAvailableMigrations.AutoSize = true;
            this.LblAvailableMigrations.Location = new System.Drawing.Point(-3, 8);
            this.LblAvailableMigrations.Name = "LblAvailableMigrations";
            this.LblAvailableMigrations.Size = new System.Drawing.Size(104, 13);
            this.LblAvailableMigrations.TabIndex = 4;
            this.LblAvailableMigrations.Text = "Available Migrations:";
            // 
            // DGVAvailableMigrations
            // 
            this.DGVAvailableMigrations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVAvailableMigrations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGVAvailableMigrations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVAvailableMigrations.Enabled = false;
            this.DGVAvailableMigrations.Location = new System.Drawing.Point(2, 28);
            this.DGVAvailableMigrations.MultiSelect = false;
            this.DGVAvailableMigrations.Name = "DGVAvailableMigrations";
            this.DGVAvailableMigrations.Size = new System.Drawing.Size(592, 154);
            this.DGVAvailableMigrations.TabIndex = 3;
            // 
            // LblEquivCommand
            // 
            this.LblEquivCommand.AutoSize = true;
            this.LblEquivCommand.Location = new System.Drawing.Point(-1, 8);
            this.LblEquivCommand.Name = "LblEquivCommand";
            this.LblEquivCommand.Size = new System.Drawing.Size(183, 13);
            this.LblEquivCommand.TabIndex = 5;
            this.LblEquivCommand.Text = "Equivalent Command Line Command:";
            // 
            // TxbCommandLineEquivalent
            // 
            this.TxbCommandLineEquivalent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TxbCommandLineEquivalent.Location = new System.Drawing.Point(0, 24);
            this.TxbCommandLineEquivalent.Multiline = true;
            this.TxbCommandLineEquivalent.Name = "TxbCommandLineEquivalent";
            this.TxbCommandLineEquivalent.Size = new System.Drawing.Size(593, 83);
            this.TxbCommandLineEquivalent.TabIndex = 0;
            // 
            // BtnRunMigrations
            // 
            this.BtnRunMigrations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRunMigrations.Enabled = false;
            this.BtnRunMigrations.Location = new System.Drawing.Point(514, 432);
            this.BtnRunMigrations.Name = "BtnRunMigrations";
            this.BtnRunMigrations.Size = new System.Drawing.Size(94, 23);
            this.BtnRunMigrations.TabIndex = 6;
            this.BtnRunMigrations.Text = "Run Migrations";
            this.BtnRunMigrations.UseVisualStyleBackColor = true;
            this.BtnRunMigrations.Click += new System.EventHandler(this.BtnRunMigrations_Click);
            // 
            // LblMigFilesFolder
            // 
            this.LblMigFilesFolder.AutoSize = true;
            this.LblMigFilesFolder.Location = new System.Drawing.Point(9, 97);
            this.LblMigFilesFolder.Name = "LblMigFilesFolder";
            this.LblMigFilesFolder.Size = new System.Drawing.Size(109, 13);
            this.LblMigFilesFolder.TabIndex = 7;
            this.LblMigFilesFolder.Text = "Migration Files Folder:";
            // 
            // TxbMigFilesFolder
            // 
            this.TxbMigFilesFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TxbMigFilesFolder.Location = new System.Drawing.Point(119, 94);
            this.TxbMigFilesFolder.Name = "TxbMigFilesFolder";
            this.TxbMigFilesFolder.Size = new System.Drawing.Size(460, 20);
            this.TxbMigFilesFolder.TabIndex = 8;
            this.TxbMigFilesFolder.TextChanged += new System.EventHandler(this.TxbMigFilesFolder_TextChanged);
            // 
            // BtnMigFilesFolderEllipsis
            // 
            this.BtnMigFilesFolderEllipsis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMigFilesFolderEllipsis.Location = new System.Drawing.Point(580, 92);
            this.BtnMigFilesFolderEllipsis.Name = "BtnMigFilesFolderEllipsis";
            this.BtnMigFilesFolderEllipsis.Size = new System.Drawing.Size(25, 23);
            this.BtnMigFilesFolderEllipsis.TabIndex = 9;
            this.BtnMigFilesFolderEllipsis.Text = "...";
            this.BtnMigFilesFolderEllipsis.UseVisualStyleBackColor = true;
            this.BtnMigFilesFolderEllipsis.Click += new System.EventHandler(this.BtnMigFilesFolderEllipsis_Click);
            // 
            // BtnTestConnection
            // 
            this.BtnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnTestConnection.Location = new System.Drawing.Point(533, 33);
            this.BtnTestConnection.Name = "BtnTestConnection";
            this.BtnTestConnection.Size = new System.Drawing.Size(75, 35);
            this.BtnTestConnection.TabIndex = 10;
            this.BtnTestConnection.Text = "Test Connection";
            this.BtnTestConnection.UseVisualStyleBackColor = true;
            this.BtnTestConnection.Click += new System.EventHandler(this.BtnTestConnection_Click);
            // 
            // LblConnectionTestSucceeded
            // 
            this.LblConnectionTestSucceeded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblConnectionTestSucceeded.ForeColor = System.Drawing.Color.ForestGreen;
            this.LblConnectionTestSucceeded.Location = new System.Drawing.Point(530, 72);
            this.LblConnectionTestSucceeded.Name = "LblConnectionTestSucceeded";
            this.LblConnectionTestSucceeded.Size = new System.Drawing.Size(75, 13);
            this.LblConnectionTestSucceeded.TabIndex = 11;
            this.LblConnectionTestSucceeded.Text = "Succeeded!";
            this.LblConnectionTestSucceeded.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LblConnectionTestSucceeded.Visible = false;
            // 
            // LblConnectionTestFailed
            // 
            this.LblConnectionTestFailed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblConnectionTestFailed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.LblConnectionTestFailed.Location = new System.Drawing.Point(530, 72);
            this.LblConnectionTestFailed.Name = "LblConnectionTestFailed";
            this.LblConnectionTestFailed.Size = new System.Drawing.Size(75, 13);
            this.LblConnectionTestFailed.TabIndex = 12;
            this.LblConnectionTestFailed.Text = "Failed!";
            this.LblConnectionTestFailed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LblConnectionTestFailed.Visible = false;
            // 
            // PnlMigDirection
            // 
            this.PnlMigDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PnlMigDirection.Controls.Add(this.RbnMigDirectionDown);
            this.PnlMigDirection.Controls.Add(this.RbnMigDirectionUp);
            this.PnlMigDirection.Controls.Add(this.LblMigDirection);
            this.PnlMigDirection.Location = new System.Drawing.Point(9, 429);
            this.PnlMigDirection.Name = "PnlMigDirection";
            this.PnlMigDirection.Size = new System.Drawing.Size(230, 28);
            this.PnlMigDirection.TabIndex = 16;
            // 
            // RbnMigDirectionDown
            // 
            this.RbnMigDirectionDown.AutoSize = true;
            this.RbnMigDirectionDown.Location = new System.Drawing.Point(159, 6);
            this.RbnMigDirectionDown.Name = "RbnMigDirectionDown";
            this.RbnMigDirectionDown.Size = new System.Drawing.Size(60, 17);
            this.RbnMigDirectionDown.TabIndex = 18;
            this.RbnMigDirectionDown.TabStop = true;
            this.RbnMigDirectionDown.Text = "DOWN";
            this.RbnMigDirectionDown.UseVisualStyleBackColor = true;
            this.RbnMigDirectionDown.CheckedChanged += new System.EventHandler(this.RbnMigDirectionDown_CheckedChanged);
            // 
            // RbnMigDirectionUp
            // 
            this.RbnMigDirectionUp.AutoSize = true;
            this.RbnMigDirectionUp.Checked = true;
            this.RbnMigDirectionUp.Location = new System.Drawing.Point(114, 6);
            this.RbnMigDirectionUp.Name = "RbnMigDirectionUp";
            this.RbnMigDirectionUp.Size = new System.Drawing.Size(40, 17);
            this.RbnMigDirectionUp.TabIndex = 17;
            this.RbnMigDirectionUp.TabStop = true;
            this.RbnMigDirectionUp.Text = "UP";
            this.RbnMigDirectionUp.UseVisualStyleBackColor = true;
            this.RbnMigDirectionUp.CheckedChanged += new System.EventHandler(this.RbnMigDirectionUp_CheckedChanged);
            // 
            // LblMigDirection
            // 
            this.LblMigDirection.AutoSize = true;
            this.LblMigDirection.Location = new System.Drawing.Point(6, 8);
            this.LblMigDirection.Name = "LblMigDirection";
            this.LblMigDirection.Size = new System.Drawing.Size(98, 13);
            this.LblMigDirection.TabIndex = 16;
            this.LblMigDirection.Text = "Migration Direction:";
            // 
            // WindowsFormsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 462);
            this.Controls.Add(this.PnlMigDirection);
            this.Controls.Add(this.LblConnectionTestFailed);
            this.Controls.Add(this.LblConnectionTestSucceeded);
            this.Controls.Add(this.BtnTestConnection);
            this.Controls.Add(this.BtnMigFilesFolderEllipsis);
            this.Controls.Add(this.TxbMigFilesFolder);
            this.Controls.Add(this.LblMigFilesFolder);
            this.Controls.Add(this.BtnRunMigrations);
            this.Controls.Add(this.SplMainUIArea);
            this.Controls.Add(this.LblConnectionString);
            this.Controls.Add(this.TxbConnectionString);
            this.Controls.Add(this.BtnTestCodeNow);
            this.Name = "WindowsFormsUI";
            this.Text = "BranchedMigrator Migration Command Designer";
            this.SplMainUIArea.Panel1.ResumeLayout(false);
            this.SplMainUIArea.Panel1.PerformLayout();
            this.SplMainUIArea.Panel2.ResumeLayout(false);
            this.SplMainUIArea.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplMainUIArea)).EndInit();
            this.SplMainUIArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGVAvailableMigrations)).EndInit();
            this.PnlMigDirection.ResumeLayout(false);
            this.PnlMigDirection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnTestCodeNow;
        private System.Windows.Forms.TextBox TxbConnectionString;
        private System.Windows.Forms.Label LblConnectionString;
        private System.Windows.Forms.SplitContainer SplMainUIArea;
        private System.Windows.Forms.Label LblAvailableMigrations;
        private System.Windows.Forms.DataGridView DGVAvailableMigrations;
        private System.Windows.Forms.Label LblEquivCommand;
        private System.Windows.Forms.TextBox TxbCommandLineEquivalent;
        private System.Windows.Forms.Button BtnRunMigrations;
        private System.Windows.Forms.Label LblMigFilesFolder;
        private System.Windows.Forms.TextBox TxbMigFilesFolder;
        private System.Windows.Forms.Button BtnMigFilesFolderEllipsis;
        private System.Windows.Forms.Button BtnTestConnection;
        private System.Windows.Forms.Label LblConnectionTestSucceeded;
        private System.Windows.Forms.Label LblConnectionTestFailed;
        private System.Windows.Forms.Panel PnlMigDirection;
        private System.Windows.Forms.RadioButton RbnMigDirectionDown;
        private System.Windows.Forms.RadioButton RbnMigDirectionUp;
        private System.Windows.Forms.Label LblMigDirection;
    }
}

