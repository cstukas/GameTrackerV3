using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Connection
    {
        public static bool ExistsInTable<T>(string table, string column, string value)
        {
            var sql = $"SELECT {column} FROM {table} WHERE {column} = '{value}'";
            var check = DBFunctions.LoadList<T>(sql);
            if (check?.Count > 0) return true;
            else return false;
        }

        public static void ClearFromDB(string table, string keyColumn, int key)
        {
            DBFunctions.RunQuery($"DELETE FROM {table} WHERE {keyColumn} = '{key}'");
        }

        public static void ClearTable(string table)
        {
            DBFunctions.RunQuery($"DELETE FROM {table}");
        }

        public static void DropTable(string tableName)
        {
            var dropSql = $"DROP TABLE {tableName}";
            DBFunctions.RunQuery(dropSql);
        }

        public static void TestConnection()
        {
            DBFunctions.TestConnection(DBFunctions.GetConnection());
        }
    }
}
