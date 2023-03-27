private static void DeleteCustomerById(int customer_id)
{
    using (SqlConnection conn = new SqlConnection(conn_str))
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("p_delete_from_Customer_by_id", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@customer_id", customer_id);
        cmd.ExecuteNonQuery();
    }
}