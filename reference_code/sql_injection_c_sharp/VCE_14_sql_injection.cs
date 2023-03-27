public DataTable SelectTable(string[] arr)
        {
            var t = "";
            string w = "";
            if (arr.Length == 2)
            {
                t = arr[1];
            }
            else if (arr.Length >= 3)
            {
                t = arr[1];
                if (arr.Length == 3 && int.TryParse(arr[2], out int relId))
                {
                    w = fsql.DbFirst.GetTableByName(t).Primarys[0].Name + "=" + relId;
                }
                else if (arr.Length == 3 && Guid.TryParse(arr[2], out Guid relGuid))
                {
                    w = fsql.DbFirst.GetTableByName(t).Primarys[0].Name + "='" + relGuid + "'";
                }
                else
                {
                    w = string.Join(" ", arr);
                    w = w.Substring(arr[0].Length + arr[2].Length, w.Length - 1);
                }
            }
            else
            {
                throw new Exception("参数错误");
            }
            string sql = "select * from " + t + (!string.IsNullOrEmpty(w) ? (" where " + w) : "");
            DataTable dt = new DataTable();
            try
            {
                dt = fsql.Ado.ExecuteDataTable(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(sql + "\r\n" + ex.Message);
            }
            return dt;
        }