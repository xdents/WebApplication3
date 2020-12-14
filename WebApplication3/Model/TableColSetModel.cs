using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Model
{
    [Serializable]
    public class TableColSetModel
    {
        public string TableName { get; set; }
        public string ProcName { get; set; }
    }
}