private void btnDelete_Click(object sender, EventArgs e)
{
    if(MessageBox.Show("Are you sure to delete?", "Quick demo CRUD", MessageBoxButtons.YesNo) == DialogResult.Yes)
    {
        using (QDCRUDEntities db = new QDCRUDEntities())
        {
            var entry = db.Entry(model);
            if (entry.State == System.Data.Entity.EntityState.Detached)
                db.Customer.Attach(model);
            db.Customer.Remove(model);
            db.SaveChanges();
            populateDataGridView();
            Clear();
            MessageBox.Show("Deleted Successfully!");
        }
    }
}