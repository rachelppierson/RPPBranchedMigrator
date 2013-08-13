using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;

namespace MigrationClasses
{
    public class CommonAnonymousFunctions
    {
        /// <summary>
        /// Anonymous Function for determining the implied DateTime of a migration file's creation from its filename.
        /// </summary>
        public static Func<string, DateTime?> TryGetDateFromFilename =
            value =>
            {
                DateTime createdWhen;

                return DateTime.TryParseExact(value.Substring(0, 15), "yyyyMMdd_HHmmss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out createdWhen)
                    ? (DateTime?)createdWhen
                    : null;
            };

        /// <summary>
        /// Anonymous Function for determining the implied description for a migration file from its filename.
        /// </summary>
        public static Func<string, string> GetDescriptionFromMigrationFilename =
            value =>
            {
                int filenamePreambleLength = SystemSettings.DateTimeFormatForMigrationFiles.Length + 1;
                //e.g., if format is 'yyyyMMdd_HHmmss' then the preamble will be 'yyyyMMdd_HHmmss_', i.e. one character longer

                return
                    value.Length > filenamePreambleLength
                    ? Path.GetFileNameWithoutExtension(value.Substring(filenamePreambleLength, value.Length - filenamePreambleLength)) 
                    : string.Empty;
            };
    }
}
