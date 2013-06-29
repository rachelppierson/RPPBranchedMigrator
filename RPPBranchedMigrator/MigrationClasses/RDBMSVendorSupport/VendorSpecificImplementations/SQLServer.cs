
#region Using Directives

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

#endregion

namespace MigrationClasses.RDBMSVendorSupport.VendorSpecificImplementations
{
    public class SQLServer : ISupportedRDBMSVendor
    {
        #region Fields

        SqlConnection connection = null;

        SqlTransaction transaction = null;

        #endregion

        #region Methods



        #endregion

        #region ISupportedRDBMSVendor implementation

        public string ConnectionString { get; set; }

        public void BeginTransaction()
        {
            if (!(connection is SqlConnection)) throw new SqlException(@"");
            if (transaction is SqlTransaction) throw new SqlException(@"");
            if (connection.State == ConnectionState.Closed) connection.Open();
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (!(connection is SqlConnection) || !(transaction is SqlTransaction)) return false;
            transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (!(connection is SqlConnection) || !(transaction is SqlTransaction)) return false;
            transaction.Rollback();
        }

        public Exception LastErrorDetails { get; set; }

        public void RunCommandsInSQLFile(string sqlFilePath)
        {

        }

        #endregion
    }
}
