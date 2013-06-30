
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
using System.Dynamic;
using System.Globalization;

#endregion

namespace MigrationClasses
{
    public enum MigrationDirectionEnum { Up, Down }

    public static class MigrationManager
    {
        #region Properties

        public static bool ConnectionSucceeded { get { return rdbmsVendor.ConnectionSucceeded; } }

        public static DateTime? MostRecentMigrationApplied { get { return rdbmsVendor.GetMostRecentMigration(); } }

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

        public static IEnumerable<dynamic> MigrationFilesWithDetails
        {
            get
            {
                ////Created an Anonymous Function for determining the implied DateTime of a migration file's creation from its filename. 
                ////
                ////NB: 1) Can't do this inline because the LINQ expression below utilises an Anonymous Type, and you can't set an
                ////       an Anonymous Type from an Anonymous Function - either the assignee or the expression needs to know the 
                ////       return type in order for the other to be able to imply the type it should take.
                ////
                ////    2) Could put lots of other useful info in here if desired, such as the contents of the file retrieved via
                ////       a FileStream, or a list of individual SQL instructions. However, as the aim is for this Property to be 
                ////       used to populate UI elements smoothly, I've left retrieving that level of detail to consuming classes.

                //Func<string, DateTime?> tryGetDateFromFilename =
                //    value =>
                //    {
                //        DateTime createdWhen;
                //        return DateTime.TryParseExact(
                //            value, "yyyyMMdd_hhmmss", null, DateTimeStyles.None, out createdWhen) ? (DateTime?)createdWhen : null;
                //    };

                return
                    from m in MigrationsFilenames()
                    select new
                        {
                            CreatedWhen = CommonAnonymousFunctions.TryGetDateFromFilename(m),
                            Filename = m
                        };

                //return (result is IEnumerable<dynamic>) ? result : null;
            }
        }

        static IEnumerable<string> MigrationsFilenames()
        {
            foreach (string filePath in Directory.GetFiles(fullyQualifiedMigrationFilesPath)) { yield return filePath; }
        }

        public static void Migrate()
        {
            try
            {
                rdbmsVendor.BeginTransaction();
                foreach (string sqlFilePath in MigrationsFilenames())
                {
                    rdbmsVendor.RunCommandsInSQLFile(new StringBuilder(sqlFilePath), MigrationDirection);
                }
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
