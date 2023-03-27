using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq.Expressions;

namespace CodeGenerator
{
    class DB
    {
        public static String mConStr = "Data Source=CBVN-PC046\\SQLEXPRESS;Initial Catalog=DBGenerator;Persist Security Info=True;Integrated Security=True";

        private static SqlConnection getCon()
        {
            SqlConnection con = new SqlConnection(mConStr);
            con.Open();
            return con;
        }
        public static void ExecQuery(String sql)
        {

            SqlConnection con = getCon();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();

        }
        public static DataSet GetDataSet(String sql)
        {

            SqlConnection con = getCon();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            da.SelectCommand = cmd;
            da.Fill(ds);
            con.Close();

            return ds;
        }
        public static DataTable GetTables()
        {
            DataTable oAllTables = new DataTable();
            DataTable UserTables = new DataTable();
            DataRow NewRow = null;
            try
            {
                using (SqlConnection conn = getCon())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_tables";
                        cmd.Connection = conn;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(oAllTables);
                            UserTables = oAllTables.Clone();
                            foreach (DataRow oRow in oAllTables.Rows)
                            {
                                if (oRow["TABLE_TYPE"].ToString() == "SYSTEM TABLE") { continue; }
                                if (oRow["TABLE_TYPE"].ToString() == "VIEW") { continue; }
                                if (oRow["TABLE_NAME"].ToString() == "dtproperties") { continue; }
                                NewRow = UserTables.NewRow();
                                NewRow.ItemArray = oRow.ItemArray;
                                NewRow["TABLE_NAME"] = NewRow["TABLE_NAME"].ToString().Replace(" ", ""); 
                                UserTables.Rows.Add(NewRow);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return UserTables;
        }
        public static DataTable GetColumns(string tableName)
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection conn = getCon())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "sp_columns";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@table_name", SqlDbType.NVarChar, 384));
                        cmd.Parameters["@table_name"].Value = tableName;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                }
            }
            catch (Exception) { throw; }
            
            return table;
        }
    }
}