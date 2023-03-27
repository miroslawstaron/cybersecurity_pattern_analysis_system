private void DeleteButton_Click(object sender, RoutedEventArgs e)
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
                        sqloutputParameter.Value = int.Parse(ProductIdTxt.Text);
                        sqloutputParameter.Direction = System.Data.ParameterDirection.Output;
                        MaxsqlCommandbyStoredProcedure.Parameters.Add(sqloutputParameter);

                        MaxsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        MaxIdFromProducs = Convert.ToInt32(sqloutputParameter.Value);

                        if (MaxIdFromProducs >= int.Parse(ProductIdTxt.Text))
                        {
                            SqlCommand DeletesqlCommandbyStoredProcedure = new SqlCommand(@"sp_deleteProduct", connection);
                            DeletesqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                            SqlParameter sqlParameter = new SqlParameter();
                            sqlParameter.SqlDbType = SqlDbType.Int;
                            sqlParameter.ParameterName = "@ProductId";
                            sqlParameter.Value = ProductIdTxt.Text;
                            DeletesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter);



                            DeletesqlCommandbyStoredProcedure.ExecuteNonQuery();

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter(@"select * from Products", connection);


                            RefreshTable();

                        }

                        if (MaxIdFromProducs <= Convert.ToInt32(ProductIdTxt.Text))
                        {
                            MessageBox.Show("You delete ProductId that are not or delete Last ProductId");

                        }
                    }

                    if (selecttablecombobox.SelectedIndex == 1)
                    {
                        SqlCommand MaxsqlCommandbyStoredProcedure = new SqlCommand(@"sp_MaxIdCustomer", connection);
                        MaxsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqloutputParameter = new SqlParameter();
                        sqloutputParameter.SqlDbType = SqlDbType.Int;
                        sqloutputParameter.ParameterName = "@MaxId";
                        sqloutputParameter.Value = int.Parse(CustomerIdTxt.Text);
                        sqloutputParameter.Direction = System.Data.ParameterDirection.Output;
                        MaxsqlCommandbyStoredProcedure.Parameters.Add(sqloutputParameter);

                        MaxsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        MaxIdFromCustomer = Convert.ToInt32(sqloutputParameter.Value);

                        if (MaxIdFromCustomer >= int.Parse(CustomerIdTxt.Text))
                        {
                            SqlCommand DeletesqlCommandbyStoredProcedure = new SqlCommand(@"sp_deleteCustomer", connection);
                            DeletesqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                            SqlParameter sqlParameter = new SqlParameter();
                            sqlParameter.SqlDbType = SqlDbType.Int;
                            sqlParameter.ParameterName = "@CustomerId";
                            sqlParameter.Value = CustomerIdTxt.Text;
                            DeletesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter);



                            DeletesqlCommandbyStoredProcedure.ExecuteNonQuery();

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter(@"select * from Customers", connection);


                            RefreshTable();

                        }

                        if (MaxIdFromCustomer <= Convert.ToInt32(CustomerIdTxt.Text))
                        {
                            MessageBox.Show("You delete CustomerId that are not or delete Last CustomerId");

                        }
                    }

                    if (selecttablecombobox.SelectedIndex == 2)
                    {
                        SqlCommand MaxsqlCommandbyStoredProcedure = new SqlCommand(@"sp_MaxIdDetailsofOrder", connection);
                        MaxsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqloutputParameter = new SqlParameter();
                        sqloutputParameter.SqlDbType = SqlDbType.Int;
                        sqloutputParameter.ParameterName = "@MaxId";
                        sqloutputParameter.Value = int.Parse(DetailsofOrderIdTxt.Text);
                        sqloutputParameter.Direction = System.Data.ParameterDirection.Output;
                        MaxsqlCommandbyStoredProcedure.Parameters.Add(sqloutputParameter);

                        MaxsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        MaxIdFromDetailsofOrder = Convert.ToInt32(sqloutputParameter.Value);

                        if (MaxIdFromDetailsofOrder >= int.Parse(DetailsofOrderIdTxt.Text))
                        {
                            SqlCommand DeletesqlCommandbyStoredProcedure = new SqlCommand(@"sp_deleteDetailsofOrder", connection);
                            DeletesqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                            SqlParameter sqlParameter = new SqlParameter();
                            sqlParameter.SqlDbType = SqlDbType.Int;
                            sqlParameter.ParameterName = "@DetailsofOrderId";
                            sqlParameter.Value = DetailsofOrderIdTxt.Text;
                            DeletesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter);



                            DeletesqlCommandbyStoredProcedure.ExecuteNonQuery();

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter(@"select * from DetailsofOrder", connection);


                            RefreshTable();

                        }

                        if (MaxIdFromDetailsofOrder <= Convert.ToInt32(DetailsofOrderIdTxt.Text))
                        {
                            MessageBox.Show("You delete DetailsofOrderId that are not or delete Last DetailsofOrderId");

                        }
                    }

                    if (selecttablecombobox.SelectedIndex == 3)
                    {
                        SqlCommand MaxsqlCommandbyStoredProcedure = new SqlCommand(@"sp_MaxIdOrder", connection);
                        MaxsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqloutputParameter = new SqlParameter();
                        sqloutputParameter.SqlDbType = SqlDbType.Int;
                        sqloutputParameter.ParameterName = "@MaxId";
                        sqloutputParameter.Value = int.Parse(OrdersIdTxt.Text);
                        sqloutputParameter.Direction = System.Data.ParameterDirection.Output;
                        MaxsqlCommandbyStoredProcedure.Parameters.Add(sqloutputParameter);

                        MaxsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        MaxIdFromOrders = Convert.ToInt32(sqloutputParameter.Value);

                        if (MaxIdFromOrders >= int.Parse(OrdersIdTxt.Text))
                        {
                            SqlCommand DeletesqlCommandbyStoredProcedure = new SqlCommand(@"sp_deleteOrder", connection);
                            DeletesqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                            SqlParameter sqlParameter = new SqlParameter();
                            sqlParameter.SqlDbType = SqlDbType.Int;
                            sqlParameter.ParameterName = "@OrderId";
                            sqlParameter.Value = OrdersIdTxt.Text;
                            DeletesqlCommandbyStoredProcedure.Parameters.Add(sqlParameter);



                            DeletesqlCommandbyStoredProcedure.ExecuteNonQuery();

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter(@"select * from Orders", connection);


                            RefreshTable();

                        }

                        if (MaxIdFromOrders <= Convert.ToInt32(OrdersIdTxt.Text))
                        {
                            MessageBox.Show("You delete OrderId that are not or delete Last OrderId");

                        }
                    }


                }
                catch (SqlException ex)
                {

                    MessageBox.Show($"{ex.Message}");
                }
            }
        }