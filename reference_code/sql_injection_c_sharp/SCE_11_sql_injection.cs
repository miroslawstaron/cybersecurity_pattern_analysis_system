/// <summary>
/// Initializes the database combobox control
/// </summary>
private void InitializeDatabaseControls()
{
    try
    {
        Cursor.Current = Cursors.WaitCursor;

        cmbDatabases.DataSource = null;
        cmbDatabases.Items.Clear();
        cmbTables.DataSource = null;
        cmbTables.Items.Clear();     // clear the tables also since they'll be invalid       

        cmbDatabases.DisplayMember = "DATABASE_NAME";
        cmbDatabases.ValueMember = "DATABASE_NAME";
        DataTable dt = DatabaseUtilities.GetDatabases(m_sSqlConnectionString);
        cmbDatabases.DataSource = dt;
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error loading database-tables into list: " + ex.Message);
    }
    finally
    {
        Cursor.Current = Cursors.Default;
        UpdateControls();
    }
}