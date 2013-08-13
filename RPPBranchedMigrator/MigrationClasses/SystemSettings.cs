using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationClasses
{
    public static class SystemSettings
    {
        // Format used for outputting all DateTime information
        public static readonly string PrefDateTimeFormat = "dd MMM yyyy HH:mm:ss";

        // Format used for encoding DateTime in the filenames of migration files 
        public static readonly string DateTimeFormatForMigrationFiles = "yyyyMMdd_HHmmss"; //TODO: set via config
    }
}
