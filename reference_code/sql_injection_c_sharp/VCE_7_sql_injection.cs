       public List<SQL_Node> getAllSQL()
       {
    	 
    	List<SQL_Node> list_sql = new LinkedList<SQL_Node>();
       	try
       	{
       	   HandleXMLConf sqlmapfileconf=new HandleXMLConf("sqlmapfile.xml");
    	   int sqlmap_file_id=sqlmapfileconf.getSQLMapFileID();
       	   String command="select id,real_sql from xmltosql where status = 0 and sqlmap_file_id="+sqlmap_file_id;
       	   Statement stmt = conn.createStatement();
   	       stmt.execute(command);
   	       ResultSet rs = stmt.getResultSet();
   	  
   	       while(rs.next())
   	       {
   	    	   SQL_Node snNode= new SQL_Node();
   	    	   snNode.id=rs.getInt("id");
   	    	   snNode.sqlString=rs.getString("real_sql");
   	    	   snNode.sqlString=formatSql(snNode.sqlString);
   	    	   list_sql.add(snNode);
   	       }
   	       
   	       rs.close();
           stmt.close();
           return list_sql;
       	}
           catch(SQLException e)
           {
        	   logger.error("检索待审核的SQL出错", e);
        	   return null;
           }
       }
       