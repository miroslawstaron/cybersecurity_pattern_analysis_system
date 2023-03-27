 private void btnDelete(object sender, EventArgs e)
{
    if (!string.IsNullOrEmpty(lblId.Text))// this is an invisible label created to check the id of the item. Here is chacking if the item exists on DB
    {
        SqlConnection sqlConnect = new SqlConnection(connectionString);

        try
        {
            sqlConnect.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM COLUMNAME WHERE ID=@id", sqlConnect);

            cmd.Parameters.Add(new SqlParameter("@id", this.lbltId.Text));
            

            cmd.ExecuteNonQuery();

            MessageBox.Show("SUccess");

            //LogHelper log = new LogHelper();
            //log.Insert("Product Delete");
        }
        catch (Exception Ex)
        {
            MessageBox.Show("Error on delete!");
            //throw;

            //LogHelper logBD = new LogHelper();
            //logBD.PrintLog(Convert.ToString(Ex));
        }
        finally
        {
            sqlConnect.Close();
        }
    }
}