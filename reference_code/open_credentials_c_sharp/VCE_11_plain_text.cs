public class accessData
{
    private string lineConnection = string.Empty;
    public SqlConnection connection { get; set; }
    public SqlCommand cmd { get; set; }
    public SqlDataReader reader { get; set; }

    public accessData()
    {
        lineConnection = "workstation id=localhost;packet size=4096;user id=admin;pwd=equivalentMind12;data source=ElityPrb01.mssql.somee.com;persist security info=False;initial catalog=ElityPrb01;";
        //lineConnection = "Data Source=FERNET\\SQLSERVER;Initial Catalog=rapidTestingPRB01;User ID=sa;Password=w9w9dorotea;";
        // lineConnection = "Data Source=ElityPrb01.mssql.somee.com;Initial Catalog=ElityPrb01;User ID=FernandoRdzVcs_SQLLogin_1;Password=wy6hlv2gpg;";
        connection = new SqlConnection(lineConnection);
        cmd = new SqlCommand();
    }
}