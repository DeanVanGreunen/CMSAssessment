using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace adonet.extensions
{
    public static class AdonetExt
    {
        public static String GetString(this SqlDataReader reader, string columnName)
        {
            return reader.GetString(reader.GetOrdinal(columnName));
        }
        public static int GetInt32(this SqlDataReader reader, string columnName)
        {
            return reader.GetInt32(reader.GetOrdinal(columnName));
        }

        public static float GetFloat(this SqlDataReader reader, string columnName)
        {
            return reader.GetFloat(reader.GetOrdinal(columnName));
        }

        public static DateTime GetDateTime(this SqlDataReader reader, string columnName)
        {
            return reader.GetDateTime(reader.GetOrdinal(columnName));
        }
    }
}