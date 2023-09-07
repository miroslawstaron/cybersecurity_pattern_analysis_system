using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;

namespace testPrb01.Models
{
    public class accessData
    {
        private string lineConnection = string.Empty;
        public SqlConnection connection { get; set; }
        public SqlCommand cmd { get; set; }
        public SqlDataReader reader { get; set; }

        public accessData()
        {
           lineConnection = "workstation id=ElityPrb01.mssql.somee.com;packet size=4096;user id=FernandoRdzVcs_SQLLogin_1;pwd=wy6hlv2gpg;data source=ElityPrb01.mssql.somee.com;persist security info=False;initial catalog=ElityPrb01;";
            //lineConnection = "Data Source=FERNET\\SQLSERVER;Initial Catalog=rapidTestingPRB01;User ID=sa;Password=w9w9dorotea;";
           // lineConnection = "Data Source=ElityPrb01.mssql.somee.com;Initial Catalog=ElityPrb01;User ID=FernandoRdzVcs_SQLLogin_1;Password=wy6hlv2gpg;";
            connection = new SqlConnection(lineConnection);
            cmd = new SqlCommand();
        }

        public int executeQuery(string query)
        {
            cmd.Connection = connection;
            cmd = connection.CreateCommand();
            cmd.CommandText = query;
            int result = cmd.ExecuteNonQuery();
            return result;
        }

        public SqlDataReader executeReader(string query)
        {
            cmd.Connection = connection;
            cmd = connection.CreateCommand();
            cmd.CommandText = query;
            reader = cmd.ExecuteReader();            
            return reader;
        }
    }
}