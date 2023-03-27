private void button1_Click(object sender, EventArgs e)
  {
   string connetionString;
   SqlConnection cnn;
   connetionString = @"Data Source=WIN-50GP30FGO75;Initial Catalog=Demodb;User ID=sa;Password=demol23";
   cnn = new SqlConnection(connetionString);
   cnn.Open();
   MessageBox.Show("Connection Open  !");
   cnn.Close();
  }