private void cmbTables_SelectedIndexChanged(object sender, EventArgs e)
{
    if (cmbTables.SelectedIndex < 0)
        return;
    
    // automatically setup a query based upon the table selected
    string sTableName = cmbTables.Text;
    txtSelectSQL.Text = "SELECT * FROM " + sTableName;
    txtTargetTable.Text = sTableName;
    
    dgTableInfo.DataSource = null;
    UpdateControls();
}