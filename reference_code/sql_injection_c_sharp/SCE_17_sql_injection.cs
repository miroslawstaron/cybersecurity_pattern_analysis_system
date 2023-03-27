public async Task<Int32> UpdateCategoryAsync(MutableCategory entity, MutableCategory whereEquals)
{
    if( entity == null ) throw new ArgumentNullException(nameof(entity));
    if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

    using( SqlCommand cmd = this.c.CreateCommand() )
    {
        StringBuilder sb = new StringBuilder(
@"UPDATE
dbo.Categories
SET
");
        Int32 setColumnsCount = 0;
        if( entity.CategoryNameIsSet )
            AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "CategoryName", "@categoryName", SqlDbType.NVarChar, entity.CategoryName );

        if( entity.DescriptionIsSet )
            AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Description", "@description", SqlDbType.NText, entity.Description );

        if( entity.PictureIsSet )
            AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Picture", "@picture", SqlDbType.Image, entity.Picture );


        if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
        
        sb.Append( "WHERE\r\n" );

        Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
        if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

        cmd.CommandText = sb.ToString();

        Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        return rows;
    }
}
