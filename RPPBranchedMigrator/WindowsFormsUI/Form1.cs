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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MigrationManager.ConnectionString = @"Server=RACHEL-ROSES\RPP_ROSES_SQL2k8;Database=MigTest;User Id=sa;Password=Password1;";
        }

        private void BtnTestCodeNow_Click(object sender, EventArgs e)
        {
            MigrationManager.MigrateToDateTime = DateTime.Now;
            MigrationManager.MigrationsFolderPath = @"C:\Projects\RPPBranchedMigrator\MigrationSourceFiles";
            MigrationManager.Migrate();
        }
    }
}
