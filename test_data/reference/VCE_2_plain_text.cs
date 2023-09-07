// <copyright file="DatabaseAccess.cs" company="engi">
// The Database Access class for the DPOH_Details and DPOH_Details_index tables
// </copyright>
namespace DPOH
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Npgsql;
    
    /// <summary>
    /// The DatabaseAccess class.  Contains the methods to access the database for:
    ///   - Number of rows and execution time
    ///   - The query plan for counting the number of rows.
    /// </summary>
    public class DatabaseAccess
    {
        /// <summary>
        /// This method returns the row count and the execution time to get the row count.
        /// </summary>
        /// <param name="isIndexed">A boolean expression to specify if we're accessing the indexed table (true) or the non-indexed table (false)</param>
        /// <returns>A Tuple containing the row count (as a long) and the execution time (as a double)</returns>
        public static Tuple<long, double> RowCountAndExecutionTime(bool isIndexed)
        {
            using (NpgsqlConnection connection = DatabaseConnection())
            {
                string indexed = string.Empty;
                if (isIndexed)
                {
                    indexed = "_indexed";
                }

                connection.Open();
                NpgsqlCommand command;
                NpgsqlDataReader results;
                
                // Getting the execution time
                command = new NpgsqlCommand("EXPLAIN ANALYZE select count(*) as rowCount from dpoh_details" + indexed, connection); 
                results = command.ExecuteReader();
                
                double timeToExecute = 0;

                int counter = 0;
                string temp = string.Empty;

                // ugly hack fix because I couldn't figure out how to get what I wanted directly
                foreach (IDataRecord t in results) 
                {
                    if (counter++ == 3)
                    {
                        temp = (string)t[0];
                    }
                }

                temp = Regex.Match(temp, @"[0-9]+[.][0-9]+").Value;
                timeToExecute = double.Parse(temp);

                results.Close();

                // getting the number of rows
                command = new NpgsqlCommand("select count(*) as rowCount from dpoh_details" + indexed, connection);
                results = command.ExecuteReader();

                long count = 0;

                foreach (IDataRecord t in results)
                {
                    count = (long)t["rowCount"]; // I really don't like doing this, but everything I've tried has failed misserably and I give up.
                }

                return new Tuple<long, double>(count, timeToExecute);
            } // End using
        } // End RowCountAndExecutionTime

        /// <summary>
        /// This method returns the execution plan for the two tables (the indexed and non-indexed tables)
        /// </summary>
        /// <param name="isIndexed">Used to select the indexed (true) or non-indexed (false) tables</param>
        /// <returns>A string containing the query plan</returns>
        public static string GetSelectQueryPlan(bool isIndexed)
        {
            using (NpgsqlConnection connection = DatabaseConnection())
            {
                string indexed = string.Empty;
                if (isIndexed)
                {
                    indexed = "_indexed";
                }

                connection.Open();
                NpgsqlCommand command;
                NpgsqlDataReader results;

                command = new NpgsqlCommand("EXPLAIN ANALYZE select count(*) as rowCount from dpoh_details" + indexed, connection);
                results = command.ExecuteReader();

                string temp = string.Empty;

                foreach (IDataRecord t in results) 
                {
                    temp += (string)t[0] + "\n";
                }

                return temp;
            } // End using
        } // End GetSelectQueryPlan

        /// <summary>
        /// This is a simple common method.  
        /// It provides a connection to the database that contains both the DPOH tables:
        ///         - DPOH_Details
        ///         - DPOH_Details_indexed
        /// </summary>
        /// <returns>A connection to the DPOH database</returns>
        private static NpgsqlConnection DatabaseConnection()
        {
            NpgsqlConnectionStringBuilder myBuilder = new NpgsqlConnectionStringBuilder();
            myBuilder.Host = "127.0.0.1";
            myBuilder.Port = 5432;
            myBuilder.Database = "Project3.DPOH";
            myBuilder.IntegratedSecurity = true;
            return new NpgsqlConnection(myBuilder);
        }
    }
}