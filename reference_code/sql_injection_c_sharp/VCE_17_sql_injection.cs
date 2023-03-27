public int UpdateTable(string[] arr)
{
    var t = "";
    var id = "";
    var v = "";
    if (arr.Length == 4)
    {
        t = arr[1];
        id = arr[2];
        v = arr[3];
    }
    else
    {
        throw new Exception("参数错误");
    }
    var arrs = v.Split(",");
    List<string> list = new List<string>();
    foreach (var item in arrs)
    {
        var ar = item.Split("=");
        list.Add(ar[0] + "='" + ar[1] + "'");
    }
    string sql = "update " + t + " set " + string.Join(",", list) + " where id = '" + id + "'";
    int rel = 0;
    try
    {
        rel = fsql.Ado.ExecuteNonQuery("update " + t + " set " + string.Join(",", list) + " where id = '" + id + "'");
    }
    catch (Exception ex)
    {
        throw new Exception(sql + "\r\n" + ex.Message);
    }
    return rel;
}