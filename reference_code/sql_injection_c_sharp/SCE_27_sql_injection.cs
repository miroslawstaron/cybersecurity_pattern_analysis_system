private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (selecttablecombobox.SelectedIndex == 0)
                {
                    using (connection = new SqlConnection())
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();

                        SqlCommand InsertsqlCommandbyStoredProcedure = new SqlCommand(@"sp_InsertProduct", connection);
                        InsertsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqlParameter1 = new SqlParameter();
                        sqlParameter1.SqlDbType = SqlDbType.NVarChar;
                        sqlParameter1.ParameterName = "@ProductName";
                        sqlParameter1.Value = ProductnameTxt.Text;
                        InsertsqlCommandbyStoredProcedure.Parameters.Add(sqlParameter1);

                        InsertsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        dataset = new DataSet();
                        sqlDataAdapter = new SqlDataAdapter(@"select * from Products", connection);



                        RefreshTable();
                    }
                }

                if (selecttablecombobox.SelectedIndex == 1)
                {
                    using (connection = new SqlConnection())
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();

                        SqlCommand InsertsqlCommandbyStoredProcedure = new SqlCommand(@"sp_InsertCustomer", connection);
                        InsertsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqlParameter1 = new SqlParameter();
                        sqlParameter1.SqlDbType = SqlDbType.NVarChar;
                        sqlParameter1.ParameterName = "@CustomerName";
                        sqlParameter1.Value = CustomernameTxt.Text;
                        InsertsqlCommandbyStoredProcedure.Parameters.Add(sqlParameter1);

                        InsertsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        dataset = new DataSet();
                        sqlDataAdapter = new SqlDataAdapter(@"select * from Customers", connection);



                        RefreshTable();
                    }
                }

                if (selecttablecombobox.SelectedIndex == 2)
                {
                    using (connection = new SqlConnection())
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        if (message == "Datetime is correct.")
                        {
                            if (DetailsofOrderIsCashTxt.Text == "True" || DetailsofOrderIsCashTxt.Text == "False" || DetailsofOrderIsCashTxt.Text == "true" || DetailsofOrderIsCashTxt.Text == "false")
                            {

                                SqlCommand InsertsqlCommandbyStoredProcedure = new SqlCommand(@"sp_InsertDetailsofOrder", connection);
                                InsertsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                                SqlParameter sqlParameter1 = new SqlParameter();
                                sqlParameter1.SqlDbType = SqlDbType.Bit;
                                sqlParameter1.ParameterName = "@iscash";
                                sqlParameter1.Value = DetailsofOrderIsCashTxt.Text;
                                InsertsqlCommandbyStoredProcedure.Parameters.Add(sqlParameter1);

                                SqlParameter sqlParameter2 = new SqlParameter();
                                sqlParameter2.SqlDbType = SqlDbType.DateTime;
                                sqlParameter2.ParameterName = "@dateorder";
                                sqlParameter2.Value = DetailsofOrderDateOrderTxt.Text;
                                InsertsqlCommandbyStoredProcedure.Parameters.Add(sqlParameter2);


                                InsertsqlCommandbyStoredProcedure.ExecuteNonQuery();

                                dataset = new DataSet();
                                sqlDataAdapter = new SqlDataAdapter(@"select * from DetailsofOrder", connection);



                                RefreshTable();
                            }
                            else
                            {
                                MessageBox.Show("erroneous input");
                            }
                        }

                    }
                }

                if (selecttablecombobox.SelectedIndex == 3)
                {
                    using (connection = new SqlConnection())
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();

                        SqlCommand InsertsqlCommandbyStoredProcedure = new SqlCommand(@"sp_InsertOrder", connection);
                        InsertsqlCommandbyStoredProcedure.CommandType = CommandType.StoredProcedure;

                        SqlParameter sqlParameter1 = new SqlParameter();
                        sqlParameter1.SqlDbType = SqlDbType.Int;
                        sqlParameter1.ParameterName = "@CustumerId";
                        sqlParameter1.Value = OrdersCustomerIdTxt.Text;
                        InsertsqlCommandbyStoredProcedure.Parameters.Add(sqlParameter1);

                        SqlParameter sqlParameter2 = new SqlParameter();
                        sqlParameter2.SqlDbType = SqlDbType.Int;
                        sqlParameter2.ParameterName = "@ProductId";
                        sqlParameter2.Value = OrdersProductIdTxt.Text;
                        InsertsqlCommandbyStoredProcedure.Parameters.Add(sqlParameter2);

                        SqlParameter sqlParameter3 = new SqlParameter();
                        sqlParameter3.SqlDbType = SqlDbType.Int;
                        sqlParameter3.ParameterName = "@DetailsofOrderId";
                        sqlParameter3.Value = OrdersDetailsofOrderIdTxt.Text;
                        InsertsqlCommandbyStoredProcedure.Parameters.Add(sqlParameter3);


                        InsertsqlCommandbyStoredProcedure.ExecuteNonQuery();

                        dataset = new DataSet();
                        sqlDataAdapter = new SqlDataAdapter(@"select * from Orders", connection);



                        RefreshTable();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"{ex.Message}");
            }
        }