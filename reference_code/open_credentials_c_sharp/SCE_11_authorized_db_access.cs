public class DB 
{
    // The connection string to the database
    // The database is in the same directory as the executable
    public SQLConnection establishConn(string strUserName, 
                              string strPassword, 
                              string strDB, 
                              string strServer, 
                              string strDSource)
    {
        string strConn = "workstation id=" + strServer + ";packet size=4096;user id=" + strUserName + ";pwd=" + strPassword + ";data source=" + strDSource + ";persist security info=False;initial catalog=" + strDB + ";";
        SqlConnection conn = new SqlConnection(strConn);
        conn.Open();
        return conn;
    }
}