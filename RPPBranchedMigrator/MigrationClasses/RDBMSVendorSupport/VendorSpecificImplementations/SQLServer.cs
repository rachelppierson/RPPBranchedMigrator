
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

        public string ConnectionString {
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
            if (transaction is SqlTransaction) transaction.Rollback();
        }

        public void CloseConnection()
        {
            if (connection is SqlConnection) connection.Close();
        }

        public void RunCommandsInSQLFile(string sqlFilePath)
        {
            using (SqlCommand cmd = connection.CreateCommand())
            {
                //cmd.Connection = connection;
                cmd.Transaction = transaction;

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
                            catch (SqlException e)
                            {
                                //Let the consumer know exactly where the error occurred, and pass on any 
                                //specific SqlException details via InnerException.
                                throw new Exception(
                                    string.Format(
                                        "A SQL Exception occurred whilst trying to run the command '{0}' in the file '{1}'.",
                                    cmd.CommandText, Path.GetFileName(sqlFilePath)), e.InnerException);
                            }
                        }
                    }
                }
            }
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
    }
}
