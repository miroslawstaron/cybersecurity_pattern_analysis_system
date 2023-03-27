private void btnSave, or btnAdd(object sender, EventArgs e)
{
    if (string.IsNullOrEmpty(lblId.Text))// this is an invisible label, created to check the item id on DB
    {
        SqlConnection sqlConnect = new SqlConnection(connectionString);

        try
        {
            sqlConnect.Open();
            string sql = "INSERT INTO PRODUCT(local table on SQL DB) VALUES (@value1, @value2)";

            SqlCommand cmd = new SqlCommand(sql, sqlConnect);

            cmd.Parameters.Add(new SqlParameter("@value1", this.tbxName.Text));
            cmd.Parameters.Add(new SqlParameter("@value2", this.tbxName.Text));
            
            cmd.ExecuteNonQuery();

            MessageBox.Show("Succses!");

            LogDAO.InsertLog(new Log
            {
                Description = "Succes description",
                Date = DateTime.Now
                //Id = 2
            });// just for log information

        }
        catch (Exception ex)
        {
            MessageBox.Show("Error");
            LogDAO.InsertLog(new Log
            {
                Description = "Error description " + ex.Message,
                Date = DateTime.Now
                //Id = 1
            });
            //LogHelper.PrintLog(Convert.ToString(Ex));
        }
        finally
        {
            sqlConnect.Close();
            this.Close();
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }
    }