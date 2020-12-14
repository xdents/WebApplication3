using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Model;

namespace WebApplication3.Control
{
    public class PeriodControl
    {
        public IEnumerable<PeriodModel> GetPeriodList()
        {
            string sql = $@"select distinct Period from HRSQL_V_CombinedData where Period is not null";
            var reader = SqlHelper.GetReader(sql);
            List<PeriodModel> PeriodList = new List<PeriodModel>();
            while (reader.Read())
            {
                PeriodList.Add(new PeriodModel
                {
                    Period = reader["Period"].ToString(),
                });
            }
            reader.Close();
            return PeriodList;
        }
    }
}