
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Web;
using System.Web.UI;

namespace WebApplication3
{
    public class ReportViewerHelper
    {
        private Page page = null;
        private HttpContext context = null;
        public ReportViewerHelper() { }
        public ReportViewerHelper(Page page, HttpContext context)
        {
            this.page = page;
            this.context = context;
        }
        public void LoadingReportData(string sql, string reportViewName, string reportDataSetName, string rdlc, params string[] parametes)
        {
            if (parametes != null)
            {
                foreach (string param in parametes)
                {
                    sql += "'" + param + "',";
                }
                sql = sql.TrimEnd(',') + ";";
            }
            if (page.FindControl(reportViewName) != null)
            {
                DataSet ds = SqlHelper.GetDataTablesByStore(sql, null);
                var rds = Get(reportDataSetName, ds.Tables[0]);
                ReloadReportViewer(page, page.FindControl(reportViewName) as ReportViewer, rdlc, rds);
            }

        }

        /// <summary>
        /// Reload local ReportViewer from RDLC file
        /// </summary>        
        public void ReloadReportViewer(Page reportPage, ReportViewer reportViewer, string fileName_RDLC,
            ReportDataSource reportDataSource, params ReportParameter[] reportParameters)
        {
            reportViewer.Reset();
            //reportViewer.LocalReport.DataSources.Clear();//清除之前的数据源
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = reportPage.Server.MapPath(fileName_RDLC);
            reportViewer.LocalReport.EnableExternalImages = true;
            LocalReport report = reportViewer.LocalReport;//这句可以取消
            reportViewer.LocalReport.SetParameters(reportParameters);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);
            reportViewer.LocalReport.Refresh();
        }

        public void ReloadReportViewer2(Page reportPage, ReportViewer reportViewer, string fileName_RDLC,
            ReportDataSource[] reportDataSource)
        {
            reportViewer.Reset();
            //reportViewer.LocalReport.DataSources.Clear();//清除之前的数据源
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = reportPage.Server.MapPath(fileName_RDLC);
            reportViewer.LocalReport.EnableExternalImages = true;
            LocalReport report = reportViewer.LocalReport;//这句可以取消
            foreach (ReportDataSource rds in reportDataSource)
            {
                reportViewer.LocalReport.DataSources.Add(rds);
            }
            reportViewer.LocalReport.Refresh();
        }

        /// <summary>
        /// Get ReportDataSource
        /// </summary>
        /// <param name="name">this name equals the dataset name in RDLC  </param>
        /// <param name="dataSourceValue">DataTable format</param>
        /// <returns>ReportDataSource</returns>
        public ReportDataSource Get(string name, DataTable dataSourceValue)
        {
            ReportDataSource rds = new ReportDataSource(name, dataSourceValue);
            return rds;
        }
        public void SetReportParams(ReportViewer reportViewer, params ReportParameter[] reportParameters)
        {
            reportViewer.LocalReport.SetParameters(reportParameters);
        }


    }
}