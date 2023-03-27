public void deleteSqlByID(int sqlmap_file_id)
	{
		String command="delete from xmltosql where sqlmap_file_id="+sqlmap_file_id;
		Statement stmt;
		try {
			stmt = conn.createStatement();
			stmt.execute(command);
			int rows=stmt.getUpdateCount();
			stmt.close();
			logger.info(rows+" is deleted.");
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}