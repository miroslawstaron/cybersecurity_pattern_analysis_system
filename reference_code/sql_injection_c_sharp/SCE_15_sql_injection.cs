private void dgvCustomer_DoubleClick(object sender, EventArgs e)
{
    if (dgvCustomer.CurrentRow.Index != -1)
    {
        model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);

        using (QDCRUDEntities db = new QDCRUDEntities())
        {
            model = db.Customer.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
            txtFirstName.Text = model.FirstName;
            txtLastName.Text = model.LastName;
            txtCity.Text = model.City;
            txtAddress.Text = model.Address;
        }
        btnAdd.Text = "U̲pdate";
        btnDelete.Enabled = true;
    }
}