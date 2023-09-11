public void saveMergeResult(int sqlmap_file_id, String tablename,
			String real_tablename, String exist_indexes, String new_indexes,
			String merge_result) {
		String command="insert into mergeresult(sqlmap_file_id,tablename,real_tablename,exist_indexes,new_indexes,merge_result,gmt_create,gmt_modified) values(";
		command=command+sqlmap_file_id+",'"+tablename+"','"+real_tablename+"','"+exist_indexes+"','"+new_indexes+"','"+merge_result+"',now(),now())";
		
		Statement stmt;
		
			stmt = conn.createStatement();
			stmt.execute(command);
			stmt.close();
		
	}