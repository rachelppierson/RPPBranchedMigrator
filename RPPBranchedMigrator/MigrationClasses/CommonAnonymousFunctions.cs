using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

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
    }
}
