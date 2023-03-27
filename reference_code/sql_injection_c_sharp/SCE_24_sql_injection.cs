using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MyNamespace
{
	public sealed partial class NorthwindDatabase : IDisposable
	{
		// Generatd 2018-11-18T05:28:17.6109929Z UTC
		// Query time: 56ms

		private readonly SqlConnection c;

		private NorthwindDatabase(SqlConnection c)
		{
			this.c = c;
		}

		public void Dispose()
		{
			this.c.Dispose();
		}

		public static async Task<NorthwindDatabase> CreateAsync(String connectionString)
		{
			SqlConnection con = new SqlConnection( connectionString );
			try
			{
				await con.OpenAsync().ConfigureAwait(false);
				return new NorthwindDatabase( con );
			}
			catch
			{
				con.Dispose();
				throw;
			}
		}

	#region Table: [Categories]

		public Task<Int32> InsertCategoryAsync( Category entity )
		{
			return this.InsertCategoryAsync
			(
				entity.CategoryName,
				entity.Description,
				entity.Picture
			);
		}

		public async Task<Int32> InsertCategoryAsync( String categoryName, String description, Byte[] picture )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Categories (
	[CategoryName], [Description], [Picture]
) VALUES (
	@categoryName , @description , @picture 
)
";				
				
				cmd.Parameters.Add( @"categoryName", SqlDbType.NVarChar ).Value = categoryName;
				cmd.Parameters.Add( @"description" , SqlDbType.NText    ).Value = ( description  == null ) ? DBNull.Value : (Object)description;
				cmd.Parameters.Add( @"picture"     , SqlDbType.Image    ).Value = ( picture      == null ) ? DBNull.Value : (Object)picture;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateCategoryAsync(MutableCategory entity, MutableCategory whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Categories
SET
");
				Int32 setColumnsCount = 0;
				if( entity.CategoryNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "CategoryName", "@categoryName", SqlDbType.NVarChar, entity.CategoryName );

				if( entity.DescriptionIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Description", "@description", SqlDbType.NText, entity.Description );

				if( entity.PictureIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Picture", "@picture", SqlDbType.Image, entity.Picture );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Category>> QueryCategoryAsync(MutableCategory whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	CategoryID,
	CategoryName,
	Description,
	Picture
FROM
	dbo.Categories
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
					List<Category> list = new List<Category>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Category row = CreateCategoryEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableCategory whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.CategoryIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CategoryID", "@where_categoryID", SqlDbType.Int, whereEquals.CategoryID );

			if( whereEquals.CategoryNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CategoryName", "@where_categoryName", SqlDbType.NVarChar, whereEquals.CategoryName );

			if( whereEquals.DescriptionIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Description", "@where_description", SqlDbType.NText, whereEquals.Description );

			if( whereEquals.PictureIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Picture", "@where_picture", SqlDbType.Image, whereEquals.Picture );

			return c;
		}

	#endregion

	#region Table: [CustomerCustomerDemo]

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

		public async Task<Int32> UpdateCustomerCustomerDemoAsync(MutableCustomerCustomerDemo entity, MutableCustomerCustomerDemo whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.CustomerCustomerDemo
SET
");
				Int32 setColumnsCount = 0;

				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableCustomerCustomerDemo whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.CustomerIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CustomerID", "@where_customerID", SqlDbType.NChar, whereEquals.CustomerID );

			if( whereEquals.CustomerTypeIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CustomerTypeID", "@where_customerTypeID", SqlDbType.NChar, whereEquals.CustomerTypeID );

			return c;
		}

	#endregion

	#region Table: [CustomerDemographics]

		public Task<Int32> InsertCustomerDemographicAsync( CustomerDemographic entity )
		{
			return this.InsertCustomerDemographicAsync
			(
				entity.CustomerTypeID,
				entity.CustomerDesc
			);
		}

		public async Task<Int32> UpdateCustomerDemographicAsync(MutableCustomerDemographic entity, MutableCustomerDemographic whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.CustomerDemographics
SET
");
				Int32 setColumnsCount = 0;
				if( entity.CustomerDescIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "CustomerDesc", "@customerDesc", SqlDbType.NText, entity.CustomerDesc );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<CustomerDemographic>> QueryCustomerDemographicAsync(MutableCustomerDemographic whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	CustomerTypeID,
	CustomerDesc
FROM
	dbo.CustomerDemographics
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
					List<CustomerDemographic> list = new List<CustomerDemographic>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						CustomerDemographic row = CreateCustomerDemographicEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableCustomerDemographic whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.CustomerTypeIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CustomerTypeID", "@where_customerTypeID", SqlDbType.NChar, whereEquals.CustomerTypeID );

			if( whereEquals.CustomerDescIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CustomerDesc", "@where_customerDesc", SqlDbType.NText, whereEquals.CustomerDesc );

			return c;
		}

	#endregion

	#region Table: [Customers]

		public Task<Int32> InsertCustomerAsync( Customer entity )
		{
			return this.InsertCustomerAsync
			(
				entity.CustomerID,
				entity.CompanyName,
				entity.ContactName,
				entity.ContactTitle,
				entity.Address,
				entity.City,
				entity.Region,
				entity.PostalCode,
				entity.Country,
				entity.Phone,
				entity.Fax
			);
		}

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

		public async Task<Int32> UpdateCustomerAsync(MutableCustomer entity, MutableCustomer whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Customers
SET
");
				Int32 setColumnsCount = 0;
				if( entity.CompanyNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "CompanyName", "@companyName", SqlDbType.NVarChar, entity.CompanyName );

				if( entity.ContactNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ContactName", "@contactName", SqlDbType.NVarChar, entity.ContactName );

				if( entity.ContactTitleIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ContactTitle", "@contactTitle", SqlDbType.NVarChar, entity.ContactTitle );

				if( entity.AddressIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Address", "@address", SqlDbType.NVarChar, entity.Address );

				if( entity.CityIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "City", "@city", SqlDbType.NVarChar, entity.City );

				if( entity.RegionIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Region", "@region", SqlDbType.NVarChar, entity.Region );

				if( entity.PostalCodeIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "PostalCode", "@postalCode", SqlDbType.NVarChar, entity.PostalCode );

				if( entity.CountryIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Country", "@country", SqlDbType.NVarChar, entity.Country );

				if( entity.PhoneIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Phone", "@phone", SqlDbType.NVarChar, entity.Phone );

				if( entity.FaxIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Fax", "@fax", SqlDbType.NVarChar, entity.Fax );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Customer>> QueryCustomerAsync(MutableCustomer whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	CustomerID,
	CompanyName,
	ContactName,
	ContactTitle,
	Address,
	City,
	Region,
	PostalCode,
	Country,
	Phone,
	Fax
FROM
	dbo.Customers
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
					List<Customer> list = new List<Customer>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Customer row = CreateCustomerEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableCustomer whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.CustomerIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CustomerID", "@where_customerID", SqlDbType.NChar, whereEquals.CustomerID );

			if( whereEquals.CompanyNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CompanyName", "@where_companyName", SqlDbType.NVarChar, whereEquals.CompanyName );

			if( whereEquals.ContactNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ContactName", "@where_contactName", SqlDbType.NVarChar, whereEquals.ContactName );

			if( whereEquals.ContactTitleIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ContactTitle", "@where_contactTitle", SqlDbType.NVarChar, whereEquals.ContactTitle );

			if( whereEquals.AddressIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Address", "@where_address", SqlDbType.NVarChar, whereEquals.Address );

			if( whereEquals.CityIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "City", "@where_city", SqlDbType.NVarChar, whereEquals.City );

			if( whereEquals.RegionIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Region", "@where_region", SqlDbType.NVarChar, whereEquals.Region );

			if( whereEquals.PostalCodeIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "PostalCode", "@where_postalCode", SqlDbType.NVarChar, whereEquals.PostalCode );

			if( whereEquals.CountryIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Country", "@where_country", SqlDbType.NVarChar, whereEquals.Country );

			if( whereEquals.PhoneIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Phone", "@where_phone", SqlDbType.NVarChar, whereEquals.Phone );

			if( whereEquals.FaxIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Fax", "@where_fax", SqlDbType.NVarChar, whereEquals.Fax );

			return c;
		}

	#endregion

	#region Table: [Employees]

		public Task<Int32> InsertEmployeeAsync( Employee entity )
		{
			return this.InsertEmployeeAsync
			(
				entity.LastName,
				entity.FirstName,
				entity.Title,
				entity.TitleOfCourtesy,
				entity.BirthDate,
				entity.HireDate,
				entity.Address,
				entity.City,
				entity.Region,
				entity.PostalCode,
				entity.Country,
				entity.HomePhone,
				entity.Extension,
				entity.Photo,
				entity.Notes,
				entity.ReportsTo,
				entity.PhotoPath
			);
		}

		public async Task<Int32> InsertEmployeeAsync( String lastName, String firstName, String title, String titleOfCourtesy, DateTime? birthDate, DateTime? hireDate, String address, String city, String region, String postalCode, String country, String homePhone, String extension, Byte[] photo, String notes, Int32? reportsTo, String photoPath )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Employees (
	[LastName], [FirstName], [Title], [TitleOfCourtesy], [BirthDate], [HireDate], [Address], [City], [Region], [PostalCode], [Country], [HomePhone], [Extension], [Photo], [Notes], [ReportsTo], [PhotoPath]
) VALUES (
	@lastName , @firstName , @title , @titleOfCourtesy , @birthDate , @hireDate , @address , @city , @region , @postalCode , @country , @homePhone , @extension , @photo , @notes , @reportsTo , @photoPath 
)
";				
				
				cmd.Parameters.Add( @"lastName"       , SqlDbType.NVarChar ).Value = lastName       ;
				cmd.Parameters.Add( @"firstName"      , SqlDbType.NVarChar ).Value = firstName      ;
				cmd.Parameters.Add( @"title"          , SqlDbType.NVarChar ).Value = ( title           == null ) ? DBNull.Value : (Object)title;
				cmd.Parameters.Add( @"titleOfCourtesy", SqlDbType.NVarChar ).Value = ( titleOfCourtesy == null ) ? DBNull.Value : (Object)titleOfCourtesy;
				cmd.Parameters.Add( @"birthDate"      , SqlDbType.DateTime ).Value = ( birthDate       == null ) ? DBNull.Value : (Object)birthDate.Value;
				cmd.Parameters.Add( @"hireDate"       , SqlDbType.DateTime ).Value = ( hireDate        == null ) ? DBNull.Value : (Object)hireDate.Value;
				cmd.Parameters.Add( @"address"        , SqlDbType.NVarChar ).Value = ( address         == null ) ? DBNull.Value : (Object)address;
				cmd.Parameters.Add( @"city"           , SqlDbType.NVarChar ).Value = ( city            == null ) ? DBNull.Value : (Object)city;
				cmd.Parameters.Add( @"region"         , SqlDbType.NVarChar ).Value = ( region          == null ) ? DBNull.Value : (Object)region;
				cmd.Parameters.Add( @"postalCode"     , SqlDbType.NVarChar ).Value = ( postalCode      == null ) ? DBNull.Value : (Object)postalCode;
				cmd.Parameters.Add( @"country"        , SqlDbType.NVarChar ).Value = ( country         == null ) ? DBNull.Value : (Object)country;
				cmd.Parameters.Add( @"homePhone"      , SqlDbType.NVarChar ).Value = ( homePhone       == null ) ? DBNull.Value : (Object)homePhone;
				cmd.Parameters.Add( @"extension"      , SqlDbType.NVarChar ).Value = ( extension       == null ) ? DBNull.Value : (Object)extension;
				cmd.Parameters.Add( @"photo"          , SqlDbType.Image    ).Value = ( photo           == null ) ? DBNull.Value : (Object)photo;
				cmd.Parameters.Add( @"notes"          , SqlDbType.NText    ).Value = ( notes           == null ) ? DBNull.Value : (Object)notes;
				cmd.Parameters.Add( @"reportsTo"      , SqlDbType.Int      ).Value = ( reportsTo       == null ) ? DBNull.Value : (Object)reportsTo.Value;
				cmd.Parameters.Add( @"photoPath"      , SqlDbType.NVarChar ).Value = ( photoPath       == null ) ? DBNull.Value : (Object)photoPath;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateEmployeeAsync(MutableEmployee entity, MutableEmployee whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Employees
SET
");
				Int32 setColumnsCount = 0;
				if( entity.LastNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "LastName", "@lastName", SqlDbType.NVarChar, entity.LastName );

				if( entity.FirstNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "FirstName", "@firstName", SqlDbType.NVarChar, entity.FirstName );

				if( entity.TitleIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Title", "@title", SqlDbType.NVarChar, entity.Title );

				if( entity.TitleOfCourtesyIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "TitleOfCourtesy", "@titleOfCourtesy", SqlDbType.NVarChar, entity.TitleOfCourtesy );

				if( entity.BirthDateIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "BirthDate", "@birthDate", SqlDbType.DateTime, entity.BirthDate );

				if( entity.HireDateIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "HireDate", "@hireDate", SqlDbType.DateTime, entity.HireDate );

				if( entity.AddressIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Address", "@address", SqlDbType.NVarChar, entity.Address );

				if( entity.CityIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "City", "@city", SqlDbType.NVarChar, entity.City );

				if( entity.RegionIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Region", "@region", SqlDbType.NVarChar, entity.Region );

				if( entity.PostalCodeIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "PostalCode", "@postalCode", SqlDbType.NVarChar, entity.PostalCode );

				if( entity.CountryIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Country", "@country", SqlDbType.NVarChar, entity.Country );

				if( entity.HomePhoneIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "HomePhone", "@homePhone", SqlDbType.NVarChar, entity.HomePhone );

				if( entity.ExtensionIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Extension", "@extension", SqlDbType.NVarChar, entity.Extension );

				if( entity.PhotoIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Photo", "@photo", SqlDbType.Image, entity.Photo );

				if( entity.NotesIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Notes", "@notes", SqlDbType.NText, entity.Notes );

				if( entity.ReportsToIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ReportsTo", "@reportsTo", SqlDbType.Int, entity.ReportsTo );

				if( entity.PhotoPathIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "PhotoPath", "@photoPath", SqlDbType.NVarChar, entity.PhotoPath );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Employee>> QueryEmployeeAsync(MutableEmployee whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	EmployeeID,
	LastName,
	FirstName,
	Title,
	TitleOfCourtesy,
	BirthDate,
	HireDate,
	Address,
	City,
	Region,
	PostalCode,
	Country,
	HomePhone,
	Extension,
	Photo,
	Notes,
	ReportsTo,
	PhotoPath
FROM
	dbo.Employees
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
					List<Employee> list = new List<Employee>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Employee row = CreateEmployeeEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableEmployee whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.EmployeeIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "EmployeeID", "@where_employeeID", SqlDbType.Int, whereEquals.EmployeeID );

			if( whereEquals.LastNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "LastName", "@where_lastName", SqlDbType.NVarChar, whereEquals.LastName );

			if( whereEquals.FirstNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "FirstName", "@where_firstName", SqlDbType.NVarChar, whereEquals.FirstName );

			if( whereEquals.TitleIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Title", "@where_title", SqlDbType.NVarChar, whereEquals.Title );

			if( whereEquals.TitleOfCourtesyIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "TitleOfCourtesy", "@where_titleOfCourtesy", SqlDbType.NVarChar, whereEquals.TitleOfCourtesy );

			if( whereEquals.BirthDateIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "BirthDate", "@where_birthDate", SqlDbType.DateTime, whereEquals.BirthDate );

			if( whereEquals.HireDateIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "HireDate", "@where_hireDate", SqlDbType.DateTime, whereEquals.HireDate );

			if( whereEquals.AddressIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Address", "@where_address", SqlDbType.NVarChar, whereEquals.Address );

			if( whereEquals.CityIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "City", "@where_city", SqlDbType.NVarChar, whereEquals.City );

			if( whereEquals.RegionIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Region", "@where_region", SqlDbType.NVarChar, whereEquals.Region );

			if( whereEquals.PostalCodeIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "PostalCode", "@where_postalCode", SqlDbType.NVarChar, whereEquals.PostalCode );

			if( whereEquals.CountryIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Country", "@where_country", SqlDbType.NVarChar, whereEquals.Country );

			if( whereEquals.HomePhoneIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "HomePhone", "@where_homePhone", SqlDbType.NVarChar, whereEquals.HomePhone );

			if( whereEquals.ExtensionIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Extension", "@where_extension", SqlDbType.NVarChar, whereEquals.Extension );

			if( whereEquals.PhotoIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Photo", "@where_photo", SqlDbType.Image, whereEquals.Photo );

			if( whereEquals.NotesIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Notes", "@where_notes", SqlDbType.NText, whereEquals.Notes );

			if( whereEquals.ReportsToIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ReportsTo", "@where_reportsTo", SqlDbType.Int, whereEquals.ReportsTo );

			if( whereEquals.PhotoPathIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "PhotoPath", "@where_photoPath", SqlDbType.NVarChar, whereEquals.PhotoPath );

			return c;
		}

	#endregion

	#region Table: [EmployeeTerritories]

		public Task<Int32> InsertEmployeeTerritoryAsync( EmployeeTerritory entity )
		{
			return this.InsertEmployeeTerritoryAsync
			(
				entity.EmployeeID,
				entity.TerritoryID
			);
		}

		public async Task<Int32> InsertEmployeeTerritoryAsync( Int32 employeeID, String territoryID )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.EmployeeTerritories (
	[EmployeeID], [TerritoryID]
) VALUES (
	@employeeID , @territoryID 
)
";				
				
				cmd.Parameters.Add( @"employeeID" , SqlDbType.Int      ).Value = employeeID ;
				cmd.Parameters.Add( @"territoryID", SqlDbType.NVarChar ).Value = territoryID;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateEmployeeTerritoryAsync(MutableEmployeeTerritory entity, MutableEmployeeTerritory whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.EmployeeTerritories
SET
");
				Int32 setColumnsCount = 0;

				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<EmployeeTerritory>> QueryEmployeeTerritoryAsync(MutableEmployeeTerritory whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	EmployeeID,
	TerritoryID
FROM
	dbo.EmployeeTerritories
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
					List<EmployeeTerritory> list = new List<EmployeeTerritory>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						EmployeeTerritory row = CreateEmployeeTerritoryEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableEmployeeTerritory whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.EmployeeIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "EmployeeID", "@where_employeeID", SqlDbType.Int, whereEquals.EmployeeID );

			if( whereEquals.TerritoryIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "TerritoryID", "@where_territoryID", SqlDbType.NVarChar, whereEquals.TerritoryID );

			return c;
		}

	#endregion

	#region Table: [Order Details]

		public Task<Int32> InsertOrderDetailAsync( OrderDetail entity )
		{
			return this.InsertOrderDetailAsync
			(
				entity.OrderID,
				entity.ProductID,
				entity.UnitPrice,
				entity.Quantity,
				entity.Discount
			);
		}

		public async Task<Int32> InsertOrderDetailAsync( Int32 orderID, Int32 productID, Decimal unitPrice, Int16 quantity, Double discount )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Order Details (
	[OrderID], [ProductID], [UnitPrice], [Quantity], [Discount]
) VALUES (
	@orderID , @productID , @unitPrice , @quantity , @discount 
)
";				
				
				cmd.Parameters.Add( @"orderID"  , SqlDbType.Int      ).Value = orderID  ;
				cmd.Parameters.Add( @"productID", SqlDbType.Int      ).Value = productID;
				cmd.Parameters.Add( @"unitPrice", SqlDbType.Money    ).Value = unitPrice;
				cmd.Parameters.Add( @"quantity" , SqlDbType.SmallInt ).Value = quantity ;
				cmd.Parameters.Add( @"discount" , SqlDbType.Real     ).Value = discount ;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateOrderDetailAsync(MutableOrderDetail entity, MutableOrderDetail whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Order Details
SET
");
				Int32 setColumnsCount = 0;
				if( entity.UnitPriceIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "UnitPrice", "@unitPrice", SqlDbType.Money, entity.UnitPrice );

				if( entity.QuantityIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Quantity", "@quantity", SqlDbType.SmallInt, entity.Quantity );

				if( entity.DiscountIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Discount", "@discount", SqlDbType.Real, entity.Discount );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<OrderDetail>> QueryOrderDetailAsync(MutableOrderDetail whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	OrderID,
	ProductID,
	UnitPrice,
	Quantity,
	Discount
FROM
	dbo.Order Details
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
					List<OrderDetail> list = new List<OrderDetail>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						OrderDetail row = CreateOrderDetailEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableOrderDetail whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.OrderIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "OrderID", "@where_orderID", SqlDbType.Int, whereEquals.OrderID );

			if( whereEquals.ProductIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ProductID", "@where_productID", SqlDbType.Int, whereEquals.ProductID );

			if( whereEquals.UnitPriceIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "UnitPrice", "@where_unitPrice", SqlDbType.Money, whereEquals.UnitPrice );

			if( whereEquals.QuantityIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Quantity", "@where_quantity", SqlDbType.SmallInt, whereEquals.Quantity );

			if( whereEquals.DiscountIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Discount", "@where_discount", SqlDbType.Real, whereEquals.Discount );

			return c;
		}

	#endregion

	#region Table: [Orders]

		public Task<Int32> InsertOrderAsync( Order entity )
		{
			return this.InsertOrderAsync
			(
				entity.CustomerID,
				entity.EmployeeID,
				entity.OrderDate,
				entity.RequiredDate,
				entity.ShippedDate,
				entity.ShipVia,
				entity.Freight,
				entity.ShipName,
				entity.ShipAddress,
				entity.ShipCity,
				entity.ShipRegion,
				entity.ShipPostalCode,
				entity.ShipCountry
			);
		}

		public async Task<Int32> InsertOrderAsync( String customerID, Int32? employeeID, DateTime? orderDate, DateTime? requiredDate, DateTime? shippedDate, Int32? shipVia, Decimal? freight, String shipName, String shipAddress, String shipCity, String shipRegion, String shipPostalCode, String shipCountry )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Orders (
	[CustomerID], [EmployeeID], [OrderDate], [RequiredDate], [ShippedDate], [ShipVia], [Freight], [ShipName], [ShipAddress], [ShipCity], [ShipRegion], [ShipPostalCode], [ShipCountry]
) VALUES (
	@customerID , @employeeID , @orderDate , @requiredDate , @shippedDate , @shipVia , @freight , @shipName , @shipAddress , @shipCity , @shipRegion , @shipPostalCode , @shipCountry 
)
";				
				
				cmd.Parameters.Add( @"customerID"    , SqlDbType.NChar    ).Value = ( customerID     == null ) ? DBNull.Value : (Object)customerID;
				cmd.Parameters.Add( @"employeeID"    , SqlDbType.Int      ).Value = ( employeeID     == null ) ? DBNull.Value : (Object)employeeID.Value;
				cmd.Parameters.Add( @"orderDate"     , SqlDbType.DateTime ).Value = ( orderDate      == null ) ? DBNull.Value : (Object)orderDate.Value;
				cmd.Parameters.Add( @"requiredDate"  , SqlDbType.DateTime ).Value = ( requiredDate   == null ) ? DBNull.Value : (Object)requiredDate.Value;
				cmd.Parameters.Add( @"shippedDate"   , SqlDbType.DateTime ).Value = ( shippedDate    == null ) ? DBNull.Value : (Object)shippedDate.Value;
				cmd.Parameters.Add( @"shipVia"       , SqlDbType.Int      ).Value = ( shipVia        == null ) ? DBNull.Value : (Object)shipVia.Value;
				cmd.Parameters.Add( @"freight"       , SqlDbType.Money    ).Value = ( freight        == null ) ? DBNull.Value : (Object)freight.Value;
				cmd.Parameters.Add( @"shipName"      , SqlDbType.NVarChar ).Value = ( shipName       == null ) ? DBNull.Value : (Object)shipName;
				cmd.Parameters.Add( @"shipAddress"   , SqlDbType.NVarChar ).Value = ( shipAddress    == null ) ? DBNull.Value : (Object)shipAddress;
				cmd.Parameters.Add( @"shipCity"      , SqlDbType.NVarChar ).Value = ( shipCity       == null ) ? DBNull.Value : (Object)shipCity;
				cmd.Parameters.Add( @"shipRegion"    , SqlDbType.NVarChar ).Value = ( shipRegion     == null ) ? DBNull.Value : (Object)shipRegion;
				cmd.Parameters.Add( @"shipPostalCode", SqlDbType.NVarChar ).Value = ( shipPostalCode == null ) ? DBNull.Value : (Object)shipPostalCode;
				cmd.Parameters.Add( @"shipCountry"   , SqlDbType.NVarChar ).Value = ( shipCountry    == null ) ? DBNull.Value : (Object)shipCountry;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateOrderAsync(MutableOrder entity, MutableOrder whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Orders
SET
");
				Int32 setColumnsCount = 0;
				if( entity.CustomerIDIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "CustomerID", "@customerID", SqlDbType.NChar, entity.CustomerID );

				if( entity.EmployeeIDIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "EmployeeID", "@employeeID", SqlDbType.Int, entity.EmployeeID );

				if( entity.OrderDateIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "OrderDate", "@orderDate", SqlDbType.DateTime, entity.OrderDate );

				if( entity.RequiredDateIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "RequiredDate", "@requiredDate", SqlDbType.DateTime, entity.RequiredDate );

				if( entity.ShippedDateIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ShippedDate", "@shippedDate", SqlDbType.DateTime, entity.ShippedDate );

				if( entity.ShipViaIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ShipVia", "@shipVia", SqlDbType.Int, entity.ShipVia );

				if( entity.FreightIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Freight", "@freight", SqlDbType.Money, entity.Freight );

				if( entity.ShipNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ShipName", "@shipName", SqlDbType.NVarChar, entity.ShipName );

				if( entity.ShipAddressIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ShipAddress", "@shipAddress", SqlDbType.NVarChar, entity.ShipAddress );

				if( entity.ShipCityIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ShipCity", "@shipCity", SqlDbType.NVarChar, entity.ShipCity );

				if( entity.ShipRegionIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ShipRegion", "@shipRegion", SqlDbType.NVarChar, entity.ShipRegion );

				if( entity.ShipPostalCodeIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ShipPostalCode", "@shipPostalCode", SqlDbType.NVarChar, entity.ShipPostalCode );

				if( entity.ShipCountryIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ShipCountry", "@shipCountry", SqlDbType.NVarChar, entity.ShipCountry );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Order>> QueryOrderAsync(MutableOrder whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	OrderID,
	CustomerID,
	EmployeeID,
	OrderDate,
	RequiredDate,
	ShippedDate,
	ShipVia,
	Freight,
	ShipName,
	ShipAddress,
	ShipCity,
	ShipRegion,
	ShipPostalCode,
	ShipCountry
FROM
	dbo.Orders
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
					List<Order> list = new List<Order>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Order row = CreateOrderEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableOrder whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.OrderIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "OrderID", "@where_orderID", SqlDbType.Int, whereEquals.OrderID );

			if( whereEquals.CustomerIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CustomerID", "@where_customerID", SqlDbType.NChar, whereEquals.CustomerID );

			if( whereEquals.EmployeeIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "EmployeeID", "@where_employeeID", SqlDbType.Int, whereEquals.EmployeeID );

			if( whereEquals.OrderDateIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "OrderDate", "@where_orderDate", SqlDbType.DateTime, whereEquals.OrderDate );

			if( whereEquals.RequiredDateIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "RequiredDate", "@where_requiredDate", SqlDbType.DateTime, whereEquals.RequiredDate );

			if( whereEquals.ShippedDateIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShippedDate", "@where_shippedDate", SqlDbType.DateTime, whereEquals.ShippedDate );

			if( whereEquals.ShipViaIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShipVia", "@where_shipVia", SqlDbType.Int, whereEquals.ShipVia );

			if( whereEquals.FreightIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Freight", "@where_freight", SqlDbType.Money, whereEquals.Freight );

			if( whereEquals.ShipNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShipName", "@where_shipName", SqlDbType.NVarChar, whereEquals.ShipName );

			if( whereEquals.ShipAddressIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShipAddress", "@where_shipAddress", SqlDbType.NVarChar, whereEquals.ShipAddress );

			if( whereEquals.ShipCityIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShipCity", "@where_shipCity", SqlDbType.NVarChar, whereEquals.ShipCity );

			if( whereEquals.ShipRegionIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShipRegion", "@where_shipRegion", SqlDbType.NVarChar, whereEquals.ShipRegion );

			if( whereEquals.ShipPostalCodeIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShipPostalCode", "@where_shipPostalCode", SqlDbType.NVarChar, whereEquals.ShipPostalCode );

			if( whereEquals.ShipCountryIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShipCountry", "@where_shipCountry", SqlDbType.NVarChar, whereEquals.ShipCountry );

			return c;
		}

	#endregion

	#region Table: [Products]

		public Task<Int32> InsertProductAsync( Product entity )
		{
			return this.InsertProductAsync
			(
				entity.ProductName,
				entity.SupplierID,
				entity.CategoryID,
				entity.QuantityPerUnit,
				entity.UnitPrice,
				entity.UnitsInStock,
				entity.UnitsOnOrder,
				entity.ReorderLevel,
				entity.Discontinued
			);
		}

		public async Task<Int32> InsertProductAsync( String productName, Int32? supplierID, Int32? categoryID, String quantityPerUnit, Decimal? unitPrice, Int16? unitsInStock, Int16? unitsOnOrder, Int16? reorderLevel, Boolean discontinued )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Products (
	[ProductName], [SupplierID], [CategoryID], [QuantityPerUnit], [UnitPrice], [UnitsInStock], [UnitsOnOrder], [ReorderLevel], [Discontinued]
) VALUES (
	@productName , @supplierID , @categoryID , @quantityPerUnit , @unitPrice , @unitsInStock , @unitsOnOrder , @reorderLevel , @discontinued 
)
";				
				
				cmd.Parameters.Add( @"productName"    , SqlDbType.NVarChar ).Value = productName    ;
				cmd.Parameters.Add( @"supplierID"     , SqlDbType.Int      ).Value = ( supplierID      == null ) ? DBNull.Value : (Object)supplierID.Value;
				cmd.Parameters.Add( @"categoryID"     , SqlDbType.Int      ).Value = ( categoryID      == null ) ? DBNull.Value : (Object)categoryID.Value;
				cmd.Parameters.Add( @"quantityPerUnit", SqlDbType.NVarChar ).Value = ( quantityPerUnit == null ) ? DBNull.Value : (Object)quantityPerUnit;
				cmd.Parameters.Add( @"unitPrice"      , SqlDbType.Money    ).Value = ( unitPrice       == null ) ? DBNull.Value : (Object)unitPrice.Value;
				cmd.Parameters.Add( @"unitsInStock"   , SqlDbType.SmallInt ).Value = ( unitsInStock    == null ) ? DBNull.Value : (Object)unitsInStock.Value;
				cmd.Parameters.Add( @"unitsOnOrder"   , SqlDbType.SmallInt ).Value = ( unitsOnOrder    == null ) ? DBNull.Value : (Object)unitsOnOrder.Value;
				cmd.Parameters.Add( @"reorderLevel"   , SqlDbType.SmallInt ).Value = ( reorderLevel    == null ) ? DBNull.Value : (Object)reorderLevel.Value;
				cmd.Parameters.Add( @"discontinued"   , SqlDbType.Bit      ).Value = discontinued   ;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateProductAsync(MutableProduct entity, MutableProduct whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Products
SET
");
				Int32 setColumnsCount = 0;
				if( entity.ProductNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ProductName", "@productName", SqlDbType.NVarChar, entity.ProductName );

				if( entity.SupplierIDIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "SupplierID", "@supplierID", SqlDbType.Int, entity.SupplierID );

				if( entity.CategoryIDIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "CategoryID", "@categoryID", SqlDbType.Int, entity.CategoryID );

				if( entity.QuantityPerUnitIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "QuantityPerUnit", "@quantityPerUnit", SqlDbType.NVarChar, entity.QuantityPerUnit );

				if( entity.UnitPriceIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "UnitPrice", "@unitPrice", SqlDbType.Money, entity.UnitPrice );

				if( entity.UnitsInStockIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "UnitsInStock", "@unitsInStock", SqlDbType.SmallInt, entity.UnitsInStock );

				if( entity.UnitsOnOrderIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "UnitsOnOrder", "@unitsOnOrder", SqlDbType.SmallInt, entity.UnitsOnOrder );

				if( entity.ReorderLevelIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ReorderLevel", "@reorderLevel", SqlDbType.SmallInt, entity.ReorderLevel );

				if( entity.DiscontinuedIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Discontinued", "@discontinued", SqlDbType.Bit, entity.Discontinued );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Product>> QueryProductAsync(MutableProduct whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	ProductID,
	ProductName,
	SupplierID,
	CategoryID,
	QuantityPerUnit,
	UnitPrice,
	UnitsInStock,
	UnitsOnOrder,
	ReorderLevel,
	Discontinued
FROM
	dbo.Products
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
					List<Product> list = new List<Product>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Product row = CreateProductEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableProduct whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.ProductIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ProductID", "@where_productID", SqlDbType.Int, whereEquals.ProductID );

			if( whereEquals.ProductNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ProductName", "@where_productName", SqlDbType.NVarChar, whereEquals.ProductName );

			if( whereEquals.SupplierIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "SupplierID", "@where_supplierID", SqlDbType.Int, whereEquals.SupplierID );

			if( whereEquals.CategoryIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CategoryID", "@where_categoryID", SqlDbType.Int, whereEquals.CategoryID );

			if( whereEquals.QuantityPerUnitIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "QuantityPerUnit", "@where_quantityPerUnit", SqlDbType.NVarChar, whereEquals.QuantityPerUnit );

			if( whereEquals.UnitPriceIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "UnitPrice", "@where_unitPrice", SqlDbType.Money, whereEquals.UnitPrice );

			if( whereEquals.UnitsInStockIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "UnitsInStock", "@where_unitsInStock", SqlDbType.SmallInt, whereEquals.UnitsInStock );

			if( whereEquals.UnitsOnOrderIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "UnitsOnOrder", "@where_unitsOnOrder", SqlDbType.SmallInt, whereEquals.UnitsOnOrder );

			if( whereEquals.ReorderLevelIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ReorderLevel", "@where_reorderLevel", SqlDbType.SmallInt, whereEquals.ReorderLevel );

			if( whereEquals.DiscontinuedIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Discontinued", "@where_discontinued", SqlDbType.Bit, whereEquals.Discontinued );

			return c;
		}

	#endregion

	#region Table: [Region]

		public Task<Int32> InsertRegionAsync( Region entity )
		{
			return this.InsertRegionAsync
			(
				entity.RegionID,
				entity.RegionDescription
			);
		}

		public async Task<Int32> InsertRegionAsync( Int32 regionID, String regionDescription )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Region (
	[RegionID], [RegionDescription]
) VALUES (
	@regionID , @regionDescription 
)
";				
				
				cmd.Parameters.Add( @"regionID"         , SqlDbType.Int      ).Value = regionID         ;
				cmd.Parameters.Add( @"regionDescription", SqlDbType.NChar    ).Value = regionDescription;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateRegionAsync(MutableRegion entity, MutableRegion whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Region
SET
");
				Int32 setColumnsCount = 0;
				if( entity.RegionDescriptionIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "RegionDescription", "@regionDescription", SqlDbType.NChar, entity.RegionDescription );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Region>> QueryRegionAsync(MutableRegion whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	RegionID,
	RegionDescription
FROM
	dbo.Region
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
					List<Region> list = new List<Region>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Region row = CreateRegionEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableRegion whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.RegionIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "RegionID", "@where_regionID", SqlDbType.Int, whereEquals.RegionID );

			if( whereEquals.RegionDescriptionIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "RegionDescription", "@where_regionDescription", SqlDbType.NChar, whereEquals.RegionDescription );

			return c;
		}

	#endregion

	#region Table: [Shippers]

		public Task<Int32> InsertShipperAsync( Shipper entity )
		{
			return this.InsertShipperAsync
			(
				entity.CompanyName,
				entity.Phone
			);
		}

		public async Task<Int32> InsertShipperAsync( String companyName, String phone )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Shippers (
	[CompanyName], [Phone]
) VALUES (
	@companyName , @phone 
)
";				
				
				cmd.Parameters.Add( @"companyName", SqlDbType.NVarChar ).Value = companyName;
				cmd.Parameters.Add( @"phone"      , SqlDbType.NVarChar ).Value = ( phone       == null ) ? DBNull.Value : (Object)phone;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateShipperAsync(MutableShipper entity, MutableShipper whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Shippers
SET
");
				Int32 setColumnsCount = 0;
				if( entity.CompanyNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "CompanyName", "@companyName", SqlDbType.NVarChar, entity.CompanyName );

				if( entity.PhoneIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Phone", "@phone", SqlDbType.NVarChar, entity.Phone );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Shipper>> QueryShipperAsync(MutableShipper whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	ShipperID,
	CompanyName,
	Phone
FROM
	dbo.Shippers
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
					List<Shipper> list = new List<Shipper>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Shipper row = CreateShipperEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableShipper whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.ShipperIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ShipperID", "@where_shipperID", SqlDbType.Int, whereEquals.ShipperID );

			if( whereEquals.CompanyNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CompanyName", "@where_companyName", SqlDbType.NVarChar, whereEquals.CompanyName );

			if( whereEquals.PhoneIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Phone", "@where_phone", SqlDbType.NVarChar, whereEquals.Phone );

			return c;
		}

	#endregion

	#region Table: [Suppliers]

		public Task<Int32> InsertSupplierAsync( Supplier entity )
		{
			return this.InsertSupplierAsync
			(
				entity.CompanyName,
				entity.ContactName,
				entity.ContactTitle,
				entity.Address,
				entity.City,
				entity.Region,
				entity.PostalCode,
				entity.Country,
				entity.Phone,
				entity.Fax,
				entity.HomePage
			);
		}

		public async Task<Int32> InsertSupplierAsync( String companyName, String contactName, String contactTitle, String address, String city, String region, String postalCode, String country, String phone, String fax, String homePage )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Suppliers (
	[CompanyName], [ContactName], [ContactTitle], [Address], [City], [Region], [PostalCode], [Country], [Phone], [Fax], [HomePage]
) VALUES (
	@companyName , @contactName , @contactTitle , @address , @city , @region , @postalCode , @country , @phone , @fax , @homePage 
)
";				
				
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
				cmd.Parameters.Add( @"homePage"    , SqlDbType.NText    ).Value = ( homePage     == null ) ? DBNull.Value : (Object)homePage;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateSupplierAsync(MutableSupplier entity, MutableSupplier whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Suppliers
SET
");
				Int32 setColumnsCount = 0;
				if( entity.CompanyNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "CompanyName", "@companyName", SqlDbType.NVarChar, entity.CompanyName );

				if( entity.ContactNameIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ContactName", "@contactName", SqlDbType.NVarChar, entity.ContactName );

				if( entity.ContactTitleIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "ContactTitle", "@contactTitle", SqlDbType.NVarChar, entity.ContactTitle );

				if( entity.AddressIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Address", "@address", SqlDbType.NVarChar, entity.Address );

				if( entity.CityIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "City", "@city", SqlDbType.NVarChar, entity.City );

				if( entity.RegionIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Region", "@region", SqlDbType.NVarChar, entity.Region );

				if( entity.PostalCodeIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "PostalCode", "@postalCode", SqlDbType.NVarChar, entity.PostalCode );

				if( entity.CountryIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Country", "@country", SqlDbType.NVarChar, entity.Country );

				if( entity.PhoneIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Phone", "@phone", SqlDbType.NVarChar, entity.Phone );

				if( entity.FaxIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "Fax", "@fax", SqlDbType.NVarChar, entity.Fax );

				if( entity.HomePageIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "HomePage", "@homePage", SqlDbType.NText, entity.HomePage );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Supplier>> QuerySupplierAsync(MutableSupplier whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	SupplierID,
	CompanyName,
	ContactName,
	ContactTitle,
	Address,
	City,
	Region,
	PostalCode,
	Country,
	Phone,
	Fax,
	HomePage
FROM
	dbo.Suppliers
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
					List<Supplier> list = new List<Supplier>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Supplier row = CreateSupplierEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableSupplier whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.SupplierIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "SupplierID", "@where_supplierID", SqlDbType.Int, whereEquals.SupplierID );

			if( whereEquals.CompanyNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "CompanyName", "@where_companyName", SqlDbType.NVarChar, whereEquals.CompanyName );

			if( whereEquals.ContactNameIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ContactName", "@where_contactName", SqlDbType.NVarChar, whereEquals.ContactName );

			if( whereEquals.ContactTitleIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "ContactTitle", "@where_contactTitle", SqlDbType.NVarChar, whereEquals.ContactTitle );

			if( whereEquals.AddressIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Address", "@where_address", SqlDbType.NVarChar, whereEquals.Address );

			if( whereEquals.CityIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "City", "@where_city", SqlDbType.NVarChar, whereEquals.City );

			if( whereEquals.RegionIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Region", "@where_region", SqlDbType.NVarChar, whereEquals.Region );

			if( whereEquals.PostalCodeIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "PostalCode", "@where_postalCode", SqlDbType.NVarChar, whereEquals.PostalCode );

			if( whereEquals.CountryIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Country", "@where_country", SqlDbType.NVarChar, whereEquals.Country );

			if( whereEquals.PhoneIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Phone", "@where_phone", SqlDbType.NVarChar, whereEquals.Phone );

			if( whereEquals.FaxIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "Fax", "@where_fax", SqlDbType.NVarChar, whereEquals.Fax );

			if( whereEquals.HomePageIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "HomePage", "@where_homePage", SqlDbType.NText, whereEquals.HomePage );

			return c;
		}

	#endregion

	#region Table: [Territories]

		public Task<Int32> InsertTerritoryAsync( Territory entity )
		{
			return this.InsertTerritoryAsync
			(
				entity.TerritoryID,
				entity.TerritoryDescription,
				entity.RegionID
			);
		}

		public async Task<Int32> InsertTerritoryAsync( String territoryID, String territoryDescription, Int32 regionID )
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
INSERT INTO dbo.Territories (
	[TerritoryID], [TerritoryDescription], [RegionID]
) VALUES (
	@territoryID , @territoryDescription , @regionID 
)
";				
				
				cmd.Parameters.Add( @"territoryID"         , SqlDbType.NVarChar ).Value = territoryID         ;
				cmd.Parameters.Add( @"territoryDescription", SqlDbType.NChar    ).Value = territoryDescription;
				cmd.Parameters.Add( @"regionID"            , SqlDbType.Int      ).Value = regionID            ;

				return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

			}
		}

		public async Task<Int32> UpdateTerritoryAsync(MutableTerritory entity, MutableTerritory whereEquals)
		{
			if( entity == null ) throw new ArgumentNullException(nameof(entity));
			if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				StringBuilder sb = new StringBuilder(
@"UPDATE
	dbo.Territories
SET
");
				Int32 setColumnsCount = 0;
				if( entity.TerritoryDescriptionIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "TerritoryDescription", "@territoryDescription", SqlDbType.NChar, entity.TerritoryDescription );

				if( entity.RegionIDIsSet )
					AddSqlUpdateSet( sb, cmd, ref setColumnsCount, "RegionID", "@regionID", SqlDbType.Int, entity.RegionID );


				if( setColumnsCount == 0 ) throw new ArgumentException( "No entity properties are set." ); 
				
				sb.Append( "WHERE\r\n" );

				Int32 whereColumnsCount = AddWhereClause( sb, cmd, whereEquals );
				if( whereColumnsCount == 0 ) throw new ArgumentException( "No columns used in WHERE." );

				cmd.CommandText = sb.ToString();

				Int32 rows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				return rows;
			}
		}

		public async Task<List<Territory>> QueryTerritoryAsync(MutableTerritory whereEquals)
		{
			//if( whereEquals == null ) throw new ArgumentNullException(nameof(whereEquals));

			const String selectSql =
@"SELECT
	TerritoryID,
	TerritoryDescription,
	RegionID
FROM
	dbo.Territories
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
					List<Territory> list = new List<Territory>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						Territory row = CreateTerritoryEntity( rdr );
						list.Add( row );
					}

					return list;
				}
			}
		}

		private static Int32 AddWhereClause( StringBuilder sb, SqlCommand cmd, MutableTerritory whereEquals )
		{
			Int32 c = 0;
			if( whereEquals.TerritoryIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "TerritoryID", "@where_territoryID", SqlDbType.NVarChar, whereEquals.TerritoryID );

			if( whereEquals.TerritoryDescriptionIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "TerritoryDescription", "@where_territoryDescription", SqlDbType.NChar, whereEquals.TerritoryDescription );

			if( whereEquals.RegionIDIsSet )
				AddSqlUpdateWhereAnd( sb, cmd, ref c, "RegionID", "@where_regionID", SqlDbType.Int, whereEquals.RegionID );

			return c;
		}

	#endregion


		private static void AddSqlUpdateSet( StringBuilder sb, SqlCommand cmd, ref Int32 index, String columnName, String variableName, SqlDbType dbType, Object value )
		{
			if( index > 0 )
			{
				sb.Append( ",\r\n" );
			}

			sb.AppendFormat( "	[{0}] = {1}", columnName, variableName );

			cmd.Parameters.Add( variableName, dbType ).Value = value ?? DBNull.Value;

			index++;
		}

		private static void AddSqlUpdateWhereAnd( StringBuilder sb, SqlCommand cmd, ref Int32 index, String columnName, String variableName, SqlDbType dbType, Object value )
		{
			if( index > 0 )
			{
				sb.Append( "\r\n\tAND\r\n" );
			}

			sb.AppendFormat( "	[{0}] = {1}", columnName, variableName );

			cmd.Parameters.Add( variableName, dbType ).Value = value ?? DBNull.Value;

			index++;
		}

		public async Task<TestResult> ExecuteTestQueryAsync()
		{
			using( SqlCommand cmd = this.c.CreateCommand() )
			{
				cmd.CommandText =
@"
SELECT
	*
FROM
	Products

SELECT
	*
FROM
	Orders

";
				
				using( SqlDataReader rdr = await cmd.ExecuteReaderAsync().ConfigureAwait(false) )
				{
					TestResult allResults = new TestResult();

					allResults.Result0 = new List<TestResult.Result0Row>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						TestResult.Result0Row row = new TestResult.Result0Row()
						{
							ProductID = rdr.GetInt32( 0 ),
							ProductName = rdr.GetString( 1 ),
							SupplierID = rdr.IsDBNull( 2 ) ? (Int32?)null : rdr.GetInt32( 2 ),
							CategoryID = rdr.IsDBNull( 3 ) ? (Int32?)null : rdr.GetInt32( 3 ),
							QuantityPerUnit = rdr.IsDBNull( 4 ) ? (String)null : rdr.GetString( 4 ),
							UnitPrice = rdr.IsDBNull( 5 ) ? (Decimal?)null : rdr.GetDecimal( 5 ),
							UnitsInStock = rdr.IsDBNull( 6 ) ? (Int16?)null : rdr.GetInt16( 6 ),
							UnitsOnOrder = rdr.IsDBNull( 7 ) ? (Int16?)null : rdr.GetInt16( 7 ),
							ReorderLevel = rdr.IsDBNull( 8 ) ? (Int16?)null : rdr.GetInt16( 8 ),
							Discontinued = rdr.GetBoolean( 9 ),
						};
						allResults.Result0.Add( row );
					}

					await rdr.NextResultAsync().ConfigureAwait(false);

					allResults.Result1 = new List<TestResult.Result1Row>();

					while( await rdr.ReadAsync().ConfigureAwait(false) )
					{
						TestResult.Result1Row row = new TestResult.Result1Row()
						{
							OrderID = rdr.GetInt32( 0 ),
							CustomerID = rdr.IsDBNull( 1 ) ? (String)null : rdr.GetString( 1 ),
							EmployeeID = rdr.IsDBNull( 2 ) ? (Int32?)null : rdr.GetInt32( 2 ),
							OrderDate = rdr.IsDBNull( 3 ) ? (DateTime?)null : rdr.GetDateTime( 3 ),
							RequiredDate = rdr.IsDBNull( 4 ) ? (DateTime?)null : rdr.GetDateTime( 4 ),
							ShippedDate = rdr.IsDBNull( 5 ) ? (DateTime?)null : rdr.GetDateTime( 5 ),
							ShipVia = rdr.IsDBNull( 6 ) ? (Int32?)null : rdr.GetInt32( 6 ),
							Freight = rdr.IsDBNull( 7 ) ? (Decimal?)null : rdr.GetDecimal( 7 ),
							ShipName = rdr.IsDBNull( 8 ) ? (String)null : rdr.GetString( 8 ),
							ShipAddress = rdr.IsDBNull( 9 ) ? (String)null : rdr.GetString( 9 ),
							ShipCity = rdr.IsDBNull( 10 ) ? (String)null : rdr.GetString( 10 ),
							ShipRegion = rdr.IsDBNull( 11 ) ? (String)null : rdr.GetString( 11 ),
							ShipPostalCode = rdr.IsDBNull( 12 ) ? (String)null : rdr.GetString( 12 ),
							ShipCountry = rdr.IsDBNull( 13 ) ? (String)null : rdr.GetString( 13 ),
						};
						allResults.Result1.Add( row );
					}

					await rdr.NextResultAsync().ConfigureAwait(false);

					return allResults;
				}
			}
		}


	}

	#region Query result types

	public class TestResult
	{
			
		public class Result0Row
		{
			public Int32 ProductID { get; set; }
			public String ProductName { get; set; }
			public Int32? SupplierID { get; set; }
			public Int32? CategoryID { get; set; }
			public String QuantityPerUnit { get; set; }
			public Decimal? UnitPrice { get; set; }
			public Int16? UnitsInStock { get; set; }
			public Int16? UnitsOnOrder { get; set; }
			public Int16? ReorderLevel { get; set; }
			public Boolean Discontinued { get; set; }
		}

		public List<Result0Row> Result0 { get; set; }

			
		public class Result1Row
		{
			public Int32 OrderID { get; set; }
			public String CustomerID { get; set; }
			public Int32? EmployeeID { get; set; }
			public DateTime? OrderDate { get; set; }
			public DateTime? RequiredDate { get; set; }
			public DateTime? ShippedDate { get; set; }
			public Int32? ShipVia { get; set; }
			public Decimal? Freight { get; set; }
			public String ShipName { get; set; }
			public String ShipAddress { get; set; }
			public String ShipCity { get; set; }
			public String ShipRegion { get; set; }
			public String ShipPostalCode { get; set; }
			public String ShipCountry { get; set; }
		}

		public List<Result1Row> Result1 { get; set; }

	}

	#endregion

	#region Table entities

	#region Entity: Category

	// Immutable entity.
	public class Category
	{
		// Constructor
		public Category
		(
			Int32 categoryID,
			String categoryName,
			String description,
			Byte[] picture
		)
		{
			this.CategoryID   = categoryID;
			this.CategoryName = categoryName;
			this.Description  = description;
			this.Picture      = picture;
		}

		// Properties
		public Int32 CategoryID { get; } // IDENTITY, PRIMARY KEY
		public String CategoryName { get; }
		public String Description { get; }
		public Byte[] Picture { get; }
	}

	// Mmutable delta entity.
	public class MutableCategory
	{
		// Constructor
		public MutableCategory()
		{
		}

		// Properties
		public Int32  CategoryID { get; set; } // IDENTITY, PRIMARY KEY
		public Boolean CategoryIDIsSet { get; set; }

		public String CategoryName { get; set; }
		public Boolean CategoryNameIsSet { get; set; }

		public String Description { get; set; }
		public Boolean DescriptionIsSet { get; set; }

		public Byte[] Picture { get; set; }
		public Boolean PictureIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Category CreateCategoryEntity( SqlDataReader rdr )
		{
			return new Category
			(
				categoryID  : rdr.GetInt32 ( 1 ),
				categoryName: rdr.GetString( 2 ),
				description : rdr.IsDBNull(  3 ) ? (String)null : rdr.GetString(  3 ),
				picture     : rdr.IsDBNull(  4 ) ? (Byte[])null : rdr.GetSqlBinary(  4 ).Value
			);
		}
	}

	#endregion

	#region Entity: CustomerCustomerDemo

	// Immutable entity.
	public class CustomerCustomerDemo
	{
		// Constructor
		public CustomerCustomerDemo
		(
			String customerID,
			String customerTypeID
		)
		{
			this.CustomerID     = customerID;
			this.CustomerTypeID = customerTypeID;
		}

		// Properties
		public String CustomerID { get; } // PRIMARY KEY, 1 FKs
		public String CustomerTypeID { get; } // PRIMARY KEY, 1 FKs
	}

	// Mmutable delta entity.
	public class MutableCustomerCustomerDemo
	{
		// Constructor
		public MutableCustomerCustomerDemo()
		{
		}

		// Properties
		public String CustomerID { get; set; } // PRIMARY KEY, 1 FKs
		public Boolean CustomerIDIsSet { get; set; }

		public String CustomerTypeID { get; set; } // PRIMARY KEY, 1 FKs
		public Boolean CustomerTypeIDIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static CustomerCustomerDemo CreateCustomerCustomerDemoEntity( SqlDataReader rdr )
		{
			return new CustomerCustomerDemo
			(
				customerID    : rdr.GetString( 1 ),
				customerTypeID: rdr.GetString( 2 )
			);
		}
	}

	#endregion

	#region Entity: CustomerDemographic

	// Immutable entity.
	public class CustomerDemographic
	{
		// Constructor
		public CustomerDemographic
		(
			String customerTypeID,
			String customerDesc
		)
		{
			this.CustomerTypeID = customerTypeID;
			this.CustomerDesc   = customerDesc;
		}

		// Properties
		public String CustomerTypeID { get; } // PRIMARY KEY
		public String CustomerDesc { get; }
	}

	// Mmutable delta entity.
	public class MutableCustomerDemographic
	{
		// Constructor
		public MutableCustomerDemographic()
		{
		}

		// Properties
		public String CustomerTypeID { get; set; } // PRIMARY KEY
		public Boolean CustomerTypeIDIsSet { get; set; }

		public String CustomerDesc { get; set; }
		public Boolean CustomerDescIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static CustomerDemographic CreateCustomerDemographicEntity( SqlDataReader rdr )
		{
			return new CustomerDemographic
			(
				customerTypeID: rdr.GetString( 1 ),
				customerDesc  : rdr.IsDBNull(  2 ) ? (String)null : rdr.GetString(  2 )
			);
		}
	}

	#endregion

	#region Entity: Customer

	// Immutable entity.
	public class Customer
	{
		// Constructor
		public Customer
		(
			String customerID,
			String companyName,
			String contactName,
			String contactTitle,
			String address,
			String city,
			String region,
			String postalCode,
			String country,
			String phone,
			String fax
		)
		{
			this.CustomerID   = customerID;
			this.CompanyName  = companyName;
			this.ContactName  = contactName;
			this.ContactTitle = contactTitle;
			this.Address      = address;
			this.City         = city;
			this.Region       = region;
			this.PostalCode   = postalCode;
			this.Country      = country;
			this.Phone        = phone;
			this.Fax          = fax;
		}

		// Properties
		public String CustomerID { get; } // PRIMARY KEY
		public String CompanyName { get; }
		public String ContactName { get; }
		public String ContactTitle { get; }
		public String Address { get; }
		public String City { get; }
		public String Region { get; }
		public String PostalCode { get; }
		public String Country { get; }
		public String Phone { get; }
		public String Fax { get; }
	}

	// Mmutable delta entity.
	public class MutableCustomer
	{
		// Constructor
		public MutableCustomer()
		{
		}

		// Properties
		public String CustomerID { get; set; } // PRIMARY KEY
		public Boolean CustomerIDIsSet { get; set; }

		public String CompanyName { get; set; }
		public Boolean CompanyNameIsSet { get; set; }

		public String ContactName { get; set; }
		public Boolean ContactNameIsSet { get; set; }

		public String ContactTitle { get; set; }
		public Boolean ContactTitleIsSet { get; set; }

		public String Address { get; set; }
		public Boolean AddressIsSet { get; set; }

		public String City { get; set; }
		public Boolean CityIsSet { get; set; }

		public String Region { get; set; }
		public Boolean RegionIsSet { get; set; }

		public String PostalCode { get; set; }
		public Boolean PostalCodeIsSet { get; set; }

		public String Country { get; set; }
		public Boolean CountryIsSet { get; set; }

		public String Phone { get; set; }
		public Boolean PhoneIsSet { get; set; }

		public String Fax { get; set; }
		public Boolean FaxIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Customer CreateCustomerEntity( SqlDataReader rdr )
		{
			return new Customer
			(
				customerID  : rdr.GetString( 1 ),
				companyName : rdr.GetString( 2 ),
				contactName : rdr.IsDBNull(  3 ) ? (String)null : rdr.GetString(  3 ),
				contactTitle: rdr.IsDBNull(  4 ) ? (String)null : rdr.GetString(  4 ),
				address     : rdr.IsDBNull(  5 ) ? (String)null : rdr.GetString(  5 ),
				city        : rdr.IsDBNull(  6 ) ? (String)null : rdr.GetString(  6 ),
				region      : rdr.IsDBNull(  7 ) ? (String)null : rdr.GetString(  7 ),
				postalCode  : rdr.IsDBNull(  8 ) ? (String)null : rdr.GetString(  8 ),
				country     : rdr.IsDBNull(  9 ) ? (String)null : rdr.GetString(  9 ),
				phone       : rdr.IsDBNull( 10 ) ? (String)null : rdr.GetString( 10 ),
				fax         : rdr.IsDBNull( 11 ) ? (String)null : rdr.GetString( 11 )
			);
		}
	}

	#endregion

	#region Entity: Employee

	// Immutable entity.
	public class Employee
	{
		// Constructor
		public Employee
		(
			Int32 employeeID,
			String lastName,
			String firstName,
			String title,
			String titleOfCourtesy,
			DateTime? birthDate,
			DateTime? hireDate,
			String address,
			String city,
			String region,
			String postalCode,
			String country,
			String homePhone,
			String extension,
			Byte[] photo,
			String notes,
			Int32? reportsTo,
			String photoPath
		)
		{
			this.EmployeeID      = employeeID;
			this.LastName        = lastName;
			this.FirstName       = firstName;
			this.Title           = title;
			this.TitleOfCourtesy = titleOfCourtesy;
			this.BirthDate       = birthDate;
			this.HireDate        = hireDate;
			this.Address         = address;
			this.City            = city;
			this.Region          = region;
			this.PostalCode      = postalCode;
			this.Country         = country;
			this.HomePhone       = homePhone;
			this.Extension       = extension;
			this.Photo           = photo;
			this.Notes           = notes;
			this.ReportsTo       = reportsTo;
			this.PhotoPath       = photoPath;
		}

		// Properties
		public Int32 EmployeeID { get; } // IDENTITY, PRIMARY KEY
		public String LastName { get; }
		public String FirstName { get; }
		public String Title { get; }
		public String TitleOfCourtesy { get; }
		public DateTime? BirthDate { get; }
		public DateTime? HireDate { get; }
		public String Address { get; }
		public String City { get; }
		public String Region { get; }
		public String PostalCode { get; }
		public String Country { get; }
		public String HomePhone { get; }
		public String Extension { get; }
		public Byte[] Photo { get; }
		public String Notes { get; }
		public Int32? ReportsTo { get; } // 1 FKs
		public String PhotoPath { get; }
	}

	// Mmutable delta entity.
	public class MutableEmployee
	{
		// Constructor
		public MutableEmployee()
		{
		}

		// Properties
		public Int32     EmployeeID { get; set; } // IDENTITY, PRIMARY KEY
		public Boolean   EmployeeIDIsSet { get; set; }

		public String    LastName { get; set; }
		public Boolean   LastNameIsSet { get; set; }

		public String    FirstName { get; set; }
		public Boolean   FirstNameIsSet { get; set; }

		public String    Title { get; set; }
		public Boolean   TitleIsSet { get; set; }

		public String    TitleOfCourtesy { get; set; }
		public Boolean   TitleOfCourtesyIsSet { get; set; }

		public DateTime? BirthDate { get; set; }
		public Boolean   BirthDateIsSet { get; set; }

		public DateTime? HireDate { get; set; }
		public Boolean   HireDateIsSet { get; set; }

		public String    Address { get; set; }
		public Boolean   AddressIsSet { get; set; }

		public String    City { get; set; }
		public Boolean   CityIsSet { get; set; }

		public String    Region { get; set; }
		public Boolean   RegionIsSet { get; set; }

		public String    PostalCode { get; set; }
		public Boolean   PostalCodeIsSet { get; set; }

		public String    Country { get; set; }
		public Boolean   CountryIsSet { get; set; }

		public String    HomePhone { get; set; }
		public Boolean   HomePhoneIsSet { get; set; }

		public String    Extension { get; set; }
		public Boolean   ExtensionIsSet { get; set; }

		public Byte[]    Photo { get; set; }
		public Boolean   PhotoIsSet { get; set; }

		public String    Notes { get; set; }
		public Boolean   NotesIsSet { get; set; }

		public Int32?    ReportsTo { get; set; } // 1 FKs
		public Boolean   ReportsToIsSet { get; set; }

		public String    PhotoPath { get; set; }
		public Boolean   PhotoPathIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Employee CreateEmployeeEntity( SqlDataReader rdr )
		{
			return new Employee
			(
				employeeID     : rdr.GetInt32    ( 1 ),
				lastName       : rdr.GetString   ( 2 ),
				firstName      : rdr.GetString   ( 3 ),
				title          : rdr.IsDBNull(  4 ) ? (String   )null : rdr.GetString   (  4 ),
				titleOfCourtesy: rdr.IsDBNull(  5 ) ? (String   )null : rdr.GetString   (  5 ),
				birthDate      : rdr.IsDBNull(  6 ) ? (DateTime?)null : rdr.GetDateTime(  6 ),
				hireDate       : rdr.IsDBNull(  7 ) ? (DateTime?)null : rdr.GetDateTime(  7 ),
				address        : rdr.IsDBNull(  8 ) ? (String   )null : rdr.GetString   (  8 ),
				city           : rdr.IsDBNull(  9 ) ? (String   )null : rdr.GetString   (  9 ),
				region         : rdr.IsDBNull( 10 ) ? (String   )null : rdr.GetString   ( 10 ),
				postalCode     : rdr.IsDBNull( 11 ) ? (String   )null : rdr.GetString   ( 11 ),
				country        : rdr.IsDBNull( 12 ) ? (String   )null : rdr.GetString   ( 12 ),
				homePhone      : rdr.IsDBNull( 13 ) ? (String   )null : rdr.GetString   ( 13 ),
				extension      : rdr.IsDBNull( 14 ) ? (String   )null : rdr.GetString   ( 14 ),
				photo          : rdr.IsDBNull( 15 ) ? (Byte[]   )null : rdr.GetSqlBinary( 15 ).Value,
				notes          : rdr.IsDBNull( 16 ) ? (String   )null : rdr.GetString   ( 16 ),
				reportsTo      : rdr.IsDBNull( 17 ) ? (Int32?   )null : rdr.GetInt32   ( 17 ),
				photoPath      : rdr.IsDBNull( 18 ) ? (String   )null : rdr.GetString   ( 18 )
			);
		}
	}

	#endregion

	#region Entity: EmployeeTerritory

	// Immutable entity.
	public class EmployeeTerritory
	{
		// Constructor
		public EmployeeTerritory
		(
			Int32 employeeID,
			String territoryID
		)
		{
			this.EmployeeID  = employeeID;
			this.TerritoryID = territoryID;
		}

		// Properties
		public Int32 EmployeeID { get; } // PRIMARY KEY, 1 FKs
		public String TerritoryID { get; } // PRIMARY KEY, 1 FKs
	}

	// Mmutable delta entity.
	public class MutableEmployeeTerritory
	{
		// Constructor
		public MutableEmployeeTerritory()
		{
		}

		// Properties
		public Int32  EmployeeID { get; set; } // PRIMARY KEY, 1 FKs
		public Boolean EmployeeIDIsSet { get; set; }

		public String TerritoryID { get; set; } // PRIMARY KEY, 1 FKs
		public Boolean TerritoryIDIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static EmployeeTerritory CreateEmployeeTerritoryEntity( SqlDataReader rdr )
		{
			return new EmployeeTerritory
			(
				employeeID : rdr.GetInt32 ( 1 ),
				territoryID: rdr.GetString( 2 )
			);
		}
	}

	#endregion

	#region Entity: OrderDetail

	// Immutable entity.
	public class OrderDetail
	{
		// Constructor
		public OrderDetail
		(
			Int32 orderID,
			Int32 productID,
			Decimal unitPrice,
			Int16 quantity,
			Double discount
		)
		{
			this.OrderID   = orderID;
			this.ProductID = productID;
			this.UnitPrice = unitPrice;
			this.Quantity  = quantity;
			this.Discount  = discount;
		}

		// Properties
		public Int32 OrderID { get; } // PRIMARY KEY, 1 FKs
		public Int32 ProductID { get; } // PRIMARY KEY, 1 FKs
		public Decimal UnitPrice { get; }
		public Int16 Quantity { get; }
		public Double Discount { get; }
	}

	// Mmutable delta entity.
	public class MutableOrderDetail
	{
		// Constructor
		public MutableOrderDetail()
		{
		}

		// Properties
		public Int32   OrderID { get; set; } // PRIMARY KEY, 1 FKs
		public Boolean OrderIDIsSet { get; set; }

		public Int32   ProductID { get; set; } // PRIMARY KEY, 1 FKs
		public Boolean ProductIDIsSet { get; set; }

		public Decimal UnitPrice { get; set; }
		public Boolean UnitPriceIsSet { get; set; }

		public Int16   Quantity { get; set; }
		public Boolean QuantityIsSet { get; set; }

		public Double  Discount { get; set; }
		public Boolean DiscountIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static OrderDetail CreateOrderDetailEntity( SqlDataReader rdr )
		{
			return new OrderDetail
			(
				orderID  : rdr.GetInt32  ( 1 ),
				productID: rdr.GetInt32  ( 2 ),
				unitPrice: rdr.GetDecimal( 3 ),
				quantity : rdr.GetInt16  ( 4 ),
				discount : rdr.GetDouble ( 5 )
			);
		}
	}

	#endregion

	#region Entity: Order

	// Immutable entity.
	public class Order
	{
		// Constructor
		public Order
		(
			Int32 orderID,
			String customerID,
			Int32? employeeID,
			DateTime? orderDate,
			DateTime? requiredDate,
			DateTime? shippedDate,
			Int32? shipVia,
			Decimal? freight,
			String shipName,
			String shipAddress,
			String shipCity,
			String shipRegion,
			String shipPostalCode,
			String shipCountry
		)
		{
			this.OrderID        = orderID;
			this.CustomerID     = customerID;
			this.EmployeeID     = employeeID;
			this.OrderDate      = orderDate;
			this.RequiredDate   = requiredDate;
			this.ShippedDate    = shippedDate;
			this.ShipVia        = shipVia;
			this.Freight        = freight;
			this.ShipName       = shipName;
			this.ShipAddress    = shipAddress;
			this.ShipCity       = shipCity;
			this.ShipRegion     = shipRegion;
			this.ShipPostalCode = shipPostalCode;
			this.ShipCountry    = shipCountry;
		}

		// Properties
		public Int32 OrderID { get; } // IDENTITY, PRIMARY KEY
		public String CustomerID { get; } // 1 FKs
		public Int32? EmployeeID { get; } // 1 FKs
		public DateTime? OrderDate { get; }
		public DateTime? RequiredDate { get; }
		public DateTime? ShippedDate { get; }
		public Int32? ShipVia { get; } // 1 FKs
		public Decimal? Freight { get; }
		public String ShipName { get; }
		public String ShipAddress { get; }
		public String ShipCity { get; }
		public String ShipRegion { get; }
		public String ShipPostalCode { get; }
		public String ShipCountry { get; }
	}

	// Mmutable delta entity.
	public class MutableOrder
	{
		// Constructor
		public MutableOrder()
		{
		}

		// Properties
		public Int32     OrderID { get; set; } // IDENTITY, PRIMARY KEY
		public Boolean   OrderIDIsSet { get; set; }

		public String    CustomerID { get; set; } // 1 FKs
		public Boolean   CustomerIDIsSet { get; set; }

		public Int32?    EmployeeID { get; set; } // 1 FKs
		public Boolean   EmployeeIDIsSet { get; set; }

		public DateTime? OrderDate { get; set; }
		public Boolean   OrderDateIsSet { get; set; }

		public DateTime? RequiredDate { get; set; }
		public Boolean   RequiredDateIsSet { get; set; }

		public DateTime? ShippedDate { get; set; }
		public Boolean   ShippedDateIsSet { get; set; }

		public Int32?    ShipVia { get; set; } // 1 FKs
		public Boolean   ShipViaIsSet { get; set; }

		public Decimal?  Freight { get; set; }
		public Boolean   FreightIsSet { get; set; }

		public String    ShipName { get; set; }
		public Boolean   ShipNameIsSet { get; set; }

		public String    ShipAddress { get; set; }
		public Boolean   ShipAddressIsSet { get; set; }

		public String    ShipCity { get; set; }
		public Boolean   ShipCityIsSet { get; set; }

		public String    ShipRegion { get; set; }
		public Boolean   ShipRegionIsSet { get; set; }

		public String    ShipPostalCode { get; set; }
		public Boolean   ShipPostalCodeIsSet { get; set; }

		public String    ShipCountry { get; set; }
		public Boolean   ShipCountryIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Order CreateOrderEntity( SqlDataReader rdr )
		{
			return new Order
			(
				orderID       : rdr.GetInt32    ( 1 ),
				customerID    : rdr.IsDBNull(  2 ) ? (String   )null : rdr.GetString   (  2 ),
				employeeID    : rdr.IsDBNull(  3 ) ? (Int32?   )null : rdr.GetInt32   (  3 ),
				orderDate     : rdr.IsDBNull(  4 ) ? (DateTime?)null : rdr.GetDateTime(  4 ),
				requiredDate  : rdr.IsDBNull(  5 ) ? (DateTime?)null : rdr.GetDateTime(  5 ),
				shippedDate   : rdr.IsDBNull(  6 ) ? (DateTime?)null : rdr.GetDateTime(  6 ),
				shipVia       : rdr.IsDBNull(  7 ) ? (Int32?   )null : rdr.GetInt32   (  7 ),
				freight       : rdr.IsDBNull(  8 ) ? (Decimal? )null : rdr.GetDecimal (  8 ),
				shipName      : rdr.IsDBNull(  9 ) ? (String   )null : rdr.GetString   (  9 ),
				shipAddress   : rdr.IsDBNull( 10 ) ? (String   )null : rdr.GetString   ( 10 ),
				shipCity      : rdr.IsDBNull( 11 ) ? (String   )null : rdr.GetString   ( 11 ),
				shipRegion    : rdr.IsDBNull( 12 ) ? (String   )null : rdr.GetString   ( 12 ),
				shipPostalCode: rdr.IsDBNull( 13 ) ? (String   )null : rdr.GetString   ( 13 ),
				shipCountry   : rdr.IsDBNull( 14 ) ? (String   )null : rdr.GetString   ( 14 )
			);
		}
	}

	#endregion

	#region Entity: Product

	// Immutable entity.
	public class Product
	{
		// Constructor
		public Product
		(
			Int32 productID,
			String productName,
			Int32? supplierID,
			Int32? categoryID,
			String quantityPerUnit,
			Decimal? unitPrice,
			Int16? unitsInStock,
			Int16? unitsOnOrder,
			Int16? reorderLevel,
			Boolean discontinued
		)
		{
			this.ProductID       = productID;
			this.ProductName     = productName;
			this.SupplierID      = supplierID;
			this.CategoryID      = categoryID;
			this.QuantityPerUnit = quantityPerUnit;
			this.UnitPrice       = unitPrice;
			this.UnitsInStock    = unitsInStock;
			this.UnitsOnOrder    = unitsOnOrder;
			this.ReorderLevel    = reorderLevel;
			this.Discontinued    = discontinued;
		}

		// Properties
		public Int32 ProductID { get; } // IDENTITY, PRIMARY KEY
		public String ProductName { get; }
		public Int32? SupplierID { get; } // 1 FKs
		public Int32? CategoryID { get; } // 1 FKs
		public String QuantityPerUnit { get; }
		public Decimal? UnitPrice { get; }
		public Int16? UnitsInStock { get; }
		public Int16? UnitsOnOrder { get; }
		public Int16? ReorderLevel { get; }
		public Boolean Discontinued { get; }
	}

	// Mmutable delta entity.
	public class MutableProduct
	{
		// Constructor
		public MutableProduct()
		{
		}

		// Properties
		public Int32    ProductID { get; set; } // IDENTITY, PRIMARY KEY
		public Boolean  ProductIDIsSet { get; set; }

		public String   ProductName { get; set; }
		public Boolean  ProductNameIsSet { get; set; }

		public Int32?   SupplierID { get; set; } // 1 FKs
		public Boolean  SupplierIDIsSet { get; set; }

		public Int32?   CategoryID { get; set; } // 1 FKs
		public Boolean  CategoryIDIsSet { get; set; }

		public String   QuantityPerUnit { get; set; }
		public Boolean  QuantityPerUnitIsSet { get; set; }

		public Decimal? UnitPrice { get; set; }
		public Boolean  UnitPriceIsSet { get; set; }

		public Int16?   UnitsInStock { get; set; }
		public Boolean  UnitsInStockIsSet { get; set; }

		public Int16?   UnitsOnOrder { get; set; }
		public Boolean  UnitsOnOrderIsSet { get; set; }

		public Int16?   ReorderLevel { get; set; }
		public Boolean  ReorderLevelIsSet { get; set; }

		public Boolean  Discontinued { get; set; }
		public Boolean  DiscontinuedIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Product CreateProductEntity( SqlDataReader rdr )
		{
			return new Product
			(
				productID      : rdr.GetInt32   ( 1 ),
				productName    : rdr.GetString  ( 2 ),
				supplierID     : rdr.IsDBNull(  3 ) ? (Int32?  )null : rdr.GetInt32  (  3 ),
				categoryID     : rdr.IsDBNull(  4 ) ? (Int32?  )null : rdr.GetInt32  (  4 ),
				quantityPerUnit: rdr.IsDBNull(  5 ) ? (String  )null : rdr.GetString  (  5 ),
				unitPrice      : rdr.IsDBNull(  6 ) ? (Decimal?)null : rdr.GetDecimal(  6 ),
				unitsInStock   : rdr.IsDBNull(  7 ) ? (Int16?  )null : rdr.GetInt16  (  7 ),
				unitsOnOrder   : rdr.IsDBNull(  8 ) ? (Int16?  )null : rdr.GetInt16  (  8 ),
				reorderLevel   : rdr.IsDBNull(  9 ) ? (Int16?  )null : rdr.GetInt16  (  9 ),
				discontinued   : rdr.GetBoolean ( 10 )
			);
		}
	}

	#endregion

	#region Entity: Region

	// Immutable entity.
	public class Region
	{
		// Constructor
		public Region
		(
			Int32 regionID,
			String regionDescription
		)
		{
			this.RegionID          = regionID;
			this.RegionDescription = regionDescription;
		}

		// Properties
		public Int32 RegionID { get; } // PRIMARY KEY
		public String RegionDescription { get; }
	}

	// Mmutable delta entity.
	public class MutableRegion
	{
		// Constructor
		public MutableRegion()
		{
		}

		// Properties
		public Int32  RegionID { get; set; } // PRIMARY KEY
		public Boolean RegionIDIsSet { get; set; }

		public String RegionDescription { get; set; }
		public Boolean RegionDescriptionIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Region CreateRegionEntity( SqlDataReader rdr )
		{
			return new Region
			(
				regionID         : rdr.GetInt32 ( 1 ),
				regionDescription: rdr.GetString( 2 )
			);
		}
	}

	#endregion

	#region Entity: Shipper

	// Immutable entity.
	public class Shipper
	{
		// Constructor
		public Shipper
		(
			Int32 shipperID,
			String companyName,
			String phone
		)
		{
			this.ShipperID   = shipperID;
			this.CompanyName = companyName;
			this.Phone       = phone;
		}

		// Properties
		public Int32 ShipperID { get; } // IDENTITY, PRIMARY KEY
		public String CompanyName { get; }
		public String Phone { get; }
	}

	// Mmutable delta entity.
	public class MutableShipper
	{
		// Constructor
		public MutableShipper()
		{
		}

		// Properties
		public Int32  ShipperID { get; set; } // IDENTITY, PRIMARY KEY
		public Boolean ShipperIDIsSet { get; set; }

		public String CompanyName { get; set; }
		public Boolean CompanyNameIsSet { get; set; }

		public String Phone { get; set; }
		public Boolean PhoneIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Shipper CreateShipperEntity( SqlDataReader rdr )
		{
			return new Shipper
			(
				shipperID  : rdr.GetInt32 ( 1 ),
				companyName: rdr.GetString( 2 ),
				phone      : rdr.IsDBNull(  3 ) ? (String)null : rdr.GetString(  3 )
			);
		}
	}

	#endregion

	#region Entity: Supplier

	// Immutable entity.
	public class Supplier
	{
		// Constructor
		public Supplier
		(
			Int32 supplierID,
			String companyName,
			String contactName,
			String contactTitle,
			String address,
			String city,
			String region,
			String postalCode,
			String country,
			String phone,
			String fax,
			String homePage
		)
		{
			this.SupplierID   = supplierID;
			this.CompanyName  = companyName;
			this.ContactName  = contactName;
			this.ContactTitle = contactTitle;
			this.Address      = address;
			this.City         = city;
			this.Region       = region;
			this.PostalCode   = postalCode;
			this.Country      = country;
			this.Phone        = phone;
			this.Fax          = fax;
			this.HomePage     = homePage;
		}

		// Properties
		public Int32 SupplierID { get; } // IDENTITY, PRIMARY KEY
		public String CompanyName { get; }
		public String ContactName { get; }
		public String ContactTitle { get; }
		public String Address { get; }
		public String City { get; }
		public String Region { get; }
		public String PostalCode { get; }
		public String Country { get; }
		public String Phone { get; }
		public String Fax { get; }
		public String HomePage { get; }
	}

	// Mmutable delta entity.
	public class MutableSupplier
	{
		// Constructor
		public MutableSupplier()
		{
		}

		// Properties
		public Int32  SupplierID { get; set; } // IDENTITY, PRIMARY KEY
		public Boolean SupplierIDIsSet { get; set; }

		public String CompanyName { get; set; }
		public Boolean CompanyNameIsSet { get; set; }

		public String ContactName { get; set; }
		public Boolean ContactNameIsSet { get; set; }

		public String ContactTitle { get; set; }
		public Boolean ContactTitleIsSet { get; set; }

		public String Address { get; set; }
		public Boolean AddressIsSet { get; set; }

		public String City { get; set; }
		public Boolean CityIsSet { get; set; }

		public String Region { get; set; }
		public Boolean RegionIsSet { get; set; }

		public String PostalCode { get; set; }
		public Boolean PostalCodeIsSet { get; set; }

		public String Country { get; set; }
		public Boolean CountryIsSet { get; set; }

		public String Phone { get; set; }
		public Boolean PhoneIsSet { get; set; }

		public String Fax { get; set; }
		public Boolean FaxIsSet { get; set; }

		public String HomePage { get; set; }
		public Boolean HomePageIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Supplier CreateSupplierEntity( SqlDataReader rdr )
		{
			return new Supplier
			(
				supplierID  : rdr.GetInt32 ( 1 ),
				companyName : rdr.GetString( 2 ),
				contactName : rdr.IsDBNull(  3 ) ? (String)null : rdr.GetString(  3 ),
				contactTitle: rdr.IsDBNull(  4 ) ? (String)null : rdr.GetString(  4 ),
				address     : rdr.IsDBNull(  5 ) ? (String)null : rdr.GetString(  5 ),
				city        : rdr.IsDBNull(  6 ) ? (String)null : rdr.GetString(  6 ),
				region      : rdr.IsDBNull(  7 ) ? (String)null : rdr.GetString(  7 ),
				postalCode  : rdr.IsDBNull(  8 ) ? (String)null : rdr.GetString(  8 ),
				country     : rdr.IsDBNull(  9 ) ? (String)null : rdr.GetString(  9 ),
				phone       : rdr.IsDBNull( 10 ) ? (String)null : rdr.GetString( 10 ),
				fax         : rdr.IsDBNull( 11 ) ? (String)null : rdr.GetString( 11 ),
				homePage    : rdr.IsDBNull( 12 ) ? (String)null : rdr.GetString( 12 )
			);
		}
	}

	#endregion

	#region Entity: Territory

	// Immutable entity.
	public class Territory
	{
		// Constructor
		public Territory
		(
			String territoryID,
			String territoryDescription,
			Int32 regionID
		)
		{
			this.TerritoryID          = territoryID;
			this.TerritoryDescription = territoryDescription;
			this.RegionID             = regionID;
		}

		// Properties
		public String TerritoryID { get; } // PRIMARY KEY
		public String TerritoryDescription { get; }
		public Int32 RegionID { get; } // 1 FKs
	}

	// Mmutable delta entity.
	public class MutableTerritory
	{
		// Constructor
		public MutableTerritory()
		{
		}

		// Properties
		public String TerritoryID { get; set; } // PRIMARY KEY
		public Boolean TerritoryIDIsSet { get; set; }

		public String TerritoryDescription { get; set; }
		public Boolean TerritoryDescriptionIsSet { get; set; }

		public Int32  RegionID { get; set; } // 1 FKs
		public Boolean RegionIDIsSet { get; set; }

	}

	public partial class NorthwindDatabase
	{
		public static Territory CreateTerritoryEntity( SqlDataReader rdr )
		{
			return new Territory
			(
				territoryID         : rdr.GetString( 1 ),
				territoryDescription: rdr.GetString( 2 ),
				regionID            : rdr.GetInt32 ( 3 )
			);
		}
	}

	#endregion


	#endregion

}