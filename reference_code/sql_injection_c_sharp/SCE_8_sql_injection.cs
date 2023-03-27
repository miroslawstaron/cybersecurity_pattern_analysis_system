namespace *project name*
{
  public partial class className : Form
  
  string connectionstring = "*SQLSERVERINSTANCE*;Database=*NAME*;Trusted_Connection=True;";
  //or..
  string connectionstring= ConfigurationManager.ConnectionStrings["*NAME OF THE conncectionStrings configurated in App.config archive*"].ConnectionString;
  
  //The SQL Select is usually used to "load" data from the SQL DB. So, in case of using a DataGridView in you application, proced like this:
    
    public Product_Details(int idProduct)
        {
            InitializeComponent();
            lblProductId.Text = idProduct.ToString();

            SqlConnection sqlConect = new SqlConnection(connectionString);


            try
            {
                // Conect DB
                sqlConect.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM PRODUCT WHERE ID = @id", sqlConect);

                cmd.Parameters.Add(new SqlParameter("@id", idProduct));

                *PROJECTNAME*.Class.Product product = new *PROJECTNAME*.Class.Product();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())

                    {
                        product.Id = Int32.Parse(reader["ID"].ToString());
                        product.Name = reader["NAME"].ToString();
                        product.Price = float.Parse(reader["PRICE"].ToString());
                        product.Active = bool.Parse(reader["ACTIVE"].ToString());
                        product.Category = new Category
                        {
                            Id = Int32.Parse(reader["FK_CATEGORY"].ToString())
                        };
                    }

                }

                tbxName.Text = product.Name;
                tbxPrice.Text = product.Price.ToString();
                cbxActive.Checked = product.Active;

                int indexCombo = 0;
                if (product.Category != null)
                {
                    indexCombo = product.Category.Id;
                    
                }

               
                InitializeComboBox(cbxCategory, indexCombo);

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error");
                //throw;

                //LogHelper logBD = new LogHelper();
                //logBD.PrintLog(Convert.ToString(Ex));
            }
            finally
            {
                sqlConect.Close();
            }
        }
}