public void Bar(SqlConnection connection, string param)
{
    SqlCommand command;
    string sensitiveQuery = string.Format("INSERT INTO Users (name) VALUES (\"{0}\")", param);
    command = new SqlCommand(sensitiveQuery); // Sensitive

    command.CommandText = sensitiveQuery; // Sensitive

    SqlDataAdapter adapter;
    adapter = new SqlDataAdapter(sensitiveQuery, connection); // Sensitive
}