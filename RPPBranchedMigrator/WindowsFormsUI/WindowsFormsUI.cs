using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MigrationClasses;

namespace WindowsFormsUI
{
    public partial class WindowsFormsUI : Form
    {
        public WindowsFormsUI()
        {
            InitializeComponent();            
            //MigrationManager.ConnectionString = @"Server=RACHEL-ROSES\RPP_ROSES_SQL2k8;Database=MigTest002;User Id=sa;Password=Password1;";
        }

        private void BtnTestCodeNow_Click(object sender, EventArgs e)
        {
            //MigrationManager.MigrateToDateTime = DateTime.Now;
            //MigrationManager.MigrationsFolderPath = @"C:\Projects\RPPBranchedMigrator\MigrationSourceFiles";
            //MigrationManager.Migrate();
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            if (MigrationManager.ConnectionSucceeded)
            {
                DGVAvailableMigrations.DataSource = MigrationManager.MigrationFilesWithDetails;
                DGVAvailableMigrations.Enabled = true;
                LblConnectionTestSucceeded.Visible = true;
                LblConnectionTestFailed.Visible = false;
            }
            else
            {
                DGVAvailableMigrations.DataSource = null;
                DGVAvailableMigrations.Enabled = false;
                LblConnectionTestSucceeded.Visible = false;
                LblConnectionTestFailed.Visible = true;
            }
            SetMigrationListIfPossible();
        }

        void SetMigrationListIfPossible()
        { 
        
        }

        private void TxbConnectionString_TextChanged(object sender, EventArgs e)
        {
            MigrationManager.ConnectionString = TxbConnectionString.Text;
        }
    }
}
