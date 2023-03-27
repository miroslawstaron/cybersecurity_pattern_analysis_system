private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableCategory whereEquals )
{
    Int32 c = 0;
    if( whereEquals.CategoryIDIsSet )
        AddSqlUpdateWhereAnd( sb, cmd, ref c, "CategoryID", "@where_categoryID", SqlDbType.Int, whereEquals.CategoryID );

    if( whereEquals.CategoryNameIsSet )
        AddSqlUpdateWhereAnd( sb, cmd, ref c, "CategoryName", "@where_categoryName", SqlDbType.NVarChar, whereEquals.CategoryName );

    if( whereEquals.DescriptionIsSet )
        AddSqlUpdateWhereAnd( sb, cmd, ref c, "Description", "@where_description", SqlDbType.NText, whereEquals.Description );

    if( whereEquals.PictureIsSet )
        AddSqlUpdateWhereAnd( sb, cmd, ref c, "Picture", "@where_picture", SqlDbType.Image, whereEquals.Picture );

    return c;
}