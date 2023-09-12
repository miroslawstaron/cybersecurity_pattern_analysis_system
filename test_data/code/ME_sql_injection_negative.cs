private static DataSet SelectByGender(string gender)
{
    DataSet ds = new DataSet();
    using (SqlConnection conn = new SqlConnection(conn_str))
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("p_select_by_gender", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@gender", gender);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        return ds;
    }
}