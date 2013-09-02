//test check in 1
//test check in 2
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
        #region Fields

        public static DateTime? MigrateToDateTime = DateTime.Now;

        #endregion

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

        public static MigrationDirectionEnum MigrationDirection { get; set; }

        #endregion

        #region Migration Methods

        /// <summary>
        /// Returns a List of dynamic objects that contain details of which migrations are available
        /// </summary>
        public static List<dynamic> AvailableMigrationsWithDetails
        {
            get
            {
                DateTime? mostRecentMigration = rdbmsVendor.GetMostRecentMigration();

                var Migrations =
                    from m in MigrationsFilenames()
                    where
                        (
                            MigrationDirection == MigrationDirectionEnum.Up && 
                            (
                                mostRecentMigration == null ||
                                CommonAnonymousFunctions.TryGetDateFromFilename(Path.GetFileName(m)) > mostRecentMigration
                            )
                        ) 
                        ||
                            (MigrationDirection == MigrationDirectionEnum.Down &&
                                CommonAnonymousFunctions.TryGetDateFromFilename(Path.GetFileName(m)) <= mostRecentMigration)
                    select new
                        {
                            CreatedWhen = CommonAnonymousFunctions.TryGetDateFromFilename(Path.GetFileName(m)),
                            Filename = Path.GetFileName(m)
                        };

                return Migrations.ToList<dynamic>();
            }
        }

        static IEnumerable<string> MigrationsFilenames()
        {
            foreach (string filePath in 
                MigrationDirection == MigrationDirectionEnum.Up 
                    ? Directory.GetFiles(fullyQualifiedMigrationFilesPath)
                    : Directory.GetFiles(fullyQualifiedMigrationFilesPath).Reverse()
                    ) { yield return filePath; }
        }

        public static void Migrate()
        {
            try
            {
                rdbmsVendor.BeginTransaction();
                DateTime? mostRecentMigration = rdbmsVendor.GetMostRecentMigration();

                foreach (string sqlFilePath in MigrationsFilenames())
                {
                    DateTime? fileDateTime = CommonAnonymousFunctions.TryGetDateFromFilename(Path.GetFileName(sqlFilePath));

                    if (
                        // Direction is UP and is a valid UP file for the target criteria chosen
                        (MigrationDirection == MigrationDirectionEnum.Up && 
                            (mostRecentMigration == null || fileDateTime > mostRecentMigration) &&
                            (MigrateToDateTime == null || fileDateTime <= MigrateToDateTime)
                        ) 
                    
                        ||

                        // Direction is DOWN and is a valid DOWN file for the target criteria chosen
                        (MigrationDirection == MigrationDirectionEnum.Down && 
                            fileDateTime <= mostRecentMigration &&
                            fileDateTime >= MigrateToDateTime 
                        )
                    )
                        
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

        /// <summary>
        /// Attempts to set the Migration Direction based on the DateTime of the desired migration
        /// compared with the DateTime of the most recently applied migration
        /// </summary>
        /// <returns>A boolean value indicating whether the migration direction could be set</returns>
        public static bool AutoSetMigrationDirection()
        { 
            DateTime? mostRecentMigration = rdbmsVendor.GetMostRecentMigration();
            if (MigrateToDateTime.HasValue && rdbmsVendor.GetMostRecentMigration().HasValue)
            {
                MigrationDirection =
                    (MigrateToDateTime.Value > mostRecentMigration.Value)
                    ? MigrationDirectionEnum.Up
                    : MigrationDirectionEnum.Down;
                return true;
            }
            else if (MigrateToDateTime.HasValue)
            {
                MigrationDirection = MigrationDirectionEnum.Up;
                return true;
            }
            return false;
        }

        #endregion
    }
}
