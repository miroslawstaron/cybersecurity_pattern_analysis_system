public int DeleteTable(string[] arr)
{
    var t = "";
    var id = "";

    if (arr.Length == 3)
    {
        t = arr[1];
        id = arr[2];
    }
    else
    {
        throw new Exception("参数错误");
    }
    string sql = "delete from " + t + " where id = '" + id + "'";
    int rel = 0;
    try
    {
        rel = fsql.Ado.ExecuteNonQuery(sql);
    }
    catch (Exception ex)
    {
        throw new Exception(sql + "\r\n" + ex.Message);
    }
    return rel;
}