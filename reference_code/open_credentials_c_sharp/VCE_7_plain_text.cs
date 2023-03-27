class Program
{
    private MySqlConnection conn;
    static void connect()
    {
        string server = "localhost";
        string database = "mysqldb1";
        string user = "root";
        string password = "u1s2e3r4";
        string port = "3306";
        string sslM = "none";

        string connString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);

        conn = new MySqlConnection(connString);
        try
        {
            conn.Open();

            Console.WriteLine("Connection Successful");

            conn.Close();
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e.Message + connString);
        }

    }

    static void Main(string[] args)
    {
        connect();
    }
}