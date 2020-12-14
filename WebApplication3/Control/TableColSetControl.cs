using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Model;

namespace WebApplication3.Control
{
    public class TableColSetControl
    {
        public IEnumerable<TableColSetModel> GetTableTypeList()
        {
            string sql = $@"select * from TableColSet";
            var reader = SqlHelper.GetReader(sql);
            List<TableColSetModel> TableTypeList = new List<TableColSetModel>();
            while (reader.Read())
            {
                TableTypeList.Add(new TableColSetModel
                {
                    TableName = reader["TableName"].ToString(),
                    ProcName = reader["ProcName"].ToString()
                });
            }
            reader.Close();
            return TableTypeList;
        }

    }
}