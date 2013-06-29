
#region Using Directives

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;
using System.Text.RegularExpressions;

#endregion

namespace MigrationClasses.RDBMSVendorSupport.VendorSpecificImplementations
{
    public class SQLServer : ISupportedRDBMSVendor
    {
        #region Fields

        SqlConnection connection = null;

        SqlTransaction transaction = null;

        #endregion

        #region ISupportedRDBMSVendor implementation

        public string ConnectionString
        {
            get { return connection.ConnectionString; }
            set { connection = new SqlConnection(value); }
        }

        public void BeginTransaction()
        {
            if (transaction is SqlTransaction) throw new Exception(@"There is already a Transaction open for this connection");
            if (connection.State == ConnectionState.Closed) connection.Open();
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (transaction is SqlTransaction) transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (transaction is SqlTransaction)
            {
                transaction.Rollback();
                transaction = null;
            }
        }

        public void CloseConnection()
        {
            if (connection is SqlConnection) connection.Close();
        }

        public void RunCommandsInSQLFile(StringBuilder sqlFilePath, MigrationDirectionEnum direction = MigrationDirectionEnum.Up)
        {
            CreateMigrationsDbObjectsIfAppropriate();

            using (FileStream _FileStrm = File.OpenRead(sqlFilePath.ToString()))
            {
                StreamReader reader = new StreamReader(_FileStrm);

                foreach (string currentSqlCommand in CleanSQL(reader))
                {
                    if (currentSqlCommand.Length > 0) runSQLcommand(new StringBuilder(currentSqlCommand), sqlFilePath);
                }
            }

            LogMigration(Path.GetFileName(sqlFilePath.ToString()), direction);
        }

        public void CreateMigrationsDbObjectsIfAppropriate()
        {
            //HACK: Can put this SQL in Resource files later, if desired. If they get too unmanageable, I will.

            StringBuilder schemaSql =
                new StringBuilder(
@"IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'migrator') 
BEGIN
	EXEC('CREATE SCHEMA [migrator] AUTHORIZATION [dbo]');
END");

            StringBuilder migTableSql =
                new StringBuilder(
@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[migrator].[VersionInfo]') AND type in (N'U'))
CREATE TABLE [migrator].[VersionInfo](
	[MigrationFilename] [nvarchar](100) NOT NULL,
	[DateTimeApplied] [datetime] NOT NULL,
	[Direction] [char](1) NOT NULL,
    CONSTRAINT [PK_VersionInfo] PRIMARY KEY CLUSTERED ([MigrationFilename] ASC) 
    WITH (
        PAD_INDEX  = OFF, 
        STATISTICS_NORECOMPUTE  = OFF, 
        IGNORE_DUP_KEY = OFF, 
        ALLOW_ROW_LOCKS  = ON, 
        ALLOW_PAGE_LOCKS  = ON) 
    ON [PRIMARY]) 
ON [PRIMARY];");

            runSQLcommand(schemaSql);
            runSQLcommand(migTableSql);
        }

        public DateTime? GetMostRecentMigration()
        {
            return null;
        }

        public void LogMigration(string filename, MigrationDirectionEnum direction)
        {
            StringBuilder recordMigrationSQL =
                new StringBuilder(string.Format(
@"IF NOT EXISTS (SELECT * FROM migrator.VersionInfo WHERE MigrationFilename = N'{0}') 
BEGIN
    INSERT INTO migrator.VersionInfo (MigrationFilename, DateTimeApplied, Direction)
    VALUES (N'{0}', CAST(N'{1}' AS DATETIME), N'{2}');
END
ELSE
BEGIN
    UPDATE migrator.VersionInfo
    SET MigrationFilename = N'{0}', DateTimeApplied = CAST(N'{1}' AS DATETIME), Direction = N'{2}'
    WHERE MigrationFilename = N'{0}'
END
",
                    filename.ToString(),
                    DateTime.Now.ToString("dd MMM yyyy hh:mm:ss"),
                    direction.ToString().ToUpper()[0]));

            runSQLcommand(recordMigrationSQL);
        }

        #endregion

        #region SQL script management

        //HACK: I'm not completely happy with using Regex for this task, but it's the best option available for now

        static string textBlocksRegex = @"'(''|[^'])*'";
        static string tabsAndLineBreaksRegex = @"[\t\r\n]";
        static string singleLineCommentsRegex = @"--[^\r\n]*";
        static string multiLineCommentsRegex = @"/\*[\w\W]*?(?=\*/)\*/";

        /// <summary>
        /// Converts SQL that contains comments and Enterprise Manager -specific delimeters like "GO", 
        /// and converts it into a sequence of independently-runable SQL commands that achieve the same
        /// effect as the original script.
        /// </summary>
        /// <param name="reader">A StreamReader that points to a SQL file</param>
        /// <returns>A string array of runable SQL commands</returns>
        public static string[] CleanSQL(StreamReader reader)
        {
            StringBuilder sql = new StringBuilder(reader.ReadToEnd());
            RegexOptions regExOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;

            string regExText = string.Format(@"({0})|{1}|({2})|({3})",
                textBlocksRegex, tabsAndLineBreaksRegex, singleLineCommentsRegex, multiLineCommentsRegex);

            MatchCollection patternMatchList = Regex.Matches(sql.ToString(), regExText, regExOptions);
            for (int patternIndex = patternMatchList.Count - 1; patternIndex >= 0; patternIndex += -1)
            {
                var match = patternMatchList[patternIndex];
                if (match.Value.StartsWith("-") || (!match.Value.StartsWith("'") && !match.Value.EndsWith("'")))
                    sql.Replace(match.Value, " ", match.Index, match.Length);
            }

            //Remove extra spacing that is not contained inside text qualifers.
            patternMatchList = Regex.Matches(sql.ToString(), "'([^']|'')*'|[ ]{2,}", regExOptions);
            for (int patternIndex = patternMatchList.Count - 1; patternIndex >= 0; patternIndex += -1)
            {
                var match2 = patternMatchList[patternIndex];
                if (match2.Value.StartsWith("-") || (!match2.Value.StartsWith("'") && !match2.Value.EndsWith("'")))
                    sql.Replace(match2.Value, " ", match2.Index, match2.Length);
            }

            sql.Replace(" GO ", "\r\nGO\r\n").Replace(" GO", "\r\nGO\r\n");

            Regex _Regx = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return _Regx.Split(sql.ToString().Trim());
        }

        #endregion

        #region SQL Command Execution

        /// <summary>
        /// Runs a SQL command within the Transaction referenced by the Private 'transaction' field. Throws
        /// an appropriate Exception if any necessary prerequisite isn't met (e.g., if no ConnectionString has
        /// been set or a Transaction hasn't been begun).
        /// </summary>
        /// <param name="sql">StringBuilder containing the SQL to be executed</param>
        /// <param name="sqlFilePath">Path to the SQL file that contains the SQL, if appropriate.</param>
        void runSQLcommand(StringBuilder sql, StringBuilder sqlFilePath = null)
        {
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;

                cmd.CommandText = sql.ToString();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    //Let the consumer know exactly where the error occurred, and pass on any 
                    //specific SqlException details via InnerException.
                    throw new Exception(
                        string.Format(
                            "A SQL Exception occurred whilst trying to run the command '{0}'",
                        cmd.CommandText,
                        sqlFilePath == null ? "." : string.Format(" in the file '{0}'.", Path.GetFileName(sqlFilePath.ToString()))),
                        e.InnerException);
                }
            }
        }

        #endregion
    }
}
