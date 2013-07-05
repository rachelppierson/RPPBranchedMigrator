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
        enum ArgsType { Positional, Named, Missing }
        enum Args { connectionstring, migrationfilesfolder, migratetodate } //Lowercase to allow args[] to take any case

        static void Main(string[] args)
        {
            //Debugger.Break();

            ArgsType argsType = args.Length > 0 && args[0].Contains('$') ? ArgsType.Named : ArgsType.Missing;

            Dictionary<Args, string> argsDict = new Dictionary<Args, string>();

            foreach (var arg in args.Select((x, i) => new { Value = x, Index = i }))
            {
                switch (argsType)
                {
                    case ArgsType.Missing:
                        throw new Exception("Invalid arguments supplied");

                    case ArgsType.Named:
                        string[] split = arg.Value.Split('$');
                        if (split.Count() != 2) throw new Exception("Invalid arguments supplied");
                        argsDict.Add(
                            (Args)Enum.Parse(typeof(Args), split[0].ToString().ToLower(), true),
                            split[1].ToString());
                        break;

                    case ArgsType.Positional:
                        switch ((Args)arg.Index)
                        {
                            case Args.connectionstring:
                                MigrationManager.ConnectionString = arg.Value; break;

                            case Args.migrationfilesfolder:
                                MigrationManager.MigrationsFolderPath = arg.Value; break;

                            case Args.migratetodate:
                                DateTime migrateToDateTimeArg;
                                if (DateTime.TryParse(arg.Value, out migrateToDateTimeArg))
                                    MigrationManager.MigrateToDateTime = migrateToDateTimeArg;
                                else
                                    throw new Exception(@"Invalid Date/Time passed for 'MigrateToDate'");
                                break;
                        }
                        break;
                }

            }

            if (argsType == ArgsType.Named)
            {
                // Determine the Connection String
                string connectionString;
                if (argsDict.TryGetValue(Args.connectionstring, out connectionString))
                    MigrationManager.ConnectionString = connectionString;
                else
                    throw new Exception(@"No 'ConnectionString' argument was passed");

                // Determine the migrations folder path
                string migFilesFolder;
                if (argsDict.TryGetValue(Args.migrationfilesfolder, out migFilesFolder))
                    MigrationManager.MigrationsFolderPath = migFilesFolder;
                else
                    throw new Exception(@"No 'MigrationFilesFolder' argument was passed");

                // If the 'MigrateToDate' argument was set, try to set the MigrateToDate
                DateTime migrateToDateTime;
                string migrateToDateTimeString;

                if (argsDict.TryGetValue(Args.migratetodate, out migrateToDateTimeString))
                    if (DateTime.TryParse(migrateToDateTimeString, out migrateToDateTime))
                    {
                        MigrationManager.MigrateToDateTime = migrateToDateTime;
                        MigrationManager.AutoSetMigrationDirection();

                        //NB: In the Windows UI, the MigrationDirection is set by the User and causes a list of
                        //    available dates for migration in the desired direction to be displayed. In the 
                        //    Console App, the opposite is true. A date is provided, and combining this information with
                        //    details of the latest migration applied (gleaned from the Db if the ConnectionString
                        //    is present and valid) the MigrationDirection can be determined. It'd be easy enough to hard
                        //    wire one or the other Property to always be set from the other. The only reason there are 
                        //    two settable properties that are capable of determining the MigrationDirection 
                        //    independently is to allow the decision about which value sets the other to be left 
                        //    open as an option during runtime.
                    }
            }

            MigrationManager.Migrate();
        }
    }
}
