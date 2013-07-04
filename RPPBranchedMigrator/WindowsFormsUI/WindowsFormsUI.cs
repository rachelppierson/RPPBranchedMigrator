using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MigrationClasses;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsUI
{
    public partial class WindowsFormsUI : Form
    {
        public WindowsFormsUI()
        {
            InitializeComponent();
            DGVAvailableMigrations.RowEnter += DGVAvailableMigrations_RowEnter; 
        }

        void DGVAvailableMigrations_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            MigrationManager.MigrateToDateTime = Convert.ToDateTime(DGVAvailableMigrations.Rows[e.RowIndex].Cells[0].Value);
            BtnRunMigrations.Enabled = true;

            TxbCommandLineEquivalent.Text =                
                string.Format("{3} ConnectionString|\"{0}\" MigrationFilesFolder|\"{1}\" MigrateToDate|\"{2}\"",
                TxbConnectionString.Text, //NB: If you use MigrationManager.ConnectionString it strips out the password 
                MigrationManager.MigrationsFolderPath,
                MigrationManager.MigrateToDateTime.Value.ToString("dd MMM yyyy HH:mm:ss"),
                BranchedMigratorConsoleAppPath());
        }

        string BranchedMigratorConsoleAppPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BranchedMigrator.exe")
                .ReplaceLastSubstring("WindowsFormsUI", "ConsoleAppUI");
        }

        private void BtnTestCodeNow_Click(object sender, EventArgs e)
        {
            TxbConnectionString.Text = @"Server=RACHEL-ROSES\RPP_ROSES_SQL2k8;Database=MigTest002;User Id=sa;Password=Password1;";
            TxbMigFilesFolder.Text = @"C:\Projects\RPPBranchedMigrator\MigrationSourceFiles";
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            if (MigrationManager.ConnectionSucceeded)
            {
                LblConnectionTestSucceeded.Visible = true;
                LblConnectionTestFailed.Visible = false;
            }
            else
            {
                LblConnectionTestSucceeded.Visible = false;
                LblConnectionTestFailed.Visible = true;
            }
            SetMigrationListIfPossible();
        }

        void SetMigrationListIfPossible()
        {
            if (MigrationsFolderIsValid && MigrationManager.ConnectionSucceeded)
            {
                DGVAvailableMigrations.DataSource = MigrationManager.AvailableMigrationsWithDetails;
                DGVAvailableMigrations.Enabled = true;
            }
            else
            {
                DGVAvailableMigrations.DataSource = null;
                DGVAvailableMigrations.Enabled = false;
            }
            BtnRunMigrations.Enabled = false;
            TxbCommandLineEquivalent.Text = string.Empty;
        }

        private void TxbConnectionString_TextChanged(object sender, EventArgs e)
        {
            MigrationManager.ConnectionString = TxbConnectionString.Text;
            SetMigrationListIfPossible();
        }

        private void BtnMigFilesFolderEllipsis_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog migFolderSelector = new FolderBrowserDialog();
            DialogResult result = migFolderSelector.ShowDialog();
            if (result == DialogResult.OK) { TxbMigFilesFolder.Text = migFolderSelector.SelectedPath; }
        }

        public bool MigrationsFolderIsValid { 
            get { 
                try
                {
                    bool hasUpFolder = false, hasDownFolder = false;
                    foreach (string dir in Directory.GetDirectories(TxbMigFilesFolder.Text))
                    {
                        if (Path.GetFileName(dir).ToUpper() == "UP") hasUpFolder = true;
                        if (Path.GetFileName(dir).ToUpper() == "DOWN") hasDownFolder = true;
                        if (hasUpFolder && hasDownFolder) return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            } 
        } 

        private void TxbMigFilesFolder_TextChanged(object sender, EventArgs e)
        {
            if (MigrationsFolderIsValid) MigrationManager.MigrationsFolderPath = TxbMigFilesFolder.Text;
            SetMigrationListIfPossible();
        }

        private void RbnMigDirectionUp_CheckedChanged(object sender, EventArgs e)
        {
            MigrationManager.MigrationDirection = MigrationDirectionEnum.Up;
            SetMigrationListIfPossible();
        }

        private void RbnMigDirectionDown_CheckedChanged(object sender, EventArgs e)
        {
            MigrationManager.MigrationDirection = MigrationDirectionEnum.Down;
            SetMigrationListIfPossible();
        }

        private void BtnRunMigrations_Click(object sender, EventArgs e)
        {
            MigrationManager.Migrate();
            SetMigrationListIfPossible();
        }
    }

    public static class Extensions
    {
        public static string ReplaceLastSubstring (this string sourceString, string searchString, string replaceString)
        {
            StringBuilder vRet = new StringBuilder(sourceString);
            vRet.Replace(searchString, replaceString, vRet.ToString().LastIndexOf(searchString), searchString.Length);
            return vRet.ToString();
        }    
    }
}
