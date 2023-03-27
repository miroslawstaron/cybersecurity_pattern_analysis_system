private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            using (connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();


                try
                {
                    if (selecttablecombobox.SelectedIndex == 0)
                    {
                        SqlCommand MaxsqlCommandbyStoredProcedure = new SqlCommand(@"sp_MaxIdProducts", connection);
                        MaxsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqloutputParameter = new SqlParameter();
                        sqloutputParameter.SqlDbType = SqlDbType.Int;
                        sqloutputParameter.ParameterName = "@MaxId";
                        sqloutputParameter.Value = ProductIdTxt.Text;
                        sqloutputParameter.Direction = System.Data.ParameterDirection.Output;
                        MaxsqlCommandbyStoredProcedure.Parameters.Add(sqloutputParameter);

                        MaxsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        MaxIdFromProducs = Convert.ToInt32(sqloutputParameter.Value);


                        if (MaxIdFromProducs >= Convert.ToInt32(ProductIdTxt.Text))
                        {
                            SqlCommand UpdatesqlCommandbyStoredProcedure = new SqlCommand(@"sp_updateProduct", connection);
                            UpdatesqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                            SqlParameter sqlParameter1 = new SqlParameter();
                            sqlParameter1.SqlDbType = SqlDbType.NVarChar;
                            sqlParameter1.ParameterName = "@ProductName";
                            sqlParameter1.Value = ProductnameTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter1);

                            SqlParameter sqlParameter2 = new SqlParameter();
                            sqlParameter2.SqlDbType = SqlDbType.Int;
                            sqlParameter2.ParameterName = "@ProductId";
                            sqlParameter2.Value = ProductIdTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter2);

                            UpdatesqlCommandbyStoredProcedure.ExecuteNonQuery();

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter(@"select * from Products", connection);


                            RefreshTable();

                        }

                        if (MaxIdFromProducs <= Convert.ToInt32(ProductIdTxt.Text))
                        {
                            MessageBox.Show("You change ProductId that are not.");

                        }
                    }


                    if (selecttablecombobox.SelectedIndex == 1)
                    {

                        SqlCommand MaxsqlCommandbyStoredProcedure = new SqlCommand(@"sp_MaxIdCustomer", connection);
                        MaxsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqloutputParameter = new SqlParameter();
                        sqloutputParameter.SqlDbType = SqlDbType.Int;
                        sqloutputParameter.ParameterName = "@MaxId";
                        sqloutputParameter.Value = CustomerIdTxt.Text;
                        sqloutputParameter.Direction = System.Data.ParameterDirection.Output;
                        MaxsqlCommandbyStoredProcedure.Parameters.Add(sqloutputParameter);

                        MaxsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        MaxIdFromCustomer = Convert.ToInt32(sqloutputParameter.Value);


                        if (MaxIdFromCustomer >= Convert.ToInt32(CustomerIdTxt.Text))
                        {
                            SqlCommand UpdatesqlCommandbyStoredProcedure = new SqlCommand(@"sp_updateCustomer", connection);
                            UpdatesqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                            SqlParameter sqlParameter1 = new SqlParameter();
                            sqlParameter1.SqlDbType = SqlDbType.Int;
                            sqlParameter1.ParameterName = "@CustomerId";
                            sqlParameter1.Value = CustomerIdTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter1);

                            SqlParameter sqlParameter2 = new SqlParameter();
                            sqlParameter2.SqlDbType = SqlDbType.NVarChar;
                            sqlParameter2.ParameterName = "@CustomerName";
                            sqlParameter2.Value = CustomernameTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter2);


                            UpdatesqlCommandbyStoredProcedure.ExecuteNonQuery();

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter(@"select * from Customers", connection);


                            RefreshTable();

                        }

                        if (MaxIdFromCustomer <= Convert.ToInt32(CustomerIdTxt.Text))
                        {
                            MessageBox.Show("You change Customer that are not.");

                        }
                    }

                    if (selecttablecombobox.SelectedIndex == 2)
                    {

                        SqlCommand MaxsqlCommandbyStoredProcedure = new SqlCommand(@"sp_MaxIdDetailsofOrder", connection);
                        MaxsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqloutputParameter = new SqlParameter();
                        sqloutputParameter.SqlDbType = SqlDbType.Int;
                        sqloutputParameter.ParameterName = "@MaxId";
                        sqloutputParameter.Value = DetailsofOrderIdTxt.Text;
                        sqloutputParameter.Direction = System.Data.ParameterDirection.Output;
                        MaxsqlCommandbyStoredProcedure.Parameters.Add(sqloutputParameter);

                        MaxsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        MaxIdFromDetailsofOrder = Convert.ToInt32(sqloutputParameter.Value);


                        if (MaxIdFromDetailsofOrder >= Convert.ToInt32(DetailsofOrderIdTxt.Text))
                        {
                            SqlCommand UpdatesqlCommandbyStoredProcedure = new SqlCommand(@"sp_updateDetailsofOrder", connection);
                            UpdatesqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                            SqlParameter sqlParameter1 = new SqlParameter();
                            sqlParameter1.SqlDbType = SqlDbType.Bit;
                            sqlParameter1.ParameterName = "@iscash";
                            sqlParameter1.Value = DetailsofOrderIsCashTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter1);

                            SqlParameter sqlParameter2 = new SqlParameter();
                            sqlParameter2.SqlDbType = SqlDbType.Int;
                            sqlParameter2.ParameterName = "@DateorderId";
                            sqlParameter2.Value = DetailsofOrderIdTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter2);

                            SqlParameter sqlParameter3 = new SqlParameter();
                            sqlParameter3.SqlDbType = SqlDbType.DateTime;
                            sqlParameter3.ParameterName = "@dateorder";
                            sqlParameter3.Value = DetailsofOrderDateOrderTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter3);

                            UpdatesqlCommandbyStoredProcedure.ExecuteNonQuery();

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter(@"select * from DetailsofOrder", connection);


                            RefreshTable();

                        }

                        if (MaxIdFromDetailsofOrder <= Convert.ToInt32(DetailsofOrderIdTxt.Text))
                        {
                            MessageBox.Show("You change Customer that are not.");

                        }
                    }

                    if (selecttablecombobox.SelectedIndex == 3)
                    {


                        SqlCommand MaxsqlCommandbyStoredProcedure = new SqlCommand(@"sp_MaxIdOrder", connection);
                        MaxsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqloutputParameter = new SqlParameter();
                        sqloutputParameter.SqlDbType = SqlDbType.Int;
                        sqloutputParameter.ParameterName = "@MaxId";
                        sqloutputParameter.Value = DetailsofOrderIdTxt.Text;
                        sqloutputParameter.Direction = System.Data.ParameterDirection.Output;
                        MaxsqlCommandbyStoredProcedure.Parameters.Add(sqloutputParameter);

                        MaxsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        MaxIdFromOrders = Convert.ToInt32(sqloutputParameter.Value);


                        if (MaxIdFromOrders >= Convert.ToInt32(OrdersIdTxt.Text))
                        {
                            SqlCommand UpdatesqlCommandbyStoredProcedure = new SqlCommand(@"sp_updateOrder", connection);
                            UpdatesqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                            SqlParameter sqlParameter1 = new SqlParameter();
                            sqlParameter1.SqlDbType = SqlDbType.Int;
                            sqlParameter1.ParameterName = "@OrderId";
                            sqlParameter1.Value = OrdersIdTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter1);

                            SqlParameter sqlParameter2 = new SqlParameter();
                            sqlParameter2.SqlDbType = SqlDbType.Int;
                            sqlParameter2.ParameterName = "@CustumerId";
                            sqlParameter2.Value = OrdersCustomerIdTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter2);

                            SqlParameter sqlParameter3 = new SqlParameter();
                            sqlParameter3.SqlDbType = SqlDbType.Int;
                            sqlParameter3.ParameterName = "@ProductId";
                            sqlParameter3.Value = OrdersProductIdTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter3);

                            SqlParameter sqlParameter4 = new SqlParameter();
                            sqlParameter4.SqlDbType = SqlDbType.Int;
                            sqlParameter4.ParameterName = "@DetailsofOrderId";
                            sqlParameter4.Value = OrdersDetailsofOrderIdTxt.Text;
                            UpdatesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter4);

                            UpdatesqlCommandbyStoredProcedure.ExecuteNonQuery();

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter(@"select * from Orders", connection);


                            RefreshTable();

                        }

                        if (MaxIdFromOrders <= Convert.ToInt32(OrdersIdTxt.Text))
                        {
                            MessageBox.Show("You change Customer that are not.");

                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message }");
                }

            }
        }