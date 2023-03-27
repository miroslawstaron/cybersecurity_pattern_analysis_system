public async Task<Int32> InsertCustomerAsync( String customerID, String companyName, String contactName, String contactTitle, String address, String city, String region, String postalCode, String country, String phone, String fax )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Customers (
	[CustomerID], [CompanyName], [ContactName], [ContactTitle], [Address], [City], [Region], [PostalCode], [Country], [Phone], [Fax]
) VALUES (
	@customerID , @companyName , @contactName , @contactTitle , @address , @city , @region , @postalCode , @country , @phone , @fax 
)
";				
				
				cmd.Parameters.Add( @"customerID"  , SqlDbType.NChar    ).Value = customerID  ;
				cmd.Parameters.Add( @"companyName" , SqlDbType.NVarChar ).Value = companyName ;
				cmd.Parameters.Add( @"contactName" , SqlDbType.NVarChar ).Value = ( contactName  == null ) ? DBNull.Value : (Object)contactName;
				cmd.Parameters.Add( @"contactTitle", SqlDbType.NVarChar ).Value = ( contactTitle == null ) ? DBNull.Value : (Object)contactTitle;
				cmd.Parameters.Add( @"address"     , SqlDbType.NVarChar ).Value = ( address      == null ) ? DBNull.Value : (Object)address;
				cmd.Parameters.Add( @"city"        , SqlDbType.NVarChar ).Value = ( city         == null ) ? DBNull.Value : (Object)city;
				cmd.Parameters.Add( @"region"      , SqlDbType.NVarChar ).Value = ( region       == null ) ? DBNull.Value : (Object)region;
				cmd.Parameters.Add( @"postalCode"  , SqlDbType.NVarChar ).Value = ( postalCode   == null ) ? DBNull.Value : (Object)postalCode;
				cmd.Parameters.Add( @"country"     , SqlDbType.NVarChar ).Value = ( country      == null ) ? DBNull.Value : (Object)country;
				cmd.Parameters.Add( @"phone"       , SqlDbType.NVarChar ).Value = ( phone        == null ) ? DBNull.Value : (Object)phone;
				cmd.Parameters.Add( @"fax"         , SqlDbType.NVarChar ).Value = ( fax          == null ) ? DBNull.Value : (Object)fax;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}
