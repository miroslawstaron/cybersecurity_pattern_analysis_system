public int InsertTable(string[] arr)
        {
            var t = "";
            var v = "";
            if (arr.Length == 3)
            {
                t = arr[1];
                v = arr[2];
            }
            else
            {
                throw new Exception("参数错误");
            }
            var arrs = v.Split(",");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in arrs)
            {
                var ar = item.Split("=");
                dic.Add(ar[0], "'" + ar[1] + "'");
            }

            string sql = "INSERT INTO " + t + " (" + string.Join(",", dic.Keys) + ") VALUES (" + string.Join(",", dic.Values) + ")";
            int rel = 0;
            try
            {
                rel = fsql.Ado.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(sql + "\r\n" + ex.Message);
            }
            return rel;
        }