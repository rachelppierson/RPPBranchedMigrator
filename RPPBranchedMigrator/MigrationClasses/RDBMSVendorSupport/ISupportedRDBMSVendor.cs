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
        void RunCommandsInSQLFile(string sqlFilePath);
    }
}
