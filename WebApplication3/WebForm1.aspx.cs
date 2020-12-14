using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication3.data;
using System.Windows;

namespace WebApplication3
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            test();
        }

        private void test()
        {
            DataTable dt = new DataTable();

            return;
            dcDataContext Db = new dcDataContext();
            
            var test = (from c in Db.QuoteHed
                        where c.Quoted == true
                        select c).FirstOrDefault();

            test.QuoteNum.ToString();

            var u = (from c in Db.QuoteHed
                     join d in Db.UD01 on c.SysRowID equals d.SysRowID
                     where c.Quoted == true
                     select d);

            

            Db.SubmitChanges();
            

            //var sumPODetail = (from c in Db.PODetail
            //                   join d in Db.PORel on new { PONum = c.PONUM, c.POLine } equals new { PONum = d.PONum, d.POLine}
            //                   where c.PONUM == 123123
            //                   group c by new { c.PartNum, d.JobSeq } into b
            //                   select new
            //                   {
            //                       PartNum = b.Key.PartNum,
            //                       CalcJobSeq = b.Key.JobSeq,
            //                       CalcOurQty = b.Sum(a => a.OrderQty),
            //                       CalcDocTotalCost = b.Sum(a => a.DocExtCost)
            //                   }
            //         ).ToList();
            //Db.SubmitChanges();
            //var test = (from c in Db.PODetail
            //            where c.PONUM == 123123
            //            select new { res = c.OrderQty * (100 + 10) / 100 }
            //            ).Sum(p=>p.res);

            return;

            //获取当前输入的数据
            //var CurJob = (from j in this.ttLaborDtl select j).FirstOrDefault();
            var CurJob = (from c in Db.LaborDtl where c.JobNum == "000117" && c.OprSeq == 20 select c).FirstOrDefault();

            //获取上一道工序号
            var LastSeqJob = (from c in Db.LaborDtl 
                            where c.Company == CurJob.Company
                                && c.JobNum == CurJob.JobNum
                                && c.OprSeq < CurJob.OprSeq 
                            orderby c.OprSeq descending
                            select c).FirstOrDefault();

            //获取当前工序已完成的数量，不包含当前行
            var CurJobCom = (from c in Db.LaborDtl
                             where c.Company == CurJob.Company
                                 && c.JobNum == CurJob.JobNum
                                 && c.OprSeq == CurJob.OprSeq
                                 && c.TimeStatus == "A"
                             select c.LaborQty
                                  ).ToList(); 
            decimal CurJobCompleted = CurJobCom == null ? 0 : CurJobCom.Sum();

            //不存在上一道工序，当前为第一工序，则 录入的数量 <= 需求数 - 已录入同工序的数
            if (LastSeqJob == null)
            {
                //获取需求的数量[runqty*(100+报废率)%]
                var planJob = (from c in Db.JobOper
                                where c.Company == CurJob.Company
                                    && c.JobNum == CurJob.JobNum
                                    && c.OprSeq == CurJob.OprSeq
                                select c).FirstOrDefault();
                var jobMtl = (from c in Db.JobMtl
                                where c.Company == CurJob.Company
                                    && c.JobNum == CurJob.JobNum
                                select c).FirstOrDefault();
                var reqQty = jobMtl != null ? Math.Floor(planJob.RunQty * (1 + jobMtl.EstScrap / 100)) : planJob.RunQty;

                if (reqQty - CurJobCompleted - CurJob.LaborQty < 0)
                {
                    //throw new Ice.Common.BusinessObjectException(
                    // new Ice.Common.BusinessObjectMessage("The required quantity:" + reqQty.ToString("0")
                    //    + "The current seq completed quantity:" + CurJobCompleted.ToString("0") + "The input labor quantity:" + CurJob.LaborQty.ToString("0")
                    //    + CurJob.LaborQty.ToString("0") + " + " + CurJobCompleted.ToString("0") + " > " + reqQty.ToString("0"))
                    // {
                    //     Type = Ice.Common.BusinessObjectMessageType.Error,
                    // });
                }
            }
            else
            {
                //获取上一道工序完工的数量
                var LastJobCompleted = (from c in Db.JobOper
                                        where c.Company == CurJob.Company
                                            && c.JobNum == CurJob.JobNum
                                            && c.OprSeq == LastSeqJob.OprSeq
                                        select c.QtyCompleted
                                        ).FirstOrDefault();

                if (LastJobCompleted - CurJobCompleted - CurJob.LaborQty < 0)
                {
                    //throw new Ice.Common.BusinessObjectException(
                    // new Ice.Common.BusinessObjectMessage("The last seq completed quantity:" + LastJobCompleted.ToString("0")
                    //    + "The current seq completed quantity:" + CurJobCompleted.ToString("0") + "The input labor quantity:" + CurJob.LaborQty.ToString("0")
                    //    + CurJob.LaborQty.ToString("0") + " + " + CurJobCompleted.ToString("0") + " > " + LastJobCompleted.ToString("0"))
                    // {
                    //     Type = Ice.Common.BusinessObjectMessageType.Error,
                    // });
                }
            }

        }

        protected void a1_TextChanged(object sender, EventArgs e)
        {
            this.lbl.Text = this.a1.Text;
        }
    }
}