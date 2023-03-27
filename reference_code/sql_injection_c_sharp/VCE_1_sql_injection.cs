public void Foo(DbContext context, string query, string param)
{
    string sensitiveQuery = string.Concat(query, param);
    context.Database.ExecuteSqlCommand(sensitiveQuery); // Sensitive
    context.Query<User>().FromSql(sensitiveQuery); // Sensitive

    context.Database.ExecuteSqlCommand($"SELECT * FROM mytable WHERE mycol={value}", param); // Sensitive, the FormattableString is evaluated and converted to RawSqlString
    string query = $"SELECT * FROM mytable WHERE mycol={param}";
    context.Database.ExecuteSqlCommand(query); // Sensitive, the FormattableString has already been evaluated, it won't be converted to a parametrized query.
}