// **************************************************
// Custom code for ReceiptEntryForm
// Created: 3/20/2018 9:37:02 AM
// **************************************************

extern alias Erp_Contracts_BO_Receipt;
extern alias Erp_Contracts_BO_ICReceiptSearch;
extern alias Erp_Contracts_BO_SupplierXRef;
extern alias Erp_Contracts_BO_Currency;
extern alias Erp_Contracts_BO_Company;
extern alias Erp_Contracts_BO_Part;
extern alias Erp_Contracts_BO_Vendor;
extern alias Erp_Contracts_BO_VendorPPSearch;
extern alias Erp_Contracts_BO_JobEntry;
extern alias Erp_Contracts_BO_JobAsmSearch;

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Erp.Adapters;
using Erp.UI;
using Ice.Lib;
using Ice.Adapters;
using Ice.Lib.Customization;
using Ice.Lib.ExtendedProps;
using Ice.Lib.Framework;
using Ice.Lib.Searches;
using Ice.UI.FormFunctions;


using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;


public static class Script
{
	// ** Wizard Insert Location - Do Not Remove 'Begin/End Wizard Added Module Level Variables' Comments! **
	// Begin Wizard Added Module Level Variables **

	private static EpiDataView edvRcvDtl;
	private static EpiBaseAdapter oTrans_receiptAdapter;
	private static EpiDataView edvRcvHead;
	private static EpiDataView edvMultiKeySearch;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public static void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization
		edvRcvHead = ((EpiDataView)(oTrans.EpiDataViews["RcvHead"]));
		edvMultiKeySearch = ((EpiDataView)(oTrans.EpiDataViews["MultiKeySearch"]));
		Script.edvRcvDtl = ((EpiDataView)(Script.oTrans.EpiDataViews["RcvDtl"]));
		Script.edvRcvDtl.EpiViewNotification += new EpiViewNotification(Script.edvRcvDtl_EpiViewNotification);
		Script.oTrans_receiptAdapter = ((EpiBaseAdapter)(Script.csm.TransAdaptersHT["oTrans_receiptAdapter"]));
		Script.oTrans_receiptAdapter.BeforeAdapterMethod += new BeforeAdapterMethod(Script.oTrans_receiptAdapter_BeforeAdapterMethod);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		Script.epiButtonC1.Click += new System.EventHandler(Script.epiButtonC1_Click);
		// End Wizard Added Custom Method Calls
	}

	public static void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		Script.edvRcvDtl.EpiViewNotification -= new EpiViewNotification(Script.edvRcvDtl_EpiViewNotification);
		Script.edvRcvDtl = null;
		Script.oTrans_receiptAdapter.BeforeAdapterMethod -= new BeforeAdapterMethod(Script.oTrans_receiptAdapter_BeforeAdapterMethod);
		Script.oTrans_receiptAdapter = null;
		Script.epiButtonC1.Click -= new System.EventHandler(Script.epiButtonC1_Click);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private static void edvRcvDtl_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		if ((args.NotifyType == EpiTransaction.NotifyType.Initialize))
		{
			if ((args.Row > -1))
			{
				DynamicQueryAdapter adapterDynamicQuery = new DynamicQueryAdapter(oTrans);
				adapterDynamicQuery.BOConnect();

				adapterDynamicQuery.ExecuteByID("DMR");
				DataView TB2 = new DataView(adapterDynamicQuery.QueryResults.Tables["Results"]);

				TB2.RowFilter = "DMRHead_Company = '" + view.dataView[args.Row]["Company"].ToString() + "' AND DMRActn_PackSlip = '" + view.dataView[args.Row]["PackSlip"].ToString() + "' AND DMRActn_PackLine='" + view.dataView[args.Row]["PackLine"].ToString() + "'";

				txtUAIQty.Text = Convert.ToString(Convert.ToDouble("0" + view.dataView[args.Row]["FailedQty"].ToString()) - Convert.ToDouble("0" + TB2.ToTable().Compute("Max(DMRActn_Quantity)", string.Empty).ToString()));
				txtFinalQty.Text = Convert.ToString(Convert.ToDouble("0" + view.dataView[args.Row]["PassedQty"].ToString()) + Convert.ToDouble("0" + view.dataView[args.Row]["FailedQty"].ToString()) - Convert.ToDouble("0" + TB2.ToTable().Compute("Max(DMRActn_Quantity)", string.Empty).ToString()));

				adapterDynamicQuery.Dispose();
			}
		}
	}

	private static void oTrans_receiptAdapter_BeforeAdapterMethod(object sender, BeforeAdapterMethodArgs args)
	{
		// ** Argument Properties and Uses **
		// ** args.MethodName **
		// ** Add Event Handler Code **

		// ** Use MessageBox to find adapter method name
		// EpiMessageBox.Show(args.MethodName)
		switch (args.MethodName)
		{
			case "UpdateMaster":
				if (edvRcvHead.dataView[edvRcvHead.Row]["Company"].ToString() == "19268A")
				{
					if (edvMultiKeySearch.dataView[edvMultiKeySearch.Row]["PackSlip"].ToString() == "")
					{
						DynamicQueryAdapter adapterDynamicQuery = new DynamicQueryAdapter(oTrans);
						adapterDynamicQuery.BOConnect();

						adapterDynamicQuery.ExecuteByID("GRN_Head");
						DataView TB2 = new DataView(adapterDynamicQuery.QueryResults.Tables["Results"]);

						TB2.RowFilter = "SUBSTRING(RcvHead_PackSlip,1,3) = 'G22'";

						string MaxNo = Convert.ToString(TB2.ToTable().Compute("Max(RcvHead_PackSlip)", string.Empty));
						if (MaxNo == "")
							MaxNo = "00000000000";
						int RNo = int.Parse(MaxNo.Substring(3, 7)) + 1;
						string RunningNo = RNo.ToString().PadLeft(7, '0');

						edvMultiKeySearch.dataView[edvMultiKeySearch.Row]["PackSlip"] = "G22" + RunningNo;

						adapterDynamicQuery.Dispose();
					}
				}

				if (edvRcvHead.dataView[edvRcvHead.Row]["Company"].ToString() == "19268B")
				{
					if (edvMultiKeySearch.dataView[edvMultiKeySearch.Row]["PackSlip"].ToString() == "")
					{
						DynamicQueryAdapter adapterDynamicQuery = new DynamicQueryAdapter(oTrans);
						adapterDynamicQuery.BOConnect();

						adapterDynamicQuery.ExecuteByID("GRN_Head");
						DataView TB2 = new DataView(adapterDynamicQuery.QueryResults.Tables["Results"]);

						TB2.RowFilter = "SUBSTRING(RcvHead_PackSlip,1,3) = 'G25'";

						string MaxNo = Convert.ToString(TB2.ToTable().Compute("Max(RcvHead_PackSlip)", string.Empty));
						if (MaxNo == "")
							MaxNo = "00000000000";
						int RNo = int.Parse(MaxNo.Substring(3, 7)) + 1;
						string RunningNo = RNo.ToString().PadLeft(7, '0');

						edvMultiKeySearch.dataView[edvMultiKeySearch.Row]["PackSlip"] = "G25" + RunningNo;

						adapterDynamicQuery.Dispose();
					}
				}

				if (edvRcvHead.dataView[edvRcvHead.Row]["Company"].ToString() == "19268")
				{
					DynamicQueryAdapter adapterDynamicQuery = new DynamicQueryAdapter(oTrans);
					adapterDynamicQuery.BOConnect();

					adapterDynamicQuery.ExecuteByID("SO_CrossComp");
					DataView TB2 = new DataView(adapterDynamicQuery.QueryResults.Tables["Results"]);

					TB2.RowFilter = "OrderHed_Company = '19268A' AND OrderHed_ICPONum = '" + edvRcvHead.dataView[edvRcvHead.Row]["PONum"].ToString() + "'";
					//MessageBox.Show(TB2.Count.ToString());
					if (TB2.Count == 0)
					{

						MessageBox.Show("Please ensure CHPE SO already been approved.");
						edvRcvHead.dataView[edvRcvHead.Row]["PONum"] = "";

					}
					adapterDynamicQuery.Dispose();
				}
				break;
		}

	}

	private static void epiButtonC1_Click(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		string legalnumber = edvRcvHead.dataView[edvRcvHead.Row]["LegalNumber"].ToString();
		OpenIe("http://cpsz-web2.compart-grp.com/Epicor_Rpt/MyReportViewer.aspx?ReportURL=/Epicor/ReceiptOrderReport&DocNo=" + legalnumber);

	}

	/// <summary>
	/// 用IE打开浏览器
	/// </summary>
	/// <param name="url"></param>
	public static void OpenIe(string url)
	{
		try
		{
			Process.Start("iexplore.exe", url);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			// IE浏览器路径安装：C:\Program Files\Internet Explorer
			// at System.Diagnostics.process.StartWithshellExecuteEx(ProcessStartInfo startInfo)注意这个错误
			try
			{
				if (File.Exists(@"C:\Program Files\Internet Explorer\iexplore.exe"))
				{
					ProcessStartInfo processStartInfo = new ProcessStartInfo
					{
						FileName = @"C:\Program Files\Internet Explorer\iexplore.exe",
						Arguments = url,
						UseShellExecute = false,
						CreateNoWindow = true
					};
					Process.Start(processStartInfo);
				}
				else
				{
					if (File.Exists(@"C:\Program Files (x86)\Internet Explorer\iexplore.exe"))
					{
						ProcessStartInfo processStartInfo = new ProcessStartInfo
						{
							FileName = @"C:\Program Files (x86)\Internet Explorer\iexplore.exe",
							Arguments = url,
							UseShellExecute = false,
							CreateNoWindow = true
						};
						Process.Start(processStartInfo);
					}
					//  else
					// {
					// if (MessageBox.Show(@"系统未安装IE浏览器，是否下载安装？", null, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
					// {
					// 打开下载链接，从微软官网下载
					//     OpenDefaultBrowserUrl("http://windows.microsoft.com/zh-cn/internet-explorer/download-ie");
					// }
					//  }
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message);
			}
		}
	}


}

