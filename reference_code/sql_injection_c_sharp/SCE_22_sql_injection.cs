		public async Task<List<CustomerCustomerDemo>> QueryCustomerCustomerDemoAsync(MutableCustomerCustomerDemo whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	CustomerID,
	CustomerTypeID
FROM
	dbo.CustomerCustomerDemo
";

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder( selectSql );

				if( whereEquals != null )
				{
					sb.Append( "WHERE\r\n" );

					Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
					if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );
				}

				cmd.CommandText = sb.ToString();

				using( SqlDataReader rdr = await cmd.ExecuteReaderAsync().ConfigureAwait(false) )
				{
					List<CustomerCustomerDemo> list = new List<CustomerCustomerDemo>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						CustomerCustomerDemo row = CreateCustomerCustomerDemoEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}