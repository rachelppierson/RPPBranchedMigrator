using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationClasses.RDBMSVendorSupport
{
    /// <summary>
    /// Minimum functionality that must be supported by any RDBMS targeted by BranchedMigrator
    /// </summary>
    public interface ISupportedRDBMSVendor
    {
        /// <summary>
        /// Contains the connection string appropriate to the RDBMS targeted. Must be set for most methods
        /// in the Class to be able to be run successfully.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>Attempts to begin a Transaction within the context of the RDBMS targeted and ConnectionString set</summary>
        void BeginTransaction();

        /// <summary>Attempts to commit a Transaction within the context of the RDBMS targeted and ConnectionString set</summary>
        void CommitTransaction();

        /// <summary>Attempts to commit a Transaction within the context of the RDBMS targeted and ConnectionString set</summary>
        void RollbackTransaction();

        /// <summary>Attempts to close any open Connection within the context of the RDBMS targeted and ConnectionString set</summary>
        void CloseConnection();

        /// <summary>Attempts to run SQL commands contained in a .SQL file</summary>
        /// <param name="sqlFilePath">Fully-qualified path to a file containing valid SQL</param>
        void RunCommandsInSQLFile(StringBuilder sqlFilePath, MigrationDirectionEnum direction = MigrationDirectionEnum.Up);

        /// <summary>
        /// Attempts to create a table called 'VersionInfo' in a Schema called 'migrator', or the nearest
        /// equivalent thereof for the RDBMS in question. This table will contain information about which 
        /// discrete SQL files have been applied to the database, and when they were applied.
        /// </summary>
        void CreateMigrationsDbObjectsIfAppropriate();

        /// <summary>
        /// Returns a DateTime indicating the most recent SQL script that was applied. 
        /// When migrating UP, this will determine the cut-off point for deciding which UP scripts have already
        /// been applied. When migrating DOWN, this will determine which DOWN script to start from.
        /// </summary>
        DateTime? GetMostRecentMigration();

        /// <summary>
        /// Creates a record of a SQL files being migrated (Up or Down), along with the time the migration occurred.
        /// </summary>
        /// <param name="filename">Filename of the SQL File that was successfully migrated</param>
        /// <param name="direction">Direction in which migration was carried out (i.e., UP or DOWN)</param>
        void LogMigration(string filename, MigrationDirectionEnum direction);

        /// <summary>Readonly Boolean Property indicating whether the current ConnectionString is valid</summary>
        bool ConnectionSucceeded { get; }
    }
}
