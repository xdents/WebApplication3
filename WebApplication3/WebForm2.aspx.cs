// **************************************************
// Custom code for PartForm
// Created: 2017/8/4 13:56:58
// **************************************************
PART
extern alias Erp_Contracts_BO_Part;
extern alias Erp_Contracts_BO_PartPlantSearch;
extern alias Erp_Contracts_BO_PO;
extern alias Erp_Contracts_BO_PartOnHandWhse;
extern alias Erp_Contracts_BO_Vendor;
extern alias Erp_Contracts_BO_VendorPPSearch;
extern alias Erp_Adapters_Part;

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

public static class Script
{
	// ** Wizard Insert Location - Do Not Remove 'Begin/End Wizard Added Module Level Variables' Comments! **
	// Begin Wizard Added Module Level Variables **

	private static EpiDataView edvPart;
	private static EpiBaseAdapter oTrans_adapter;
	private static EpiBaseAdapter oTrans_partOnHandWhseAdapter;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public static void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		Script.Part_Column.ColumnChanging += new DataColumnChangeEventHandler(Script.Part_BeforeFieldChange);
		Script.Part_Column.ColumnChanged += new DataColumnChangeEventHandler(Script.Part_AfterFieldChange);
		Script.edvPart = ((EpiDataView)(Script.oTrans.EpiDataViews["Part"]));
		Script.edvPart.EpiViewNotification += new EpiViewNotification(Script.edvPart_EpiViewNotification);
		Script.oTrans_adapter = ((EpiBaseAdapter)(Script.csm.TransAdaptersHT["oTrans_adapter"]));
		Script.oTrans_adapter.BeforeAdapterMethod += new BeforeAdapterMethod(Script.oTrans_adapter_BeforeAdapterMethod);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		Script.btnGenWH.Click += new System.EventHandler(Script.btnGenWH_Click);
		// End Wizard Added Custom Method Calls
	}

	public static void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		Script.Part_Column.ColumnChanging -= new DataColumnChangeEventHandler(Script.Part_BeforeFieldChange);
		Script.Part_Column.ColumnChanged -= new DataColumnChangeEventHandler(Script.Part_AfterFieldChange);
		Script.edvPart.EpiViewNotification -= new EpiViewNotification(Script.edvPart_EpiViewNotification);
		Script.edvPart = null;
		Script.oTrans_adapter.BeforeAdapterMethod -= new BeforeAdapterMethod(Script.oTrans_adapter_BeforeAdapterMethod);
		Script.oTrans_adapter = null;
		Script.btnGenWH.Click -= new System.EventHandler(Script.btnGenWH_Click);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private static void Part_BeforeFieldChange(object sender, DataColumnChangeEventArgs args)
	{
		// ** Argument Properties and Uses **
		// args.Row["FieldName"]
		// args.Column, args.ProposedValue, args.Row
		// Add Event Handler Code
		switch (args.Column.ColumnName)
		{
			case "Character06":
				//				if (args.Row["Character06"].ToString() == "Lee")
				//					MessageBox.Show("1");
				//				else 
				//					MessageBox.Show("2");
				break;
		}
	}

	private static void Part_AfterFieldChange(object sender, DataColumnChangeEventArgs args)
	{
		// ** Argument Properties and Uses **
		// args.Row["FieldName"]
		// args.Column, args.ProposedValue, args.Row
		// Add Event Handler Code

		switch (args.Column.ColumnName)
		{
			case "ClassID":
				if ((args.Row["ClassID"].ToString() == "FG") || (args.Row["ClassID"].ToString() == "SFG"))
				{
					args.Row["TypeCode"] = "M";
					args.Row["UOMClassID"] = "Count";
					args.Row["IUM"] = "Each";
					args.Row["SalesUM"] = "Each";
					args.Row["PUM"] = "Each";
				}
				else if ((args.Row["ClassID"].ToString() == "DM") || (args.Row["ClassID"].ToString() == "IM") || (args.Row["ClassID"].ToString() == "PK"))
				{
					args.Row["TypeCode"] = "P";
				}
				else
				{
					args.Row["TypeCode"] = "P";
				}
				break;
		}
	}

	private static void edvPart_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		if ((args.NotifyType == EpiTransaction.NotifyType.Initialize))
		{

			btnGenWH.ReadOnly = true;

			if ((args.Row > -1))
			{
				btnGenWH.ReadOnly = false;
				if ((view.dataView[args.Row]["ClassID"].ToString() == "FG") || (view.dataView[args.Row]["ClassID"].ToString() == "SFG"))
				{
					cbMType.ReadOnly = true;
					cbFType.ReadOnly = false;
				}
				else if ((view.dataView[args.Row]["ClassID"].ToString() == "DM") || (view.dataView[args.Row]["ClassID"].ToString() == "IM") || (view.dataView[args.Row]["ClassID"].ToString() == "PK"))
				{
					cbMType.ReadOnly = false;
					cbFType.ReadOnly = true;
				}
				else
				{
					cbMType.ReadOnly = true;
					cbFType.ReadOnly = true;
				}
			}
		}
	}

	private static void oTrans_adapter_BeforeAdapterMethod(object sender, BeforeAdapterMethodArgs args)
	{
		// ** Argument Properties and Uses **
		// ** args.MethodName **
		// ** Add Event Handler Code **

		// ** Use MessageBox to find adapter method name
		//MessageBox.Show(args.MethodName);

		switch (args.MethodName)
		{
			case "Update":
				if (edvPart.Row > -1)
				{

					if (edvPart.CurrentDataRow.RowState.ToString() == "Added")
					{

						if ((edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "FG") || (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "SFG"))
						{
							if (edvPart.dataView[edvPart.Row]["PartNum"].ToString() == "")
							{
								DynamicQueryAdapter adapterDynamicQuery = new DynamicQueryAdapter(oTrans);
								adapterDynamicQuery.BOConnect();

								adapterDynamicQuery.ExecuteByID("ItemMaster");
								DataView TB2 = new DataView(adapterDynamicQuery.QueryResults.Tables["Results"]);

								string P01 = edvPart.dataView[edvPart.Row]["ShortChar02"].ToString();

								TB2.RowFilter = "SUBSTRING(Part_PartNum,1,2) = '" + P01.Substring(0, 2) + "'";
								string MaxNo = Convert.ToString(TB2.ToTable().Compute("Max(Calculated_FGCode)", string.Empty));

								if (MaxNo == "")
									MaxNo = "00000000";

								int RNo = int.Parse(MaxNo.Substring(3, 5)) + 1;
								string RunningNo = RNo.ToString().PadLeft(5, '0');


								if (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "SFG")
									edvPart.dataView[edvPart.Row]["PartNum"] = P01 + RunningNo + "KT";
								else
									edvPart.dataView[edvPart.Row]["PartNum"] = P01 + RunningNo;

								adapterDynamicQuery.Dispose();
							}
							edvPart.dataView[edvPart.Row]["Character01"] = "";
							edvPart.dataView[edvPart.Row]["CostMethod"] = "S";



						}
						else if ((edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "CT") || (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "DM") || (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "IM") || (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "PK"))
						{
							if (edvPart.dataView[edvPart.Row]["PartNum"].ToString() == "")
							{
								DynamicQueryAdapter adapterDynamicQuery = new DynamicQueryAdapter(oTrans);
								adapterDynamicQuery.BOConnect();

								adapterDynamicQuery.ExecuteByID("ItemMaster");
								DataView TB2 = new DataView(adapterDynamicQuery.QueryResults.Tables["Results"]);

								string P01 = edvPart.dataView[edvPart.Row]["Character01"].ToString();
								P01 = P01.Substring(0, 4);
								TB2.RowFilter = "SUBSTRING(Part_PartNum,1,4) = '" + P01 + "'";

								string MaxNo = Convert.ToString(TB2.ToTable().Compute("Max(Part_PartNum)", string.Empty));
								if (MaxNo == "")
									MaxNo = "0000000000";
								MessageBox.Show(MaxNo.Substring(5, 5));
								int RNo = int.Parse(MaxNo.Substring(5, 5)) + 1;
								string RunningNo = RNo.ToString().PadLeft(5, '0');

								edvPart.dataView[edvPart.Row]["PartNum"] = P01 + "-" + RunningNo;

								adapterDynamicQuery.Dispose();
							}
							edvPart.dataView[edvPart.Row]["ShortChar02"] = "";
							edvPart.dataView[edvPart.Row]["CostMethod"] = "A";
						}
						else
						{
							cbMType.ReadOnly = true;
							cbFType.ReadOnly = true;
							edvPart.dataView[edvPart.Row]["CostMethod"] = "S";
						}

					}
				}
				break;
		}

	}


	private static void btnGenWH_Click(object sender, System.EventArgs args)
	{

		PartAdapter InsertWH = new PartAdapter(oTrans);
		InsertWH.BOConnect();

		if (edvPart.dataView[edvPart.Row]["Company"].ToString() == "19268")
		{
			string[] ArrWH = { "310322", "310323", "310324", "310325", "310326", "310327", "310328", "310329", "310330", "310331", "310332", "310333", "310334", "310335", "310336", "310337", "310338", "310339", "310340", "310341", "310342", "310345", "310346", "310347", "310348", "310349", "310350", "310351", "310354", "310355", "310356", "310357", "310358", "310352", "310353" };
			for (int i = 0; i <= ArrWH.Length - 1; i++)
			{
				InsertWH.GetByID(edvPart.dataView[edvPart.Row]["PartNum"].ToString());
				InsertWH.GetNewPartWhse(edvPart.dataView[edvPart.Row]["PartNum"].ToString(), "MfgSys");

				DataRow newXAttchRow = InsertWH.PartData.PartWhse[InsertWH.PartData.PartWhse.Rows.Count - 1];
				newXAttchRow["Company"] = edvPart.dataView[edvPart.Row]["Company"].ToString();
				newXAttchRow["PartNum"] = edvPart.dataView[edvPart.Row]["PartNum"].ToString();
				newXAttchRow["WarehouseCode"] = ArrWH[i].ToString();
				try
				{
					InsertWH.Update();
				}
				catch (Exception ex)
				{

				}
			}
		}
		else
		{
			if ((edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "FG") || (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "SFG"))
			{
				string[] ArrWH = { "320328", "320330", "320331", "320332", "320333", "320334", "320335", "320337", "320338", "320339", "320341", "320329" };
				for (int i = 0; i <= ArrWH.Length - 1; i++)
				{

					InsertWH.GetByID(edvPart.dataView[edvPart.Row]["PartNum"].ToString());
					InsertWH.GetNewPartWhse(edvPart.dataView[edvPart.Row]["PartNum"].ToString(), "MfgSys");

					DataRow newXAttchRow = InsertWH.PartData.PartWhse[InsertWH.PartData.PartWhse.Rows.Count - 1];
					newXAttchRow["Company"] = edvPart.dataView[edvPart.Row]["Company"].ToString();
					newXAttchRow["PartNum"] = edvPart.dataView[edvPart.Row]["PartNum"].ToString();
					newXAttchRow["WarehouseCode"] = ArrWH[i].ToString();
					try
					{
						InsertWH.Update();
					}
					catch (Exception ex)
					{

					}

				}
			}
			else if (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "DM")
			{
				string[] ArrWH = { "320321", "320322", "320336", "320337", "320338", "320329" };
				for (int i = 0; i <= ArrWH.Length - 1; i++)
				{

					InsertWH.GetByID(edvPart.dataView[edvPart.Row]["PartNum"].ToString());
					InsertWH.GetNewPartWhse(edvPart.dataView[edvPart.Row]["PartNum"].ToString(), "MfgSys");

					DataRow newXAttchRow = InsertWH.PartData.PartWhse[InsertWH.PartData.PartWhse.Rows.Count - 1];
					newXAttchRow["Company"] = edvPart.dataView[edvPart.Row]["Company"].ToString();
					newXAttchRow["PartNum"] = edvPart.dataView[edvPart.Row]["PartNum"].ToString();
					newXAttchRow["WarehouseCode"] = ArrWH[i].ToString();
					try
					{
						InsertWH.Update();
					}
					catch (Exception ex)
					{

					}

				}
			}
			else if (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "IM")
			{

				string[] ArrWH = { "320323", "320324", "320337", "320338" };
				for (int i = 0; i <= ArrWH.Length - 1; i++)
				{

					InsertWH.GetByID(edvPart.dataView[edvPart.Row]["PartNum"].ToString());
					InsertWH.GetNewPartWhse(edvPart.dataView[edvPart.Row]["PartNum"].ToString(), "MfgSys");

					DataRow newXAttchRow = InsertWH.PartData.PartWhse[InsertWH.PartData.PartWhse.Rows.Count - 1];
					newXAttchRow["Company"] = edvPart.dataView[edvPart.Row]["Company"].ToString();
					newXAttchRow["PartNum"] = edvPart.dataView[edvPart.Row]["PartNum"].ToString();
					newXAttchRow["WarehouseCode"] = ArrWH[i].ToString();
					try
					{
						InsertWH.Update();
					}
					catch (Exception ex)
					{

					}

				}
			}
			else if (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "PK")
			{
				string[] ArrWH = { "320325", "320326", "320337", "320338" };
				for (int i = 0; i <= ArrWH.Length - 1; i++)
				{

					InsertWH.GetByID(edvPart.dataView[edvPart.Row]["PartNum"].ToString());
					InsertWH.GetNewPartWhse(edvPart.dataView[edvPart.Row]["PartNum"].ToString(), "MfgSys");

					DataRow newXAttchRow = InsertWH.PartData.PartWhse[InsertWH.PartData.PartWhse.Rows.Count - 1];
					newXAttchRow["Company"] = edvPart.dataView[edvPart.Row]["Company"].ToString();
					newXAttchRow["PartNum"] = edvPart.dataView[edvPart.Row]["PartNum"].ToString();
					newXAttchRow["WarehouseCode"] = ArrWH[i].ToString();
					try
					{
						InsertWH.Update();
					}
					catch (Exception ex)
					{

					}

				}
			}
			else if (edvPart.dataView[edvPart.Row]["ClassID"].ToString() == "CT")
			{
				string[] ArrWH = { "320340" };
				for (int i = 0; i <= ArrWH.Length - 1; i++)
				{

					InsertWH.GetByID(edvPart.dataView[edvPart.Row]["PartNum"].ToString());
					InsertWH.GetNewPartWhse(edvPart.dataView[edvPart.Row]["PartNum"].ToString(), "MfgSys");

					DataRow newXAttchRow = InsertWH.PartData.PartWhse[InsertWH.PartData.PartWhse.Rows.Count - 1];
					newXAttchRow["Company"] = edvPart.dataView[edvPart.Row]["Company"].ToString();
					newXAttchRow["PartNum"] = edvPart.dataView[edvPart.Row]["PartNum"].ToString();
					newXAttchRow["WarehouseCode"] = ArrWH[i].ToString();
					try
					{
						InsertWH.Update();
					}
					catch (Exception ex)
					{

					}

				}
			}
		}
		InsertWH.Dispose();


	}
}












