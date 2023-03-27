public async Task<Int32> UpdateCustomerCustomerDemoAsync(MutableCustomerCustomerDemo entity, MutableCustomerCustomerDemo whereEquals)
{
    if( entity == null ) throw new ArgumentNullException(nameof(entity));
    if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

    using( SqlCommand cmd = this.c.CreateCommand() )
    {
        StringBuilder sb = new StringBuilder(
@"UPDATE
dbo.CustomerCustomerDemo
SET
");
        Int32 setColumnsCount = 0;

        if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
        
        sb.Append( "WHERE\r\n" );

        Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
        if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

        cmd.CommandText = sb.ToString();

        Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        return rows;
    }
}
