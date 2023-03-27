public void deleteMergeResult(int sqlmap_file_id) {
		
		String command="delete from mergeresult where sqlmap_file_id="+sqlmap_file_id;
		Statement stmt;
		try {
			stmt = conn.createStatement();
			stmt.execute(command);
			int rows=stmt.getUpdateCount();
			stmt.close();
			logger.debug("mergeresult sqlmap_file_id "+sqlmap_file_id+rows+" is deleted.");
		} catch (SQLException e) {
			logger.error("some error happen:",e);
		}
	}