public class accessData
{
    private string lineConnection = string.Empty;
    public SqlConnection connection { get; set; }
    public SqlCommand cmd { get; set; }
    public SqlDataReader reader { get; set; }

    public accessData(string uName, string uPass, string uServer, string uDB)
    {
        lineConnection = "workstation id=localhost;packet size=4096;user id=" + uName + ";pwd=" + uPass + ";data source=" + uServer + ";persist security info=False;initial catalog=" + uDB + ";";
        connection = new SqlConnection(lineConnection);
        cmd = new SqlCommand();
    }
}