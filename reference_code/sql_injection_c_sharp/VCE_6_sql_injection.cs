// from: https://github.com/taobao/sqlautoreview/blob/master/src/main/java/com/taobao/sqlautoreview/HandleSQLReviewDB.java
public boolean insertDB(int sqlmap_file_id,String java_class_id,String sql_xml,String real_sql,String sql_comment)
{
    try{
        sql_comment=new String(sql_comment.getBytes(),"GBK");
        String command="insert into xmltosql(sqlmap_file_id,java_class_id,sql_xml,real_sql,sql_comment,gmt_create,gmt_modified,status) "; 
        command=command+"values("+sqlmap_file_id+",'"+java_class_id+"','"+sql_xml+"','"+real_sql+"','"+sql_comment+"',"+"now(),"+"now(),"+"0);";
        Statement stmt = conn.createStatement();
        stmt.execute(command);
        stmt.close();
    }
    catch(Exception e)
    {
        logger.error("写入数据库出错", e);
        return false;
    }

    return true;
}