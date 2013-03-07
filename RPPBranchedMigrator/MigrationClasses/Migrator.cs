﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;

namespace MigrationClasses
{
    public enum MigrationDirectionEnum { Up, Down }

    public static class MigrationManager
    {
        struct ConnOutcomeSQLServer
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

        public static string ConnectionString { get; set; }

        public static String MigrationsFolderPath { get; set; }

        public static DateTime? MigrateToDateTime { get; set; }

        public static MigrationDirectionEnum MigrationDirection
        {
            get { return MigrationDirectionEnum.Up; } //TODO: Make dynamic
        }

        static IEnumerable<string> Migrations()
        {
            foreach (string filePath in Directory.GetFiles(
                string.Format("{0}\\{1}", MigrationsFolderPath, MigrationDirection.ToString()))) { yield return filePath; }
        }

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

        public static StringBuilder CleanSQL(StreamReader reader)
        {
            string commandText = reader.ReadToEnd();

            RegexOptions regExOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;

            string rawText = commandText;

            string regExText = "('(''|[^'])*')|[\\t\\r\\n]|(--[^\\r\\n]*)|(/\\*[\\w\\W]*?(?=\\*/)\\*/)";
            //Replaces Carriage Returns, Tabs, Line Feeds, Single-Row and Multi-row Comments 
            //with a space when not included inside a text block.
            //string regExText = "(--[^\\r\\n]*)|(/\\*[\\w\\W]*?(?=\\*/)\\*/)";

            MatchCollection patternMatchList = Regex.Matches(rawText, regExText, regExOptions);
            for (int patternIndex = patternMatchList.Count - 1; patternIndex >= 0; patternIndex += -1)
            {
                var match = patternMatchList[patternIndex];
                if (match.Value.StartsWith("-") || (!match.Value.StartsWith("'") && !match.Value.EndsWith("'")))
                {
                    rawText = rawText.Substring(0, match.Index) + " " + rawText.Substring(match.Index + match.Length);
                }
            }
            //Remove extra spacing that is not contained inside text qualifers.
            patternMatchList = Regex.Matches(rawText, "'([^']|'')*'|[ ]{2,}", regExOptions);
            for (int patternIndex = patternMatchList.Count - 1; patternIndex >= 0; patternIndex += -1)
            {
                var match2 = patternMatchList[patternIndex];
                //if (!match2.Value.StartsWith("'") && !match2.Value.EndsWith("'"))
                if (match2.Value.StartsWith("-") || (!match2.Value.StartsWith("'") && !match2.Value.EndsWith("'")))
                {
                    rawText = rawText.Substring(0, match2.Index) + rawText.Substring(match2.Index + match2.Length - 1);
                }
            }
            //Return value without leading and trailing spaces.
            return new StringBuilder(rawText.Replace(" GO ", "\r\nGO\r\n").Replace(" GO", "\r\nGO\r\n").Trim());
        }

        public static void Migrate()
        {
            ConnOutcomeSQLServer connState = getConnSQLServer();

            if (connState.Connection.State == ConnectionState.Closed) connState.Connection.Open();

            StringBuilder currentSqlCommand;

            SqlTransaction _Tran = connState.Connection.BeginTransaction();

            foreach (string sqlFilePath in Migrations())
            {
                using (SqlCommand _Cmd = connState.Connection.CreateCommand())
                {
                    _Cmd.Connection = connState.Connection;
                    _Cmd.Transaction = _Tran;

                    using (FileStream _FileStrm = File.OpenRead(sqlFilePath))
                    {
                        StreamReader reader = new StreamReader(_FileStrm);

                        StringBuilder cleanSql = CleanSQL(reader);

                        while (!reader.EndOfStream)
                        {
                            currentSqlCommand = new StringBuilder(reader.ReadLine());

                            if (currentSqlCommand.Length > 0)
                            {
                                _Cmd.CommandText = currentSqlCommand.ToString();
                                _Cmd.CommandType = CommandType.Text;

                                try
                                {
                                    _Cmd.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                    _Tran.Rollback();
                                    throw ex;
                                }
                            }

                        }
                    }
                }

                _Tran.Commit();
            }
        }
    }

    #region retiredCode





    //private struct ConnOutcomeSQLServer
    //{
    //    public bool Suceeded;
    //    public SqlConnection Connection;
    //    public Exception Exception;

    //    public ConnOutcomeSQLServer(bool suceeded, SqlConnection connection = null, Exception exception = null)
    //    {
    //        Suceeded = suceeded;
    //        Connection = connection;
    //        Exception = exception;
    //    }
    //}

    //public static class Migrator
    //{
    //    private SqlConnection conn;

    //    private ConnOutcomeSQLServer GetConnSQLServer(string connStr, bool open = true)
    //    {
    //        ConnOutcomeSQLServer connOutcome = new ConnOutcomeSQLServer(false);

    //        try
    //        {
    //            SqlConnection conn = new SqlConnection(connStr);
    //            if (open) conn.Open();
    //            connOutcome.Suceeded = true;
    //            connOutcome.Connection = conn;
    //        }
    //        catch (Exception ex)
    //        {
    //            connOutcome.Exception = ex;
    //        }

    //        return connOutcome;
    //    }

    //    //public static void SetCredentials()
    //    //{ 
    //    //
    //    //}

    //    public static void ExecuteSqlScript_SQLServer(
    //    SqlConnection conn, string sql, bool sqlIsFilePath, bool closeConnectionAfterCompletion = false)
    //    {
    //        string _SQLStr = "";

    //        if (sqlIsFilePath)
    //        {
    //            using (FileStream _FileStrm = File.OpenRead(sql))
    //            {
    //                StreamReader reader = new StreamReader(_FileStrm);
    //                _SQLStr = reader.ReadToEnd();
    //            }
    //        }
    //        else
    //        {
    //            _SQLStr = sql;
    //        }

    //        Regex _Regx = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
    //        string[] _LinesAry = _Regx.Split(_SQLStr);

    //        if (conn.State == ConnectionState.Closed)
    //            conn.Open();

    //        SqlTransaction _Tran = conn.BeginTransaction();
    //        using (SqlCommand _Cmd = conn.CreateCommand())
    //        {
    //            _Cmd.Connection = conn;
    //            _Cmd.Transaction = _Tran;

    //            foreach (string _CurrentLineStr in _LinesAry)
    //            {
    //                if (_CurrentLineStr.Length > 0)
    //                {
    //                    _Cmd.CommandText = _CurrentLineStr;
    //                    _Cmd.CommandType = CommandType.Text;

    //                    try
    //                    {
    //                        _Cmd.ExecuteNonQuery();
    //                    }
    //                    catch (SqlException generatedExceptionName)
    //                    {
    //                        _Tran.Rollback();
    //                        throw;
    //                    }
    //                }
    //            }
    //        }

    //        _Tran.Commit();

    //        if (closeConnectionAfterCompletion)
    //            conn.Close();
    //    }    
    //}

    #endregion

}
