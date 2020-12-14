using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Microsoft.Reporting.WebForms;
using WebApplication3.Model;
using WebApplication3.Control;

namespace WebApplication3
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        public IEnumerable<TableColSetModel> TableTypeList
        {
            get { return ViewState["TableTypeList"] as IEnumerable<TableColSetModel>; }
            set { ViewState["TableTypeList"] = value; }
        }

        public IEnumerable<PeriodModel> PeriodList
        {
            get { return ViewState["PeriodList"] as IEnumerable<PeriodModel>; }
            set { ViewState["PeriodList"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (TableTypeList == null)
                {
                    TableTypeList = new TableColSetControl().GetTableTypeList();
                }

                if (PeriodList == null)
                {
                    PeriodList = new PeriodControl().GetPeriodList();
                }
            }
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            PeriodModel input = json.Deserialize<PeriodModel>(this.hfInfo.Value);

            string Period = (input.Period == null || input.Period == "-1" || input.Period == "") ? "" : input.Period;

            ReportViewerHelper viewerHelper = new ReportViewerHelper();
            DataSet ds = SqlHelper.GetDataTablesByStore("select * from HRSQL_V_CombinedData where Period = '" + Period  + "'", null);
            ReportDataSource rd = viewerHelper.Get("dsHR", ds.Tables[0]);
            viewerHelper.ReloadReportViewer(this, this.ReportViewer1, @"HRCombineData.rdlc", rd);

        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUpload.HasFile)
                {
                    string FileName = FileUpload.PostedFile.FileName;
                    string Ext = Path.GetExtension(FileName).ToLower();
                    string[] allowExtension = { ".xlsx", ".xls" };

                    if (allowExtension.Contains(Ext))
                    {
                        using (ExcelHelper opExcel = new ExcelHelper())
                        {
                            JavaScriptSerializer json = new JavaScriptSerializer();
                            TableColSetModel input = json.Deserialize<TableColSetModel>(this.hfInfo.Value);

                            string ProcName = (input.ProcName == null || input.ProcName == "-1" || input.ProcName == "") ? "" : input.ProcName;
  
                            //取出文件对应列的格式
                            DataTable dtColType = SqlHelper.ExecuteDataTable(CommandType.Text, "select * from TableColSet where ProcName = '" + ProcName + "'", null);
                            //读取EXCEL
                            DataTable dt = opExcel.ExcelToDataTable(FileName, null, dtColType);
                            //导入到SQL
                            SqlHelper.ExecteNonQuery(CommandType.StoredProcedure, ProcName,
                                new SqlParameter[]{
                                    new SqlParameter("@table", dt),
                                });

                            LblMsg.Text = "success";
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('只能上传EXCEL文件!')</script>");
                        Response.End();
                    }
                }
                else
                {
                    //文件不存在
                    Response.Write("<script>alert('文件不存在!')</script>");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                LblMsg.Text = ex.Message.ToString();
            }

        }
    }
}