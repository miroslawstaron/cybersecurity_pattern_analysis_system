protected void btnLogin_Click(object sender, EventArgs e)
{
    try
    {
        conn.Open();
        command.Connection = conn;

        //Create the MD5 Message digest (hash value) for the Password

        string md5Password = GetMd5Hash(txtPasswd.Text);

        //Insert data into database
        command.CommandText = string.Format("select UserName, UserPassword from userTbl where UserName='{0}' and UserPassword='{1}'", txtUName.Text, md5Password);
        queryResults = command.ExecuteReader();

        if (queryResults.Read())

            FormsAuthentication.RedirectFromLoginPage(txtUName.Text, true);
        else
            Label1.Text = "No such user or wrong password";

        queryResults.Close();
    }

    catch (Exception ex)
    {
        Label1.Text = ex.Message;
    }

    finally
    {
        conn.Close();
    }

}