
#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;
using System.Reflection;
using MigrationClasses.RDBMSVendorSupport;
using MigrationClasses.RDBMSVendorSupport.VendorSpecificImplementations;

#endregion

namespace MigrationClasses
{
    public enum MigrationDirectionEnum { Up, Down }

    public static class MigrationManager
    {
        #region Properties

        static ISupportedRDBMSVendor rdbmsVendor = new SQLServer(); //Default vendor is SQL Server

        public static string ConnectionString
        {
            get { return rdbmsVendor.ConnectionString; }
            set { rdbmsVendor.ConnectionString = value; }
        }

        public static String MigrationsFolderPath { get; set; }

        /// <summary>
        /// Returns a fully-qualified folder path to the folder that contains SQL files
        /// appropriate to the currently-indicated migration direction. i.e., the "Up" or "Down"
        /// SQL migration files, as appropriate.
        /// </summary>
        static string fullyQualifiedMigrationFilesPath
        {
            get { return Path.Combine(MigrationsFolderPath, MigrationDirection.ToString().ToUpper()); }
        }

        public static DateTime? MigrateToDateTime { get; set; }

        public static MigrationDirectionEnum MigrationDirection
        {
            get { return MigrationDirectionEnum.Up; } //TODO: Make this able to be detected from the context
        }

        #endregion

        #region Migration Methods

        static IEnumerable<string> Migrations()
        {
            foreach (string filePath in Directory.GetFiles(fullyQualifiedMigrationFilesPath)) { yield return filePath; }
        }

        public static void Migrate()
        {
            try
            {
                rdbmsVendor.BeginTransaction();
                foreach (string sqlFilePath in Migrations()) { rdbmsVendor.RunCommandsInSQLFile(new StringBuilder(sqlFilePath)); }
                rdbmsVendor.CommitTransaction();
            }
            catch (Exception e)
            {
                rdbmsVendor.RollbackTransaction();
                rdbmsVendor.CloseConnection();
                throw e;        
            }
        }

        #endregion
    }
}
