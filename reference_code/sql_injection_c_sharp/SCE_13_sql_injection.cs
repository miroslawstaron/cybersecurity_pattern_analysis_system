/// <summary>
/// Generates sql statements based upon the generator type selected and the tableinfo provided
/// </summary>
private void GenerateSqlStatements()
{
    // clear the string member
    m_sSqlStatementText = string.Empty;

    // create an array of all the columns that are to be included
    ArrayList aryColumns = new ArrayList();
    for (int i = 0; i < chklstIncludeFields.CheckedItems.Count; i++)
    {
        aryColumns.Add(chklstIncludeFields.CheckedItems[i].ToString());
    }

    // if no columns included, return with a msg
    if(aryColumns.Count <= 0)
    {
        MessageBox.Show("No columns selected!  Please check/select some columns to include!");
        return;
    }

    string sTargetTableName = txtTargetTable.Text.Trim();
    if(sTargetTableName == string.Empty)
    {
        MessageBox.Show("No valid target table name!  Please enter a table name to be used in the SQL statements!");
        return;
    }            

    // generate the sql by type
    if (cmbSqlType.SelectedIndex == (int)STATEMENT_TYPES.INSERT)
    {
        m_sSqlStatementText = SqlScriptGenerator.GenerateSqlInserts(aryColumns, m_TableInfo, sTargetTableName);
    }
    else if (cmbSqlType.SelectedIndex == (int)STATEMENT_TYPES.UPDATE)
    {
        // get an array of all the active table columns         
        ArrayList aryWhereColumns = new ArrayList();
        for (int i = 0; i < chklstIncludeFields.Items.Count; i++)
        {
            aryWhereColumns.Add(chklstIncludeFields.GetItemText(chklstIncludeFields.Items[i]));
        }

        // create dlg and pass in array of columns
        SelectMultipleItems dlg = new SelectMultipleItems();
        dlg.Text = "Select WHERE Columns";
        dlg.Description = "Select WHERE-Clause Columns for UPDATE SQLs:";
        dlg.Initialize(aryWhereColumns, string.Empty, false);

        // user cancelled, so exit
        if (dlg.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        aryWhereColumns.Clear();
        aryWhereColumns = dlg.UserSelectedItems;

        m_sSqlStatementText = SqlScriptGenerator.GenerateSqlUpdates(aryColumns, aryWhereColumns, m_TableInfo, sTargetTableName);
    }
    else if (cmbSqlType.SelectedIndex == (int)STATEMENT_TYPES.DELETE)
    {
        m_sSqlStatementText = SqlScriptGenerator.GenerateSqlDeletes(aryColumns, m_TableInfo, sTargetTableName);
    }            
}