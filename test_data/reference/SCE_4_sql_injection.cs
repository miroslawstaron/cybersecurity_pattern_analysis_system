private static void InsertCustomer
    (
    string first_name,
    string last_name,
    string email,
    string password,
    string address,
    long phone,
    string gender,
    DateTime birthdate,
    DateTime reg_date,
    int bonus_percent
    )
{

    using (SqlConnection conn = new SqlConnection(conn_str))
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("p_insert_to_customer", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@first_name", first_name);
        cmd.Parameters.AddWithValue("@last_name", last_name);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Parameters.AddWithValue("@address", address);
        cmd.Parameters.AddWithValue("@phone", phone);
        cmd.Parameters.AddWithValue("@gender", gender);
        cmd.Parameters.AddWithValue("@birthdate", birthdate);
        cmd.Parameters.AddWithValue("@reg_date", reg_date);
        cmd.Parameters.AddWithValue("@bonus_percent", bonus_percent);
        cmd.ExecuteNonQuery();
    }
}