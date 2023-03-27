private void btnUpdate(object sender, EventArgs e)
{
    if (string.IsNullOrEmpty(lblId.Text))// this is an invisible label created to check the item
    {
        SqlConnection sqlConnect = new SqlConnection(connectionString);

        try
        {
            sqlConnect.Open();
                string sql = "UPDATE PRODUCT SET COLUM = @name, COLUM2 = @name2, ACTIVE = @active, WHERE ID = @id";

            SqlCommand cmd = new SqlCommand(sql, sqlConnect);

            cmd.Parameters.Add(new SqlParameter("@name", this.tbxName.Text));
            cmd.Parameters.Add(new SqlParameter("@name2", this.tbxName2.Text));
            

            cmd.ExecuteNonQuery();

            MessageBox.Show("Success");

            //LogHelper log = new LogHelper();
            //log.Insert("Product Update");
        }
        catch (Exception Ex)
        {
            MessageBox.Show("Error");
            //throw;

            //LogHelper logBD = new LogHelper();
            //logBD.PrintLog(Convert.ToString(Ex));
        }
        finally
        {
            sqlConnect.Close();

            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Hide();
        }
    }