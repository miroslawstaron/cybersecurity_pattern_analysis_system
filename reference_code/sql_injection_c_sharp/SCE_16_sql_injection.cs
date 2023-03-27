public async Task<Int32> InsertCategoryAsync( String categoryName, String description, Byte[] picture )
{
    using( SqlCommand cmd = this.c.CreateCommand() )
    {
        cmd.CommandText =
@"
INSERT INTO dbo.Categories (
[CategoryName], [Description], [Picture]
) VALUES (
@categoryName , @description , @picture 
)
";				
        
        cmd.Parameters.Add( @"categoryName", SqlDbType.NVarChar ).Value = categoryName;
        cmd.Parameters.Add( @"description" , SqlDbType.NText    ).Value = ( description  == null ) ? DBNull.Value : (Object)description;
        cmd.Parameters.Add( @"picture"     , SqlDbType.Image    ).Value = ( picture      == null ) ? DBNull.Value : (Object)picture;

        return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

    }
}