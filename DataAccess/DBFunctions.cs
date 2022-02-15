using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;


namespace DataAccess
{
    public static class DBFunctions
    {

        // ----- Connection Functions -----
        public static string GetConnection()
        {
            string hostname = "gametrackerv3.chqjomcmy9q4.us-east-2.rds.amazonaws.com,1433";
            string dbname = "GameTrackerV3";
            string username = "admin";
            string password = "buggagram11";
            return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }


        /// <summary>
        /// Establishes a initial connection
        /// Nice to do this when the program starts since the initial connection takes a bit longer than normal
        /// </summary>,
        public static int TestConnection(string connection)
        {
            string sql = "SELECT 1;";
            try
            {
                using (IDbConnection conn = new SqlConnection(connection))
                {
                    Logger.Log("Testing Connection: " + connection, true);
                    int result = (Int32)conn.ExecuteScalar(sql);
                    return result;

                }
            }
            catch (Exception e)
            {
                DatabaseQueryFailed(e);
                return -1;
            }
        }


        // ----- CRUD Operations -----
        public static void RunQuery(string sql)
        {
            // Run Query
            try
            {
                using (IDbConnection db = new SqlConnection(GetConnection()))
                {
                    Logger.Log("Run Query: " + sql, true);
                    db.Execute(sql);
                    return;
                }
            }
            catch (Exception e)
            {
                DatabaseQueryFailed(e);
            }
        }

        // CREATE
        /// <summary>
        /// Inserts Object into Database
        /// </summary>
        public static void InsertObject<T>(T dto, string table, string ignore = "")
        {
            List<Property> properties = ReflectionFuncs.GetAllValidProperties(dto); 

            // build sql query
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"INSERT INTO " + table + " ( ");
            //column names
            for (int i = 0; i < properties.Count; i++)
            {
                Property prop = properties[i];
                sqlBuilder.Append(prop.Name);
                if (i < properties.Count - 1) sqlBuilder.Append(",");
            }

            sqlBuilder.Append(") VALUES (");
            //values
            for (int i = 0; i < properties.Count; i++)
            {
                Property prop = properties[i];

                // Check for apostrophes
                string val = prop.Value as string;
                if(val != null) prop.Value = val.Replace("'", "''");

                if (prop.Value as string == "NULL")
                    sqlBuilder.Append("" + prop.Value + "");
                else
                    sqlBuilder.Append("'" + prop.Value + "'");


                if (i < properties.Count - 1) sqlBuilder.Append(",");
            }
            sqlBuilder.Append(")");

            RunQuery(sqlBuilder.ToString());
        }

        // READ 
        /// <summary>
        /// Loads a list of objects from the Database
        /// </summary>
        public static List<T> LoadList<T>(string sql)
        {
            List<T> list = new List<T>();
            try
            {
                using (IDbConnection db = new SqlConnection(GetConnection()))
                {
                    Logger.Log("Load List: " + sql, true);
                    list = db.Query<T>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                DatabaseQueryFailed(e);


            }
            return list;
        }

        /// <summary>
        /// Loads a single object from the Database
        /// </summary>
        public static T LoadObject<T>(string sql)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(GetConnection()))
                {
                  //  Logger.Log("Load Object: " + sql, true);
                    T obj = db.QuerySingleOrDefault<T>(sql);
                    return obj;
                }
            }
            catch (Exception e)
            {
                DatabaseQueryFailed(e,sql);
                throw;
            }

        }

        // UPDATE
        /// <summary>
        /// Updates Object ONLY on properties that have been changed
        /// </summary>
        public static void UpdateObject<T>(T dto, T origDto, string table, string[] keys)
        {
            RunUpdate(ReflectionFuncs.GetChangedProperties(dto, origDto), dto, table, keys);
        }

        /// <summary>
        /// Updates entire object
        /// </summary>
        public static void UpdateObjectAllProperties<T>(T dto, string table, string[] keys)
        {
            RunUpdate(ReflectionFuncs.GetAllValidProperties(dto), dto, table, keys);
        }

        private static void RunUpdate<T>(List<Property> props, T dto, string table, string[] keys)
        {
            if (props.Count == 0) return;

            // Get Key Values
            List<Property> keyProperties = new List<Property>();
            foreach (string key in keys)
            {
                keyProperties.Add(ReflectionFuncs.GetPropertyInfo(dto, key));
            }

            if (keyProperties.Count == 0)
                throw new Exception("Can't Update without a Key");

            // Build SQL Query
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"UPDATE " + table + " SET ");
            for (int i = 0; i < props.Count; i++)
            {
                Property prop = props[i];

                // Check for apostrophes
                string val = prop.Value as string;
                if (val != null) prop.Value = val.Replace("'", "''");

                if (prop.Value as string == "NULL")
                    sqlBuilder.Append(prop.Name + "=" + prop.Value + "");
                else
                    sqlBuilder.Append(prop.Name + "='" + prop.Value + "'");

                if (i < props.Count - 1) sqlBuilder.Append(",");
            }
            sqlBuilder.Append(" WHERE ");
            for (int k = 0; k < keyProperties.Count; k++)
            {
                Property prop = keyProperties[k];
                sqlBuilder.Append(prop.Name + "='" + prop.Value + "'");
                if (k < keyProperties.Count - 1) sqlBuilder.Append(" AND ");
            }

            RunQuery(sqlBuilder.ToString());
        }


        // ----- Stored Precedures -----
        /// <summary>
        /// Calls the stored precedure for getting the next key from the Database
        /// </summary>
        /// <param name="type">Determine which row we pull the key from. No type means we pull a general key</param>
        public static int GetNextKey(int count, string type)
        {
            string keyType = type;

            if (type.ToLower() == "labno") keyType = "LabNo";
            if (type.ToLower() == "reportkey") keyType = "ReportKey";
            if (type == "") keyType = "Key";

            DynamicParameters para = new DynamicParameters();
            para.Add("@keyname", keyType);
            para.Add("@keyblock", count);
            para.Add("@nextKey", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int key = -1;
            try
            {
                using (IDbConnection db = new SqlConnection(GetConnection()))
                {
                    Logger.Log($"Get Next {type} Key, Count: {count}");

                    db.Query("getNextKeyValue", para, commandType: CommandType.StoredProcedure);
                    key = para.Get<int>("@nextKey");
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return key;
        }


        // ----- Helper Functions -----
        /// <summary>
        /// Returns the Count of a column
        /// </summary>
        public static int GetDBValueCount(string table, string column, string value)
        {
            string sql = "SELECT COUNT(" + column + ") FROM " + table + " WHERE " + column + " = '" + value + "'";
            return GetDBValueCount(sql);
        }

        /// <summary>
        /// Returns the Count from a select query
        /// </summary>
        public static int GetDBValueCount(string sql)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(GetConnection()))
                {
                    Logger.Log("Get DB Count: " + sql, true);
                    int count = (Int32)conn.ExecuteScalar(sql);
                    return count;
                }
            }
            catch (Exception e)
            {
                DatabaseQueryFailed(e);
                return 0;

            }
        }

        /// <summary>
        /// Handles the situation where a Database query has failed
        /// </summary>
        public static void DatabaseQueryFailed(Exception e, string customMessage = "")
        {
            string errorMessage = "";
            if(customMessage != "")
                errorMessage = customMessage + " - ";
            errorMessage += e.Message;

            Logger.Log("DB Failed: " + errorMessage, true);
            Debug.WriteLine(errorMessage);
            Environment.Exit(0);
        }



    }
}
