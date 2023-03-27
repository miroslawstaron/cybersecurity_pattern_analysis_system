    public void buildDBMetaData()
    {
    	String command;
    	command="select table_name from information_schema.tables where table_schema='"+dbname+"';";
    	try
    	{
    	   Statement stmt = conn.createStatement();
	       stmt.execute(command);
	       ResultSet rs = stmt.getResultSet();
	       while(rs.next())
	       {
	    	   buildTableMetaData(rs.getString("TABLE_NAME"));
	       }
	       
	       rs.close();
	       stmt.close();
    	}
    	 catch(SQLException e)
         {
      	   e.printStackTrace();
         }
    	 
    }