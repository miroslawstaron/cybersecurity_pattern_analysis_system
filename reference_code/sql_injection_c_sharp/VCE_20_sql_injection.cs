public void DeleteData(string tableName, string columnName, int idNo)
{
    SqlConnection con = new SqlConnection(ConnectionString);
    con.Open();
    string sqlCommandText = "Delete from " + tableName + " where " + columnName + " = " + idNo + "";
    SqlCommand cmd = new SqlCommand(sqlCommandText, con);
    cmd.ExecuteNonQuery();
    con.Close();
}