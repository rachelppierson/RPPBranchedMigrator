
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

#endregion

namespace MigrationClasses
{
    public enum MigrationDirectionEnum { Up, Down }

    public static class MigrationManager
    {
        #region Constructors 

        static MigrationManager()
        {
            //TODO - get a real path
            DbOjectsCreateScriptPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        #endregion 

        #region Enums and Structs

        class ConnOutcomeSQLServer
        {
            public bool Suceeded;
            public SqlConnection Connection;
            public Exception Exception;

            public ConnOutcomeSQLServer(bool suceeded, SqlConnection connection = null, Exception exception = null)
            {
                Suceeded = suceeded;
                Connection = connection;
                Exception = exception;
            }
        }

        #endregion

        #region Properties

        public static string ConnectionString { get; set; }

        public static String MigrationsFolderPath { get; set; }

        public static DateTime? MigrateToDateTime { get; set; }

        public static MigrationDirectionEnum MigrationDirection
        {
            get { return MigrationDirectionEnum.Up; } //TODO: Make dynamic
        }

        #endregion

        #region SQL Server -specific functionality

        static ConnOutcomeSQLServer getConnSQLServer(bool open = true)
        {
            ConnOutcomeSQLServer connOutcome = new ConnOutcomeSQLServer(false);

            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                if (open) conn.Open();
                connOutcome.Suceeded = true;
                connOutcome.Connection = conn;
            }
            catch (Exception ex)
            {
                connOutcome.Exception = ex;
            }

            return connOutcome;
        }

        #endregion

        #region SQL script management

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

        #region BranchedMigrator -specific database objects

        static string DbOjectsCreateScriptPath = @"";

        static void CheckCreateBranchedMigratordbObjects(
        ConnOutcomeSQLServer connState = null, SqlTransaction tran = null, bool concludeTranWithinThisMethod = false)
        {
            if (connState.Equals(null))
            {
                connState = getConnSQLServer();
                if (connState.Connection.State == ConnectionState.Closed) connState.Connection.Open();
                tran = connState.Connection.BeginTransaction();
                concludeTranWithinThisMethod = true;
            }

            using (SqlCommand cmd = connState.Connection.CreateCommand())
            {
                cmd.Connection = connState.Connection;
                cmd.Transaction = tran;

                using (FileStream _FileStrm = File.OpenRead(DbOjectsCreateScriptPath))
                {
                    StreamReader reader = new StreamReader(_FileStrm);

                    foreach (string currentSqlCommand in CleanSQL(reader))
                    {
                        if (currentSqlCommand.Length > 0)
                        {
                            cmd.CommandText = currentSqlCommand;

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                tran.Rollback();
                                throw ex;
                            }
                        }
                    }
                }

                if (concludeTranWithinThisMethod == true) tran.Commit();
            }
        }

        #endregion

        #region Migration Methods

        static IEnumerable<string> Migrations()
        {
            foreach (string filePath in Directory.GetFiles(
                string.Format("{0}\\{1}", MigrationsFolderPath, MigrationDirection.ToString()))) { yield return filePath; }
        }

        public static void Migrate()
        {
            ConnOutcomeSQLServer connState = getConnSQLServer();
            if (connState.Connection.State == ConnectionState.Closed) connState.Connection.Open();

            SqlTransaction tran = connState.Connection.BeginTransaction();

            CheckCreateBranchedMigratordbObjects(connState, tran);

            foreach (string sqlFilePath in Migrations())
            {
                using (SqlCommand cmd = connState.Connection.CreateCommand())
                {
                    cmd.Connection = connState.Connection;
                    cmd.Transaction = tran;

                    using (FileStream _FileStrm = File.OpenRead(sqlFilePath))
                    {
                        StreamReader reader = new StreamReader(_FileStrm);

                        foreach (string currentSqlCommand in CleanSQL(reader))
                        {
                            if (currentSqlCommand.Length > 0)
                            {
                                cmd.CommandText = currentSqlCommand;

                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    tran.Rollback();
                                    throw ex;
                                }
                            }
                        }
                    }
                }
            }
            tran.Commit();
        }

        #endregion
    }
}
