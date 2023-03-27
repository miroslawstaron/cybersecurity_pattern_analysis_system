		public Task<Int32> InsertCustomerCustomerDemoAsync( CustomerCustomerDemo entity )
		{
			return this.InsertCustomerCustomerDemoAsync
			(
				entity.CustomerID,
				entity.CustomerTypeID
			);
		}

		public async Task<Int32> InsertCustomerCustomerDemoAsync( String customerID, String customerTypeID )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.CustomerCustomerDemo (
	[CustomerID], [CustomerTypeID]
) VALUES (
	@customerID , @customerTypeID 
)
";				
				
				cmd.Parameters.Add( @"customerID"    , SqlDbType.NChar    ).Value = customerID    ;
				cmd.Parameters.Add( @"customerTypeID", SqlDbType.NChar    ).Value = customerTypeID;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}