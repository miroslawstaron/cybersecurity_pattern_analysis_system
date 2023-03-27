public int updateSQLStatus(int status,String sql_auto_index,int id,String auto_review_err,String auto_review_tip)
{
    String command="update xmltosql set status="+status+",sql_auto_index='"+sql_auto_index+"',";
    command=command+"auto_review_err='"+auto_review_err+"',";
    command=command+"auto_review_tip='"+auto_review_tip+"',";
    command=command+"auto_review_time=now(),gmt_modified=now() where id="+id+";";
    try {
        Statement stmt = conn.createStatement();
        stmt.execute(command);
        if(stmt.getUpdateCount()!=1)
            return -1;
        else {
            return 0;
        }
    } catch (SQLException e) {
        logger.error("执行command="+command+"出现异常", e);
        return -2;
    }
}