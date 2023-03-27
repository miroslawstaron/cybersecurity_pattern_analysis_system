/// <summary>
/// Loads the grid and column list controls according to the specified query
/// </summary>
/// <param name="sSQL"></param>
private void LoadQueryRecords(string sSQL)
{
    if (sSQL.Trim() == string.Empty)
    {
        MessageBox.Show("The SQL query was empty!  Please enter a valid SQL query!");
        return;
    }

    try
    {
        Cursor.Current = Cursors.WaitCursor;

        m_TableInfo = DatabaseUtilities.LoadDataTable(m_sSqlConnectionString, cmbDatabases.Text, sSQL);
        dgTableInfo.DataSource = m_TableInfo;

        // load the list of columns to include in the generator
        chklstIncludeFields.Items.Clear();
        foreach (DataColumn col in m_TableInfo.Columns)
        {
            // exclude the primary/auto-increment key by default, but select/check all the others
            chklstIncludeFields.Items.Add(col.ColumnName, (chklstIncludeFields.Items.Count > 0));
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("LoadQueryRecords error: " + ex.Message);
    }
    finally
    {
        Cursor.Current = Cursors.Default;
        UpdateControls();
    }
}