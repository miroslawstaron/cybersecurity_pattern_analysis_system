public async Task<List<Category>> QueryCategoryAsync(MutableCategory whereEquals)
{
    //if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

    const String selectSql =
@"SELECT
CategoryID,
CategoryName,
Description,
Picture
FROM
dbo.Categories
";

    using( SqlCommand cmd = this.c.CreateCommand() )
    {
        StringBuilder sb = new StringBuilder( selectSql );

        if( whereEquals != null )
        {
            sb.Append( "WHERE\r\n" );

            Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
            if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );
        }

        cmd.CommandText = sb.ToString();

        using( SqlDataReader rdr = await cmd.ExecuteReaderAsync().ConfigureAwait(false) )
        {
            List<Category> list = new List<Category>();

            while( await rdr.ReadAsync().ConfigureAwait(false) )
            {
                Category row = CreateCategoryEntity( rdr );
                list.Add( row );
            }

            return list;
        }
    }
}