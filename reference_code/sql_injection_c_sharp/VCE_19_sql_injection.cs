public void SelectData(string sqlTableName)
        {
            SqlTableName = sqlTableName;
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("Select * From " + sqlTableName + "", con);
            SqlDataAdapter sqlDa = new SqlDataAdapter(cmd);
            sqlDt = new DataTable();
            sqlDa.Fill(sqlDt);
        }
