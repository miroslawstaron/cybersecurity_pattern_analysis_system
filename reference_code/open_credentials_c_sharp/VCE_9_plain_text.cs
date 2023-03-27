protected void btn_connect_Click(object sender, EventArgs e) {
    string connetionString;
    SqlConnection cnn;
    connetionString = @ "Data Source=WIN-787926;Initial Catalog=testdb;User ID=sa;Password=sa@l23";
    cnn = new SqlConnection(connetionString);
    cnn.Open();
    lblmsg.Text = "Successfully Connected to database !";
    cnn.Close();
}