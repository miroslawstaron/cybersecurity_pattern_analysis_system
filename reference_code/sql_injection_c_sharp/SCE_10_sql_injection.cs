private void cmbDatabases_SelectedIndexChanged(object sender, EventArgs e)
{
    if (cmbDatabases.SelectedIndex < 0)
        return;

    // user selected a different database, so load the table combobox
    try
    {
        Cursor.Current = Cursors.WaitCursor;
        cmbTables.DataSource = null;
        cmbTables.Items.Clear();
        cmbTables.DisplayMember = "TABLE_NAME";
        cmbTables.ValueMember = "TABLE_NAME";

        DataTable dt = DatabaseUtilities.GetDatabaseTables(m_sSqlConnectionString, cmbDatabases.Text);
        cmbTables.DataSource = dt;
    }
    catch (Exception ex)
    {
        MessageBox.Show("cmbDatabases_SelectedIndexChanged error: " + ex.Message);
    }
    finally
    { 
        Cursor.Current = Cursors.Default;
        UpdateControls();
    }            
}