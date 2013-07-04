using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MigrationClasses;

namespace BranchedMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Debugger.Break();
            Dictionary<string, string> argsDict = new Dictionary<string, string>();

            foreach (string str in args)
            {
                string[] split = str.Split('=');
                argsDict.Add(split[0].ToString().ToLower(), split[1].ToString());
            }

            string connectionString;
            if (argsDict.TryGetValue("connectionstring", out connectionString))
                MigrationManager.ConnectionString = connectionString;
            else
                throw new Exception("No ConnectionString passed");
        }
    }

    public class Migrate
    {
        public Migrate(string ConnectionString, DateTime MigrateToDate)
        {

        }
    }
}
