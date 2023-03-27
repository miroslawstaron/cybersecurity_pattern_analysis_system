protected void btnsave_Click(object sender, EventArgs e)
{
    string dataconn = "Data Source=desktop-tb3r1mj;Initial Catalog=access;Integrated Security=True";

    SqlConnection sqlconn = new SqlConnection(dataconn);
    try
    {
        sqlconn.Open();
        lblMessage.Text = "Database connected";
        string sql = @"insert into ordermaster(partnumber,tranparticular,quantity,unitprice,totalprice,orderdate) values
            ('" + txtpartnum.Text + "','" + txtparticulars.Text + "','" + txtqunatity.Text + "','" + txtunitprice.Text + "','" + txttotalprice.Text + "','" + txtdate.Text + "')";

        SqlCommand sqlcommand = new SqlCommand(sql, sqlconn);
        sqlcommand.CommandType = System.Data.CommandType.Text;
        sqlcommand.ExecuteNonQuery();
        lblMessage.Text = "Data inserted successfully!!";
        sqlconn.Close();
    }
    catch (Exception ex)
    {
        lblMessage.Text = "Database not connected";
    }
}