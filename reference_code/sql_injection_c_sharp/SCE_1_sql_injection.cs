public void Foo(DbContext context, string query, string param)
{
    context.Database.ExecuteSqlCommand("SELECT * FROM mytable WHERE mycol=@p0", param); // Compliant, it's a parametrized safe query
}